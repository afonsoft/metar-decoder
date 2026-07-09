using System;

namespace Decoder.Shared
{
    /// <summary>
    /// Helper for building DateTime values from METAR/TAF day, hour and minute
    /// components, applying month/year rollover and day clamping.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Builds a UTC DateTime from the parsed day, hour and minute using the
        /// optional reference date for month/year resolution.
        /// </summary>
        /// <param name="referenceDate">Optional reference date. When null, UtcNow is used.</param>
        /// <param name="day">Day of the month.</param>
        /// <param name="hour">Hour of the day.</param>
        /// <param name="minute">Minute of the hour.</param>
        /// <returns>Resolved UTC DateTime.</returns>
        public static DateTime BuildDateTime(DateTime? referenceDate, int day, int hour, int minute)
        {
            var reference = referenceDate ?? DateTime.UtcNow;

            var currentYear = reference.Year;
            var month = reference.Month;
            if (day > reference.Day)
            {
                if (month == 1)
                {
                    month = 12;
                    currentYear--;
                }
                else
                {
                    month--;
                }
            }

            var daysInMonth = DateTime.DaysInMonth(currentYear, month);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            return new DateTime(currentYear, month, day, hour, minute, 0, DateTimeKind.Utc);
        }
    }
}
