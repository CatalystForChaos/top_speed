using System;

namespace TopSpeed.Physics.Powertrain
{
    public static class Calculator
    {
        private const float AirDensityKgPerM3 = 1.225f;
        private const float Gravity = 9.80665f;
        private const float TwoPi = (float)(Math.PI * 2.0);

        public static float DriveRpm(
            Config config,
            int gear,
            float speedMps,
            float throttle,
            bool inReverse)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var wheelCircumference = config.WheelRadiusM * TwoPi;
            var ratio = inReverse ? config.ReverseGearRatio : config.GetGearRatio(gear);
            var speedBasedRpm = wheelCircumference > 0f
                ? (speedMps / wheelCircumference) * 60f * ratio * config.FinalDriveRatio
                : 0f;
            var launchTarget = config.IdleRpm + (Clamp(throttle, 0f, 1f) * (config.LaunchRpm - config.IdleRpm));
            var rpm = Math.Max(speedBasedRpm, launchTarget);
            return Clamp(rpm, config.IdleRpm, config.RevLimiter);
        }

        public static float RpmAtSpeed(Config config, float speedMps, int gear)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var wheelCircumference = config.WheelRadiusM * TwoPi;
            if (wheelCircumference <= 0f)
                return 0f;
            var gearRatio = config.GetGearRatio(gear);
            return (speedMps / wheelCircumference) * 60f * gearRatio * config.FinalDriveRatio;
        }

        public static float EngineTorque(Config config, float rpm)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            return Math.Max(0f, config.TorqueCurve.EvaluateTorque(Clamp(rpm, config.IdleRpm, config.RevLimiter)));
        }

        public static float Horsepower(float torqueNm, float rpm)
        {
            if (rpm <= 0f || torqueNm <= 0f)
                return 0f;
            // SAE horsepower: P[hp] = T[N·m] × n[rpm] × 2π / (60 × 745.69987 W/hp) ≈ T × n / 7120.8
            return (torqueNm * rpm) / 7120.8f;
        }

        public static float DriveAccel(
            Config config,
            int gear,
            float speedMps,
            float throttle,
            float surfaceTractionModifier,
            float longitudinalGripFactor)
        {
            return DriveAccelCore(
                config,
                gear,
                inReverse: false,
                speedMps,
                throttle,
                surfaceTractionModifier,
                longitudinalGripFactor);
        }

        public static float ReverseAccel(
            Config config,
            float speedMps,
            float throttle,
            float surfaceTractionModifier,
            float longitudinalGripFactor)
        {
            return DriveAccelCore(
                config,
                1,
                inReverse: true,
                speedMps,
                throttle,
                surfaceTractionModifier,
                longitudinalGripFactor);
        }

        public static float BrakeDecelKph(
            Config config,
            float brakeInput,
            float surfaceDecelerationModifier)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (brakeInput <= 0f)
                return 0f;

            var grip = Math.Max(0.1f, config.TireGripCoefficient * surfaceDecelerationModifier);
            var decelMps2 = Clamp(brakeInput, 0f, 1f) * config.BrakeStrength * grip * Gravity;
            return decelMps2 * 3.6f;
        }

        public static float EngineBrakeDecelKph(
            Config config,
            int gear,
            bool inReverse,
            float speedMps,
            float surfaceDecelerationModifier,
            float currentEngineRpm)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.EngineBrakingTorqueNm <= 0f || config.MassKg <= 0f || config.WheelRadiusM <= 0f)
                return 0f;

            var rpmRange = config.RevLimiter - config.IdleRpm;
            if (rpmRange <= 0f)
                return 0f;

            var ratio = inReverse ? config.ReverseGearRatio : config.GetGearRatio(gear);
            var speedBasedRpm = RpmForRatio(config, speedMps, ratio);
            var effectiveRpm = Math.Max(currentEngineRpm, speedBasedRpm);
            var rpmFactor = (effectiveRpm - config.IdleRpm) / rpmRange;
            if (rpmFactor <= 0f)
                return 0f;
            rpmFactor = Clamp(rpmFactor, 0f, 1f);

            var drivelineTorque = config.EngineBrakingTorqueNm * config.EngineBraking * rpmFactor;
            var wheelTorque = drivelineTorque * ratio * config.FinalDriveRatio * config.DrivetrainEfficiency;
            var wheelForce = wheelTorque / config.WheelRadiusM;
            var decelMps2 = (wheelForce / config.MassKg) * surfaceDecelerationModifier;
            return Math.Max(0f, decelMps2 * 3.6f);
        }

        public static float ResistiveForce(Config config, float speedMps)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var dragForce = 0.5f * AirDensityKgPerM3 * config.DragCoefficient * config.FrontalAreaM2 * speedMps * speedMps;
            // Rolling resistance increases with speed (ISO 18164 approximation):
            // Crr_eff = Crr0 * (1 + 0.0095 * v[m/s])
            var rollingSpeedFactor = 1f + (0.0095f * Math.Abs(speedMps));
            var rollingForce = config.RollingResistanceCoefficient * config.MassKg * Gravity * rollingSpeedFactor;
            return dragForce + rollingForce;
        }

        private static float DriveAccelCore(
            Config config,
            int gear,
            bool inReverse,
            float speedMps,
            float throttle,
            float surfaceTractionModifier,
            float longitudinalGripFactor)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var clampedThrottle = Clamp(throttle, 0f, 1f);
            if (clampedThrottle <= 0f)
                return 0f;

            var driveRpm = DriveRpm(config, gear, speedMps, clampedThrottle, inReverse);
            // Subtract internal engine friction before torque reaches the drivetrain.
            var rawTorque = EngineTorque(config, driveRpm) * clampedThrottle * config.PowerFactor;
            var engineTorque = Math.Max(0f, rawTorque - config.EngineFrictionTorqueNm);
            var ratio = inReverse ? config.ReverseGearRatio : config.GetGearRatio(gear);
            var wheelTorque = engineTorque * ratio * config.FinalDriveRatio * config.DrivetrainEfficiency;
            var wheelForce = wheelTorque / config.WheelRadiusM;
            var tractionLimit = config.TireGripCoefficient * surfaceTractionModifier * config.MassKg * Gravity;
            if (wheelForce > tractionLimit)
                wheelForce = tractionLimit;
            wheelForce *= Clamp(longitudinalGripFactor, 0f, 1f);
            if (inReverse)
                wheelForce *= config.ReversePowerFactor;

            var netForce = wheelForce - ResistiveForce(config, speedMps);
            return netForce / config.MassKg;
        }

        private static float RpmForRatio(Config config, float speedMps, float ratio)
        {
            var wheelCircumference = config.WheelRadiusM * TwoPi;
            if (wheelCircumference <= 0f || ratio <= 0f)
                return 0f;
            return (speedMps / wheelCircumference) * 60f * ratio * config.FinalDriveRatio;
        }

        private static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
