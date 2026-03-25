namespace TopSpeed.Physics.Tires
{
    public readonly struct TireModelParameters
    {
        public TireModelParameters(
            float steeringResponse,
            float maxSteerDeg,
            float wheelbaseM,
            float trackWidthM,
            float vehicleLengthM,
            float tireGripCoefficient,
            float lateralGripCoefficient,
            float highSpeedStability,
            float massKg,
            float highSpeedSteerGain,
            float highSpeedSteerStartKph,
            float highSpeedSteerFullKph,
            float combinedGripPenalty,
            float slipAnglePeakDeg,
            float slipAngleFalloff,
            float turnResponse,
            float massSensitivity,
            float downforceGripGain,
            float cornerStiffnessFront,
            float cornerStiffnessRear,
            float yawInertiaScale,
            float steeringCurve,
            float transientDamping,
            float cgHeightM = 0.55f)
        {
            SteeringResponse = steeringResponse;
            MaxSteerDeg = maxSteerDeg;
            WheelbaseM = wheelbaseM;
            TrackWidthM = trackWidthM;
            VehicleLengthM = vehicleLengthM;
            TireGripCoefficient = tireGripCoefficient;
            LateralGripCoefficient = lateralGripCoefficient;
            HighSpeedStability = highSpeedStability;
            MassKg = massKg;
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
            CgHeightM = cgHeightM > 0f ? cgHeightM : 0.55f;
        }

        public float SteeringResponse { get; }
        public float MaxSteerDeg { get; }
        public float WheelbaseM { get; }
        public float TrackWidthM { get; }
        public float VehicleLengthM { get; }
        public float TireGripCoefficient { get; }
        public float LateralGripCoefficient { get; }
        public float HighSpeedStability { get; }
        public float MassKg { get; }
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
        /// <summary>Centre-of-gravity height in metres. Used for lateral load transfer calculation.</summary>
        public float CgHeightM { get; }
    }
}
