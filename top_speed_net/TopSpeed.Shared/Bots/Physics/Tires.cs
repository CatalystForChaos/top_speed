using TopSpeed.Physics.Tires;

namespace TopSpeed.Bots
{
    public static partial class BotPhysics
    {
        private static TireModelParameters BuildTireModelParameters(BotPhysicsConfig config)
        {
            return new TireModelParameters(
                config.Steering,
                config.MaxSteerDeg,
                config.WheelbaseM,
                config.WidthM,
                config.LengthM,
                config.TireGripCoefficient,
                config.LateralGripCoefficient,
                config.HighSpeedStability,
                config.MassKg,
                config.HighSpeedSteerGain,
                config.HighSpeedSteerStartKph,
                config.HighSpeedSteerFullKph,
                config.CombinedGripPenalty,
                config.SlipAnglePeakDeg,
                config.SlipAngleFalloff,
                config.TurnResponse,
                config.MassSensitivity,
                config.DownforceGripGain,
                config.CornerStiffnessFront,
                config.CornerStiffnessRear,
                config.YawInertiaScale,
                config.SteeringCurve,
                config.TransientDamping,
                config.CgHeightM);
        }

        private static TireModelOutput SolveTireModel(BotPhysicsConfig config, float elapsed, float speedMps, int steeringInput, float surfaceTractionMod, float lateralMultiplier, in TireModelState state)
        {
            var parameters = BuildTireModelParameters(config);
            var input = new TireModelInput(elapsed, speedMps, steeringInput, surfaceTractionMod, lateralMultiplier);
            return TireModelSolver.Solve(parameters, input, state);
        }
    }
}
