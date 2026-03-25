using System;
using System.IO;
using TopSpeed.Audio;
using TopSpeed.Core;
using TopSpeed.Data;
using TopSpeed.Input;
using TopSpeed.Physics.Powertrain;
using TopSpeed.Protocol;
using TopSpeed.Tracks;
using TopSpeed.Vehicles.Audio;
using TopSpeed.Vehicles.Control;
using TopSpeed.Vehicles.Core;
using TopSpeed.Vehicles.Events;
using TopSpeed.Vehicles.Physics;
using TopSpeed.Input.Devices.Vibration;

namespace TopSpeed.Vehicles
{
    internal partial class Car
    {
        public Car(
            AudioManager audio,
            Track track,
            RaceInput input,
            RaceSettings settings,
            int vehicleIndex,
            string? vehicleFile,
            Func<float> currentTime,
            Func<bool> started,
            IVibrationDevice? vibrationDevice = null)
            : base(new RaceInputCarController(input))
        {
            _audio = audio;
            _track = track;
            _settings = settings;
            _currentTime = currentTime;
            _started = started;
            _legacyRoot = Path.Combine(AssetPaths.SoundsRoot, "Legacy");
            _effectsRoot = Path.Combine(AssetPaths.Root, "Effects");
            _events = new EventQueue();
            _runtimeContext = new CarRuntimeContext();
            _physicsModel = new Default();
            _audioFlow = new Flow();
            _eventProcessor = new Processor(
                HandleEventCarStart,
                HandleEventCarRestart,
                HandleEventCrashComplete,
                HandleEventInGear,
                HandleEventStopVibration,
                HandleEventStopBumpVibration);

            InitializeRuntimeDefaults(track);
            var definition = LoadDefinition(vehicleIndex, vehicleFile, track.Weather);
            ApplyDefinition(definition);
            InitializeDriveSystems(definition);
            InitializeAudioAssets(definition);
            _vibration = InitializeVibration(vibrationDevice);
            ConfigureInitialAudioState();
        }

        private void InitializeRuntimeDefaults(Track track)
        {
            _surface = track.InitialSurface;
            _gear = 1;
            SetState(CarState.Stopped);
            _manualTransmission = false;
            _hasWipers = 0;
            _switchingGear = 0;
            _speed = 0;
            _frame = 1;
            _throttleVolume = 0.0f;
            _prevThrottleVolume = 0.0f;
            _prevFrequency = 0;
            _prevBrakeFrequency = 0;
            _brakeFrequency = 0;
            _prevSurfaceFrequency = 0;
            _surfaceFrequency = 0;
            _laneWidth = track.LaneWidth * 2;
            _relPos = 0f;
            _panPos = 0;
            _currentSteering = 0;
            _currentThrottle = 0;
            _currentBrake = 0;
            _currentSurfaceTractionFactor = 0;
            _currentDeceleration = 0;
            _currentSurfaceLateralMultiplier = 1f;
            _speedDiff = 0;
            _factor1 = 100;
            _lateralVelocityMps = 0f;
            _yawRateRad = 0f;
        }

        private VehicleDefinition LoadDefinition(int vehicleIndex, string? vehicleFile, TrackWeather weather)
        {
            VehicleDefinition definition;
            if (string.IsNullOrWhiteSpace(vehicleFile))
            {
                definition = VehicleLoader.LoadOfficial(vehicleIndex, weather);
                _carType = definition.CarType;
            }
            else
            {
                definition = VehicleLoader.LoadCustom(vehicleFile!, weather);
                _carType = definition.CarType;
                _customFile = definition.CustomFile;
                _userDefined = true;
            }

            return definition;
        }

