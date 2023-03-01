using System;
using System.Collections.Generic;
using Taf.Decoder.entity;

namespace Taf.Decoder.chunkdecoder
{
    public sealed class TemperatureChunkDecoder : TafChunkDecoder
    {
        public const string MinimumTemperatureParameterName = "MinimumTemperature";
        public const string MaximumTemperatureParameterName = "MaximumTemperature";

        private const string TempRegexPattern = "(TX|TN){1}(M?[0-9]{2})/([0-9]{2})([0-9]{2})Z";

        public override string GetRegex()
        {
            return $"^{TempRegexPattern} {TempRegexPattern}?( )?";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            // temperatures are so often missing from forecasts we consider them as optional
            Temperature maximumTemperature = null;
            Temperature minimumTemperature = null;

            if (found.Count > 1 && !string.IsNullOrEmpty(found[2].Value.Trim()) && !string.IsNullOrEmpty(found[6].Value.Trim()))
            {
                // retrieve found params
                maximumTemperature = new Temperature()
                {
                    Type = found[1].Value,
                    TemperatureValue = new Value(Convert.ToDouble(found[2].Value), Value.Unit.DegreeCelsius),
                    Day = Convert.ToInt32(found[3].Value),
                    Hour = Convert.ToInt32(found[4].Value),
                };

                if (!string.IsNullOrEmpty(found[5].Value.Trim()))
                {
                    minimumTemperature = new Temperature()
                    {
                        Type = found[5].Value,
                        TemperatureValue = new Value(Convert.ToDouble(found[6].Value.Replace("M", "-")), Value.Unit.DegreeCelsius),
                        Day = Convert.ToInt32(found[7].Value),
                        Hour = Convert.ToInt32(found[8].Value),
                    };
                }

                // handle the case where min and max temperatures are inconsistent
                if (minimumTemperature != null && minimumTemperature.TemperatureValue.ActualValue > maximumTemperature.TemperatureValue.ActualValue)
                {
                    throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.InconsistentValuesForTemperatureInformation, this);
                }

                result.Add(MinimumTemperatureParameterName, minimumTemperature);
                result.Add(MaximumTemperatureParameterName, maximumTemperature);
            }

            return GetResults(newRemainingTaf, result);
        }
    }
}