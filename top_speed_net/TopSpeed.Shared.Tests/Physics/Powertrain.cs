using System;
using TopSpeed.Physics.Powertrain;
using TopSpeed.Physics.Torque;
using Xunit;

namespace TopSpeed.Shared.Tests.Physics
{
    public sealed class Powertrain
    {
        [Fact]
        public void TorqueCurveProfile_Interpolates_AndClamps()
        {
            var curve = new CurveProfile(new[]
            {
                new CurvePoint(1000f, 100f),
                new CurvePoint(3000f, 300f),
                new CurvePoint(6000f, 180f)
            });

            Assert.Equal(100f, curve.EvaluateTorque(500f));
            Assert.Equal(300f, curve.EvaluateTorque(3000f));
            Assert.Equal(240f, curve.EvaluateTorque(4500f));
            Assert.Equal(180f, curve.EvaluateTorque(7000f));
        }

        [Fact]
        public void PowertrainCalculator_Computes_PositiveDriveAcceleration()
        {
            var configuration = BuildConfiguration();
            var acceleration = Calculator.DriveAccel(
                configuration,
                gear: 2,
                speedMps: 22f,
                throttle: 0.8f,
                surfaceTractionModifier: 1f,
                longitudinalGripFactor: 1f);

            Assert.True(acceleration > 0f);
        }

        [Fact]
        public void PowertrainCalculator_Computes_EngineBrakingDeceleration()
        {
            var configuration = BuildConfiguration();
            var decelKph = Calculator.EngineBrakeDecelKph(
                configuration,
                gear: 3,
                inReverse: false,
                speedMps: 35f,
                surfaceDecelerationModifier: 1f,
                currentEngineRpm: 4500f);

            Assert.True(decelKph > 0f);
        }

        // SAE hp: P[hp] = T[N·m] × n[rpm] × 2π / (60 × 745.69987) ≈ T × n / 7120.8
        [Fact]
        public void PowertrainCalculator_ComputesHorsepower_UsingSaeFormula()
        {
            var horsepower = Calculator.Horsepower(400f, 6000f);
            // 400 × 6000 / 7120.8 ≈ 337.1 hp
            Assert.InRange(horsepower, 336.5f, 337.5f);
        }

        // Engine friction torque must be subtracted before wheel torque is calculated.
        [Fact]
        public void DriveAccel_IsReducedBy_EngineFriction()
        {
            var noFriction = BuildConfigurationWithFriction(0f);
            var withFriction = BuildConfigurationWithFriction(80f);

            var accelNoFriction = Calculator.DriveAccel(
                noFriction, gear: 1, speedMps: 5f,
                throttle: 0.5f, surfaceTractionModifier: 1f, longitudinalGripFactor: 1f);

            var accelWithFriction = Calculator.DriveAccel(
                withFriction, gear: 1, speedMps: 5f,
                throttle: 0.5f, surfaceTractionModifier: 1f, longitudinalGripFactor: 1f);

            Assert.True(accelNoFriction > accelWithFriction,
                "Higher engine friction torque must reduce drive acceleration.");
        }

        // Rolling resistance must increase with speed (ISO 18164 speed correction).
        [Fact]
        public void ResistiveForce_RollingResistance_IncreasesWithSpeed()
        {
            var config = BuildConfiguration();

            // At standstill: only rolling resistance (drag = 0).
            var forceAtStop = Calculator.ResistiveForce(config, 0f);
            var expectedStaticRolling = config.RollingResistanceCoefficient * config.MassKg * 9.80665f;
            Assert.InRange(forceAtStop, expectedStaticRolling * 0.99f, expectedStaticRolling * 1.01f);

            // At 30 m/s the rolling component must be larger than the static value.
            var forceAt30 = Calculator.ResistiveForce(config, 30f);
            var dragAt30 = 0.5f * 1.225f * config.DragCoefficient * config.FrontalAreaM2 * 30f * 30f;
            var rollingAt30 = forceAt30 - dragAt30;
            Assert.True(rollingAt30 > expectedStaticRolling,
                "Rolling resistance must be higher at speed than at standstill.");
        }

        // Engine friction must reduce engine braking torque, just as it reduces drive torque.
        [Fact]
        public void EngineBrakeDecel_IsReducedBy_EngineFriction()
        {
            var noFriction = BuildConfigurationWithFriction(0f);
            var withFriction = BuildConfigurationWithFriction(200f);

            var decelNoFriction = Calculator.EngineBrakeDecelKph(
                noFriction, gear: 3, inReverse: false, speedMps: 35f,
                surfaceDecelerationModifier: 1f, currentEngineRpm: 4500f);

            var decelWithFriction = Calculator.EngineBrakeDecelKph(
                withFriction, gear: 3, inReverse: false, speedMps: 35f,
                surfaceDecelerationModifier: 1f, currentEngineRpm: 4500f);

            Assert.True(decelNoFriction > decelWithFriction,
                "Higher engine friction torque must reduce engine braking deceleration.");
        }

        private static Config BuildConfiguration() => BuildConfigurationWithFriction(20f);

        private static Config BuildConfigurationWithFriction(float engineFrictionTorqueNm)
        {
            var torqueCurve = CurveFactory.FromLegacy(
                idleRpm: 900f,
                revLimiter: 7600f,
                peakTorqueRpm: 3600f,
                idleTorqueNm: 180f,
                peakTorqueNm: 650f,
                redlineTorqueNm: 360f);

            return new Config(
                massKg: 1650f,
                drivetrainEfficiency: 0.85f,
                engineBrakingTorqueNm: 300f,
                tireGripCoefficient: 1.0f,
                brakeStrength: 1.0f,
                wheelRadiusM: 0.34f,
                engineBraking: 0.3f,
                idleRpm: 900f,
                revLimiter: 7600f,
                finalDriveRatio: 3.70f,
                powerFactor: 0.7f,
                peakTorqueNm: 650f,
                peakTorqueRpm: 3600f,
                idleTorqueNm: 180f,
                redlineTorqueNm: 360f,
                dragCoefficient: 0.30f,
                frontalAreaM2: 2.2f,
                rollingResistanceCoefficient: 0.015f,
                launchRpm: 2400f,
                reversePowerFactor: 0.55f,
                reverseGearRatio: 3.2f,
                engineInertiaKgm2: 0.24f,
                engineFrictionTorqueNm: engineFrictionTorqueNm,
                drivelineCouplingRate: 12f,
                gears: 6,
                gearRatios: new[] { 3.5f, 2.2f, 1.5f, 1.2f, 1.0f, 0.85f },
                torqueCurve: torqueCurve);
        }
    }
}
