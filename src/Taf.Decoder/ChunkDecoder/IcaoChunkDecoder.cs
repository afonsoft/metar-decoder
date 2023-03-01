using System.Collections.Generic;

namespace Taf.Decoder.chunkdecoder
{
    public sealed class IcaoChunkDecoder : TafChunkDecoder
    {
        public const string ICAOParameterName = "Icao";

        public override string GetRegex()
        {
            return "^([A-Z0-9]{4}) ";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            // handle the case where nothing has been found
            if (found.Count <= 1)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.ICAONotFound, this);
            }

            // retrieve found params
            result.Add(ICAOParameterName, found[1].Value);

            return GetResults(newRemainingTaf, result);
        }
    }
}