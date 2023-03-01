using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("VisibilityChunkDecoder")]
    public class VisibilityChunkDecoderTest
    {
        private static readonly VisibilityChunkDecoder chunkDecoder = new VisibilityChunkDecoder();

        /// <summary>
        /// Test parsing of valid visibility chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParse(Tuple<string, bool, bool, double?, Value.Unit, string> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);

            var visibility = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[VisibilityChunkDecoder.VisibilityParameterName] as Visibility;
            var cavok = (bool)((decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[VisibilityChunkDecoder.CavokParameterName]);

            Assert.AreEqual(chunk.Item2, cavok);

            if (!chunk.Item4.HasValue)
            {
                Assert.IsNull(visibility);
            }
            if (!cavok && visibility != null) 
            {
                Assert.AreEqual(chunk.Item4, visibility.ActualVisibility.ActualValue);
                Assert.AreEqual(chunk.Item5, visibility.ActualVisibility.ActualUnit);
                if (chunk.Item3)
                {
                    Assert.IsTrue(visibility.Greater);
                }
            }
            Assert.AreEqual(chunk.Item6, decoded[TafDecoder.RemainingTafKey]);
        }

        /// <summary>
        /// Test parsing of invalid visibility chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public static void TestParseInvalidChunk(string chunk)
        {
            var exception = Assert.Throws(typeof(TafChunkDecoderException), () =>
            {
                chunkDecoder.Parse(chunk);
            });
            Assert.AreEqual("Bad format for visibility information", exception.Message);
        }


        public static List<Tuple<string, bool, bool, double?, Value.Unit, string>> ValidChunks => new List<Tuple<string, bool, bool, double?, Value.Unit, string>>()
        {
            new Tuple<string, bool, bool, double?, Value.Unit, string>("0200 AAA",      false, false,    200, Value.Unit.Meter,         "AAA"),
            new Tuple<string, bool, bool, double?, Value.Unit, string>("CAVOK BBB",     true, false,    null, Value.Unit.Meter,         "BBB"),
            new Tuple<string, bool, bool, double?, Value.Unit, string>("8000 CCC",      false, false,   8000, Value.Unit.Meter,         "CCC"),
            new Tuple<string, bool, bool, double?, Value.Unit, string>("P6SM DDD",      false, true,       6, Value.Unit.StatuteMile,   "DDD"),
            new Tuple<string, bool, bool, double?, Value.Unit, string>("6 1/4SM EEE",   false, false,   6.25, Value.Unit.StatuteMile,   "EEE"),
            new Tuple<string, bool, bool, double?, Value.Unit, string>("P6 1/4SM FFF",  false, true,    6.25, Value.Unit.StatuteMile,   "FFF"),
            new Tuple<string, bool, bool, double?, Value.Unit, string>("//// HHH",      false, false,   null, Value.Unit.None,          "HHH"),
        };

        public static List<string> InvalidChunks => new List<string>()
        {
            "CAVO EEE",
            "CAVOKO EEE",
            "123 EEE",
            "12335 EEE",
            "SS EEE",
        };
    }
}
