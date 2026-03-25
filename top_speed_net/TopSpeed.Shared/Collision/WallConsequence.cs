using System;

namespace TopSpeed.Collision
{
    public static class CollisionWallConsequence
    {
        private const float Epsilon = 0.0001f;
        private const float MinCrashPenaltyKph = 25f;

        public static VehicleCollisionImpulse Apply(
            in VehicleCollisionBody body,
            in VehicleCollisionImpulse impulse,
            in VehicleCollisionResponse response,
            float wallLeftM,
            float wallRightM)
        {
            if (wallRightM <= wallLeftM + Epsilon)
                return impulse;

            var predictedX = body.PositionX + (2f * impulse.BumpX);
            if (predictedX >= wallLeftM && predictedX <= wallRightM)
                return impulse;

            var targetX = predictedX > wallRightM ? wallRightM : wallLeftM;
            var overflow = Math.Abs(predictedX - targetX);
            var wallSpan = Math.Max(Epsilon, wallRightM - wallLeftM);
            var wallDepth = Clamp01(overflow / Math.Max(0.25f, Math.Min(body.WidthM * 0.5f, wallSpan * 0.5f)));
            // Kinetic energy ∝ v² — wall impact at double speed is four times as destructive.
            // Quadratic scaling means low-speed scrapes are far less severe than high-speed impacts.
            var speedRatio = response.RelativeSpeedKph / 100f;
            var speedSeverity = Clamp01(speedRatio * speedRatio);
            var severity = Clamp01(speedSeverity + (0.35f * wallDepth));
            var crashLike = severity >= 0.70f && response.RelativeSpeedKph >= 70f;

            var constrainedBumpX = (targetX - body.PositionX) * 0.5f;
            var speedPenalty = crashLike
                ? Math.Max(MinCrashPenaltyKph, body.SpeedKph * (0.45f + (0.35f * severity)))
                : (4f + (10f * severity) + (8f * wallDepth));
            var adjustedSpeedDelta = impulse.SpeedDeltaKph - speedPenalty;
            if (adjustedSpeedDelta < -body.SpeedKph)
                adjustedSpeedDelta = -body.SpeedKph;

            return new VehicleCollisionImpulse(constrainedBumpX, impulse.BumpY, adjustedSpeedDelta);
        }

        public static VehicleCollisionImpulse Apply(
            in VehicleCollisionBody body,
            in VehicleCollisionImpulse impulse,
            in VehicleCollisionResponse response,
            float wallHalfWidthM = 5f)
        {
            if (wallHalfWidthM <= 0f)
                return impulse;
            return Apply(body, impulse, response, -wallHalfWidthM, wallHalfWidthM);
        }

        private static float Clamp01(float value)
        {
            if (value <= Epsilon)
                return 0f;
            if (value >= 1f)
                return 1f;
            return value;
        }
    }
}
