using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("PresentWeatherChunkDecoder")]
    public class PresentWeatherChunkDecoderTest
    {
        private readonly PresentWeatherChunkDecoder chunkDecoder = new PresentWeatherChunkDecoder();

        /// <summary>
        /// Test parsing of valid recent weather chunks.
        /// </summary>
        /// <param name="chunkToTest"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParsePresentWeatherChunk(PresentWeatherChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            var pw = (decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[PresentWeatherChunkDecoder.PresentWeatherParameterName] as List<WeatherPhenomenon>;

            Assert.That(pw.Count, Is.EqualTo(chunkToTest.NbPhenomens));
            if (chunkToTest.NbPhenomens > 0)
            {
                var phenom1 = pw[0];
                Assert.That(phenom1.IntensityProximity, Is.EqualTo(chunkToTest.intensity1));
                Assert.That(phenom1.Characteristics, Is.EqualTo(chunkToTest.carac1));
                Assert.That(phenom1.Types, Is.EqualTo(chunkToTest.type1));
            }
            if (chunkToTest.NbPhenomens > 1)
            {
                var phenom2 = pw[1];
                Assert.That(phenom2.Types, Is.EqualTo(chunkToTest.type2));
            }

            Assert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        #region TestCaseSources

        public static List<PresentWeatherChunkDecoderTester> ValidChunks()
        {
            return new List<PresentWeatherChunkDecoderTester>() {
                new PresentWeatherChunkDecoderTester
                {
                    Chunk = "NOTHING HERE",
                    NbPhenomens = 0,
                    intensity1 = null,
                    carac1 = string.Empty,
                    type1 = null,
                    type2 = null,
                    RemainingMetar = "NOTHING HERE",
                },
                new PresentWeatherChunkDecoderTester()
                {
                    Chunk = "FZRA +SN BCFG AAA",
                    NbPhenomens = 3,
                    intensity1 = string.Empty,
                    carac1 = "FZ",
                    type1 = new string[] { "RA" },
                    type2 = new string[] { "SN" },
                    RemainingMetar = "AAA",
                },
                new PresentWeatherChunkDecoderTester()
                {
                    Chunk = "-SG BBB",
                    NbPhenomens = 1,
                    intensity1 = "-",
                    carac1 = string.Empty,
                    type1 = new string[] { "SG" },
                    type2 = null,
                    RemainingMetar = "BBB",
                },
                new PresentWeatherChunkDecoderTester()
                {
                    Chunk = "+GSBRFU VCDRFCPY // CCC",
                    NbPhenomens = 2,
                    intensity1 = "+",
                    carac1 = string.Empty,
                    type1 = new string[] { "GS", "BR", "FU" },
                    type2 = new string[] { "FC", "PY" },
                    RemainingMetar = "CCC",
                },
                new PresentWeatherChunkDecoderTester()
                {
                    Chunk = "// DDD",
                    NbPhenomens = 0,
                    intensity1 = null,
                    carac1 = string.Empty,
                    type1 = null,
                    type2 = null,
                    RemainingMetar = "DDD",
                },
            };
        }

        public class PresentWeatherChunkDecoderTester
        {
            public string Chunk { get; set; }
            public int NbPhenomens { get; set; }
            public string intensity1 { get; set; }
            public string carac1 { get; set; }
            public string[] type1 { get; set; }
            public string[] type2 { get; set; }
            public string RemainingMetar { get; set; }

            public override string ToString()
            {
                return $@"""{Chunk}""";
            }
        }

        #endregion TestCaseSources
    }
}