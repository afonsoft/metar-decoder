using Metar.Decoder.Entity;
using System.Collections.Generic;

namespace Metar.Decoder.ChunkDecoder
{
    /// <summary>
    /// Chunk decoder for METAR trend forecast section (NOSIG, BECMG, TEMPO).
    /// Per ICAO Annex 3, METAR can include a trend forecast indicating expected changes.
    /// </summary>
    public sealed class TrendChunkDecoder : MetarChunkDecoder
    {
        public const string TrendForecastParameterName = "TrendForecast";
        public const string TrendTypeParameterName = "TrendType";

        public override string GetRegex()
        {
            return @"^(NOSIG|BECMG\s+.*|TEMPO\s+.*)( )";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            if (found.Count > 1)
            {
                var trendRaw = found[1].Value.Trim();
                string trendType;

                if (trendRaw.StartsWith("NOSIG"))
                {
                    trendType = "NOSIG";
                }
                else if (trendRaw.StartsWith("BECMG"))
                {
                    trendType = "BECMG";
                }
                else
                {
                    trendType = "TEMPO";
                }

                result.Add(TrendTypeParameterName, trendType);
                result.Add(TrendForecastParameterName, trendRaw);
            }

            return GetResults(newRemainingMetar, result);
        }
    }
}
