namespace Taf.Decoder.entity
{
    public sealed class ForecastPeriod
    {
        /// <summary>
        /// Starting day of forecast period
        /// </summary>
        public int? FromDay { get; set; }

        /// <summary>
        /// Starting hour of forecast period
        /// </summary>
        public int? FromHour { get; set; }

        /// <summary>
        /// Ending day of forecast period
        /// </summary>
        public int? ToDay { get; set; }

        /// <summary>
        /// Ending hour of forecast period
        /// </summary>
        public int? ToHour { get; set; }

        public bool IsValid
        {
            get
            {
                // check that attribute aren't null
                if (!FromDay.HasValue || !FromHour.HasValue || !ToDay.HasValue || !ToHour.HasValue)
                {
                    return false;
                }
                // check ranges
                if (FromDay < 1 || FromDay > 31)
                {
                    return false;
                }
                if (ToDay < 1 || ToDay > 31)
                {
                    return false;
                }
                if (FromHour > 24 || ToHour > 24)
                {
                    return false;
                }
                if (FromDay == ToDay && FromHour >= ToHour)
                {
                    return false;
                }
                return true;
            }
        }
    }
}