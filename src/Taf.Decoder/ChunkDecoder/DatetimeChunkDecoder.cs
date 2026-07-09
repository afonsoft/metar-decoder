using System;
using System.Collections.Generic;

namespace Taf.Decoder.ChunkDecoder
{
    public sealed class DatetimeChunkDecoder : TafChunkDecoder
    {
        public const string DayParameterName = "Day";
        public const string TimeParameterName = "Time";
        public const string OriginDateTimeParameterName = "OriginDateTime";

        private readonly DateTime? _referenceDate;

        /// <summary>
        /// DatetimeChunkDecoder
        /// </summary>
        public DatetimeChunkDecoder()
        {
        }

        /// <summary>
        /// DatetimeChunkDecoder
        /// </summary>
        /// <param name="referenceDate">Reference date for rollover calculations</param>
        public DatetimeChunkDecoder(DateTime referenceDate)
        {
            _referenceDate = referenceDate;
        }

        public override string GetRegex()
        {
            return "^([0-9]{2})([0-9]{2})([0-9]{2})Z ";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.BadDayHourMinuteInformation, this);
            }

            // retrieve found params and check them
            var day = Convert.ToInt32(found[1].Value);
            var hour = Convert.ToInt32(found[2].Value);
            var minute = Convert.ToInt32(found[3].Value);

            if (!CheckValidity(day, hour, minute))
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.InvalidDayHourMinuteRanges, this);
            }

            result.Add(DayParameterName, day);
            result.Add(TimeParameterName, $"{hour:00}:{minute:00} UTC");

            var originDateTime = BuildOriginDateTime(day, hour, minute);
            result.Add(OriginDateTimeParameterName, originDateTime);

            return GetResults(newRemainingTaf, result);
        }

        /// <summary>
        /// Build the origin DateTime from parsed components applying rollover logic.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        private DateTime BuildOriginDateTime(int day, int hour, int minute)
        {
            // Use the provided reference date, or the current UTC time when none is supplied
            var referenceDate = _referenceDate ?? DateTime.UtcNow;

            // Create DateTime from parsed components
            var currentYear = referenceDate.Year;
            var month = referenceDate.Month;

            // Handle day/year rollover - if day > current day, assume previous month
            if (day > referenceDate.Day)
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

            // Ensure day is valid for the resolved month/year
            var daysInMonth = DateTime.DaysInMonth(currentYear, month);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            return new DateTime(currentYear, month, day, hour, minute, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// Check the validity of the decoded information for date time.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        private bool CheckValidity(int day, int hour, int minute)
        {
            // check value range
            if (day < 1 || day > 31)
            {
                return false;
            }
            if (hour < 0 || hour > 23)
            {
                return false;
            }
            if (minute < 0 || minute > 59)
            {
                return false;
            }
            return true;
        }
    }
}