        private void ApplyDefinition(VehicleDefinition definition)
        {
            VehicleName = definition.Name;
            _surfaceTractionFactor = Math.Max(0.01f, SanitizeFinite(definition.SurfaceTractionFactor, 0.01f));
            _deceleration = Math.Max(0.01f, SanitizeFinite(definition.Deceleration, 0.01f));
            _topSpeed = Math.Max(1f, SanitizeFinite(definition.TopSpeed, 1f));
            _massKg = Math.Max(1f, SanitizeFinite(definition.MassKg, 1f));
            _drivetrainEfficiency = Math.Max(0.1f, Math.Min(1.0f, SanitizeFinite(definition.DrivetrainEfficiency, 0.85f)));
            _engineBrakingTorqueNm = Math.Max(0f, SanitizeFinite(definition.EngineBrakingTorqueNm, 0f));
            _tireGripCoefficient = Math.Max(0.1f, SanitizeFinite(definition.TireGripCoefficient, 0.1f));
            _brakeStrength = Math.Max(0.1f, SanitizeFinite(definition.BrakeStrength, 0.1f));
            _wheelRadiusM = Math.Max(0.01f, SanitizeFinite(definition.TireCircumferenceM, 0f) / (2.0f * (float)Math.PI));
            _engineBraking = Math.Max(0.05f, Math.Min(1.0f, SanitizeFinite(definition.EngineBraking, 0.3f)));
            _idleRpm = Math.Max(0f, SanitizeFinite(definition.IdleRpm, 0f));
            _revLimiter = Math.Max(_idleRpm, SanitizeFinite(definition.RevLimiter, _idleRpm));
            _finalDriveRatio = Math.Max(0.1f, SanitizeFinite(definition.FinalDriveRatio, 0.1f));
            _reverseMaxSpeedKph = Math.Max(5f, SanitizeFinite(definition.ReverseMaxSpeedKph, 35f));
            _reversePowerFactor = Math.Max(0.1f, SanitizeFinite(definition.ReversePowerFactor, 0.55f));
            _reverseGearRatio = Math.Max(0.1f, SanitizeFinite(definition.ReverseGearRatio, 3.2f));
            _powerFactor = Math.Max(0.1f, SanitizeFinite(definition.PowerFactor, 0.1f));
            _peakTorqueNm = Math.Max(0f, SanitizeFinite(definition.PeakTorqueNm, 0f));
            _peakTorqueRpm = Math.Max(_idleRpm + 100f, SanitizeFinite(definition.PeakTorqueRpm, _idleRpm + 100f));
            _idleTorqueNm = Math.Max(0f, SanitizeFinite(definition.IdleTorqueNm, 0f));
            _redlineTorqueNm = Math.Max(0f, SanitizeFinite(definition.RedlineTorqueNm, 0f));
            _dragCoefficient = Math.Max(0.01f, SanitizeFinite(definition.DragCoefficient, 0.01f));
            _frontalAreaM2 = Math.Max(0.1f, SanitizeFinite(definition.FrontalAreaM2, 0.1f));
            _rollingResistanceCoefficient = Math.Max(0.001f, SanitizeFinite(definition.RollingResistanceCoefficient, 0.001f));
            _launchRpm = Math.Max(_idleRpm, Math.Min(_revLimiter, SanitizeFinite(definition.LaunchRpm, _idleRpm)));
            _engineInertiaKgm2 = Math.Max(0.01f, SanitizeFinite(definition.EngineInertiaKgm2, 0.24f));
            _engineFrictionTorqueNm = Math.Max(0f, SanitizeFinite(definition.EngineFrictionTorqueNm, 20f));
            _drivelineCouplingRate = Math.Max(0.1f, SanitizeFinite(definition.DrivelineCouplingRate, 12f));
            _lateralGripCoefficient = Math.Max(0.1f, SanitizeFinite(definition.LateralGripCoefficient, 0.1f));
            _highSpeedStability = Math.Max(0f, Math.Min(1.0f, SanitizeFinite(definition.HighSpeedStability, 0f)));
            _wheelbaseM = Math.Max(0.5f, SanitizeFinite(definition.WheelbaseM, 0.5f));
            _maxSteerDeg = Math.Max(5f, Math.Min(60f, SanitizeFinite(definition.MaxSteerDeg, 35f)));
            _highSpeedSteerGain = Math.Max(0.7f, Math.Min(1.6f, SanitizeFinite(definition.HighSpeedSteerGain, 1.08f)));
            _highSpeedSteerStartKph = Math.Max(60f, Math.Min(260f, SanitizeFinite(definition.HighSpeedSteerStartKph, 140f)));
            _highSpeedSteerFullKph = Math.Max(100f, Math.Min(350f, SanitizeFinite(definition.HighSpeedSteerFullKph, 240f)));
            if (_highSpeedSteerFullKph <= _highSpeedSteerStartKph)
                _highSpeedSteerFullKph = _highSpeedSteerStartKph + 1f;
            _combinedGripPenalty = Math.Max(0f, Math.Min(1f, SanitizeFinite(definition.CombinedGripPenalty, 0.72f)));
            _slipAnglePeakDeg = Math.Max(0.5f, Math.Min(20f, SanitizeFinite(definition.SlipAnglePeakDeg, 8f)));
            _slipAngleFalloff = Math.Max(0.01f, Math.Min(5f, SanitizeFinite(definition.SlipAngleFalloff, 1.25f)));
            _turnResponse = Math.Max(0.2f, Math.Min(2.5f, SanitizeFinite(definition.TurnResponse, 1f)));
            _massSensitivity = Math.Max(0f, Math.Min(1f, SanitizeFinite(definition.MassSensitivity, 0.75f)));
            _downforceGripGain = Math.Max(0f, Math.Min(1f, SanitizeFinite(definition.DownforceGripGain, 0.05f)));
            _cornerStiffnessFront = Math.Max(0.2f, Math.Min(3f, SanitizeFinite(definition.CornerStiffnessFront, 1f)));
            _cornerStiffnessRear = Math.Max(0.2f, Math.Min(3f, SanitizeFinite(definition.CornerStiffnessRear, 1f)));
            _yawInertiaScale = Math.Max(0.5f, Math.Min(2f, SanitizeFinite(definition.YawInertiaScale, 1f)));
            _steeringCurve = Math.Max(0.5f, Math.Min(2f, SanitizeFinite(definition.SteeringCurve, 1f)));
            _transientDamping = Math.Max(0f, Math.Min(6f, SanitizeFinite(definition.TransientDamping, 1.0f)));
            _cgHeightM = definition.CgHeightM > 0f ? definition.CgHeightM : 0.55f;
            _widthM = Math.Max(0.5f, SanitizeFinite(definition.WidthM, 0.5f));
            _lengthM = Math.Max(0.5f, SanitizeFinite(definition.LengthM, 0.5f));
            _idleFreq = definition.IdleFreq;
            _topFreq = definition.TopFreq;
            _shiftFreq = definition.ShiftFreq;
            _pitchCurveExponent = VehicleDefinition.ClampPitchCurveExponent(definition.PitchCurveExponent);
            _gears = Math.Max(1, definition.Gears);
            _steering = SanitizeFinite(definition.Steering, 0.1f);
            _frequency = _idleFreq;
        }

