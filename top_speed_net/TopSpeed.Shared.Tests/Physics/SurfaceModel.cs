using TopSpeed.Data;
using TopSpeed.Physics.Surface;
using Xunit;

namespace TopSpeed.Shared.Tests.Physics
{
    public sealed class SurfaceModelTests
    {
        [Fact]
        public void Asphalt_Returns_BaselineModifiers()
        {
            var mods = SurfaceModel.Resolve(TrackSurface.Asphalt, baseTraction: 1f, baseDeceleration: 1f);

            Assert.Equal(1f, mods.Traction);
            Assert.Equal(1f, mods.Deceleration);
            Assert.Equal(1f, mods.LateralSpeedMultiplier);
        }

        [Fact]
        public void Gravel_ReducesTraction_AndDeceleration()
        {
            var mods = SurfaceModel.Resolve(TrackSurface.Gravel, baseTraction: 1f, baseDeceleration: 1f);

            Assert.True(mods.Traction < 1f, "Gravel must reduce traction.");
            Assert.True(mods.Deceleration < 1f, "Gravel must reduce braking.");
        }

        [Fact]
        public void Snow_ReducesTraction_AndDeceleration()
        {
            var mods = SurfaceModel.Resolve(TrackSurface.Snow, baseTraction: 1f, baseDeceleration: 1f);

            // Snow has poor grip for both acceleration and braking.
            Assert.True(mods.Traction < 1f,
                "Snow must reduce drive traction, not only braking.");
            Assert.True(mods.Deceleration < 1f,
                "Snow must reduce braking deceleration.");
            Assert.True(mods.LateralSpeedMultiplier > 1f,
                "Snow must increase lateral (sliding) speed.");
        }

        [Fact]
        public void Snow_TractionReduction_IsAtLeastAsLargeAs_Sand()
        {
            var snow = SurfaceModel.Resolve(TrackSurface.Snow, baseTraction: 1f, baseDeceleration: 1f);
            var sand = SurfaceModel.Resolve(TrackSurface.Sand, baseTraction: 1f, baseDeceleration: 1f);

            // Snow is at least as slippery as sand for drive traction.
            Assert.True(snow.Traction <= sand.Traction,
                "Snow traction must not exceed sand traction.");
        }

        [Fact]
        public void Water_ReducesTraction_LessThan_Sand()
        {
            var water = SurfaceModel.Resolve(TrackSurface.Water, baseTraction: 1f, baseDeceleration: 1f);
            var sand = SurfaceModel.Resolve(TrackSurface.Sand, baseTraction: 1f, baseDeceleration: 1f);

            Assert.True(water.Traction > sand.Traction,
                "Water should be less extreme than sand for drive traction.");
        }
    }
}
