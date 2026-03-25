using System;
using TopSpeed.Vehicles;
using Xunit;

namespace TopSpeed.Shared.Tests.Physics
{
    public sealed class VehicleCatalogTests
    {
        // Every official vehicle must have a named torque curve preset so the
        // engine character matches the vehicle type rather than falling back to
        // the generic family_sedan shape.
        [Fact]
        public void AllVehicles_HaveTorqueCurvePreset_Set()
        {
            foreach (var spec in OfficialVehicleCatalog.Vehicles)
            {
                Assert.False(
                    string.IsNullOrWhiteSpace(spec.TorqueCurvePreset),
                    $"{spec.Name} must have a TorqueCurvePreset assigned.");
            }
        }

        // Supersport bikes must use the supersport_bike preset, which models the
        // aggressive high-RPM power delivery of inline-4 and V4 engines.
        [Fact]
        public void Motorcycles_Use_SupersportBikePreset()
        {
            foreach (var spec in OfficialVehicleCatalog.Vehicles)
            {
                if (spec.HasWipers == 0)
                {
                    Assert.Equal(
                        "supersport_bike",
                        spec.TorqueCurvePreset,
                        StringComparer.Ordinal);
                }
            }
        }

        // Consecutive gear ratios on supersport bikes must not drop by more than
        // 35% per step. The previous data had a ~49% drop from 2nd to 3rd gear,
        // which caused an unrealistically large RPM fall-off on upshift.
        [Fact]
        public void BikeGearRatios_HaveUniformSteps_NoStepExceeds35Percent()
        {
            foreach (var spec in OfficialVehicleCatalog.Vehicles)
            {
                if (spec.HasWipers != 0)
                    continue;

                var ratios = spec.GearRatios;
                Assert.NotNull(ratios);
                Assert.True(ratios!.Length >= 2, $"{spec.Name} must have at least 2 gears.");

                for (var i = 1; i < ratios.Length; i++)
                {
                    var prev = ratios[i - 1];
                    var curr = ratios[i];
                    Assert.True(prev > 0f && curr > 0f,
                        $"{spec.Name} gear {i} or {i + 1} has a non-positive ratio.");
                    var stepFraction = (prev - curr) / prev;
                    Assert.True(
                        stepFraction <= 0.35f,
                        $"{spec.Name} gear {i}→{i + 1}: ratio step {stepFraction:P0} exceeds 35% " +
                        $"(prev={prev}, curr={curr}).");
                }
            }
        }

        // Engine friction must be smaller for motorcycles than for large cars.
        // Bikes have small, highly polished engines; trucks and supercars have
        // large engines with proportionally higher internal losses.
        [Fact]
        public void EngineFriction_IsLowerForBikes_ThanForLargeCars()
        {
            float maxBikeFriction = 0f;
            float minLargeCarFriction = float.MaxValue;

            foreach (var spec in OfficialVehicleCatalog.Vehicles)
            {
                if (spec.HasWipers == 0)
                    maxBikeFriction = Math.Max(maxBikeFriction, spec.EngineFrictionTorqueNm);
                else if (spec.MassKg >= 1500f)
                    minLargeCarFriction = Math.Min(minLargeCarFriction, spec.EngineFrictionTorqueNm);
            }

            Assert.True(
                maxBikeFriction < minLargeCarFriction,
                $"Max bike friction ({maxBikeFriction} N·m) must be less than " +
                $"min large-car friction ({minLargeCarFriction} N·m).");
        }
    }
}
