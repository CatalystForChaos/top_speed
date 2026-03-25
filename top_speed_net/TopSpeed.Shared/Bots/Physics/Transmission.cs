using System;
using TopSpeed.Physics.Powertrain;

namespace TopSpeed.Bots
{
    public static partial class BotPhysics
    {
        private static void UpdateAutomaticGear(
            BotPhysicsConfig config,
            ref BotPhysicsState state,
            float elapsed,
            float speedMps,
            float throttle,
            float surfaceTractionMod,
            float longitudinalGripFactor)
        {
            if (config.Gears <= 1)
                return;

            if (state.AutoShiftCooldownSeconds > 0f)
            {
                state.AutoShiftCooldownSeconds -= elapsed;
                return;
            }

            var currentAccel = ComputeNetAccelForGear(config, state.Gear, speedMps, throttle, surfaceTractionMod, longitudinalGripFactor);
            var currentRpm = SpeedToRpm(config, speedMps, state.Gear);
            var upAccel = state.Gear < config.Gears
                ? ComputeNetAccelForGear(config, state.Gear + 1, speedMps, throttle, surfaceTractionMod, longitudinalGripFactor)
                : float.NegativeInfinity;
            var downAccel = state.Gear > 1
                ? ComputeNetAccelForGear(config, state.Gear - 1, speedMps, throttle, surfaceTractionMod, longitudinalGripFactor)
                : float.NegativeInfinity;

            var decision = AutomaticTransmissionLogic.Decide(
                new AutomaticShiftInput(
                    state.Gear,
                    config.Gears,
                    speedMps,
                    config.TopSpeedKph / 3.6f,
                    config.IdleRpm,
                    config.RevLimiter,
                    currentRpm,
                    currentAccel,
                    upAccel,
                    downAccel),
                config.TransmissionPolicy);

            if (decision.Changed)
            {
                state.Gear = decision.NewGear;
                state.AutoShiftCooldownSeconds = decision.CooldownSeconds;
                // Simulate drivetrain torque interruption during the gear change.
                state.ShiftTransientSeconds = config.ShiftTransientSeconds;
            }
        }

        private static float ComputeNetAccelForGear(
            BotPhysicsConfig config,
            int gear,
            float speedMps,
            float throttle,
            float surfaceTractionMod,
            float longitudinalGripFactor)
        {
            var rpm = SpeedToRpm(config, speedMps, gear);
            if (rpm <= 0f)
                return float.NegativeInfinity;
            if (rpm > config.RevLimiter && gear < config.Gears)
                return float.NegativeInfinity;

            return Calculator.DriveAccel(
                config.Powertrain,
                gear,
                speedMps,
                throttle,
                surfaceTractionMod,
                longitudinalGripFactor);
        }

        private static float SpeedToRpm(BotPhysicsConfig config, float speedMps, int gear)
        {
            return Calculator.RpmAtSpeed(config.Powertrain, speedMps, gear);
        }
    }
}


