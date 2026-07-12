using Metar.Decoder.Entity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Metar.Decoder.Entity.RunwayVisualRange;

namespace Metar.Decoder.ChunkDecoder
{
    public sealed class RunwayVisualRangeChunkDecoder : MetarChunkDecoder
    {
        public const string RunwaysVisualRangeParameterName = "RunwaysVisualRange";

        private const string RunwayRegexPattern = "R([0-9]{2}[LCR]?)/([PM]?([0-9]{4})V)?[PM]?([0-9]{4})(FT)?/?([UDN]?)";

        public override string GetRegex()
        {
            return $"^({RunwayRegexPattern})( {RunwayRegexPattern})?( {RunwayRegexPattern})?( {RunwayRegexPattern})?( )";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            if (found.Count > 1)
            {
                var runways = ParseRunways(this, found, remainingMetar, newRemainingMetar);
                result.Add(RunwaysVisualRangeParameterName, runways);
            }

            return GetResults(newRemainingMetar, result);
        }

        private static List<RunwayVisualRange> ParseRunways(MetarChunkDecoder decoder, List<Group> found, string remainingMetar, string newRemainingMetar)
        {
            var runways = new List<RunwayVisualRange>();
            // iterate on the results to get all runways visual range found
            for (int i = 1; i <= 20; i += 7)
            {
                if (!string.IsNullOrEmpty(found[i].Value))
                {
                    runways.Add(ParseRunway(decoder, found, i, remainingMetar, newRemainingMetar));
                }
            }

            return runways;
        }

        private static RunwayVisualRange ParseRunway(MetarChunkDecoder decoder, List<Group> found, int i, string remainingMetar, string newRemainingMetar)
        {
            // check runway qfu validity
            var qfuAsInt = Value.ToInt(found[i + 1].Value);
            var qfu = qfuAsInt.GetValueOrDefault();
            if (qfu > 36 || qfu < 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.InvalidRunwayQFURunwayVisualRangeInformation, decoder);
            }

            // get distance unit
            var rangeUnit = GetRangeUnit(found[i + 5].Value);
            var tendency = GetTendency(found[i + 6].Value);
            var observation = new RunwayVisualRange()
            {
                Runway = found[i + 1].Value,
                PastTendency = tendency,
            };

            if (!string.IsNullOrEmpty(found[i + 3].Value))
            {
                observation.Variable = true;
                var min = CreateValue(found[i + 3].Value, rangeUnit);
                var max = CreateValue(found[i + 4].Value, rangeUnit);
                observation.VisualRangeInterval = new Value[] { min, max };
            }
            else
            {
                observation.Variable = false;
                observation.VisualRange = CreateValue(found[i + 4].Value, rangeUnit);
            }

            return observation;
        }

        private static Value CreateValue(string rawValue, Value.Unit rangeUnit)
        {
            if (string.IsNullOrEmpty(rawValue))
            {
                return null;
            }

            var value = Value.ToInt(rawValue);
            if (!value.HasValue)
            {
                return null;
            }

            return new Value(value.Value, rangeUnit);
        }

        private static Value.Unit GetRangeUnit(string unit)
        {
            return unit == "FT" ? Value.Unit.Feet : Value.Unit.Meter;
        }

        private static Tendency GetTendency(string tendency)
        {
            switch (tendency)
            {
                case "U":
                    return Tendency.U;

                case "D":
                    return Tendency.D;

                case "N":
                    return Tendency.N;

                default:
                    return Tendency.NONE;
            }
        }
    }
}
