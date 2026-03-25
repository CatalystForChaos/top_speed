using TopSpeed.Protocol;

namespace TopSpeed.Vehicles
{
    public sealed class OfficialVehicleSpec
    {
        public OfficialVehicleSpec(
            CarType carType,
            string name,
            int hasWipers,
            float surfaceTractionFactor,
            float deceleration,
            float topSpeed,
            int idleFreq,
            int topFreq,
            int shiftFreq,
            int gears,
            float steering,
            float idleRpm,
            float maxRpm,
            float revLimiter,
            float autoShiftRpm,
            float engineBraking,
            float massKg,
            float drivetrainEfficiency,
            float engineBrakingTorqueNm,
            float tireGripCoefficient,
            float peakTorqueNm,
            float peakTorqueRpm,
            float idleTorqueNm,
            float redlineTorqueNm,
            float dragCoefficient,
            float frontalAreaM2,
            float rollingResistanceCoefficient,
            float launchRpm,
            float engineInertiaKgm2,
            float engineFrictionTorqueNm,
            float drivelineCouplingRate,
            float finalDriveRatio,
            float reverseMaxSpeedKph,
            float reversePowerFactor,
            float reverseGearRatio,
            float tireCircumferenceM,
            float lateralGripCoefficient,
            float highSpeedStability,
            float wheelbaseM,
            float maxSteerDeg,
            float widthM,
            float lengthM,
            float powerFactor,
            float[] gearRatios,
            float brakeStrength,
            float highSpeedSteerGain = 1.08f,
            float highSpeedSteerStartKph = 140f,
            float highSpeedSteerFullKph = 240f,
            float combinedGripPenalty = 0.72f,
            float slipAnglePeakDeg = 8f,
            float slipAngleFalloff = 1.25f,
            float turnResponse = 1.0f,
            float massSensitivity = 0.75f,
            float downforceGripGain = 0.05f,
            float cornerStiffnessFront = 1.0f,
            float cornerStiffnessRear = 1.0f,
            float yawInertiaScale = 1.0f,
            float steeringCurve = 1.0f,
            float transientDamping = 1.0f,
            float[]? torqueCurveRpm = null,
            float[]? torqueCurveTorqueNm = null,
            string? torqueCurvePreset = null,
            TransmissionPolicy? transmissionPolicy = null,
            float cgHeightM = 0.55f)
        {
            CarType = carType;
            Name = name;
            HasWipers = hasWipers;
            SurfaceTractionFactor = surfaceTractionFactor;
            Deceleration = deceleration;
            TopSpeed = topSpeed;
            IdleFreq = idleFreq;
            TopFreq = topFreq;
            ShiftFreq = shiftFreq;
            Gears = gears;
            Steering = steering;
            IdleRpm = idleRpm;
            MaxRpm = maxRpm;
            RevLimiter = revLimiter;
            AutoShiftRpm = autoShiftRpm;
            EngineBraking = engineBraking;
            MassKg = massKg;
            DrivetrainEfficiency = drivetrainEfficiency;
            EngineBrakingTorqueNm = engineBrakingTorqueNm;
            TireGripCoefficient = tireGripCoefficient;
            PeakTorqueNm = peakTorqueNm;
            PeakTorqueRpm = peakTorqueRpm;
            IdleTorqueNm = idleTorqueNm;
            RedlineTorqueNm = redlineTorqueNm;
            DragCoefficient = dragCoefficient;
            FrontalAreaM2 = frontalAreaM2;
            RollingResistanceCoefficient = rollingResistanceCoefficient;
            LaunchRpm = launchRpm;
            EngineInertiaKgm2 = engineInertiaKgm2;
            EngineFrictionTorqueNm = engineFrictionTorqueNm;
            DrivelineCouplingRate = drivelineCouplingRate;
            FinalDriveRatio = finalDriveRatio;
            ReverseMaxSpeedKph = reverseMaxSpeedKph;
            ReversePowerFactor = reversePowerFactor;
            ReverseGearRatio = reverseGearRatio;
            TireCircumferenceM = tireCircumferenceM;
            LateralGripCoefficient = lateralGripCoefficient;
            HighSpeedStability = highSpeedStability;
            WheelbaseM = wheelbaseM;
            MaxSteerDeg = maxSteerDeg;
            HighSpeedSteerGain = highSpeedSteerGain;
            HighSpeedSteerStartKph = highSpeedSteerStartKph;
            HighSpeedSteerFullKph = highSpeedSteerFullKph;
            CombinedGripPenalty = combinedGripPenalty;
            SlipAnglePeakDeg = slipAnglePeakDeg;
            SlipAngleFalloff = slipAngleFalloff;
            TurnResponse = turnResponse;
            MassSensitivity = massSensitivity;
            DownforceGripGain = downforceGripGain;
            CornerStiffnessFront = cornerStiffnessFront;
            CornerStiffnessRear = cornerStiffnessRear;
            YawInertiaScale = yawInertiaScale;
            SteeringCurve = steeringCurve;
            TransientDamping = transientDamping;
            WidthM = widthM;
            LengthM = lengthM;
            PowerFactor = powerFactor;
            GearRatios = gearRatios;
            BrakeStrength = brakeStrength;
            TorqueCurveRpm = torqueCurveRpm;
            TorqueCurveTorqueNm = torqueCurveTorqueNm;
            TorqueCurvePreset = torqueCurvePreset;
            TransmissionPolicy = transmissionPolicy ?? TransmissionPolicy.Default;
            CgHeightM = cgHeightM > 0f ? cgHeightM : 0.55f;
        }

