using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("DatetimeChunkDecoder")]
    public class DatetimeChunkDecoderTest
    {
        private readonly DatetimeChunkDecoder chunkDecoder = new DatetimeChunkDecoder();

        /// <summary>
        /// Test parsing of valid icao chunks.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="icao"></param>
        /// <param name="remaining"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseDatetimeChunk(Tuple<string, int, string, string> chunk)
        {
            var decoded = new Dictionary<string, object>();
            ClassicAssert.DoesNotThrow(() =>
             {
                 decoded = chunkDecoder.Parse(chunk.Item1);
             });

            ClassicAssert.IsTrue(decoded.ContainsKey(MetarDecoder.ResultKey));

            //check Day
            ClassicAssert.IsTrue(((Dictionary<string, object>)decoded[MetarDecoder.ResultKey]).ContainsKey(DatetimeChunkDecoder.DayParameterName));
            ClassicAssert.NotNull(((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[DatetimeChunkDecoder.DayParameterName]);
            ClassicAssert.That((int)((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[DatetimeChunkDecoder.DayParameterName], Is.EqualTo(chunk.Item2));

            //check Time
            ClassicAssert.IsTrue(((Dictionary<string, object>)decoded[MetarDecoder.ResultKey]).ContainsKey(DatetimeChunkDecoder.TimeParameterName));
            ClassicAssert.NotNull(((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[DatetimeChunkDecoder.TimeParameterName]);
            ClassicAssert.That(((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[DatetimeChunkDecoder.TimeParameterName] as string, Is.EqualTo(chunk.Item3 + " UTC"));

            //check RemainingMetar
            ClassicAssert.IsTrue(decoded.ContainsKey(MetarDecoder.RemainingMetarKey));
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey] as string, Is.EqualTo(chunk.Item4));
        }

        /// <summary>
        /// Test parsing of invalid icao chunks.
        /// </summary>
        /// <param name="chunk"></param>
        ///
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseInvalidChunk(string chunk)
        {
            var decoded = new Dictionary<string, object>();
            var ex = ClassicAssert.Throws(typeof(MetarChunkDecoderException), () =>
            {
                decoded = chunkDecoder.Parse(chunk);
            }) as MetarChunkDecoderException;
            ClassicAssert.That(decoded.ContainsKey(MetarDecoder.ResultKey), Is.False);
            ClassicAssert.That(ex.RemainingMetar, Is.EqualTo(chunk));
        }

        #region TestCaseSources

        public static List<Tuple<string, int, string, string>> ValidChunks()
        {
            return new List<Tuple<string, int, string, string>>()
            {
                new Tuple<string, int, string, string>("271035Z AAA", 27, "10:35", "AAA"),
                new Tuple<string, int, string, string>("012342Z BBB", 1,  "23:42", "BBB"),
                new Tuple<string, int, string, string>("311200Z CCC", 31, "12:00", "CCC"),
            };
        }

        public static List<string> InvalidChunks()
        {
            return new List<string>() {
                "271035 AAA",
                "2102Z AAA",
                "123580Z AAA",
                "122380Z AAA",
                "351212Z AAA",
                "35018Z AAA",
            };
        }

        #endregion TestCaseSources
    }
}