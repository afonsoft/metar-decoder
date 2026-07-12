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
            return $"^(WS ALL RWY|({RunwayRegexPattern})( {RunwayRegexPattern})?( {RunwayRegexPattern})?)( )";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            bool? all;
            List<string> runways;
            ParseRunways(this, found, remainingMetar, newRemainingMetar, out all, out runways);

            result.Add(WindshearAllRunwaysParameterName, all);
            result.Add(WindshearRunwaysParameterName, runways);

            return GetResults(newRemainingMetar, result);
        }

        private static void ParseRunways(MetarChunkDecoder decoder, List<Group> found, string remainingMetar, string newRemainingMetar, out bool? all, out List<string> runways)
        {
            if (found.Count <= 1)
            {
                all = null;
                runways = new List<string>();
                return;
            }

            // detect if we have windshear on all runway or only one
            if (found[1].Value == "WS ALL RWY")
            {
                all = true;
                runways = null;
                return;
            }

            // one or more runways, build array
            all = false;
            runways = new List<string>();
            for (var k = 2; k < 9; k += 3)
            {
                if (!string.IsNullOrEmpty(found[k].Value))
                {
                    runways.Add(ExtractRunway(decoder, found, k, remainingMetar, newRemainingMetar));
                }
            }
        }

        private static string ExtractRunway(MetarChunkDecoder decoder, List<Group> found, int k, string remainingMetar, string newRemainingMetar)
        {
            var runway = found[k + 2].Value;
            var qfuAsInt = Value.ToInt(runway);
            // check runway qfu validity
            if (qfuAsInt > 36 || qfuAsInt < 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.InvalidRunwayQFURunwaVisualRangeInformation, decoder);
            }

            return runway;
        }
    }
}
