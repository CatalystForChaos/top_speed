using TopSpeed.Physics.Tires;
using Xunit;

namespace TopSpeed.Shared.Tests.Physics
{
    public sealed class Tires
    {
        private static TireModelParameters BuildParameters(
            float massKg = 1500f,
            float highSpeedSteerGain = 1.2f,
            float trackWidthM = 1.86f,
            float cgHeightM = 0.55f)
        {
            return new TireModelParameters(
                steeringResponse: 1.8f,
                maxSteerDeg: 35f,
                wheelbaseM: 2.8f,
                trackWidthM: trackWidthM,
                vehicleLengthM: 4.7f,
                tireGripCoefficient: 1.0f,
                lateralGripCoefficient: 1.0f,
                highSpeedStability: 0.1f,
                massKg: massKg,
                highSpeedSteerGain: highSpeedSteerGain,
                highSpeedSteerStartKph: 140f,
                highSpeedSteerFullKph: 240f,
                combinedGripPenalty: 0.72f,
                slipAnglePeakDeg: 8f,
                slipAngleFalloff: 1.25f,
                turnResponse: 1.0f,
                massSensitivity: 0.8f,
                downforceGripGain: 0.1f,
                cornerStiffnessFront: 1.05f,
                cornerStiffnessRear: 0.95f,
                yawInertiaScale: 1.0f,
                steeringCurve: 1.1f,
                transientDamping: 0.35f,
                cgHeightM: cgHeightM);
        }

        [Fact]
        public void Solve_IsDeterministic_ForSameInput()
        {
            var parameters = BuildParameters();
            var input = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 55.55f, steeringInput: 35, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var initialState = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var expected = TireModelSolver.Solve(parameters, input, initialState);
            for (var i = 0; i < 64; i++)
            {
                var actual = TireModelSolver.Solve(parameters, input, initialState);
                Assert.Equal(expected.LongitudinalGripFactor, actual.LongitudinalGripFactor);
                Assert.Equal(expected.LateralSpeedMps, actual.LateralSpeedMps);
                Assert.Equal(expected.State.LateralVelocityMps, actual.State.LateralVelocityMps);
                Assert.Equal(expected.State.YawRateRad, actual.State.YawRateRad);
            }
        }

        [Fact]
        public void Solve_ClampsLongitudinalGrip_AndKeepsFiniteOutputs()
        {
            var parameters = BuildParameters();
            var input = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 68f, steeringInput: 100, surfaceTractionMod: 0.4f, surfaceLateralMultiplier: 1.44f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var result = TireModelSolver.Solve(parameters, input, state);

            Assert.True(result.LongitudinalGripFactor >= 0f && result.LongitudinalGripFactor <= 1f);
            Assert.False(float.IsNaN(result.LongitudinalGripFactor) || float.IsInfinity(result.LongitudinalGripFactor));
            Assert.False(float.IsNaN(result.LateralSpeedMps) || float.IsInfinity(result.LateralSpeedMps));
            Assert.False(float.IsNaN(result.State.LateralVelocityMps) || float.IsInfinity(result.State.LateralVelocityMps));
            Assert.False(float.IsNaN(result.State.YawRateRad) || float.IsInfinity(result.State.YawRateRad));
        }

        [Fact]
        public void Solve_AppliesSurfaceLateralMultiplier()
        {
            var parameters = BuildParameters();
            var baseInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 50f, steeringInput: 35, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var snowInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 50f, steeringInput: 35, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1.44f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var baseResult = TireModelSolver.Solve(parameters, baseInput, state);
            var snowResult = TireModelSolver.Solve(parameters, snowInput, state);

            Assert.True(System.Math.Abs(snowResult.LateralSpeedMps) > System.Math.Abs(baseResult.LateralSpeedMps));
        }

        [Fact]
        public void Solve_MassSensitivity_MakesLighterVehicleTurnFaster()
        {
            var light = BuildParameters(massKg: 220f);
            var heavy = BuildParameters(massKg: 1900f);
            var input = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 50f, steeringInput: 30, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var lightResult = TireModelSolver.Solve(light, input, state);
            var heavyResult = TireModelSolver.Solve(heavy, input, state);

            Assert.True(System.Math.Abs(lightResult.LateralSpeedMps) > System.Math.Abs(heavyResult.LateralSpeedMps));
        }

