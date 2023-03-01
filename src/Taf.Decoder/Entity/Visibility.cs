namespace Taf.Decoder.entity
{
    public sealed class Visibility : AbstractEntity
    {
        /// <summary>
        /// Prevailing visibility
        /// </summary>
        public Value ActualVisibility { get; set; }

        /// <summary>
        /// Visibility is greater than the given value
        /// </summary>
        public bool Greater { get; set; }
    }
}