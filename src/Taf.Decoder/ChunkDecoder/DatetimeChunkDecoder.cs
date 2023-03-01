using System;
using System.Collections.Generic;

namespace Taf.Decoder.chunkdecoder
{
    public sealed class DatetimeChunkDecoder : TafChunkDecoder
    {
        public const string DayParameterName = "Day";
        public const string TimeParameterName = "Time";

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

            if (!checkValidity(day, hour, minute))
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.InvalidDayHourMinuteRanges, this);
            }

            result.Add(DayParameterName, day);
            result.Add(TimeParameterName, $"{hour:00}:{minute:00} UTC");

            return GetResults(newRemainingTaf, result);
        }

        /// <summary>
        /// Check the validity of the decoded information for date time.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        private bool checkValidity(int day, int hour, int minute)
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