using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("WindShearChunkDecoder")]
    public class WindShearChunkDecoderTest
    {
        private readonly WindShearChunkDecoder chunkDecoder = new WindShearChunkDecoder();

        /// <summary>
        /// Test parsing of valid windshear chunks.
        /// </summary>
        /// <param name="chunkToTest"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseWindShearChunk(WindShearChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            var result = decoded[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.That((bool)result[WindShearChunkDecoder.WindshearAllRunwaysParameterName], Is.EqualTo(chunkToTest.AllRunways));
            ClassicAssert.That(result[WindShearChunkDecoder.WindshearRunwaysParameterName] as List<string>, Is.EqualTo(chunkToTest.Runways));
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        /// <summary>
        /// Test parsing of invalid wind shear chunks (bad format).
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Test, TestCaseSource("BadFormatChunk")]
        public void TestParseBadFormatChunk(string chunk)
        {
            //need fine tuning
            var decoded = chunkDecoder.Parse(chunk);
            _ = decoded[MetarDecoder.ResultKey];
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunk));
        }

        /// <summary>
        /// Test parsing of invalid wind shear chunks (invalid QFU).
        /// </summary>
        /// <param name=""></param>
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

        public static List<WindShearChunkDecoderTester> ValidChunks()
        {
            return new List<WindShearChunkDecoderTester>()
            {
                new WindShearChunkDecoderTester()
                {
                    Chunk = "WS R03L WS R32 WS R25C AAA",
                    AllRunways = false,
                    Runways = new string[] { "03L", "32", "25C" },
                    RemainingMetar = "AAA",
                },
                new WindShearChunkDecoderTester()
                {
                    Chunk = "WS R18C BBB",
                    AllRunways = false,
                    Runways = new string[] { "18C" },
                    RemainingMetar = "BBB",
                },
                new WindShearChunkDecoderTester()
                {
                    Chunk = "WS ALL RWY CCC",
                    AllRunways = true,
                    Runways = null,
                    RemainingMetar = "CCC",
                },
                new WindShearChunkDecoderTester()
                {
                    Chunk = "WS RWY22 DDD",
                    AllRunways = false,
                    Runways = new string[] { "22" },
                    RemainingMetar = "DDD",
                },
            };
        }

        public class WindShearChunkDecoderTester
        {
            public string Chunk { get; set; }
            public bool AllRunways { get; set; }
            public string[] Runways { get; set; }
            public string RemainingMetar { get; set; }

            public override string ToString()
            {
                return $@"""{Chunk}""";
            }
        }

        public static List<string> BadFormatChunk()
        {
            return new List<string>()
            {
                "W RWY AAA", "WS ALL BBB", "WS R12P CCC"
            };
        }

        public static List<string> InvalidChunks()
        {
            return new List<string>()
            {
                "WS RWY00 AAA", "WS R40 BBB", "WS R50C CCC"
            };
        }

        #endregion TestCaseSources
    }
}