        public CarType CarType { get; }
        public string Name { get; }
        public int HasWipers { get; }
        public float SurfaceTractionFactor { get; }
        public float Deceleration { get; }
        public float TopSpeed { get; }
        public int IdleFreq { get; }
        public int TopFreq { get; }
        public int ShiftFreq { get; }
        public int Gears { get; }
        public float Steering { get; }
        public float IdleRpm { get; }
        public float MaxRpm { get; }
        public float RevLimiter { get; }
        public float AutoShiftRpm { get; }
        public float EngineBraking { get; }
        public float MassKg { get; }
        public float DrivetrainEfficiency { get; }
        public float EngineBrakingTorqueNm { get; }
        public float TireGripCoefficient { get; }
        public float PeakTorqueNm { get; }
        public float PeakTorqueRpm { get; }
        public float IdleTorqueNm { get; }
        public float RedlineTorqueNm { get; }
        public float DragCoefficient { get; }
        public float FrontalAreaM2 { get; }
        public float RollingResistanceCoefficient { get; }
        public float LaunchRpm { get; }
        public float EngineInertiaKgm2 { get; }
        public float EngineFrictionTorqueNm { get; }
        public float DrivelineCouplingRate { get; }
        public float FinalDriveRatio { get; }
        public float ReverseMaxSpeedKph { get; }
        public float ReversePowerFactor { get; }
        public float ReverseGearRatio { get; }
        public float TireCircumferenceM { get; }
        public float LateralGripCoefficient { get; }
        public float HighSpeedStability { get; }
        public float WheelbaseM { get; }
        public float MaxSteerDeg { get; }
        public float HighSpeedSteerGain { get; }
        public float HighSpeedSteerStartKph { get; }
        public float HighSpeedSteerFullKph { get; }
        public float CombinedGripPenalty { get; }
        public float SlipAnglePeakDeg { get; }
        public float SlipAngleFalloff { get; }
        public float TurnResponse { get; }
        public float MassSensitivity { get; }
        public float DownforceGripGain { get; }
        public float CornerStiffnessFront { get; }
        public float CornerStiffnessRear { get; }
        public float YawInertiaScale { get; }
        public float SteeringCurve { get; }
        public float TransientDamping { get; }
        public float WidthM { get; }
        public float LengthM { get; }
        public float PowerFactor { get; }
        public float[] GearRatios { get; }
        public float BrakeStrength { get; }
        public float[]? TorqueCurveRpm { get; }
        public float[]? TorqueCurveTorqueNm { get; }
        public string? TorqueCurvePreset { get; }
        public TransmissionPolicy TransmissionPolicy { get; }
        public float CgHeightM { get; }
    }
}
