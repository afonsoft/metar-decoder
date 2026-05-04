namespace Metar.Decoder.Entity
{
    /// <summary>
    /// Represents visibility information decoded from a METAR report,
    /// including prevailing visibility, minimum directional visibility, and NDV indicator.
    /// </summary>
    public sealed class Visibility
    {
        /// <summary>
        /// Prevailing visibility
        /// </summary>
        public Value PrevailingVisibility { get; set; }

        /// <summary>
        /// Minimum visibility
        /// </summary>
        public Value MinimumVisibility { get; set; }

        /// <summary>
        /// Direction of minimum visibility
        /// </summary>
        public string MinimumVisibilityDirection { get; set; }

        /// <summary>
        /// No Directional Variation. When true, visibility is the same in all directions.
        /// </summary>
        public bool NDV { get; set; } = false;
    }
}