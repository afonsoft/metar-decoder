using System.Collections.Generic;

namespace Taf.Decoder.Entity
{
    public class AbstractEntity
    {
        /// <summary>
        /// An evolution can contain embedded evolutions with different probabilities
        /// </summary>
        public List<Evolution> Evolutions { get; set; } = new List<Evolution>();
    }
}