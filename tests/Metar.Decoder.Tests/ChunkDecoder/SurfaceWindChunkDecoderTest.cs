using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("SurfaceWindChunkDecoder")]
    public class SurfaceWindChunkDecoderTest
    {
        private readonly SurfaceWindChunkDecoder chunkDecoder = new SurfaceWindChunkDecoder();

        /// <summary>
        /// Test parsing of valid surface wind chunks.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="direction"></param>
        /// <param name="variable_direction"></param>
        /// <param name="speed"></param>
        /// <param name="speed_variations"></param>
        /// <param name="speed_unit"></param>
        /// <param name="direction_variations"></param>
        /// <param name="remaining"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseSurfaceWindChunk(ValidSurfaceWindChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            ClassicAssert.That(decoded, Is.Not.Null);
            var wind = (decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[SurfaceWindChunkDecoder.SurfaceWindParameterName] as SurfaceWind;
            if (!chunkToTest.VariableDirection)
            {
                ClassicAssert.That(wind.MeanDirection.ActualValue, Is.EqualTo(chunkToTest.Direction));
                ClassicAssert.That(wind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            }
            ClassicAssert.That(wind.VariableDirection, Is.EqualTo(chunkToTest.VariableDirection));
            if (chunkToTest.DirectionVariations != null)
            {
                ClassicAssert.That(wind.DirectionVariations[0].ActualValue, Is.EqualTo(chunkToTest.DirectionVariations[0]));
                ClassicAssert.That(wind.DirectionVariations[1].ActualValue, Is.EqualTo(chunkToTest.DirectionVariations[1]));
                ClassicAssert.That(wind.DirectionVariations[0].ActualUnit, Is.EqualTo(Value.Unit.Degree));
            }
            ClassicAssert.That(wind.MeanSpeed.ActualValue, Is.EqualTo(chunkToTest.Speed));
            if (chunkToTest.SpeedVariations.HasValue)
            {
                ClassicAssert.That(wind.SpeedVariations.ActualValue, Is.EqualTo(chunkToTest.SpeedVariations));
            }
            ClassicAssert.That(wind.MeanSpeed.ActualUnit, Is.EqualTo(chunkToTest.SpeedUnit));
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        /// <summary>
        /// Test parsing of invalid surface wind chunks.
        /// </summary>
        /// <param name="chunk">chunk to test</param>
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

        /// <summary>
        /// Test parsing of chunk with no information.
        /// </summary>
        [Test]
        public void TestEmptyInformationChunk()
        {
            var ex = ClassicAssert.Throws(typeof(MetarChunkDecoderException), () =>
            {
                chunkDecoder.Parse("/////KT PPP");
            }) as MetarChunkDecoderException;
            ClassicAssert.That(ex.NewRemainingMetar, Is.EqualTo("PPP"));
        }

        #region TestCaseSources

        public static List<ValidSurfaceWindChunkDecoderTester> ValidChunks()
        {
            return new List<ValidSurfaceWindChunkDecoderTester>()
            {
                new ValidSurfaceWindChunkDecoderTester()
                {
                    Chunk = "VRB01MPS AAA",
                    Direction = null,
                    VariableDirection = true,
                    Speed = 1,
                    SpeedVariations = null,
                    SpeedUnit = Value.Unit.MeterPerSecond,
                    DirectionVariations = null,
                    RemainingMetar = "AAA"
                },
                new ValidSurfaceWindChunkDecoderTester()
                {
                   Chunk = "24004MPS BBB",
                    Direction = 240,
                    VariableDirection = false,
                    Speed = 4,
                    SpeedVariations = null,
                    SpeedUnit = Value.Unit.MeterPerSecond,
                    DirectionVariations = null,
                    RemainingMetar = "BBB"
                },
                new ValidSurfaceWindChunkDecoderTester()
                {
                  Chunk = "140P99KT CCC",
                    Direction = 140,
                    VariableDirection =false,
                    Speed=99,
                    SpeedVariations = null,
                    SpeedUnit = Value.Unit.Knot,
                    DirectionVariations = null,
                    RemainingMetar = "CCC"
                },
                new ValidSurfaceWindChunkDecoderTester()
                {
                    Chunk = "02005MPS 350V070 DDD",
                    Direction = 20,
                    VariableDirection =false,
                    Speed  =5,
                    SpeedVariations = null,
                    SpeedUnit = Value.Unit.MeterPerSecond,
                    DirectionVariations = new int[] {350, 70 },
                    RemainingMetar = "DDD"
                },
                new ValidSurfaceWindChunkDecoderTester()
                {
                  Chunk =  "12003KPH FFF",
                    Direction = 120,
                    VariableDirection =false,
                    Speed = 3,
                    SpeedVariations = null,
                    SpeedUnit = Value.Unit.KilometerPerHour,
                    DirectionVariations = null,
                    RemainingMetar =  "FFF"
                },
            };
        }

        public class ValidSurfaceWindChunkDecoderTester
        {
            public string Chunk { get; set; }
            public int? Direction { get; set; }
            public bool VariableDirection { get; set; }
            public double Speed { get; set; }
            public int? SpeedVariations { get; set; }
            public Value.Unit SpeedUnit { get; set; }
            public int[] DirectionVariations { get; set; }
            public string RemainingMetar { get; set; }

            public override string ToString()
            {
                return $@"""{Chunk}""";
            }
        }

        public static List<string> InvalidChunks()
        {
            return new List<string> {
                "12003G09 AAA",
                "VRB01MP BBB",
                "38003G12MPS CCC",
                "12003KPA DDD",
                "02005MPS 450V070 EEE",
                "02005MPS 110V600 FFF"
            };
        }

        #endregion TestCaseSources
    }
}