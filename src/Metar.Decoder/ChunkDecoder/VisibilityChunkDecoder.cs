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

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.ForVisibilityInformationBadFormat, this);
            }

            bool cavok;
            var visibility = BuildVisibility(found, out cavok);

            result.Add(CavokParameterName, cavok);
            result.Add(VisibilityParameterName, visibility);

            return GetResults(newRemainingMetar, result);
        }

        private static Visibility BuildVisibility(List<Group> found, out bool cavok)
        {
            if (found[1].Value == CavokRegexPattern)
            {
                cavok = true;
                return null;
            }

            cavok = false;
            if (found[1].Value == "////")
            {
                return null;
            }

            var visibility = new Visibility();
            if (!string.IsNullOrEmpty(found[2].Value))
            {
                ParseIcaoVisibility(visibility, found);
            }
            else
            {
                ParseUsVisibility(visibility, found);
            }

            return visibility;
        }

        private static void ParseIcaoVisibility(Visibility visibility, List<Group> found)
        {
            visibility.PrevailingVisibility = new Value(Convert.ToDouble(found[2].Value), Value.Unit.Meter);
            visibility.NDV = !string.IsNullOrEmpty(found[3].Value);

            if (!string.IsNullOrEmpty(found[5].Value))
            {
                visibility.MinimumVisibility = new Value(Convert.ToDouble(found[5].Value), Value.Unit.Meter);
                visibility.MinimumVisibilityDirection = found[6].Value;
            }
        }

        private static void ParseUsVisibility(Visibility visibility, List<Group> found)
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
