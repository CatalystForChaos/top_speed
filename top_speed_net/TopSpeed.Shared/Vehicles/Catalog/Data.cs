using TopSpeed.Protocol;

namespace TopSpeed.Vehicles
{
    public static partial class OfficialVehicleCatalog
    {
        private static readonly float[] GtrRatios = { 3.90f, 2.36f, 1.69f, 1.31f, 1.03f, 0.84f };
        private static readonly float[] Gt3RsRatios = { 4.10f, 2.62f, 1.92f, 1.52f, 1.25f, 1.04f, 0.88f };
        private static readonly float[] Fiat500Ratios = { 3.75f, 2.16f, 1.48f, 1.13f, 0.86f };
        private static readonly float[] MiniCooperSRatios = { 3.74f, 2.11f, 1.45f, 1.16f, 0.94f, 0.79f };
        private static readonly float[] Mustang69Ratios = { 2.78f, 1.93f, 1.36f, 1.00f };
        private static readonly float[] CamryRatios = { 4.70f, 2.84f, 1.91f, 1.45f, 1.20f, 1.00f, 0.84f, 0.70f };
        private static readonly float[] AventadorRatios = { 3.91f, 2.10f, 1.55f, 1.30f, 1.19f, 1.06f, 0.98f };
        private static readonly float[] Bmw3SeriesRatios = { 4.71f, 3.14f, 2.11f, 1.67f, 1.29f, 1.00f, 0.96f, 0.90f };
        private static readonly float[] SprinterRatios = { 4.3772f, 2.8586f, 1.9206f, 1.3684f, 1.0000f, 0.9200f, 0.8500f };
        // Corrected: previous values had a ~49% ratio drop from 2nd to 3rd gear.
        // Real supersport gearboxes use ~25-27% steps between consecutive gears.
        private static readonly float[] Zx10rRatios = { 3.10f, 2.35f, 1.75f, 1.31f, 0.98f, 0.73f };
        private static readonly float[] PanigaleV4Ratios = { 3.00f, 2.25f, 1.68f, 1.26f, 0.94f, 0.70f };
        private static readonly float[] R1Ratios = { 3.05f, 2.30f, 1.72f, 1.29f, 0.97f, 0.72f };
        private static readonly float[] Auto8Upshifts = { 0f, 0f, 0f, 0f, 0.18f, 0.24f, 0.30f, 0.34f };
        private static readonly float[] Auto7Upshifts = { 0f, 0f, 0f, 0f, 0.18f, 0.24f, 0.28f };
        private static readonly float[] Auto6Upshifts = { 0f, 0f, 0f, 0f, 0.20f, 0.26f };

