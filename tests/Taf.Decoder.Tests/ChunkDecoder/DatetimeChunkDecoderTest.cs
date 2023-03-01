using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("DatetimeChunkDecoder")]
    public class DatetimeChunkDecoderTest
    {
        private static readonly DatetimeChunkDecoder chunkDecoder = new DatetimeChunkDecoder();

        /// <summary>
        /// Test parsing of valid datetime chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public static void TestParse(Tuple<string, int, string, string> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);
            var expectedTime = chunk.Item3 + " UTC";
            Assert.AreEqual(chunk.Item2, (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[DatetimeChunkDecoder.DayParameterName]);
            Assert.AreEqual(expectedTime, (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[DatetimeChunkDecoder.TimeParameterName]);
            Assert.AreEqual(chunk.Item4, (decoded[TafDecoder.RemainingTafKey]));
        }

        /// <summary>
        /// Test parsing of invalid datetime chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public static void TestParseInvalidChunk(string chunk)
        {
            Assert.Throws(typeof(TafChunkDecoderException), () =>
            {
                chunkDecoder.Parse(chunk);
            });
        }

        public static List<Tuple<string, int, string, string>> ValidChunks => new List<Tuple<string, int, string, string>>()
        {
            new Tuple<string, int, string, string>("271035Z AAA", 27, "10:35",  "AAA"),
            new Tuple<string, int, string, string>("012342Z BBB",  1, "23:42",  "BBB"),
            new Tuple<string, int, string, string>("311200Z CCC", 31, "12:00",  "CCC"),
        };

        public static List<string> InvalidChunks => new List<string>()
        {
            "271035 AAA", "2102Z AAA", "123580Z AAA", "122380Z AAA", "351212Z AAA", "35018Z AAA",
        };
    }
}
