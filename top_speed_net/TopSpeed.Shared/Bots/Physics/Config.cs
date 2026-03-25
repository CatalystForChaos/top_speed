using System;
using TopSpeed.Physics.Powertrain;
using TopSpeed.Physics.Torque;
using TopSpeed.Vehicles;

namespace TopSpeed.Bots
{
    public sealed class BotPhysicsConfig
    {
        public BotPhysicsConfig(
            float surfaceTractionFactor,
            float deceleration,
            float topSpeedKph,
            float massKg,
            float drivetrainEfficiency,
            float engineBrakingTorqueNm,
            float tireGripCoefficient,
            float brakeStrength,
            float wheelRadiusM,
            float engineBraking,
            float idleRpm,
            float revLimiter,
            float finalDriveRatio,
            float powerFactor,
            float peakTorqueNm,
            float peakTorqueRpm,
            float idleTorqueNm,
            float redlineTorqueNm,
            float dragCoefficient,
            float frontalAreaM2,
            float rollingResistanceCoefficient,
            float launchRpm,
            float reversePowerFactor,
            float reverseGearRatio,
            float engineInertiaKgm2,
            float engineFrictionTorqueNm,
            float drivelineCouplingRate,
            float lateralGripCoefficient,
            float highSpeedStability,
            float wheelbaseM,
            float widthM,
            float lengthM,
            float maxSteerDeg,
            float steering,
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
            int gears,
            CurveProfile torqueCurve,
            float[]? gearRatios = null,
            TransmissionPolicy? transmissionPolicy = null,
            float shiftTransientSeconds = 0.15f,
            float cgHeightM = 0.55f)
        {
            SurfaceTractionFactor = Math.Max(0.01f, surfaceTractionFactor);
            Deceleration = Math.Max(0.01f, deceleration);
            TopSpeedKph = Math.Max(1f, topSpeedKph);
            MassKg = Math.Max(1f, massKg);
            DrivetrainEfficiency = Math.Max(0.1f, Math.Min(1.0f, drivetrainEfficiency));
            EngineBrakingTorqueNm = Math.Max(0f, engineBrakingTorqueNm);
            TireGripCoefficient = Math.Max(0.1f, tireGripCoefficient);
            BrakeStrength = Math.Max(0.1f, brakeStrength);
            WheelRadiusM = Math.Max(0.01f, wheelRadiusM);
            EngineBraking = Math.Max(0.05f, Math.Min(1.0f, engineBraking));
            IdleRpm = Math.Max(500f, idleRpm);
            RevLimiter = Math.Max(IdleRpm, revLimiter);
            FinalDriveRatio = Math.Max(0.1f, finalDriveRatio);
            PowerFactor = Math.Max(0.1f, powerFactor);
            PeakTorqueNm = Math.Max(0f, peakTorqueNm);
            PeakTorqueRpm = Math.Max(IdleRpm + 100f, peakTorqueRpm);
            IdleTorqueNm = Math.Max(0f, idleTorqueNm);
            RedlineTorqueNm = Math.Max(0f, redlineTorqueNm);
            DragCoefficient = Math.Max(0.01f, dragCoefficient);
            FrontalAreaM2 = Math.Max(0.1f, frontalAreaM2);
            RollingResistanceCoefficient = Math.Max(0.001f, rollingResistanceCoefficient);
            LaunchRpm = Math.Max(IdleRpm, Math.Min(RevLimiter, launchRpm));
            ReversePowerFactor = Math.Max(0.1f, reversePowerFactor);
            ReverseGearRatio = Math.Max(0.1f, reverseGearRatio);
            EngineInertiaKgm2 = Math.Max(0.01f, engineInertiaKgm2);
            EngineFrictionTorqueNm = Math.Max(0f, engineFrictionTorqueNm);
            DrivelineCouplingRate = Math.Max(0.1f, drivelineCouplingRate);
            LateralGripCoefficient = Math.Max(0.1f, lateralGripCoefficient);
            HighSpeedStability = Math.Max(0f, Math.Min(1.0f, highSpeedStability));
            WheelbaseM = Math.Max(0.5f, wheelbaseM);
            WidthM = Math.Max(0.5f, widthM);
            LengthM = Math.Max(0.5f, lengthM);
            MaxSteerDeg = Math.Max(5f, Math.Min(60f, maxSteerDeg));
            Steering = steering;
            HighSpeedSteerGain = Math.Max(0.7f, Math.Min(1.6f, highSpeedSteerGain));
            HighSpeedSteerStartKph = Math.Max(60f, Math.Min(260f, highSpeedSteerStartKph));
            HighSpeedSteerFullKph = Math.Max(100f, Math.Min(350f, highSpeedSteerFullKph));
            if (HighSpeedSteerFullKph <= HighSpeedSteerStartKph)
                HighSpeedSteerFullKph = HighSpeedSteerStartKph + 1f;
            CombinedGripPenalty = Math.Max(0f, Math.Min(1f, combinedGripPenalty));
            SlipAnglePeakDeg = Math.Max(0.5f, Math.Min(20f, slipAnglePeakDeg));
            SlipAngleFalloff = Math.Max(0.01f, Math.Min(5f, slipAngleFalloff));
            TurnResponse = Math.Max(0.2f, Math.Min(2.5f, turnResponse));
            MassSensitivity = Math.Max(0f, Math.Min(1f, massSensitivity));
            DownforceGripGain = Math.Max(0f, Math.Min(1f, downforceGripGain));
            CornerStiffnessFront = Math.Max(0.2f, Math.Min(3f, cornerStiffnessFront));
            CornerStiffnessRear = Math.Max(0.2f, Math.Min(3f, cornerStiffnessRear));
            YawInertiaScale = Math.Max(0.5f, Math.Min(2f, yawInertiaScale));
            SteeringCurve = Math.Max(0.5f, Math.Min(2f, steeringCurve));
            TransientDamping = Math.Max(0f, Math.Min(6f, transientDamping));
            Gears = Math.Max(1, gears);
            GearRatios = BuildRatios(Gears, gearRatios);
            TransmissionPolicy = transmissionPolicy ?? TransmissionPolicy.Default;
            TorqueCurve = torqueCurve ?? throw new ArgumentNullException(nameof(torqueCurve));
            ShiftTransientSeconds = Math.Max(0f, Math.Min(0.5f, shiftTransientSeconds));
            CgHeightM = cgHeightM > 0f ? cgHeightM : 0.55f;

            Powertrain = new Config(
                MassKg,
                DrivetrainEfficiency,
                EngineBrakingTorqueNm,
                TireGripCoefficient,
                BrakeStrength,
                WheelRadiusM,
                EngineBraking,
                IdleRpm,
                RevLimiter,
                FinalDriveRatio,
                PowerFactor,
                PeakTorqueNm,
                PeakTorqueRpm,
                IdleTorqueNm,
                RedlineTorqueNm,
                DragCoefficient,
                FrontalAreaM2,
                RollingResistanceCoefficient,
                LaunchRpm,
                ReversePowerFactor,
                ReverseGearRatio,
                EngineInertiaKgm2,
                EngineFrictionTorqueNm,
                DrivelineCouplingRate,
                Gears,
                GearRatios,
                TorqueCurve);
        }

