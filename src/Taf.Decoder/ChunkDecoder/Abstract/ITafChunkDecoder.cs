using System.Collections.Generic;

namespace Taf.Decoder.chunkdecoder
{
    public interface ITafChunkDecoder
    {
        /// <summary>
        /// Get the regular expression that will be used by chunk decoder
        /// Each chunk decoder must declare its own.
        /// </summary>
        string GetRegex();

        /// <summary>
        /// Decode the chunk targeted by the chunk decoder and returns the
        /// decoded information and the remaining taf without this chunk.
        /// </summary>
        /// <param name="remainingTaf"></param>
        /// <param name="withCavok"></param>
        Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false);
    }
}