using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("TemperatureChunkDecoder")]
    public class TemperatureChunkDecoderTest
    {
        private readonly TemperatureChunkDecoder chunkDecoder = new TemperatureChunkDecoder();

        [Test, TestCaseSource("ValidChunks")]
        public void TestParseTemperatureChunk(TemperatureChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            if (!chunkToTest.AirTemperature.HasValue)
            {
                ClassicAssert.That((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>).ContainsKey(TemperatureChunkDecoder.AirTemperatureParameterName), Is.False);
            }
            else
            {
                ClassicAssert.That(((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[TemperatureChunkDecoder.AirTemperatureParameterName] as Value).ActualValue, Is.EqualTo(chunkToTest.AirTemperature));
                ClassicAssert.That(((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[TemperatureChunkDecoder.AirTemperatureParameterName] as Value).ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));
            }
            if (!chunkToTest.DewPointTemperature.HasValue)
            {
                ClassicAssert.That((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>).ContainsKey(TemperatureChunkDecoder.DewPointTemperatureParameterName), Is.False);
            }
            else
            {
                ClassicAssert.That(((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[TemperatureChunkDecoder.DewPointTemperatureParameterName] as Value).ActualValue, Is.EqualTo(chunkToTest.DewPointTemperature));
                ClassicAssert.That(((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[TemperatureChunkDecoder.DewPointTemperatureParameterName] as Value).ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));
            }
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        public static List<TemperatureChunkDecoderTester> ValidChunks()
        {
            return new List<TemperatureChunkDecoderTester>()
            {
                new TemperatureChunkDecoderTester()
                {
                    Chunk = "M01/M10 AAA",
                    AirTemperature = -1,
                    DewPointTemperature = -10,
                    RemainingMetar = "AAA",
                },
                new TemperatureChunkDecoderTester()
                {
                    Chunk = "05/12 BBB",
                    AirTemperature = 5,
                    DewPointTemperature = 12,
                    RemainingMetar = "BBB",
                },
                new TemperatureChunkDecoderTester()
                {
                    Chunk = "10/M01 CCC",
                    AirTemperature = 10,
                    DewPointTemperature = -1,
                    RemainingMetar = "CCC",
                },
                // partial information
                new TemperatureChunkDecoderTester()
                {
                    Chunk = "M15/ DDD",
                    AirTemperature = -15,
                    DewPointTemperature = null,
                    RemainingMetar = "DDD",
                },
                new TemperatureChunkDecoderTester()
                {
                    Chunk = "NOTHING EEE",
                    AirTemperature = null,
                    DewPointTemperature = null,
                    RemainingMetar = "NOTHING EEE",
                },
                // invalid formats
                new TemperatureChunkDecoderTester() {
                    Chunk = "M01//10 FFF",
                    AirTemperature = null,
                    DewPointTemperature = null,
                    RemainingMetar = "M01//10 FFF",
                },
                new TemperatureChunkDecoderTester()
                {
                    Chunk = "M1/10 GGG",
                    AirTemperature = null,
                    DewPointTemperature = null,
                    RemainingMetar = "M1/10 GGG",
                },
            };
        }

        public class TemperatureChunkDecoderTester
        {
            public string Chunk { get; set; }
            public int? AirTemperature { get; set; }
            public int? DewPointTemperature { get; set; }
            public string RemainingMetar { get; set; }

            public override string ToString()
            {
                return $@"""{Chunk}""";
            }
        }
    }
}