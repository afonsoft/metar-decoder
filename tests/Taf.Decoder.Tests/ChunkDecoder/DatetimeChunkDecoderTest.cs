using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.ChunkDecoder;

namespace Taf.Decoder.Tests.ChunkDecoder
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
            ClassicAssert.AreEqual(chunk.Item2, (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[DatetimeChunkDecoder.DayParameterName]);
            ClassicAssert.AreEqual(expectedTime, (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[DatetimeChunkDecoder.TimeParameterName]);
            ClassicAssert.AreEqual(chunk.Item4, (decoded[TafDecoder.RemainingTafKey]));
        }

        /// <summary>
        /// Test parsing of invalid datetime chunks
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

        public static List<Tuple<string, int, string, string>> ValidChunks => new List<Tuple<string, int, string, string>>()
        {
            new Tuple<string, int, string, string>("271035Z AAA", 27, "10:35",  "AAA"),
            new Tuple<string, int, string, string>("012342Z BBB",  1, "23:42",  "BBB"),
            new Tuple<string, int, string, string>("311200Z CCC", 31, "12:00",  "CCC"),
        };

        /// <summary>
        /// Test rollover logic for origin DateTime.
        /// </summary>
        [Test]
        public void TestOriginDateTimeRollover()
        {
            var referenceDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc);
            var decoder = new DatetimeChunkDecoder(referenceDate);
            var decoded = decoder.Parse("101200Z AAA");

            var result = decoded[TafDecoder.ResultKey] as Dictionary<string, object>;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ContainsKey(DatetimeChunkDecoder.OriginDateTimeParameterName), Is.True);
            var originDateTime = (DateTime)result[DatetimeChunkDecoder.OriginDateTimeParameterName];

            Assert.That(originDateTime.Year, Is.EqualTo(2025));
            Assert.That(originDateTime.Month, Is.EqualTo(12));
            Assert.That(originDateTime.Day, Is.EqualTo(10));
        }

        /// <summary>
        /// Test month rollover to previous month when not in January.
        /// </summary>
        [Test]
        public void TestOriginDateTimeMonthRollover()
        {
            var referenceDate = new DateTime(2026, 7, 9, 0, 0, 0, DateTimeKind.Utc);
            var decoder = new DatetimeChunkDecoder(referenceDate);
            var decoded = decoder.Parse("101200Z AAA");

            var result = decoded[TafDecoder.ResultKey] as Dictionary<string, object>;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ContainsKey(DatetimeChunkDecoder.OriginDateTimeParameterName), Is.True);
            var originDateTime = (DateTime)result[DatetimeChunkDecoder.OriginDateTimeParameterName];

            Assert.That(originDateTime.Month, Is.EqualTo(6));
            Assert.That(originDateTime.Day, Is.EqualTo(10));
        }

        /// <summary>
        /// Test day clamping when the resolved month has fewer days than the parsed day.
        /// </summary>
        [Test]
        public void TestOriginDateTimeDayClamping()
        {
            var referenceDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            var decoder = new DatetimeChunkDecoder(referenceDate);
            var decoded = decoder.Parse("311200Z AAA");

            var result = decoded[TafDecoder.ResultKey] as Dictionary<string, object>;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ContainsKey(DatetimeChunkDecoder.OriginDateTimeParameterName), Is.True);
            var originDateTime = (DateTime)result[DatetimeChunkDecoder.OriginDateTimeParameterName];

            Assert.That(originDateTime.Month, Is.EqualTo(4));
            Assert.That(originDateTime.Day, Is.EqualTo(30));
        }

        public static List<string> InvalidChunks => new List<string>()
        {
            "271035 AAA", "2102Z AAA", "123580Z AAA", "122380Z AAA", "351212Z AAA", "35018Z AAA",
        };
    }
}