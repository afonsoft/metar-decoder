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
                result.Add(RunwaysVisualRangeParameterName, ParseRunways(found, remainingMetar, newRemainingMetar));
            }

            return GetResults(newRemainingMetar, result);
        }

        private List<RunwayVisualRange> ParseRunways(List<Group> found, string remainingMetar, string newRemainingMetar)
        {
            var runways = new List<RunwayVisualRange>();

            for (var i = 1; i <= 20; i += 7)
            {
                if (!string.IsNullOrEmpty(found[i].Value))
                {
                    runways.Add(ParseRunway(found, i, remainingMetar, newRemainingMetar));
                }
            }

            return runways;
        }

        private RunwayVisualRange ParseRunway(List<Group> found, int index, string remainingMetar, string newRemainingMetar)
        {
            ValidateRunway(found[index + 1].Value, remainingMetar, newRemainingMetar);

            var rangeUnit = GetRangeUnit(found[index + 5].Value);
            var observation = new RunwayVisualRange
            {
                Runway = found[index + 1].Value,
                PastTendency = GetTendency(found[index + 6].Value),
            };

            if (!string.IsNullOrEmpty(found[index + 3].Value))
            {
                observation.Variable = true;
                observation.VisualRangeInterval = new Value[]
                {
                    ToValue(found[index + 3].Value, rangeUnit),
                    ToValue(found[index + 4].Value, rangeUnit),
                };
            }
            else
            {
                observation.Variable = false;
                observation.VisualRange = ToValue(found[index + 4].Value, rangeUnit);
            }

            return observation;
        }

        private void ValidateRunway(string runway, string remainingMetar, string newRemainingMetar)
        {
            var qfuAsInt = Value.ToInt(runway);
            if (!qfuAsInt.HasValue || qfuAsInt.Value > 36 || qfuAsInt.Value < 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.InvalidRunwayQFURunwayVisualRangeInformation, this);
            }
        }

        private static Value.Unit GetRangeUnit(string value)
        {
            return value == "FT" ? Value.Unit.Feet : Value.Unit.Meter;
        }

        private static Tendency GetTendency(string value)
        {
            return value switch
            {
                "U" => Tendency.U,
                "D" => Tendency.D,
                "N" => Tendency.N,
                _ => Tendency.NONE,
            };
        }

        private static Value ToValue(string value, Value.Unit unit)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var measuredValue = Value.ToInt(value);
            return measuredValue.HasValue ? new Value(measuredValue.Value, unit) : null;
        }
    }
}