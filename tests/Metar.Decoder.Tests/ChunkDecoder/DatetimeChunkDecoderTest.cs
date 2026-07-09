using Metar.Decoder;
using Metar.Decoder.ChunkDecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;

namespace Metar.Decoder.Tests.ChunkDecoder
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

        /// <summary>
        /// Test rollover logic for observation DateTime.
        /// </summary>
        [Test]
        public void TestObservationDateTimeRollover()
        {
            // Reference date is January 5th, so a day greater than 5 should roll back to December previous year
            var referenceDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc);
            var decoder = new DatetimeChunkDecoder(referenceDate);
            var decoded = decoder.Parse("101200Z AAA");

            var result = decoded[MetarDecoder.ResultKey] as Dictionary<string, object>;
            var observationDateTime = (DateTime)result[DatetimeChunkDecoder.ObservationDateTimeParameterName];

            Assert.That(observationDateTime.Year, Is.EqualTo(2025));
            Assert.That(observationDateTime.Month, Is.EqualTo(12));
            Assert.That(observationDateTime.Day, Is.EqualTo(10));
        }

        /// <summary>
        /// Test month rollover to previous month when not in January.
        /// </summary>
        [Test]
        public void TestObservationDateTimeMonthRollover()
        {
            var referenceDate = new DateTime(2026, 7, 9, 0, 0, 0, DateTimeKind.Utc);
            var decoder = new DatetimeChunkDecoder(referenceDate);
            var decoded = decoder.Parse("101200Z AAA");

            var result = decoded[MetarDecoder.ResultKey] as Dictionary<string, object>;
            var observationDateTime = (DateTime)result[DatetimeChunkDecoder.ObservationDateTimeParameterName];

            Assert.That(observationDateTime.Month, Is.EqualTo(6));
            Assert.That(observationDateTime.Day, Is.EqualTo(10));
        }

        /// <summary>
        /// Test day clamping when the resolved month has fewer days than the parsed day.
        /// </summary>
        [Test]
        public void TestObservationDateTimeDayClamping()
        {
            var referenceDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            var decoder = new DatetimeChunkDecoder(referenceDate);
            // April has 30 days, so day 31 should be clamped to 30
            var decoded = decoder.Parse("311200Z AAA");

            var result = decoded[MetarDecoder.ResultKey] as Dictionary<string, object>;
            var observationDateTime = (DateTime)result[DatetimeChunkDecoder.ObservationDateTimeParameterName];

            Assert.That(observationDateTime.Month, Is.EqualTo(4));
            Assert.That(observationDateTime.Day, Is.EqualTo(30));
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