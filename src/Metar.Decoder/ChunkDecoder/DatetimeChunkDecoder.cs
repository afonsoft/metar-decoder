using System;
using System.Collections.Generic;

namespace Metar.Decoder.Chunkdecoder
{
    public sealed class DatetimeChunkDecoder : MetarChunkDecoder
    {
        public const string DayParameterName = "Day";
        public const string TimeParameterName = "Time";
        public const string ObservationDateTimeParameterName = "ObservationDateTime";

        public override string GetRegex()
        {
            return "^([0-9]{2})([0-9]{2})([0-9]{2})Z ";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.BadDayHourMinuteInformation, this);
            }

            // retrieve found params and check them
            var day = Convert.ToInt32(found[1].Value);
            var hour = Convert.ToInt32(found[2].Value);
            var minute = Convert.ToInt32(found[3].Value);

            if (!CheckValidity(day, hour, minute))
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.InvalidDayHourMinuteRanges, this);
            }

            result.Add(DayParameterName, day);
            result.Add(TimeParameterName, $"{hour:00}:{minute:00} UTC");

            // Create DateTime from parsed components
            var currentYear = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            
            // Handle day/year rollover - if day > current day, assume previous month
            if (day > DateTime.Now.Day)
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
            
            var observationDateTime = new DateTime(currentYear, month, day, hour, minute, 0, DateTimeKind.Utc);
            result.Add(ObservationDateTimeParameterName, observationDateTime);

            return GetResults(newRemainingMetar, result);
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