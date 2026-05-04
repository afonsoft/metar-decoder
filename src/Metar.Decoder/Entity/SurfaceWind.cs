namespace Metar.Decoder.Entity
{
    /// <summary>
    /// Represents surface wind information decoded from a METAR report,
    /// including mean direction, speed, gusts, and variable direction range.
    /// </summary>
    public sealed class SurfaceWind
    {
        /// <summary>
        /// Wind direction
        /// </summary>
        public Value MeanDirection { get; set; }

        /// <summary>
        /// Indicates whether the wind direction is variable (VRB). When true, <see cref="MeanDirection"/> is null.
        /// </summary>
        public bool VariableDirection { get; set; }

        /// <summary>
        /// Wind speed
        /// </summary>
        public Value MeanSpeed { get; set; }

        /// <summary>
        /// Wind speed variation (gusts)
        /// </summary>
        public Value SpeedVariations { get; set; }

        /// <summary>
        /// Boundaries for wind direction variation
        /// </summary>
        public Value[] DirectionVariations { get; private set; } = null;

        /// <summary>
        /// Sets the variable wind direction boundaries (e.g., 180V240 means wind varies between 180 and 240 degrees).
        /// </summary>
        /// <param name="directionMax">Maximum wind direction in degrees.</param>
        /// <param name="directionMin">Minimum wind direction in degrees.</param>
        public void SetDirectionVariations(Value directionMax, Value directionMin)
        {
            DirectionVariations = new Value[] { directionMax, directionMin };
        }
    }
}