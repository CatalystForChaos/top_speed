namespace TopSpeed.Bots
{
    public struct BotPhysicsState
    {
        public float PositionX;
        public float PositionY;
        public float SpeedKph;
        public float LateralVelocityMps;
        public float YawRateRad;
        public int Gear;
        public float AutoShiftCooldownSeconds;
        /// <summary>Remaining seconds of the gear-shift torque interruption (drivetrain disconnected).</summary>
        public float ShiftTransientSeconds;
    }
}
