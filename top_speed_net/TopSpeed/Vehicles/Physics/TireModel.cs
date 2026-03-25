using TopSpeed.Physics.Tires;

namespace TopSpeed.Vehicles
{
    internal partial class Car
    {
        private TireModelParameters BuildTireModelParameters()
        {
            return new TireModelParameters(
                _steering,
                _maxSteerDeg,
                _wheelbaseM,
                _widthM,
                _lengthM,
                _tireGripCoefficient,
                _lateralGripCoefficient,
                _highSpeedStability,
                _massKg,
                _highSpeedSteerGain,
                _highSpeedSteerStartKph,
                _highSpeedSteerFullKph,
                _combinedGripPenalty,
                _slipAnglePeakDeg,
                _slipAngleFalloff,
                _turnResponse,
                _massSensitivity,
                _downforceGripGain,
                _cornerStiffnessFront,
                _cornerStiffnessRear,
                _yawInertiaScale,
                _steeringCurve,
                _transientDamping,
                _cgHeightM);
        }

        private TireModelOutput SolveTireModel(float elapsed, float speedMps, int steeringInput, float surfaceTractionMod, float lateralMultiplier, bool commitState = true)
        {
            var parameters = BuildTireModelParameters();
            var input = new TireModelInput(elapsed, speedMps, steeringInput, surfaceTractionMod, lateralMultiplier);
            var state = new TireModelState(_lateralVelocityMps, _yawRateRad);
            var output = TireModelSolver.Solve(parameters, input, state);
            if (commitState)
            {
                _lateralVelocityMps = output.State.LateralVelocityMps;
                _yawRateRad = output.State.YawRateRad;
            }

            return output;
        }
    }
}
