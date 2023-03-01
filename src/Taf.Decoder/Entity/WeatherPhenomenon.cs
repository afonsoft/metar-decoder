using System.Collections.Generic;

namespace Taf.Decoder.entity
{
    public sealed class WeatherPhenomenon : AbstractEntity
    {
        private readonly List<string> _types = new List<string>();

        /// <summary>
        /// Intensity/proximity of the phenomenon + / - / VC (=vicinity)
        /// </summary>
        public string IntensityProximity { get; set; }

        /// <summary>
        /// Characteristics of the phenomenon
        /// </summary>
        public string Descriptor { get; set; }

        /// <summary>
        /// Types of phenomena
        /// </summary>
        public List<string> Phenomena { get; set; } = new List<string>();
    }
}