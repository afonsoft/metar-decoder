using System;
using System.Collections.Generic;
using Taf.Decoder.entity;

namespace Taf.Decoder.chunkdecoder
{
    public class ForecastPeriodChunkDecoder : TafChunkDecoder
    {
        public const string ForecastPeriodParameterName = "ForecastPeriod";

        public override string GetRegex()
        {
            return "^([0-9]{2})([0-9]{2})/([0-9]{2})([0-9]{2}) ";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.InvalidForecastPeriodInformation, this);
            }
            else
            {
                // retrieve found params and check them
                var forecastPeriod = new ForecastPeriod()
                {
                    FromDay = Convert.ToInt32(found[1].Value),
                    FromHour = Convert.ToInt32(found[2].Value),
                    ToDay = Convert.ToInt32(found[3].Value),
                    ToHour = Convert.ToInt32(found[4].Value),
                };
                if (!forecastPeriod.IsValid)
                {
                    throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.InvalidValuesForTheForecastPeriod, this);
                }

                result.Add(ForecastPeriodParameterName, forecastPeriod);
            }

            return GetResults(newRemainingTaf, result);
        }
    }
}