using System;
using System.Collections.Generic;
using Taf.Decoder.entity;

namespace Taf.Decoder.chunkdecoder
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

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.ForVisibilityInformationBadFormat, this);
            }

            var cavok = false;
            Visibility visibility = null;
            if (found[1].Value == CavokRegexPattern)
            {
                // cloud and visibility OK
                cavok = true;
                visibility = null;
            }
            else if (found[1].Value == NoInfoRegexPattern)
            {
                // information not available
                cavok = false;
                visibility = null;
            }
            else
            {
                cavok = false;
                visibility = new Visibility();
                if (!string.IsNullOrEmpty(found[2].Value.Trim()))
                {
                    // icao visibility
                    visibility.ActualVisibility = new Value(Convert.ToDouble(found[2].Value), Value.Unit.Meter);
                }
                else
                {
                    // us visibility
                    var main = Convert.ToDouble(found[4].Value);
                    var isGreater = found[3].Value == "P";

                    int fractionTop;
                    int fractionBottom;
                    var visibilityValue = main;
                    if (int.TryParse(found[6].Value, out fractionTop) && int.TryParse(found[7].Value, out fractionBottom))
                    {
                        if (fractionBottom != 0)
                        {
                            visibilityValue = (double)main + (double)fractionTop / fractionBottom;
                        }
                        else
                        {
                            visibilityValue = main;
                        }
                    }
                    visibility.ActualVisibility = new Value(visibilityValue, Value.Unit.StatuteMile);
                    visibility.Greater = isGreater;
                }
            }

            result.Add(CavokParameterName, cavok);
            result.Add(VisibilityParameterName, visibility);

            return GetResults(newRemainingTaf, result);
        }
    }
}