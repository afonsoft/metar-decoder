﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metar.Decoder.Chunkdecoder
{
    public interface IMetarChunkDecoder
    {
        /// <summary>
        /// Get the regular expression that will be used by chunk decoder
        /// Each chunk decoder must declare its own.
        /// </summary>
        string GetRegex();

        /// <summary>
        /// Decode the chunk targeted by the chunk decoder and returns the
        /// decoded information and the remaining metar without this chunk.
        /// </summary>
        /// <param name="remainingMetar"></param>
        /// <param name="withCavok"></param>
        Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false);
    }
}