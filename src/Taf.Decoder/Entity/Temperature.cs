namespace Taf.Decoder.entity
{
    public class Temperature : AbstractEntity
    {
        /// <summary>
        /// Annotation defining whether it's the minimum or maximum forecast temperature
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Temperature value
        /// </summary>
        public Value TemperatureValue { get; set; }

        /// <summary>
        /// Day of occurrence
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Hur of occurrence
        /// </summary>
        public int Hour { get; set; }
    }
}