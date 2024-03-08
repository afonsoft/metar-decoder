using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("IcaoChunkDecoder")]
    public class IcaoChunkDecoderTest
    {
        private static readonly IcaoChunkDecoder chunkDecoder = new IcaoChunkDecoder();

        /// <summary>
        /// Test parsing of valid icao chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public static void testParseIcaoChunk(Tuple<string, string, string> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);
            ClassicAssert.AreEqual(chunk.Item2, (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[IcaoChunkDecoder.ICAOParameterName]);
            ClassicAssert.AreEqual(chunk.Item3, decoded[TafDecoder.RemainingTafKey]);
        }

        /// <summary>
        /// Test parsing of invalid icao chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public static void TestParseInvalidChunk(string chunk)
        {
            ClassicAssert.Throws(typeof(TafChunkDecoderException), () =>
             {
                 chunkDecoder.Parse(chunk);
             });
        }

        public static List<Tuple<string, string, string>> ValidChunks => new List<Tuple<string, string, string>>()
        {
            new Tuple<string, string, string>("LFPG AAA", "LFPG", "AAA"),
            new Tuple<string, string, string>("LFPO BBB", "LFPO", "BBB"),
            new Tuple<string, string, string>("LFIO CCC", "LFIO", "CCC"),
        };

        public static List<string> InvalidChunks => new List<string>()
        {
            "LFA AAA", "L AAA", "LFP BBB", "LF8 CCC"
        };
    }
}