using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("VisibilityChunkDecoder")]
    public class VisibilityChunkDecoderTest
    {
        private readonly VisibilityChunkDecoder chunkDecoder = new VisibilityChunkDecoder();

        /// <summary>
        /// Test parsing of valid surface wind chunks.
        /// </summary>
        /// <param name="chunkToTest"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseVisibilityChunk(VisibilityChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            if (chunkToTest.Cavok)
            {
                ClassicAssert.IsTrue((bool)(decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[VisibilityChunkDecoder.CavokParameterName]);
            }
            else if (!chunkToTest.Visibility.HasValue)
            {
                ClassicAssert.That((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[VisibilityChunkDecoder.VisibilityParameterName], Is.Null);
                ClassicAssert.That((bool)(decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[VisibilityChunkDecoder.CavokParameterName], Is.False);
            }
            else
            {
                var visibility = (decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[VisibilityChunkDecoder.VisibilityParameterName] as Visibility;
                ClassicAssert.That(visibility.PrevailingVisibility.ActualValue, Is.EqualTo(chunkToTest.Visibility));
                ClassicAssert.That(visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(chunkToTest.VisibilityUnit));
                ClassicAssert.That(visibility.NDV, Is.EqualTo(chunkToTest.NDV));
                if (chunkToTest.Minimum.HasValue)
                {
                    ClassicAssert.That(visibility.MinimumVisibility.ActualValue, Is.EqualTo(chunkToTest.Minimum));
                    ClassicAssert.That(visibility.MinimumVisibilityDirection, Is.EqualTo(chunkToTest.MinimumDirection));
                }
            }
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        /// <summary>
        /// Test parsing of invalid surface wind chunks.
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

        public static List<VisibilityChunkDecoderTester> ValidChunks()
        {
            return new List<VisibilityChunkDecoderTester>()
            {
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "0200 AAA",
                    Cavok = false,
                    Visibility = 200,
                    VisibilityUnit = Value.Unit.Meter,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "AAA",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "CAVOK BBB",
                    Cavok = true,
                    Visibility = null,
                    VisibilityUnit = Value.Unit.Meter,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "BBB",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "8000 1200N CCC",
                    Cavok = false,
                    Visibility = 8000,
                    VisibilityUnit = Value.Unit.Meter,
                    Minimum = 1200,
                    MinimumDirection = "N",
                    NDV = false,
                    RemainingMetar = "CCC",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "2500 2200 DDD",
                    Cavok = false,
                    Visibility = 2500,
                    VisibilityUnit = Value.Unit.Meter,
                    Minimum = 2200,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "DDD",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "1 1/4SM EEE",
                    Cavok = false,
                    Visibility = 1.25,
                    VisibilityUnit = Value.Unit.StatuteMile,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "EEE",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "10SM FFF",
                    Cavok = false,
                    Visibility = 10,
                    VisibilityUnit = Value.Unit.StatuteMile,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "FFF",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "3/4SM GGG",
                    Cavok = false,
                    Visibility = 0.75,
                    VisibilityUnit = Value.Unit.StatuteMile,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "GGG",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "//// HHH",
                    Cavok = false,
                    Visibility = null,
                    VisibilityUnit = Value.Unit.None,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = false,
                    RemainingMetar = "HHH",
                },
                new VisibilityChunkDecoderTester()
                {
                    Chunk = "9000NDV JJJ",
                    Cavok = false,
                    Visibility = 9000,
                    VisibilityUnit = Value.Unit.Meter,
                    Minimum = null,
                    MinimumDirection = string.Empty,
                    NDV = true,
                    RemainingMetar = "JJJ",
                },
            };
        }

        public class VisibilityChunkDecoderTester
        {
            public string Chunk { get; set; }
            public bool Cavok { get; set; }
            public double? Visibility { get; set; }
            public Value.Unit VisibilityUnit { get; set; }
            public int? Minimum { get; set; }
            public string MinimumDirection { get; set; } = string.Empty;
            public bool NDV { get; set; }
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
                    "CAVO EEE", "CAVOKO EEE", "123 EEE", "12335 EEE", "1233NVV EEE"
                };
            }
        }

        #endregion TestCaseSources
    }
}