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

            if (found.Count <= 1)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.ForVisibilityInformationBadFormat, this);
            }

            var result = DecodeVisibility(found);
            return GetResults(newRemainingTaf, result);
        }

        private static Dictionary<string, object> DecodeVisibility(List<Group> found)
        {
            var token = found[1].Value;
            var cavok = token == CavokRegexPattern;
            var visibility = token == CavokRegexPattern || token == NoInfoRegexPattern
                ? null
                : ParseVisibility(found);

            return new Dictionary<string, object>
            {
                { CavokParameterName, cavok },
                { VisibilityParameterName, visibility }
            };
        }

        private static Visibility ParseVisibility(List<Group> found)
        {
            var visibility = new Visibility();
            if (!string.IsNullOrEmpty(found[2].Value.Trim()))
            {
                visibility.ActualVisibility = new Value(Convert.ToDouble(found[2].Value), Value.Unit.Meter);
                return visibility;
            }

            visibility.ActualVisibility = new Value(ParseUsVisibility(found), Value.Unit.StatuteMile);
            visibility.Greater = found[3].Value == "P";
            return visibility;
        }

        private static double ParseUsVisibility(List<Group> found)
        {
            var main = Convert.ToDouble(found[4].Value);
            if (!int.TryParse(found[6].Value, out var fractionTop)
                || !int.TryParse(found[7].Value, out var fractionBottom)
                || fractionBottom == 0)
            {
                return main;
            }

            return main + (double)fractionTop / fractionBottom;
        }
    }
}