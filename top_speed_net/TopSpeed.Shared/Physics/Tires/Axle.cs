using System;

namespace TopSpeed.Physics.Tires
{
    internal readonly struct TireAxleData
    {
        public TireAxleData(float wheelbase, float trackWidth, float a, float b, float frontForce, float rearForce, float lateralForceRatio)
        {
            Wheelbase = wheelbase;
            TrackWidth = trackWidth;
            A = a;
            B = b;
            FrontForce = frontForce;
            RearForce = rearForce;
            LateralForceRatio = lateralForceRatio;
        }

        public float Wheelbase { get; }
        public float TrackWidth { get; }
        public float A { get; }
        public float B { get; }
        public float FrontForce { get; }
        public float RearForce { get; }
        public float LateralForceRatio { get; }
        public float TotalForce => FrontForce + RearForce;
    }

    internal static class TireAxle
    {
        public static TireAxleData Compute(in TireModelParameters parameters, in TireModelState state, in TireSteerData steer, in TireGripData grip)
        {
            var wheelbase = Math.Max(0.5f, parameters.WheelbaseM);
            var trackWidth = Math.Max(0.45f, parameters.TrackWidthM * 0.92f);
            var vehicleLength = Math.Max(wheelbase + 0.1f, parameters.VehicleLengthM);

            var frontStaticLoad = 0.5f + (((vehicleLength - wheelbase) / vehicleLength) * 0.10f);
            // Range [0.35, 0.65]: covers rear-biased sports cars (0.35) through
            // front-motor vans (0.65). The previous [0.42, 0.60] was too narrow.
            frontStaticLoad = TireModelMath.Clamp(frontStaticLoad, 0.35f, 0.65f);
            var rearStaticLoad = 1f - frontStaticLoad;

            var a = wheelbase * rearStaticLoad;
            var b = wheelbase - a;

            var peakSlipRad = TireModelMath.DegToRad(Math.Max(0.5f, parameters.SlipAnglePeakDeg));
            var frontSlip = steer.SteerRad - (float)Math.Atan2(state.LateralVelocityMps + (a * state.YawRateRad), steer.ForwardSpeed);
            var rearSlip = -(float)Math.Atan2(state.LateralVelocityMps - (b * state.YawRateRad), steer.ForwardSpeed);
            var frontSlipEff = ShapeSlip(frontSlip, peakSlipRad, parameters.SlipAngleFalloff);
            var rearSlipEff = ShapeSlip(rearSlip, peakSlipRad, parameters.SlipAngleFalloff);

            var baseCornerStiffness = Math.Max(100f, grip.GripForce / Math.Max(0.05f, peakSlipRad));
            var cornerFront = baseCornerStiffness * Math.Max(0.2f, parameters.CornerStiffnessFront);
            var cornerRear = baseCornerStiffness * Math.Max(0.2f, parameters.CornerStiffnessRear);
            var frontForce = cornerFront * frontSlipEff;
            var rearForce = cornerRear * rearSlipEff;

            var latAccelEstimate = Math.Abs(state.YawRateRad * steer.ForwardSpeed);
            // Load transfer coefficient = CgHeight / (2 × trackWidth).
            // Derived from vehicle-specific CoG height; default 0.55 m matches a mid-size car.
            var loadTransfer = TireModelMath.Clamp01((latAccelEstimate / TireModelConstants.Gravity) * (parameters.CgHeightM / trackWidth));
            var frontLimit = grip.GripForce * frontStaticLoad * (1f - (0.25f * loadTransfer));
            var rearLimit = grip.GripForce * rearStaticLoad * (1f - (0.50f * loadTransfer));
            frontLimit = Math.Max(0.05f * grip.GripForce, frontLimit);
            rearLimit = Math.Max(0.05f * grip.GripForce, rearLimit);

            frontForce = TireModelMath.Clamp(frontForce, -frontLimit, frontLimit);
            rearForce = TireModelMath.Clamp(rearForce, -rearLimit, rearLimit);

            var lateralForceRatio = grip.GripForce > 0.0001f
                ? Math.Min(1f, Math.Abs(frontForce + rearForce) / grip.GripForce)
                : 0f;

            return new TireAxleData(wheelbase, trackWidth, a, b, frontForce, rearForce, lateralForceRatio);
        }

        private static float ShapeSlip(float slip, float peakSlipRad, float falloff)
        {
            var denom = 1f + ((Math.Abs(slip) / Math.Max(0.01f, peakSlipRad)) * Math.Max(0.01f, falloff));
            return slip / denom;
        }
    }
}
