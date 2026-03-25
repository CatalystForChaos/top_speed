using TopSpeed.Collision;
using Xunit;

namespace TopSpeed.Shared.Tests.Collision
{
    public sealed class CollisionContextTests
    {
        [Fact]
        public void RearEndCollision_ReportsImpactContext()
        {
            var rear = new VehicleCollisionBody(0f, 100f, 130f, 1.8f, 4.5f, 1500f);
            var front = new VehicleCollisionBody(0f, 101.8f, 80f, 1.8f, 4.5f, 1500f);

            var collided = VehicleCollisionResolver.TryResolve(rear, front, out var response);

            Assert.True(collided);
            Assert.Equal(VehicleCollisionContactType.RearEnd, response.ContactType);
            Assert.True(response.ImpactSeverity > 0f);
            Assert.True(response.RelativeSpeedKph > 0f);
        }

        [Fact]
        public void WallConsequence_ConstrainBumpInsideWall_AndApplyPenalty()
        {
            var body = new VehicleCollisionBody(4.9f, 100f, 120f, 1.8f, 4.5f, 1500f);
            var impulse = new VehicleCollisionImpulse(0.4f, 0f, 0f);
            var response = new VehicleCollisionResponse(
                impulse,
                new VehicleCollisionImpulse(-0.4f, 0f, 0f),
                VehicleCollisionContactType.RearEnd,
                impactSeverity: 0.9f,
                relativeSpeedKph: 65f);

            var adjusted = CollisionWallConsequence.Apply(body, impulse, response, wallHalfWidthM: 5f);
            var predictedX = body.PositionX + (2f * adjusted.BumpX);

            Assert.True(predictedX <= 5.0001f);
            Assert.True(adjusted.SpeedDeltaKph < impulse.SpeedDeltaKph);
        }

        // Wall impact severity is proportional to v² (kinetic energy).
        // Doubling the relative speed must more than double the speed penalty.
        [Fact]
        public void WallConsequence_SpeedPenalty_IsQuadraticInRelativeSpeed()
        {
            var body = new VehicleCollisionBody(4.9f, 100f, 120f, 1.8f, 4.5f, 1500f);
            var impulse = new VehicleCollisionImpulse(0.4f, 0f, 0f);
            var dummy = new VehicleCollisionImpulse(-0.4f, 0f, 0f);

            var responseLow = new VehicleCollisionResponse(
                impulse, dummy, VehicleCollisionContactType.RearEnd,
                impactSeverity: 0f, relativeSpeedKph: 50f);
            var responseHigh = new VehicleCollisionResponse(
                impulse, dummy, VehicleCollisionContactType.RearEnd,
                impactSeverity: 0f, relativeSpeedKph: 100f);

            var lowAdjusted  = CollisionWallConsequence.Apply(body, impulse, responseLow,  wallHalfWidthM: 5f);
            var highAdjusted = CollisionWallConsequence.Apply(body, impulse, responseHigh, wallHalfWidthM: 5f);

            var lowPenalty  = impulse.SpeedDeltaKph - lowAdjusted.SpeedDeltaKph;
            var highPenalty = impulse.SpeedDeltaKph - highAdjusted.SpeedDeltaKph;

            Assert.True(highPenalty > 2f * lowPenalty,
                "Wall penalty at 100 kph must be more than twice the penalty at 50 kph (quadratic scaling).");
        }

        // Collision momentum: light vehicle hitting a heavy vehicle loses more speed (Δv = J/m).
        [Fact]
        public void Collision_LightVehicleHitsHeavy_LightVehicleLosesMoreSpeed()
        {
            var light = new VehicleCollisionBody(0f, 100f, 130f, 1.8f, 4.5f, 800f);
            var heavy = new VehicleCollisionBody(0f, 101.8f, 80f, 1.8f, 4.5f, 3000f);

            var collided = VehicleCollisionResolver.TryResolve(light, heavy, out var response);

            Assert.True(collided);
            Assert.True(-response.First.SpeedDeltaKph > response.Second.SpeedDeltaKph,
                "Light vehicle must lose more speed than the heavy vehicle gains.");
        }
    }
}
