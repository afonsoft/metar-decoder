using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;
using static Taf.Decoder.entity.Value;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("SurfaceWindChunkDecoder")]
    public class SurfaceWindChunkDecoderTest
    {
        private static readonly SurfaceWindChunkDecoder chunkDecoder = new SurfaceWindChunkDecoder();

        /// <summary>
        /// Test parsing of valid surface wind chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public static void TestParse(SurfaceWindChunkDecoderTester chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Chunk);
            var surfaceWind = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[SurfaceWindChunkDecoder.SurfaceWindParameterName] as SurfaceWind;

            if (!chunk.VariableDirection)
            {
                ClassicAssert.AreEqual(chunk.Direction, surfaceWind.MeanDirection.ActualValue);
                ClassicAssert.AreEqual(Unit.Degree, surfaceWind.MeanDirection.ActualUnit);
            }
            ClassicAssert.AreEqual(chunk.VariableDirection, surfaceWind.VariableDirection);
            if (surfaceWind.DirectionVariations != null)
            {
                var minimumDirectionVariation = surfaceWind.DirectionVariations[0];
                var maximumDirectionVariation = surfaceWind.DirectionVariations[1];
                if (chunk.DirectionVariations != null)
                {
                    ClassicAssert.AreEqual(chunk.DirectionVariations[0].ActualValue, minimumDirectionVariation.ActualValue);
                    ClassicAssert.AreEqual(chunk.DirectionVariations[1].ActualValue, maximumDirectionVariation.ActualValue);
                    ClassicAssert.AreEqual(Unit.Degree, minimumDirectionVariation.ActualUnit);
                }
            }
            ClassicAssert.AreEqual(chunk.Speed, surfaceWind.MeanSpeed.ActualValue);
            if (chunk.SpeedVariations != null)
            {
                ClassicAssert.AreEqual(chunk.SpeedVariations, surfaceWind.SpeedVariations.ActualValue);
            }
            ClassicAssert.AreEqual(chunk.SpeedUnit, surfaceWind.MeanSpeed.ActualUnit);
            ClassicAssert.AreEqual(chunk.Remaining, decoded[TafDecoder.RemainingTafKey]);
        }

        /// <summary>
        /// Test parsing of invalid surface wind chunks
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

        /// <summary>
        /// Test parsing of chunk with no information
        /// </summary>
        [Test]
        public static void TestEmptyInformationChunk()
        {
            Dictionary<string, object> decoded = null;
            var exception = ClassicAssert.Throws(typeof(TafChunkDecoderException), () =>
            {
                decoded = chunkDecoder.Parse("/////KT PPP");
            }) as TafChunkDecoderException;
            ClassicAssert.AreEqual("PPP", exception.NewRemainingTaf);
        }

        public static List<SurfaceWindChunkDecoderTester> ValidChunks => new List<SurfaceWindChunkDecoderTester>()
        {
            new SurfaceWindChunkDecoderTester("VRB01MPS AAA", null, true, 1, null, Unit.MeterPerSecond, null, "AAA"),
            new SurfaceWindChunkDecoderTester("24004MPS BBB", 240, false, 4, null, Unit.MeterPerSecond, null, "BBB"),
            new SurfaceWindChunkDecoderTester("140P99MPS CCC", 140, false, 99, null, Unit.MeterPerSecond, null, "CCC"),
            new SurfaceWindChunkDecoderTester("02005MPS 350V070 DDD", 20, false, 5, null, Unit.MeterPerSecond, new Value[2] { new Value(350, Unit.MeterPerSecond), new Value(70, Unit.MeterPerSecond) }, "DDD"),
            new SurfaceWindChunkDecoderTester("12008G12KPH FFF", 120, false, 8, 12, Unit.KilometerPerHour, null, "FFF"),
        };

        public static List<string> InvalidChunks => new List<string>()
        {
            "12003G09 AAA",
            "VRB01MP BBB",
            "38003G12MPS CCC",
            "12003KPA DDD",
            "02005MPS 450V070 EEE",
            "02005MPS 110V600 FFF",
        };

        public class SurfaceWindChunkDecoderTester
        {
            public string Chunk;
            public int? Direction;
            public bool VariableDirection;
            public int Speed;
            public int? SpeedVariations;
            public Unit SpeedUnit;
            public Value[] DirectionVariations;
            public string Remaining;

            public override string ToString()
            {
                return Chunk;
            }

            public SurfaceWindChunkDecoderTester(string chunk, int? direction, bool variableDirection, int speed, int? speedVariations, Unit speedUnit, Value[] directionVariations, string remaining)
            {
                Chunk = chunk;
                Direction = direction;
                VariableDirection = variableDirection;
                Speed = speed;
                SpeedVariations = speedVariations;
                SpeedUnit = speedUnit;
                DirectionVariations = directionVariations;
                Remaining = remaining;
            }
        }
    }
}