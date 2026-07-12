using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Taf.Decoder.Entity;

namespace Taf.Decoder.ChunkDecoder
{
    public sealed class VisibilityChunkDecoder : TafChunkDecoder
    {
        public const string CavokParameterName = "Cavok";
        public const string VisibilityParameterName = "Visibility";

        private const string CavokRegexPattern = "CAVOK";
        private const string VisibilityRegexPattern = "([0-9]{4})?";
        private const string UsVisibilityRegexPattern = "M?(P)?([0-9]{0,2}) ?(([1357])/(2|4|8|16))?SM";
        private const string NoInfoRegexPattern = "////";

        public override string GetRegex()
        {
            return $"^({CavokRegexPattern}|{VisibilityRegexPattern}|{UsVisibilityRegexPattern}|{NoInfoRegexPattern})( )";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            if (found.Count <= 1)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.ForVisibilityInformationBadFormat, this);
            }

            var (cavok, visibility) = BuildVisibility(found);

            result.Add(CavokParameterName, cavok);
            result.Add(VisibilityParameterName, visibility);

            return GetResults(newRemainingTaf, result);
        }

        private static (bool Cavok, Visibility Visibility) BuildVisibility(List<Group> found)
        {
            if (found[1].Value == CavokRegexPattern)
            {
                return (true, null);
            }

            if (found[1].Value == NoInfoRegexPattern)
            {
                return (false, null);
            }

            var visibility = new Visibility();
            if (!string.IsNullOrEmpty(found[2].Value.Trim()))
            {
                visibility.ActualVisibility = new Value(Convert.ToDouble(found[2].Value), Value.Unit.Meter);
            }
            else
            {
                visibility.ActualVisibility = BuildUsVisibility(found);
                visibility.Greater = found[3].Value == "P";
            }

            return (false, visibility);
        }

        private static Value BuildUsVisibility(List<Group> found)
        {
            var main = Convert.ToDouble(found[4].Value);
            var visibilityValue = main;

            if (int.TryParse(found[6].Value, out var fractionTop) && int.TryParse(found[7].Value, out var fractionBottom) && fractionBottom != 0)
            {
                visibilityValue = main + fractionTop / (double)fractionBottom;
            }

            return new Value(visibilityValue, Value.Unit.StatuteMile);
        }
    }
}