namespace TopSpeed.Vehicles.Loader
{
    internal static partial class Spec
    {
        public static void Apply(VehicleDefinition def, Common spec)
        {
            def.SurfaceTractionFactor = spec.SurfaceTractionFactor;
            def.Deceleration = spec.Deceleration;
            def.TopSpeed = spec.TopSpeed;
            def.IdleFreq = spec.IdleFreq;
            def.TopFreq = spec.TopFreq;
            def.ShiftFreq = spec.ShiftFreq;
            def.PitchCurveExponent = spec.PitchCurveExponent;
            def.Gears = spec.Gears;
            def.Steering = spec.Steering;
            def.HasWipers = spec.HasWipers;
            def.IdleRpm = spec.IdleRpm;
            def.MaxRpm = spec.MaxRpm;
            def.RevLimiter = spec.RevLimiter;
            def.AutoShiftRpm = spec.AutoShiftRpm;
            def.EngineBraking = spec.EngineBraking;
            def.MassKg = spec.MassKg;
            def.DrivetrainEfficiency = spec.DrivetrainEfficiency;
            def.EngineBrakingTorqueNm = spec.EngineBrakingTorqueNm;
            def.TireGripCoefficient = spec.TireGripCoefficient;
            def.PeakTorqueNm = spec.PeakTorqueNm;
            def.PeakTorqueRpm = spec.PeakTorqueRpm;
            def.IdleTorqueNm = spec.IdleTorqueNm;
            def.RedlineTorqueNm = spec.RedlineTorqueNm;
            def.DragCoefficient = spec.DragCoefficient;
            def.FrontalAreaM2 = spec.FrontalAreaM2;
            def.RollingResistanceCoefficient = spec.RollingResistanceCoefficient;
            def.LaunchRpm = spec.LaunchRpm;
            def.EngineInertiaKgm2 = spec.EngineInertiaKgm2;
            def.EngineFrictionTorqueNm = spec.EngineFrictionTorqueNm;
            def.DrivelineCouplingRate = spec.DrivelineCouplingRate;
            def.FinalDriveRatio = spec.FinalDriveRatio;
            def.ReverseMaxSpeedKph = spec.ReverseMaxSpeedKph;
            def.ReversePowerFactor = spec.ReversePowerFactor;
            def.ReverseGearRatio = spec.ReverseGearRatio;
            def.TireCircumferenceM = spec.TireCircumferenceM;
            def.LateralGripCoefficient = spec.LateralGripCoefficient;
            def.HighSpeedStability = spec.HighSpeedStability;
            def.WheelbaseM = spec.WheelbaseM;
            def.MaxSteerDeg = spec.MaxSteerDeg;
            def.HighSpeedSteerGain = spec.HighSpeedSteerGain;
            def.HighSpeedSteerStartKph = spec.HighSpeedSteerStartKph;
            def.HighSpeedSteerFullKph = spec.HighSpeedSteerFullKph;
            def.CombinedGripPenalty = spec.CombinedGripPenalty;
            def.SlipAnglePeakDeg = spec.SlipAnglePeakDeg;
            def.SlipAngleFalloff = spec.SlipAngleFalloff;
            def.TurnResponse = spec.TurnResponse;
            def.MassSensitivity = spec.MassSensitivity;
            def.DownforceGripGain = spec.DownforceGripGain;
            def.CornerStiffnessFront = spec.CornerStiffnessFront;
            def.CornerStiffnessRear = spec.CornerStiffnessRear;
            def.YawInertiaScale = spec.YawInertiaScale;
            def.SteeringCurve = spec.SteeringCurve;
            def.TransientDamping = spec.TransientDamping;
            def.CgHeightM = spec.CgHeightM;
            def.WidthM = spec.WidthM;
            def.LengthM = spec.LengthM;
            def.PowerFactor = spec.PowerFactor;
            def.GearRatios = spec.GearRatios;
            def.TorqueCurveRpm = spec.TorqueCurveRpm;
            def.TorqueCurveTorqueNm = spec.TorqueCurveTorqueNm;
            def.TorqueCurvePreset = spec.TorqueCurvePreset;
            def.BrakeStrength = spec.BrakeStrength;
            def.TransmissionPolicy = spec.TransmissionPolicy;
        }
    }
}
