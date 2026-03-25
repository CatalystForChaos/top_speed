namespace TopSpeed.Vehicles.Loader
{
    internal static partial class Spec
    {
        internal sealed class Common
        {
            public float SurfaceTractionFactor { get; set; }
            public float Deceleration { get; set; }
            public float TopSpeed { get; set; }
            public int IdleFreq { get; set; }
            public int TopFreq { get; set; }
            public int ShiftFreq { get; set; }
            public float PitchCurveExponent { get; set; } = 0.85f;
            public int Gears { get; set; }
            public float Steering { get; set; }
            public int HasWipers { get; set; }
            public float IdleRpm { get; set; }
            public float MaxRpm { get; set; }
            public float RevLimiter { get; set; }
            public float AutoShiftRpm { get; set; }
            public float EngineBraking { get; set; }
            public float MassKg { get; set; }
            public float DrivetrainEfficiency { get; set; }
            public float EngineBrakingTorqueNm { get; set; }
            public float TireGripCoefficient { get; set; }
            public float PeakTorqueNm { get; set; }
            public float PeakTorqueRpm { get; set; }
            public float IdleTorqueNm { get; set; }
            public float RedlineTorqueNm { get; set; }
            public float DragCoefficient { get; set; }
            public float FrontalAreaM2 { get; set; }
            public float RollingResistanceCoefficient { get; set; }
            public float LaunchRpm { get; set; }
            public float EngineInertiaKgm2 { get; set; }
            public float EngineFrictionTorqueNm { get; set; }
            public float DrivelineCouplingRate { get; set; }
            public float FinalDriveRatio { get; set; }
            public float ReverseMaxSpeedKph { get; set; }
            public float ReversePowerFactor { get; set; }
            public float ReverseGearRatio { get; set; }
            public float TireCircumferenceM { get; set; }
            public float LateralGripCoefficient { get; set; }
            public float HighSpeedStability { get; set; }
            public float WheelbaseM { get; set; }
            public float MaxSteerDeg { get; set; }
            public float HighSpeedSteerGain { get; set; }
            public float HighSpeedSteerStartKph { get; set; }
            public float HighSpeedSteerFullKph { get; set; }
            public float CombinedGripPenalty { get; set; }
            public float SlipAnglePeakDeg { get; set; }
            public float SlipAngleFalloff { get; set; }
            public float TurnResponse { get; set; }
            public float MassSensitivity { get; set; }
            public float DownforceGripGain { get; set; }
            public float CornerStiffnessFront { get; set; }
            public float CornerStiffnessRear { get; set; }
            public float YawInertiaScale { get; set; }
            public float SteeringCurve { get; set; }
            public float TransientDamping { get; set; }
            public float CgHeightM { get; set; } = 0.55f;
            public float WidthM { get; set; }
            public float LengthM { get; set; }
            public float PowerFactor { get; set; }
            public float[]? GearRatios { get; set; }
            public float[]? TorqueCurveRpm { get; set; }
            public float[]? TorqueCurveTorqueNm { get; set; }
            public string? TorqueCurvePreset { get; set; }
            public float BrakeStrength { get; set; }
            public TransmissionPolicy TransmissionPolicy { get; set; } = TransmissionPolicy.Default;
        }
    }
}
