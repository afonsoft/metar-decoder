using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Metar.Decoder.Entity
{
    /// <summary>
    /// Represents a weather phenomenon decoded from a METAR or TAF report,
    /// including intensity/proximity, descriptor characteristics (e.g., FZ, TS, SH),
    /// and precipitation/obscuration types (e.g., RA, SN, FG, BR).
    /// </summary>
    public sealed class WeatherPhenomenon
    {
        private readonly List<string> _types = new();

        /// <summary>
        /// Intensity/proximity of the phenomenon + / - / VC (=vicinity)
        /// </summary>
        public string IntensityProximity { get; set; }

        /// <summary>
        /// Characteristics of the phenomenon
        /// </summary>
        public string Characteristics { get; set; }

        /// <summary>
        /// Types of phenomenon
        /// </summary>
        public ReadOnlyCollection<string> Types
        {
            get
            {
                return new ReadOnlyCollection<string>(_types);
            }
        }

        /// <summary>
        /// Adds a weather type code (e.g., RA, SN, FG) to this phenomenon.
        /// </summary>
        /// <param name="type">The METAR/TAF weather type code to add.</param>
        public void AddType(string type)
        {
            _types.Add(type);
        }
    }
}