        public float SurfaceTractionFactor { get; }
        public float Deceleration { get; }
        public float TopSpeedKph { get; }
        public float MassKg { get; }
        public float DrivetrainEfficiency { get; }
        public float EngineBrakingTorqueNm { get; }
        public float TireGripCoefficient { get; }
        public float BrakeStrength { get; }
        public float WheelRadiusM { get; }
        public float EngineBraking { get; }
        public float IdleRpm { get; }
        public float RevLimiter { get; }
        public float FinalDriveRatio { get; }
        public float PowerFactor { get; }
        public float PeakTorqueNm { get; }
        public float PeakTorqueRpm { get; }
        public float IdleTorqueNm { get; }
        public float RedlineTorqueNm { get; }
        public float DragCoefficient { get; }
        public float FrontalAreaM2 { get; }
        public float RollingResistanceCoefficient { get; }
        public float LaunchRpm { get; }
        public float ReversePowerFactor { get; }
        public float ReverseGearRatio { get; }
        public float EngineInertiaKgm2 { get; }
        public float EngineFrictionTorqueNm { get; }
        public float DrivelineCouplingRate { get; }
        public float LateralGripCoefficient { get; }
        public float HighSpeedStability { get; }
        public float WheelbaseM { get; }
        public float WidthM { get; }
        public float LengthM { get; }
        public float MaxSteerDeg { get; }
        public float Steering { get; }
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
        public int Gears { get; }
        public float[] GearRatios { get; }
        public CurveProfile TorqueCurve { get; }
        public TransmissionPolicy TransmissionPolicy { get; }
        public Config Powertrain { get; }
        public float ShiftTransientSeconds { get; }
        public float CgHeightM { get; }

        public float GetGearRatio(int gear)
        {
            var clamped = Math.Max(1, Math.Min(Gears, gear));
            return GearRatios[clamped - 1];
        }

        private static float[] BuildRatios(int gears, float[]? provided)
        {
            if (provided != null && provided.Length == gears)
                return provided;

            var ratios = new float[gears];
            const float first = 3.5f;
            const float last = 0.85f;
            var logFirst = Math.Log(first);
            var logLast = Math.Log(last);
            for (var i = 0; i < gears; i++)
            {
                var t = gears > 1 ? i / (float)(gears - 1) : 0f;
                ratios[i] = (float)Math.Exp(logFirst + ((logLast - logFirst) * t));
            }

            return ratios;
        }
    }
}