        public static readonly OfficialVehicleSpec[] Vehicles =
        {
            new OfficialVehicleSpec(
                CarType.Vehicle1, "Nissan GT-R Nismo",
                hasWipers: 1, surfaceTractionFactor: 0.06f, deceleration: 0.40f, topSpeed: 268.0f,
                idleFreq: 22050, topFreq: 55000, shiftFreq: 26000, gears: 6, steering: 1.20f,
                idleRpm: 900f, maxRpm: 8000f, revLimiter: 7600f, autoShiftRpm: 7600f * 0.92f, engineBraking: 0.25f,
                massKg: 1774f, drivetrainEfficiency: 0.80f, engineBrakingTorqueNm: 652f, tireGripCoefficient: 1.0f,
                peakTorqueNm: 652f, peakTorqueRpm: 3600f, idleTorqueNm: 652f * 0.3f, redlineTorqueNm: 652f * 0.6f,
                dragCoefficient: 0.26f, frontalAreaM2: 2.2f, rollingResistanceCoefficient: 0.015f, launchRpm: 2500f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 26f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.70f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(285, 35, 20), lateralGripCoefficient: 1.0f, highSpeedStability: 0.24f,
                wheelbaseM: 2.779f, maxSteerDeg: 35f, widthM: 1.895f, lengthM: 4.689f,
                powerFactor: 0.67f, gearRatios: GtrRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.94f, highSpeedSteerStartKph: 150f, highSpeedSteerFullKph: 250f,
                combinedGripPenalty: 0.70f, slipAnglePeakDeg: 8.3f, slipAngleFalloff: 1.20f,
                turnResponse: 0.96f, massSensitivity: 0.68f, downforceGripGain: 0.09f,
                cornerStiffnessFront: 1.15f, cornerStiffnessRear: 1.08f, yawInertiaScale: 1.20f, steeringCurve: 1.06f, transientDamping: 1.35f,
                torqueCurvePreset: "sport_coupe",
                transmissionPolicy: Policy(5, true, Auto6Upshifts, upshiftRpmFraction: 0.88f)),

            new OfficialVehicleSpec(
                CarType.Vehicle2, "Porsche 911 GT3 RS",
                hasWipers: 1, surfaceTractionFactor: 0.07f, deceleration: 0.45f, topSpeed: 248.0f,
                idleFreq: 22050, topFreq: 60000, shiftFreq: 35000, gears: 7, steering: 1.15f,
                idleRpm: 950f, maxRpm: 9000f, revLimiter: 8500f, autoShiftRpm: 8500f * 0.92f, engineBraking: 0.22f,
                massKg: 1450f, drivetrainEfficiency: 0.85f, engineBrakingTorqueNm: 465f, tireGripCoefficient: 1.05f,
                peakTorqueNm: 465f, peakTorqueRpm: 6250f, idleTorqueNm: 465f * 0.3f, redlineTorqueNm: 465f * 0.6f,
                dragCoefficient: 0.30f, frontalAreaM2: 2.0f, rollingResistanceCoefficient: 0.015f, launchRpm: 3000f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 22f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.97f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(325, 30, 21), lateralGripCoefficient: 1.0f, highSpeedStability: 0.24f,
                wheelbaseM: 2.456f, maxSteerDeg: 35f, widthM: 1.852f, lengthM: 4.572f,
                powerFactor: 0.64f, gearRatios: Gt3RsRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.93f, highSpeedSteerStartKph: 150f, highSpeedSteerFullKph: 250f,
                combinedGripPenalty: 0.71f, slipAnglePeakDeg: 8.0f, slipAngleFalloff: 1.18f,
                turnResponse: 0.98f, massSensitivity: 0.66f, downforceGripGain: 0.11f,
                cornerStiffnessFront: 1.20f, cornerStiffnessRear: 1.12f, yawInertiaScale: 1.18f, steeringCurve: 1.05f, transientDamping: 1.35f,
                torqueCurvePreset: "supercar_na",
                transmissionPolicy: Policy(5, true, Auto7Upshifts, upshiftRpmFraction: 0.85f)),

            new OfficialVehicleSpec(
                CarType.Vehicle3, "Fiat 500",
                hasWipers: 1, surfaceTractionFactor: 0.035f, deceleration: 0.30f, topSpeed: 122.0f,
                idleFreq: 6000, topFreq: 25000, shiftFreq: 19000, gears: 5, steering: 1.12f,
                idleRpm: 750f, maxRpm: 6000f, revLimiter: 5500f, autoShiftRpm: 5500f * 0.92f, engineBraking: 0.40f,
                massKg: 865f, drivetrainEfficiency: 0.88f, engineBrakingTorqueNm: 102f, tireGripCoefficient: 0.88f,
                peakTorqueNm: 102f, peakTorqueRpm: 3000f, idleTorqueNm: 102f * 0.3f, redlineTorqueNm: 102f * 0.6f,
                dragCoefficient: 0.30f, frontalAreaM2: 2.1f, rollingResistanceCoefficient: 0.014f, launchRpm: 1800f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 9f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.353f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(195, 45, 16), lateralGripCoefficient: 1.0f, highSpeedStability: 0.28f,
                wheelbaseM: 2.300f, maxSteerDeg: 35f, widthM: 1.627f, lengthM: 3.546f,
                powerFactor: 0.78f, gearRatios: Fiat500Ratios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.90f, highSpeedSteerStartKph: 120f, highSpeedSteerFullKph: 200f,
                combinedGripPenalty: 0.66f, slipAnglePeakDeg: 9.2f, slipAngleFalloff: 1.35f,
                turnResponse: 0.72f, massSensitivity: 0.82f, downforceGripGain: 0.01f,
                cornerStiffnessFront: 0.92f, cornerStiffnessRear: 0.90f, yawInertiaScale: 1.36f, steeringCurve: 1.24f, transientDamping: 2.20f,
                torqueCurvePreset: "city_compact",
                transmissionPolicy: Policy(4, true, upshiftRpmFraction: 0.84f)),

            new OfficialVehicleSpec(
                CarType.Vehicle4, "Mini Cooper S",
                hasWipers: 1, surfaceTractionFactor: 0.045f, deceleration: 0.35f, topSpeed: 192.0f,
                idleFreq: 6000, topFreq: 27000, shiftFreq: 20000, gears: 6, steering: 1.08f,
                idleRpm: 800f, maxRpm: 6500f, revLimiter: 6000f, autoShiftRpm: 6000f * 0.92f, engineBraking: 0.32f,
                massKg: 1265f, drivetrainEfficiency: 0.88f, engineBrakingTorqueNm: 280f, tireGripCoefficient: 0.95f,
                peakTorqueNm: 280f, peakTorqueRpm: 1250f, idleTorqueNm: 280f * 0.3f, redlineTorqueNm: 280f * 0.6f,
                dragCoefficient: 0.31f, frontalAreaM2: 2.1f, rollingResistanceCoefficient: 0.014f, launchRpm: 2200f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 16f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.59f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(195, 55, 16), lateralGripCoefficient: 1.0f, highSpeedStability: 0.26f,
                wheelbaseM: 2.494f, maxSteerDeg: 35f, widthM: 1.744f, lengthM: 3.876f,
                powerFactor: 0.80f, gearRatios: MiniCooperSRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.91f, highSpeedSteerStartKph: 130f, highSpeedSteerFullKph: 220f,
                combinedGripPenalty: 0.69f, slipAnglePeakDeg: 8.8f, slipAngleFalloff: 1.28f,
                turnResponse: 0.76f, massSensitivity: 0.78f, downforceGripGain: 0.03f,
                cornerStiffnessFront: 0.98f, cornerStiffnessRear: 0.95f, yawInertiaScale: 1.32f, steeringCurve: 1.20f, transientDamping: 2.05f,
                torqueCurvePreset: "hot_hatch",
                transmissionPolicy: Policy(5, true, Auto6Upshifts, upshiftRpmFraction: 0.86f)),

            new OfficialVehicleSpec(
                CarType.Vehicle5, "Ford Mustang 1969",
                hasWipers: 1, surfaceTractionFactor: 0.04f, deceleration: 0.35f, topSpeed: 186.0f,
                idleFreq: 6000, topFreq: 33000, shiftFreq: 27500, gears: 4, steering: 1.25f,
                idleRpm: 650f, maxRpm: 5500f, revLimiter: 5000f, autoShiftRpm: 5000f * 0.92f, engineBraking: 0.35f,
                massKg: 1440f, drivetrainEfficiency: 0.85f, engineBrakingTorqueNm: 481f, tireGripCoefficient: 0.90f,
                peakTorqueNm: 481f, peakTorqueRpm: 3000f, idleTorqueNm: 481f * 0.3f, redlineTorqueNm: 481f * 0.6f,
                dragCoefficient: 0.40f, frontalAreaM2: 2.5f, rollingResistanceCoefficient: 0.017f, launchRpm: 2000f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 24f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.25f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(215, 70, 14), lateralGripCoefficient: 1.0f, highSpeedStability: 0.30f,
                wheelbaseM: 2.743f, maxSteerDeg: 35f, widthM: 1.811f, lengthM: 4.760f,
                powerFactor: 0.78f, gearRatios: Mustang69Ratios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.89f, highSpeedSteerStartKph: 125f, highSpeedSteerFullKph: 210f,
                combinedGripPenalty: 0.67f, slipAnglePeakDeg: 9.0f, slipAngleFalloff: 1.30f,
                turnResponse: 0.72f, massSensitivity: 0.80f, downforceGripGain: 0.02f,
                cornerStiffnessFront: 0.95f, cornerStiffnessRear: 0.90f, yawInertiaScale: 1.42f, steeringCurve: 1.24f, transientDamping: 2.25f,
                torqueCurvePreset: "muscle_v8",
                transmissionPolicy: Policy(4, false, upshiftRpmFraction: 0.84f)),

            new OfficialVehicleSpec(
                CarType.Vehicle6, "Toyota Camry",
                hasWipers: 1, surfaceTractionFactor: 0.035f, deceleration: 0.30f, topSpeed: 170.0f,
                idleFreq: 7025, topFreq: 40000, shiftFreq: 32500, gears: 8, steering: 1.20f,
                idleRpm: 700f, maxRpm: 5600f, revLimiter: 5000f, autoShiftRpm: 4600f, engineBraking: 0.38f,
                massKg: 1470f, drivetrainEfficiency: 0.88f, engineBrakingTorqueNm: 250f, tireGripCoefficient: 0.90f,
                peakTorqueNm: 250f, peakTorqueRpm: 3800f, idleTorqueNm: 250f * 0.4f, redlineTorqueNm: 250f * 0.90f,
                dragCoefficient: 0.27f, frontalAreaM2: 2.2f, rollingResistanceCoefficient: 0.014f, launchRpm: 2000f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 15f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.20f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(215, 55, 17), lateralGripCoefficient: 1.0f, highSpeedStability: 0.32f,
                wheelbaseM: 2.825f, maxSteerDeg: 35f, widthM: 1.839f, lengthM: 4.879f,
                powerFactor: 0.64f, gearRatios: CamryRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.88f, highSpeedSteerStartKph: 120f, highSpeedSteerFullKph: 200f,
                combinedGripPenalty: 0.65f, slipAnglePeakDeg: 9.6f, slipAngleFalloff: 1.38f,
                turnResponse: 0.70f, massSensitivity: 0.86f, downforceGripGain: 0.01f,
                cornerStiffnessFront: 0.90f, cornerStiffnessRear: 0.86f, yawInertiaScale: 1.46f, steeringCurve: 1.30f, transientDamping: 2.40f,
                torqueCurvePreset: "family_sedan",
                transmissionPolicy: Policy(6, true, Auto8Upshifts, upshiftRpmFraction: 0.84f, minUpshiftNetAccelerationMps2: -0.12f)),

            new OfficialVehicleSpec(
                CarType.Vehicle7, "Lamborghini Aventador",
                hasWipers: 1, surfaceTractionFactor: 0.08f, deceleration: 0.80f, topSpeed: 282.0f,
                idleFreq: 6000, topFreq: 26000, shiftFreq: 21000, gears: 7, steering: 1.28f,
                idleRpm: 1000f, maxRpm: 8500f, revLimiter: 8000f, autoShiftRpm: 8000f * 0.92f, engineBraking: 0.20f,
                massKg: 1640f, drivetrainEfficiency: 0.80f, engineBrakingTorqueNm: 720f, tireGripCoefficient: 1.05f,
                peakTorqueNm: 720f, peakTorqueRpm: 6200f, idleTorqueNm: 720f * 0.22f, redlineTorqueNm: 720f * 0.58f,
                dragCoefficient: 0.30f, frontalAreaM2: 2.0f, rollingResistanceCoefficient: 0.015f, launchRpm: 2400f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 32f, drivelineCouplingRate: 12f,
                finalDriveRatio: 2.86f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(355, 25, 21), lateralGripCoefficient: 1.05f, highSpeedStability: 0.27f,
                wheelbaseM: 2.700f, maxSteerDeg: 25f, widthM: 2.030f, lengthM: 4.780f,
                powerFactor: 0.56f, gearRatios: AventadorRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.95f, highSpeedSteerStartKph: 155f, highSpeedSteerFullKph: 255f,
                combinedGripPenalty: 0.66f, slipAnglePeakDeg: 8.5f, slipAngleFalloff: 1.22f,
                turnResponse: 0.94f, massSensitivity: 0.56f, downforceGripGain: 0.30f,
                cornerStiffnessFront: 1.18f, cornerStiffnessRear: 1.10f, yawInertiaScale: 1.24f, steeringCurve: 1.05f, transientDamping: 1.40f,
                torqueCurvePreset: "supercar_na",
                transmissionPolicy: Policy(5, true, Auto7Upshifts, upshiftRpmFraction: 0.90f)),

            new OfficialVehicleSpec(
                CarType.Vehicle8, "BMW 3 Series",
                hasWipers: 1, surfaceTractionFactor: 0.045f, deceleration: 0.40f, topSpeed: 222.0f,
                idleFreq: 10000, topFreq: 45000, shiftFreq: 34000, gears: 8, steering: 1.22f,
                idleRpm: 750f, maxRpm: 6500f, revLimiter: 6000f, autoShiftRpm: 6000f * 0.92f, engineBraking: 0.30f,
                massKg: 1524f, drivetrainEfficiency: 0.85f, engineBrakingTorqueNm: 346f, tireGripCoefficient: 0.93f,
                peakTorqueNm: 350f, peakTorqueRpm: 1250f, idleTorqueNm: 350f * 0.3f, redlineTorqueNm: 350f * 0.6f,
                dragCoefficient: 0.27f, frontalAreaM2: 2.2f, rollingResistanceCoefficient: 0.014f, launchRpm: 2000f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 20f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.15f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(225, 50, 17), lateralGripCoefficient: 0.96f, highSpeedStability: 0.28f,
                wheelbaseM: 2.810f, maxSteerDeg: 35f, widthM: 1.811f, lengthM: 4.624f,
                powerFactor: 0.91f, gearRatios: Bmw3SeriesRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.92f, highSpeedSteerStartKph: 145f, highSpeedSteerFullKph: 240f,
                combinedGripPenalty: 0.68f, slipAnglePeakDeg: 9.0f, slipAngleFalloff: 1.30f,
                turnResponse: 0.74f, massSensitivity: 0.75f, downforceGripGain: 0.05f,
                cornerStiffnessFront: 0.98f, cornerStiffnessRear: 0.93f, yawInertiaScale: 1.38f, steeringCurve: 1.24f, transientDamping: 2.10f,
                torqueCurvePreset: "sport_sedan",
                transmissionPolicy: Policy(6, true, Auto8Upshifts, upshiftRpmFraction: 0.80f, minUpshiftNetAccelerationMps2: -0.20f)),

            new OfficialVehicleSpec(
                CarType.Vehicle9, "Mercedes Sprinter",
                hasWipers: 1, surfaceTractionFactor: 0.02f, deceleration: 0.20f, topSpeed: 138.0f,
                idleFreq: 22050, topFreq: 30550, shiftFreq: 22550, gears: 7, steering: 1.00f,
                idleRpm: 600f, maxRpm: 4500f, revLimiter: 4000f, autoShiftRpm: 4000f * 0.92f, engineBraking: 0.45f,
                massKg: 1970f, drivetrainEfficiency: 0.85f, engineBrakingTorqueNm: 380f, tireGripCoefficient: 0.82f,
                peakTorqueNm: 440f, peakTorqueRpm: 1400f, idleTorqueNm: 440f * 0.3f, redlineTorqueNm: 440f * 0.6f,
                dragCoefficient: 0.34f, frontalAreaM2: 2.9f, rollingResistanceCoefficient: 0.018f, launchRpm: 1800f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 30f, drivelineCouplingRate: 12f,
                finalDriveRatio: 3.923f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(245, 75, 16), lateralGripCoefficient: 0.90f, highSpeedStability: 0.42f,
                wheelbaseM: 3.658f, maxSteerDeg: 35f, widthM: 2.019f, lengthM: 5.931f,
                powerFactor: 0.58f, gearRatios: SprinterRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.86f, highSpeedSteerStartKph: 85f, highSpeedSteerFullKph: 150f,
                combinedGripPenalty: 0.54f, slipAnglePeakDeg: 10.8f, slipAngleFalloff: 1.55f,
                turnResponse: 0.56f, massSensitivity: 0.98f, downforceGripGain: 0.01f,
                cornerStiffnessFront: 0.74f, cornerStiffnessRear: 0.66f, yawInertiaScale: 1.74f, steeringCurve: 1.32f, transientDamping: 2.90f,
                torqueCurvePreset: "diesel_truck",
                transmissionPolicy: Policy(5, true, Auto7Upshifts, upshiftRpmFraction: 0.72f, minUpshiftNetAccelerationMps2: -0.30f)),

            new OfficialVehicleSpec(
                CarType.Vehicle10, "Kawasaki Ninja ZX-10R",
                hasWipers: 0, surfaceTractionFactor: 0.09f, deceleration: 0.50f, topSpeed: 170.0f,
                idleFreq: 22050, topFreq: 60000, shiftFreq: 35000, gears: 6, steering: 1.08f,
                idleRpm: 1100f, maxRpm: 14000f, revLimiter: 13500f, autoShiftRpm: 13500f * 0.92f, engineBraking: 0.28f,
                massKg: 207f, drivetrainEfficiency: 0.92f, engineBrakingTorqueNm: 114.9f, tireGripCoefficient: 1.10f,
                peakTorqueNm: 114.9f, peakTorqueRpm: 11500f, idleTorqueNm: 114.9f * 0.3f, redlineTorqueNm: 114.9f * 0.25f,
                dragCoefficient: 0.68f, frontalAreaM2: 0.66f, rollingResistanceCoefficient: 0.018f, launchRpm: 3900f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 10f, drivelineCouplingRate: 12f,
                finalDriveRatio: 4.20f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(190, 55, 17), lateralGripCoefficient: 0.76f, highSpeedStability: 0.46f,
                wheelbaseM: 1.450f, maxSteerDeg: 35f, widthM: 0.749f, lengthM: 2.085f,
                powerFactor: 0.70f, gearRatios: Zx10rRatios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.82f, highSpeedSteerStartKph: 115f, highSpeedSteerFullKph: 190f,
                combinedGripPenalty: 0.90f, slipAnglePeakDeg: 6.6f, slipAngleFalloff: 1.00f,
                turnResponse: 0.96f, massSensitivity: 0.96f, downforceGripGain: 0.03f,
                cornerStiffnessFront: 1.46f, cornerStiffnessRear: 0.96f, yawInertiaScale: 1.08f, steeringCurve: 1.05f, transientDamping: 1.40f,
                torqueCurvePreset: "supersport_bike",
                transmissionPolicy: Policy(6, false, upshiftRpmFraction: 0.90f)),

            new OfficialVehicleSpec(
                CarType.Vehicle11, "Ducati Panigale V4",
                hasWipers: 0, surfaceTractionFactor: 0.10f, deceleration: 0.55f, topSpeed: 165.0f,
                idleFreq: 22050, topFreq: 60000, shiftFreq: 35000, gears: 6, steering: 1.04f,
                idleRpm: 1200f, maxRpm: 15000f, revLimiter: 14500f, autoShiftRpm: 14500f * 0.92f, engineBraking: 0.25f,
                massKg: 191f, drivetrainEfficiency: 0.92f, engineBrakingTorqueNm: 121f, tireGripCoefficient: 1.12f,
                peakTorqueNm: 121f, peakTorqueRpm: 10000f, idleTorqueNm: 121f * 0.3f, redlineTorqueNm: 121f * 0.25f,
                dragCoefficient: 0.66f, frontalAreaM2: 0.66f, rollingResistanceCoefficient: 0.018f, launchRpm: 3900f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 10f, drivelineCouplingRate: 12f,
                finalDriveRatio: 4.95f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(200, 60, 17), lateralGripCoefficient: 0.77f, highSpeedStability: 0.45f,
                wheelbaseM: 1.469f, maxSteerDeg: 35f, widthM: 0.806f, lengthM: 2.110f,
                powerFactor: 0.66f, gearRatios: PanigaleV4Ratios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.81f, highSpeedSteerStartKph: 115f, highSpeedSteerFullKph: 190f,
                combinedGripPenalty: 0.90f, slipAnglePeakDeg: 6.7f, slipAngleFalloff: 1.02f,
                turnResponse: 0.94f, massSensitivity: 0.95f, downforceGripGain: 0.03f,
                cornerStiffnessFront: 1.42f, cornerStiffnessRear: 0.98f, yawInertiaScale: 1.06f, steeringCurve: 1.06f, transientDamping: 1.38f,
                torqueCurvePreset: "supersport_bike",
                transmissionPolicy: Policy(5, true, Auto6Upshifts, upshiftRpmFraction: 0.90f)),

            new OfficialVehicleSpec(
                CarType.Vehicle12, "Yamaha YZF-R1",
                hasWipers: 0, surfaceTractionFactor: 0.085f, deceleration: 0.48f, topSpeed: 160.0f,
                idleFreq: 22050, topFreq: 27550, shiftFreq: 23550, gears: 6, steering: 1.10f,
                idleRpm: 1100f, maxRpm: 14500f, revLimiter: 14000f, autoShiftRpm: 14000f * 0.92f, engineBraking: 0.30f,
                massKg: 201f, drivetrainEfficiency: 0.92f, engineBrakingTorqueNm: 113.3f, tireGripCoefficient: 1.10f,
                peakTorqueNm: 112.4f, peakTorqueRpm: 11500f, idleTorqueNm: 112.4f * 0.3f, redlineTorqueNm: 112.4f * 0.25f,
                dragCoefficient: 0.68f, frontalAreaM2: 0.66f, rollingResistanceCoefficient: 0.018f, launchRpm: 3900f,
                engineInertiaKgm2: 0.24f, engineFrictionTorqueNm: 9f, drivelineCouplingRate: 12f,
                finalDriveRatio: 4.45f, reverseMaxSpeedKph: 35f, reversePowerFactor: 0.55f, reverseGearRatio: 3.2f,
                tireCircumferenceM: TireCircumferenceM(190, 55, 17), lateralGripCoefficient: 0.75f, highSpeedStability: 0.47f,
                wheelbaseM: 1.405f, maxSteerDeg: 35f, widthM: 0.690f, lengthM: 2.055f,
                powerFactor: 0.68f, gearRatios: R1Ratios!, brakeStrength: 1.0f,
                highSpeedSteerGain: 0.82f, highSpeedSteerStartKph: 115f, highSpeedSteerFullKph: 190f,
                combinedGripPenalty: 0.91f, slipAnglePeakDeg: 6.5f, slipAngleFalloff: 1.01f,
                turnResponse: 0.95f, massSensitivity: 0.95f, downforceGripGain: 0.03f,
                cornerStiffnessFront: 1.44f, cornerStiffnessRear: 0.95f, yawInertiaScale: 1.08f, steeringCurve: 1.05f, transientDamping: 1.40f,
                torqueCurvePreset: "supersport_bike",
                transmissionPolicy: Policy(5, true, Auto6Upshifts, upshiftRpmFraction: 0.88f))
        };

    }
}


