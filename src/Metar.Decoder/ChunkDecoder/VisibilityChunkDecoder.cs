using Metar.Decoder.Entity;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Metar.Decoder.ChunkDecoder
{
    public sealed class VisibilityChunkDecoder : MetarChunkDecoder
    {
        public const string CavokParameterName = "Cavok";
        public const string VisibilityParameterName = "Visibility";

        private const string CavokRegexPattern = "CAVOK";
        private const string VisibilityRegexPattern = "([0-9]{4})(NDV)?";
        private const string UsVisibilityRegexPattern = "M?([0-9]{0,2}) ?(([1357])/(2|4|8|16))?SM";
        private const string MinimumVisibilityRegexPattern = "( ([0-9]{4})(N|NE|E|SE|S|SW|W|NW)?)?"; // optional
        private const string NoInfoRegexPattern = "////";

        public override string GetRegex()
        {
            return $"^({CavokRegexPattern}|{VisibilityRegexPattern}{MinimumVisibilityRegexPattern}|{UsVisibilityRegexPattern}|{NoInfoRegexPattern})( )";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            if (found.Count <= 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.ForVisibilityInformationBadFormat, this);
            }

            var parsedVisibility = ParseVisibility(found);

            result.Add(CavokParameterName, parsedVisibility.Cavok);
            result.Add(VisibilityParameterName, parsedVisibility.Visibility);

            return GetResults(newRemainingMetar, result);
        }

        private static (bool Cavok, Visibility Visibility) ParseVisibility(List<Group> found)
        {
            return found[1].Value switch
            {
                CavokRegexPattern => (true, null),
                "////" => (false, null),
                _ => (false, ParseDetailedVisibility(found)),
            };
        }

        private static Visibility ParseDetailedVisibility(List<Group> found)
        {
            var visibility = new Visibility();

            if (!string.IsNullOrEmpty(found[2].Value))
            {
                ParseIcaoVisibility(found, visibility);
            }
            else
            {
                ParseUsVisibility(found, visibility);
            }

            return visibility;
        }

        private static void ParseIcaoVisibility(List<Group> found, Visibility visibility)
        {
            visibility.PrevailingVisibility = new Value(Convert.ToDouble(found[2].Value), Value.Unit.Meter);

            if (!string.IsNullOrEmpty(found[4].Value))
            {
                visibility.MinimumVisibility = new Value(Convert.ToDouble(found[5].Value), Value.Unit.Meter);
                visibility.MinimumVisibilityDirection = found[6].Value;
            }

            visibility.NDV = !string.IsNullOrEmpty(found[3].Value);
        }

        private static void ParseUsVisibility(List<Group> found, Visibility visibility)
        {
            double visibilityValue = 0;

            if (!string.IsNullOrEmpty(found[7].Value))
            {
                visibilityValue += Convert.ToInt32(found[7].Value);
            }

            if (!string.IsNullOrEmpty(found[9].Value) && !string.IsNullOrEmpty(found[10].Value))
            {
                var fractionTop = Convert.ToInt32(found[9].Value);
                var fractionBottom = Convert.ToInt32(found[10].Value);
                if (fractionBottom != 0)
                {
                    visibilityValue += (double)fractionTop / fractionBottom;
                }
            }

            visibility.PrevailingVisibility = new Value(visibilityValue, Value.Unit.StatuteMile);
        }
    }
}