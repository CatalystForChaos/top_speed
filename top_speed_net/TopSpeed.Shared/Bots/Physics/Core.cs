using System;
using TopSpeed.Physics.Powertrain;
using TopSpeed.Physics.Surface;
using TopSpeed.Physics.Tires;

namespace TopSpeed.Bots
{
    public static partial class BotPhysics
    {
        public static void Step(BotPhysicsConfig config, ref BotPhysicsState state, in BotPhysicsInput input)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (input.ElapsedSeconds <= 0f)
                return;

            if (state.Gear < 1 || state.Gear > config.Gears)
                state.Gear = 1;

            // Count down the gear-shift torque interruption before anything else.
            if (state.ShiftTransientSeconds > 0f)
                state.ShiftTransientSeconds = Math.Max(0f, state.ShiftTransientSeconds - input.ElapsedSeconds);
            var shiftTransientActive = state.ShiftTransientSeconds > 0f;

            var surface = SurfaceModel.Resolve(input.Surface, config.SurfaceTractionFactor, config.Deceleration);
            var surfaceTraction = surface.Traction;
            var surfaceDecel = surface.Deceleration;

            var thrust = 0f;
            if (input.Throttle == 0)
                thrust = input.Brake;
            else if (input.Brake == 0 || -input.Brake <= input.Throttle)
                thrust = input.Throttle;
            else
                thrust = input.Brake;

            var speedKph = Math.Max(0f, state.SpeedKph);
            var speedMpsCurrent = speedKph / 3.6f;
            var throttle = Math.Max(0f, Math.Min(100f, input.Throttle)) / 100f;
            var steeringInput = input.Steering;
            var surfaceTractionMod = surfaceTraction / config.SurfaceTractionFactor;
            var longitudinalGripFactor = 1.0f;
            var speedDiffKph = 0f;
            var tireState = new TireModelState(state.LateralVelocityMps, state.YawRateRad);
            // Brake fraction for friction-circle steering reduction (computed here so it
            // is available after the thrust branch regardless of which path was taken).
            var brakeFraction = thrust <= 10f
                ? Math.Max(0f, Math.Min(100f, -input.Brake)) / 100f
                : 0f;

            if (thrust > 10f && !shiftTransientActive)
            {
                var tireOutput = SolveTireModel(config, input.ElapsedSeconds, speedMpsCurrent, steeringInput, surfaceTractionMod, 1f, tireState);
                longitudinalGripFactor = tireOutput.LongitudinalGripFactor;
                // For real launch behavior, use the same drive acceleration path as the player model.
                var accelMps2 = Calculator.DriveAccel(
                    config.Powertrain,
                    state.Gear,
                    speedMpsCurrent,
                    throttle,
                    surfaceTractionMod,
                    longitudinalGripFactor);
                var newSpeedMps = speedMpsCurrent + (accelMps2 * input.ElapsedSeconds);
                if (newSpeedMps < 0f)
                    newSpeedMps = 0f;
                speedDiffKph = (newSpeedMps - speedMpsCurrent) * 3.6f;
            }
            else
            {
                var surfaceDecelMod = surfaceDecel / config.Deceleration;
                var brakeDecel = CalculateBrakeDecel(config, brakeFraction, surfaceDecelMod);
                // Engine braking is zero during a shift transient (clutch disengaged).
                var engineBrakeDecel = shiftTransientActive
                    ? 0f
                    : CalculateEngineBrakingDecel(config, state.Gear, speedMpsCurrent, surfaceDecelMod);
                var totalDecel = thrust < -10f ? (brakeDecel + engineBrakeDecel) : engineBrakeDecel;
                speedDiffKph = -totalDecel * input.ElapsedSeconds;
            }

            speedKph += speedDiffKph;
            var safetySpeed = ResolveForwardSafetySpeedKph(config.TopSpeedKph);
            if (speedKph > safetySpeed)
                speedKph = safetySpeed;
            if (speedKph < 0f)
                speedKph = 0f;

            UpdateAutomaticGear(config, ref state, input.ElapsedSeconds, speedKph / 3.6f, throttle, surfaceTractionMod, longitudinalGripFactor);
            if (brakeFraction > 0f && speedKph > 0f)
            {
                // Friction circle: braking consumes longitudinal grip; the remaining lateral
                // grip budget = sqrt(1 - brakeFraction²). Clamped to 0.3 to preserve
                // minimum steering authority even under maximum braking.
                var lateralGripAvailable = (float)Math.Sqrt(Math.Max(0f, 1f - (brakeFraction * brakeFraction)));
                steeringInput = steeringInput * Math.Max(0.3f, lateralGripAvailable);
            }

            var speedMps = speedKph / 3.6f;
            state.PositionY += speedMps * input.ElapsedSeconds;
            state.SpeedKph = speedKph;

            var surfaceTractionModLat = surfaceTraction / config.SurfaceTractionFactor;
            var lateralOutput = SolveTireModel(config, input.ElapsedSeconds, speedMps, steeringInput, surfaceTractionModLat, surface.LateralSpeedMultiplier, tireState);
            state.PositionX += lateralOutput.LateralSpeedMps * input.ElapsedSeconds;
            state.LateralVelocityMps = lateralOutput.State.LateralVelocityMps;
            state.YawRateRad = lateralOutput.State.YawRateRad;
        }
    }
}
