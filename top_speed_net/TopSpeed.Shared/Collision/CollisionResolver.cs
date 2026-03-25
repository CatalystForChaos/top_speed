using System;

namespace TopSpeed.Collision
{
    public static class VehicleCollisionResolver
    {
        private const float Epsilon = 0.0001f;
        private const float MaxTransferKph = 65f;

        public static bool TryResolve(
            in VehicleCollisionBody first,
            in VehicleCollisionBody second,
            out VehicleCollisionResponse response)
        {
            response = default;

            var halfWidthSum = (first.WidthM * 0.5f) + (second.WidthM * 0.5f);
            var halfLengthSum = (first.LengthM * 0.5f) + (second.LengthM * 0.5f);

            var dx = first.PositionX - second.PositionX;
            var dy = first.PositionY - second.PositionY;
            var absDx = Math.Abs(dx);
            var absDy = Math.Abs(dy);
            if (absDx >= halfWidthSum || absDy >= halfLengthSum)
                return false;

            var xOverlap = halfWidthSum - absDx;
            var yOverlap = halfLengthSum - absDy;
            if (xOverlap <= 0f || yOverlap <= 0f)
                return false;

            // Reduced mass μ = m1*m2/(m1+m2) ensures impulse-based momentum conservation:
            // J = μ × Δv_rel × restitutionFactor
            // Δv1 = J/m1 = (m2/(m1+m2)) × Δv_rel × factor  (heavier vehicle changes less)
            // Δv2 = J/m2 = (m1/(m1+m2)) × Δv_rel × factor
            var totalMass = first.MassKg + second.MassKg;
            var firstMassEffect = second.MassKg / totalMass;
            var secondMassEffect = first.MassKg / totalMass;
            var reducedMass = (first.MassKg * second.MassKg) / totalMass;

            var speedDiff = first.SpeedKph - second.SpeedKph;
            var firstRearClosing = dy < 0f && speedDiff > 0f;
            var secondRearClosing = dy > 0f && speedDiff < 0f;
            var sideSign = ResolveSign(dx, speedDiff);
            var longitudinalSign = ResolveSign(dy, speedDiff);
            var longitudinalContact = (yOverlap <= xOverlap) || firstRearClosing || secondRearClosing;
            var closingSpeed = firstRearClosing ? speedDiff : (secondRearClosing ? -speedDiff : 0f);
            var relativeSpeed = Math.Abs(speedDiff);

            var severity = Clamp01(closingSpeed / 70f);
            var exchangeFactor = longitudinalContact ? 0.78f : 0.32f;
            // Impulse magnitude (unit: kph·kg) — capped via reduced mass so per-vehicle Δv
            // stays within MaxTransferKph for any mass ratio.
            var restitutionFactor = exchangeFactor * (0.40f + (0.60f * severity));
            var impulse = reducedMass * closingSpeed * restitutionFactor;
            var maxImpulse = reducedMass * MaxTransferKph;
            if (impulse > maxImpulse)
                impulse = maxImpulse;

            var lateralBase = xOverlap * (longitudinalContact ? 0.55f : 0.80f);
            var lateralImpact = (closingSpeed / 120f) * (longitudinalContact ? 0.55f : 0.35f);
            var lateralMagnitude = Math.Max(0.02f, lateralBase + lateralImpact);

            var longitudinalBase = yOverlap * (longitudinalContact ? 1.05f : 0.35f);
            var longitudinalImpact = (closingSpeed / 120f) * 0.40f;
            var longitudinalMagnitude = Math.Max(0.01f, longitudinalBase + longitudinalImpact);

            var firstDelta = 0f;
            var secondDelta = 0f;

            if (firstRearClosing)
            {
                firstDelta -= impulse / first.MassKg;
                secondDelta += impulse / second.MassKg;
            }
            else if (secondRearClosing)
            {
                secondDelta -= impulse / second.MassKg;
                firstDelta += impulse / first.MassKg;
            }

            // Side scrub: equal and opposite contact force → deceleration ∝ 1/mass.
            var sideScrubForce = (lateralMagnitude * (longitudinalContact ? 1.1f : 1.8f))
                + (longitudinalMagnitude * 0.2f)
                + (closingSpeed * (longitudinalContact ? 0.035f : 0.02f));
            firstDelta -= sideScrubForce * firstMassEffect;
            secondDelta -= sideScrubForce * secondMassEffect;

            if (firstDelta < -first.SpeedKph)
                firstDelta = -first.SpeedKph;
            if (secondDelta < -second.SpeedKph)
                secondDelta = -second.SpeedKph;

            var overlapSeverity = Clamp01(Math.Max(
                xOverlap / Math.Max(Epsilon, halfWidthSum),
                yOverlap / Math.Max(Epsilon, halfLengthSum)));
            var impactSeverity = Math.Max(severity, overlapSeverity * 0.75f);
            var contactType = ResolveContactType(longitudinalContact, firstRearClosing, secondRearClosing, xOverlap, yOverlap);

            response = new VehicleCollisionResponse(
                new VehicleCollisionImpulse(
                    sideSign * lateralMagnitude * firstMassEffect,
                    longitudinalSign * longitudinalMagnitude * firstMassEffect,
                    firstDelta),
                new VehicleCollisionImpulse(
                    -sideSign * lateralMagnitude * secondMassEffect,
                    -longitudinalSign * longitudinalMagnitude * secondMassEffect,
                    secondDelta),
                contactType,
                impactSeverity,
                relativeSpeed);
            return true;
        }

        private static VehicleCollisionContactType ResolveContactType(
            bool longitudinalContact,
            bool firstRearClosing,
            bool secondRearClosing,
            float xOverlap,
            float yOverlap)
        {
            if (firstRearClosing || secondRearClosing)
                return VehicleCollisionContactType.RearEnd;
            if (!longitudinalContact)
                return VehicleCollisionContactType.SideSwipe;
            if (xOverlap > yOverlap)
                return VehicleCollisionContactType.Mixed;
            return VehicleCollisionContactType.Unknown;
        }

        private static float ResolveSign(float axisDelta, float speedDiff)
        {
            if (Math.Abs(axisDelta) > Epsilon)
                return axisDelta > 0f ? 1f : -1f;
            if (Math.Abs(speedDiff) > Epsilon)
                return speedDiff > 0f ? 1f : -1f;
            return 1f;
        }

        private static float Clamp01(float value)
        {
            if (value <= 0f)
                return 0f;
            if (value >= 1f)
                return 1f;
            return value;
        }
    }
}
