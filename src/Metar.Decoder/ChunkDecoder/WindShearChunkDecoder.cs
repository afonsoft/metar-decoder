using Metar.Decoder.Entity;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Metar.Decoder.ChunkDecoder
{
    public sealed class WindShearChunkDecoder : MetarChunkDecoder
    {
        public const string WindshearAllRunwaysParameterName = "WindshearAllRunways";
        public const string WindshearRunwaysParameterName = "WindshearRunways";

        private const string RunwayRegexPattern = "WS R(WY)?([0-9]{2}[LCR]?)";

        public override string GetRegex()
        {
            return $@"^(WS ALL RWY|({RunwayRegexPattern})( {RunwayRegexPattern})?( {RunwayRegexPattern})?)( )";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            bool? all = null;
            var runways = new List<string>();

            if (found.Count > 1)
            {
                (all, runways) = ParseWindShear(found, remainingMetar, newRemainingMetar);
            }

            result.Add(WindshearAllRunwaysParameterName, all);
            result.Add(WindshearRunwaysParameterName, runways);

            return GetResults(newRemainingMetar, result);
        }

        private (bool? AllRunways, List<string> Runways) ParseWindShear(List<Group> found, string remainingMetar, string newRemainingMetar)
        {
            if (found[1].Value == "WS ALL RWY")
            {
                return (true, null);
            }

            return (false, ParseRunways(found, remainingMetar, newRemainingMetar));
        }

        private List<string> ParseRunways(List<Group> found, string remainingMetar, string newRemainingMetar)
        {
            var runways = new List<string>();

            for (var k = 2; k < 9; k += 3)
            {
                if (!string.IsNullOrEmpty(found[k].Value))
                {
                    var runway = found[k + 2].Value;
                    ValidateRunway(runway, remainingMetar, newRemainingMetar);
                    runways.Add(runway);
                }
            }

            return runways;
        }

        private void ValidateRunway(string runway, string remainingMetar, string newRemainingMetar)
        {
            var qfuAsInt = Value.ToInt(runway);
            if (qfuAsInt > 36 || qfuAsInt < 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.InvalidRunwayQFURunwaVisualRangeInformation, this);
            }
        }
    }
}