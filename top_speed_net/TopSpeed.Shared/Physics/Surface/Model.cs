using TopSpeed.Data;

namespace TopSpeed.Physics.Surface
{
    public static class SurfaceModel
    {
        public static SurfaceModifiers Resolve(TrackSurface surface, float baseTraction, float baseDeceleration)
        {
            var traction = baseTraction;
            var deceleration = baseDeceleration;
            var lateralMultiplier = 1.0f;

            switch (surface)
            {
                case TrackSurface.Gravel:
                    traction = (traction * 2f) / 3f;
                    deceleration = (deceleration * 2f) / 3f;
                    break;
                case TrackSurface.Water:
                    traction = (traction * 3f) / 5f;
                    deceleration = (deceleration * 3f) / 5f;
                    break;
                case TrackSurface.Sand:
                    traction *= 0.5f;
                    deceleration = (deceleration * 3f) / 2f;
                    break;
                case TrackSurface.Snow:
                    // Snow has poor traction for both acceleration and braking.
                    // Traction grip on snow is roughly half that of dry asphalt.
                    traction *= 0.5f;
                    deceleration *= 0.5f;
                    lateralMultiplier = 1.44f;
                    break;
            }

            return new SurfaceModifiers(traction, deceleration, lateralMultiplier);
        }
    }
}