        private void InitializeDriveSystems(VehicleDefinition definition)
        {
            var torqueCurve = PowertrainProfileBuilder.Build(definition);

            _engine = new EngineModel(
                definition.IdleRpm,
                definition.MaxRpm,
                definition.RevLimiter,
                definition.AutoShiftRpm,
                definition.EngineBraking,
                definition.TopSpeed,
                definition.FinalDriveRatio,
                definition.TireCircumferenceM,
                definition.Gears,
                definition.GearRatios,
                definition.PeakTorqueNm,
                definition.PeakTorqueRpm,
                definition.IdleTorqueNm,
                definition.RedlineTorqueNm,
                definition.EngineBrakingTorqueNm,
                definition.PowerFactor,
                definition.EngineInertiaKgm2,
                definition.EngineFrictionTorqueNm,
                definition.DrivelineCouplingRate,
                torqueCurve);

            _powertrainConfiguration = new Config(
                _massKg,
                _drivetrainEfficiency,
                _engineBrakingTorqueNm,
                _tireGripCoefficient,
                _brakeStrength,
                _wheelRadiusM,
                _engineBraking,
                _idleRpm,
                _revLimiter,
                _finalDriveRatio,
                _powerFactor,
                _peakTorqueNm,
                _peakTorqueRpm,
                _idleTorqueNm,
                _redlineTorqueNm,
                _dragCoefficient,
                _frontalAreaM2,
                _rollingResistanceCoefficient,
                _launchRpm,
                _reversePowerFactor,
                _reverseGearRatio,
                _engineInertiaKgm2,
                _engineFrictionTorqueNm,
                _drivelineCouplingRate,
                _gears,
                definition.GearRatios ?? Array.Empty<float>(),
                torqueCurve);
            _transmissionPolicy = definition.TransmissionPolicy ?? TransmissionPolicy.Default;
        }
    }
}

