using System;
using System.Collections.Generic;

namespace Taf.Decoder.chunkdecoder
{
    public sealed class ReportTypeChunkDecoder : TafChunkDecoder
    {
        public const string TypeParameterName = "Type";

        public override string GetRegex()
        {
            return "^((TAF|RTD)( TAF)*( AMD| COR){0,1}) ";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                result.Add(TypeParameterName, entity.DecodedTaf.TafType.NULL);
            }
            else
            {
                // retrieve found params
                // 'TAF' sometimes happens to be duplicated
                result.Add(TypeParameterName, (entity.DecodedTaf.TafType)Enum.Parse(typeof(entity.DecodedTaf.TafType), found[1].Value.Replace("TAF TAF", "TAF").Replace(" ", string.Empty)));
            }

            return GetResults(newRemainingTaf, result);
        }
    }
}