using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("RunwayVisualRangeChunkDecoder")]
    public class RunwayVisualRangeChunkDecoderTest
    {
        private readonly RunwayVisualRangeChunkDecoder chunkDecoder = new RunwayVisualRangeChunkDecoder();

        /// <summary>
        /// Test parsing of valid report type chunks.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="type"></param>
        /// <param name="remaining"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseRunwayVisualRangeChunk(RunwayVisualRangeChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            var runways = (decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[RunwayVisualRangeChunkDecoder.RunwaysVisualRangeParameterName] as List<RunwayVisualRange>;
            var visualRange = runways[0];
            Assert.That(runways.Count, Is.EqualTo(chunkToTest.NbRunways));
            Assert.That(visualRange.Runway, Is.EqualTo(chunkToTest.runway1Name));
            Assert.That(visualRange.Variable, Is.EqualTo(chunkToTest.runway1Variable));
            if (chunkToTest.runway1Variable)
            {
                Assert.That(visualRange.VisualRangeInterval[0].ActualValue, Is.EqualTo(chunkToTest.runway1Interval[0]));
                Assert.That(visualRange.VisualRangeInterval[1].ActualValue, Is.EqualTo(chunkToTest.runway1Interval[1]));
                Assert.That(visualRange.VisualRangeInterval[0].ActualUnit, Is.EqualTo(chunkToTest.runway1Unit));
            }
            else
            {
                Assert.That(visualRange.VisualRange.ActualValue, Is.EqualTo(chunkToTest.runway1Vis));
                Assert.That(visualRange.VisualRange.ActualUnit, Is.EqualTo(chunkToTest.runway1Unit));
            }

            Assert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        /// <summary>
        /// Test parsing of invalid runway visual range chunks.
        /// </summary>
        /// <param name=""></param>
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseInvalidChunk(string chunk)
        {
            var decoded = new Dictionary<string, object>();
            var ex = Assert.Throws(typeof(MetarChunkDecoderException), () =>
            {
                decoded = chunkDecoder.Parse(chunk);
            }) as MetarChunkDecoderException;
            Assert.That(decoded.ContainsKey(MetarDecoder.ResultKey), Is.False);
            Assert.That(ex.RemainingMetar, Is.EqualTo(chunk));
        }

        #region TestCaseSources

        public static List<RunwayVisualRangeChunkDecoderTester> ValidChunks()
        {
            return new List<RunwayVisualRangeChunkDecoderTester>() {
                new RunwayVisualRangeChunkDecoderTester()
                {
                    Chunk = "R18L/0800 AAA",
                    NbRunways = 1,
                    runway1Name = "18L",
                    runway1Vis = 800,
                    runway1Unit = Value.Unit.Meter,
                    runway1Interval = null,
                    runway1Variable = false,
                    RemainingMetar = "AAA",
                },
                new RunwayVisualRangeChunkDecoderTester()
                {
                    Chunk = "R20C/M1200 BBB",
                    NbRunways = 1,
                    runway1Name = "20C",
                    runway1Vis = 1200,
                    runway1Unit = Value.Unit.Meter,
                    runway1Interval = null,
                    runway1Variable = false,
                    RemainingMetar = "BBB",
                },
                new RunwayVisualRangeChunkDecoderTester()
                {
                    Chunk = "R12/M0800VP1200 R26/0040U CCC",
                    NbRunways = 2,
                    runway1Name = "12",
                    runway1Vis = null,
                    runway1Unit = Value.Unit.Meter,
                    runway1Interval = new int[] { 800, 1200 },
                    runway1Variable = true,
                    RemainingMetar = "CCC",
                },
                new RunwayVisualRangeChunkDecoderTester()
                {
                    Chunk = "R30/5000FT R26/2500V3000FTU DDD",
                    NbRunways = 2,
                    runway1Name = "30",
                    runway1Vis = 5000,
                    runway1Unit = Value.Unit.Feet,
                    runway1Interval = null,
                    runway1Variable = false,
                    RemainingMetar = "DDD",
                },
            };
        }

        public class RunwayVisualRangeChunkDecoderTester
        {
            public string Chunk { get; set; }
            public int NbRunways { get; set; }
            public string runway1Name { get; set; }
            public int? runway1Vis { get; set; }
            public Value.Unit runway1Unit { get; set; }
            public int[] runway1Interval { get; set; }
            public bool runway1Variable { get; set; }
            public string RemainingMetar { get; set; }

            public override string ToString()
            {
                return $@"""{Chunk}""";
            }
        }

        public static List<string> InvalidChunks
        {
            get
            {
                return new List<string>()
                {
                    "R42L/0500 AAA", "R00C/0050 BBB",
                };
            }
        }

        #endregion TestCaseSources
    }
}