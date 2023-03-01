using NUnit.Framework;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("ForecastPeriodChunkDecoder")]
    public class ForecastPeriodChunkDecoderTest
    {
        private static readonly ForecastPeriodChunkDecoder chunkDecoder = new ForecastPeriodChunkDecoder();

        /// <summary>
        /// Test parsing of valid forecast period chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParse(Tuple<string, int, int, int, int, bool, string> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);
            var ForecastPeriod = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[ForecastPeriodChunkDecoder.ForecastPeriodParameterName] as ForecastPeriod;
            Assert.AreEqual(chunk.Item2, ForecastPeriod.FromDay);
            Assert.AreEqual(chunk.Item3, ForecastPeriod.FromHour);
            Assert.AreEqual(chunk.Item4, ForecastPeriod.ToDay);
            Assert.AreEqual(chunk.Item5, ForecastPeriod.ToHour);
            Assert.AreEqual(chunk.Item6, ForecastPeriod.IsValid);
            Assert.AreEqual(chunk.Item7, decoded[TafDecoder.RemainingTafKey]);
        }

        /// <summary>
        /// Test parsing of invalid forecast period chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseInvalidChunk(Tuple<string, int, int, int, int, bool, string> chunk)
        {
            Assert.Throws(typeof(TafChunkDecoderException), () =>
            {
                chunkDecoder.Parse(chunk.Item1);
            });
        }

        public static List<Tuple<string, int, int, int, int, bool, string>> ValidChunks => new List<Tuple<string, int, int, int, int, bool, string>>()
        {
            new Tuple<string, int, int, int, int, bool, string>("0318/0406 CNL", 3, 18, 4,  6, true, "CNL"),
            new Tuple<string, int, int, int, int, bool, string>("0318/0323 CNL", 3, 18, 3, 23, true, "CNL"),
        };

        public static List<Tuple<string, int, int, int, int, bool, string>> InvalidChunks => new List<Tuple<string, int, int, int, int, bool, string>>()
        {
            new Tuple<string, int, int, int, int, bool, string>("0318 CNL",          3, 18,  0,  0, false, "CNL"),
            new Tuple<string, int, int, int, int, bool, string>("3218/0206 CNL",    32, 18,  2,  6, false, "CNL"),
            new Tuple<string, int, int, int, int, bool, string>("3018/0000 CNL",    32, 18,  0,  0, false, "CNL"),
            new Tuple<string, int, int, int, int, bool, string>("3018/3210 CNL",     3, 18, 32 ,10, false, "CNL"),
            new Tuple<string, int, int, int, int, bool, string>("2825/2910 CNL",    28, 25, 29 ,10, false, "CNL"),
            new Tuple<string, int, int, int, int, bool, string>("2818/2818 CNL",    28, 18, 28 ,18, false, "CNL"),
        };
    }
}