        [Fact]
        public void Solve_HighSpeedRange_ReducesYawPerSpeed()
        {
            var parameters = BuildParameters(highSpeedSteerGain: 1.35f);
            var mediumInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 36f, steeringInput: 35, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var highInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 58f, steeringInput: 35, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var medium = TireModelSolver.Solve(parameters, mediumInput, state);
            var high = TireModelSolver.Solve(parameters, highInput, state);

            var mediumYawPerSpeed = System.Math.Abs(medium.State.YawRateRad) / mediumInput.SpeedMps;
            var highYawPerSpeed = System.Math.Abs(high.State.YawRateRad) / highInput.SpeedMps;

            Assert.True(highYawPerSpeed < mediumYawPerSpeed);
            Assert.True(System.Math.Abs(high.LateralSpeedMps) > 0.01f);
        }

        [Fact]
        public void Solve_HighSpeedSteerGain_RemainsABoundedBoost()
        {
            var lowGain = BuildParameters(highSpeedSteerGain: 0.90f);
            var highGain = BuildParameters(highSpeedSteerGain: 1.35f);
            var input = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 58f, steeringInput: 35, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var low = TireModelSolver.Solve(lowGain, input, state);
            var high = TireModelSolver.Solve(highGain, input, state);

            var lowYaw = System.Math.Abs(low.State.YawRateRad);
            var highYaw = System.Math.Abs(high.State.YawRateRad);

            Assert.True(highYaw > lowYaw);
            Assert.True(highYaw < lowYaw * 1.35f);
        }

        [Fact]
        public void Solve_PreservesSteeringDirection_LeftAndRight()
        {
            var parameters = BuildParameters();
            var leftInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 45f, steeringInput: -40, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var rightInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 45f, steeringInput: 40, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var left = TireModelSolver.Solve(parameters, leftInput, state);
            var right = TireModelSolver.Solve(parameters, rightInput, state);

            Assert.True(left.LateralSpeedMps < 0f);
            Assert.True(right.LateralSpeedMps > 0f);
        }

        [Fact]
        public void Solve_RecentersAfterSteeringRelease()
        {
            var parameters = BuildParameters();
            var steerInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 50f, steeringInput: 55, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var neutralInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 50f, steeringInput: 0, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);

            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);
            var steerStep = TireModelSolver.Solve(parameters, steerInput, state);
            state = steerStep.State;

            for (var i = 0; i < 90; i++)
                state = TireModelSolver.Solve(parameters, neutralInput, state).State;

            Assert.True(System.Math.Abs(state.LateralVelocityMps) < 0.6f);
            Assert.True(System.Math.Abs(state.YawRateRad) < 0.35f);
        }

        [Fact]
        public void Solve_KeepsDirectionConsistent_AcrossSpeeds()
        {
            var parameters = BuildParameters();
            var speeds = new[] { 10f, 25f, 45f, 65f };

            foreach (var speed in speeds)
            {
                var rightInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: speed, steeringInput: 45, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
                var leftInput = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: speed, steeringInput: -45, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);

                var rightState = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);
                var leftState = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);
                TireModelOutput right = default;
                TireModelOutput left = default;

                for (var i = 0; i < 30; i++)
                {
                    right = TireModelSolver.Solve(parameters, rightInput, rightState);
                    rightState = right.State;
                    left = TireModelSolver.Solve(parameters, leftInput, leftState);
                    leftState = left.State;
                }

                Assert.True(right.LateralSpeedMps > 0f);
                Assert.True(left.LateralSpeedMps < 0f);
            }
        }

        // Higher CoG height increases lateral load transfer, reducing grip at the limit.
        // A vehicle with a van-like CoG (0.82 m) should exhibit more lateral speed
        // in the same cornering scenario than a sports-car CoG (0.40 m).
        [Fact]
        public void Solve_HigherCgHeight_IncreasesLateralResponse()
        {
            var lowCog  = BuildParameters(trackWidthM: 1.86f, cgHeightM: 0.40f);
            var highCog = BuildParameters(trackWidthM: 1.86f, cgHeightM: 0.82f);
            var input = new TireModelInput(elapsedSeconds: 1f / 60f, speedMps: 50f, steeringInput: 50, surfaceTractionMod: 1f, surfaceLateralMultiplier: 1f);
            var state = new TireModelState(lateralVelocityMps: 0f, yawRateRad: 0f);

            var lowCogResult  = TireModelSolver.Solve(lowCog,  input, state);
            var highCogResult = TireModelSolver.Solve(highCog, input, state);

            Assert.True(System.Math.Abs(highCogResult.LateralSpeedMps) > System.Math.Abs(lowCogResult.LateralSpeedMps),
                "Higher CoG must increase lateral response due to greater load transfer.");
        }

    }
}
