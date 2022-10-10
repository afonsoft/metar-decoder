using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using System.Collections.Generic;
using static Metar.Decoder.Entity.CloudLayer;

namespace Metar.Decoder_tests.chunkdecoder
{
    /// <summary>
    /// CloudChunkDecoderTest
    /// </summary>
    [TestFixture, Category("CloudChunkDecoder")]
    public class CloudChunkDecoderTest
    {
        private readonly CloudChunkDecoder chunkDecoder = new CloudChunkDecoder();

        /// <summary>
        /// Test parsing of valid cloud chunks.
        /// </summary>
        /// <param name="chunkToTest"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseCloudChunk(CloudChunkDecoderTester chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Chunk);
            var clouds = (decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[CloudChunkDecoder.CloudsParameterName] as List<CloudLayer>;

            Assert.That(clouds.Count, Is.EqualTo(chunkToTest.NbLayers));

            if (clouds.Count > 0)
            {
                var cloud = clouds[0];
                Assert.That(cloud.Amount, Is.EqualTo(chunkToTest.layer1Amount));
                if (chunkToTest.layer1BaseHeight != null)
                {
                    Assert.That(cloud.BaseHeight.ActualValue, Is.EqualTo(chunkToTest.layer1BaseHeight));
                    Assert.That(cloud.BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
                }
                else
                {
                    Assert.IsNull(cloud.BaseHeight);
                }
                Assert.That(cloud.Type, Is.EqualTo(chunkToTest.layer1Type));
            }
            Assert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.RemainingMetar));
        }

        /// <summary>
        /// TestParseCAVOKChunk
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseCAVOKChunk(string chunk)
        {
            var decoded = chunkDecoder.Parse(chunk, true);
            Assert.That(((decoded[MetarDecoder.ResultKey] as Dictionary<string, object>)[CloudChunkDecoder.CloudsParameterName] as List<CloudLayer>).Count, Is.EqualTo(0));
        }

        /// <summary>
        /// TestParseInvalidChunk
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseInvalidChunk(string chunk)
        {
            var decoded = new Dictionary<string, object>();
            var ex = Assert.Throws(typeof(MetarChunkDecoderException), () =>
            {
                decoded = chunkDecoder.Parse(chunk);
            }) as MetarChunkDecoderException;
            Assert.IsFalse(decoded.ContainsKey(MetarDecoder.ResultKey));
            Assert.That(ex.RemainingMetar, Is.EqualTo(chunk));
        }

        #region TestCaseSources

        public static List<CloudChunkDecoderTester> ValidChunks()
        {
            return new List<CloudChunkDecoderTester>()
            {
                new CloudChunkDecoderTester()
                {
                    Chunk = "VV085 AAA",
                    NbLayers = 1,
                    layer1Amount = CloudAmount.VV,
                    layer1BaseHeight = 8500,
                    layer1Type = CloudType.NULL,
                    RemainingMetar = "AAA"
                },
                new CloudChunkDecoderTester()
                {
                    Chunk = "BKN200TCU OVC250 VV/// BBB",
                    NbLayers = 3,
                    layer1Amount = CloudAmount.BKN,
                    layer1BaseHeight = 20000,
                    layer1Type = CloudType.TCU,
                    RemainingMetar = "BBB"
                },
                new CloudChunkDecoderTester()
                {
                    Chunk = "OVC////// FEW250 CCC",
                    NbLayers = 2,
                    layer1Amount = CloudAmount.OVC,
                    layer1BaseHeight = null,
                    layer1Type = CloudType.CannotMeasure,
                    RemainingMetar = "CCC"
                },
                new CloudChunkDecoderTester()
                {
                    Chunk = "OVC////// SCT250 CCC",
                    NbLayers = 2,
                    layer1Amount = CloudAmount.OVC,
                    layer1BaseHeight = null,
                    layer1Type = CloudType.CannotMeasure,
                    RemainingMetar = "CCC"
                },
                new CloudChunkDecoderTester()
                {
                    Chunk = "NSC DDD",
                    NbLayers = 0,
                    layer1Amount = CloudAmount.NULL,
                    layer1BaseHeight = null,
                    layer1Type = CloudType.NULL,
                    RemainingMetar = "DDD"
                },
                new CloudChunkDecoderTester()
                {
                    Chunk = "SKC EEE",
                    NbLayers = 0,
                    layer1Amount = CloudAmount.NULL,
                    layer1BaseHeight = null,
                    layer1Type = CloudType.NULL,
                    RemainingMetar = "EEE"
                },
                new CloudChunkDecoderTester()
                {
                    Chunk = "NCD FFF",
                    NbLayers = 0,
                    layer1Amount = CloudAmount.NULL,
                    layer1BaseHeight = null,
                    layer1Type = CloudType.NULL,
                    RemainingMetar = "FFF"
                },
            };
        }

        public class CloudChunkDecoderTester
        {
            public string Chunk { get; set; }
            public int NbLayers { get; set; }
            public CloudAmount layer1Amount { get; set; }
            public int? layer1BaseHeight { get; set; }
            public CloudType layer1Type { get; set; }
            public string RemainingMetar { get; set; }

            public override string ToString()
            {
                return $@"""{Chunk}""";
            }
        }

        public static List<string> InvalidChunks()
        {
            return new List<string> {
                "FEW10 EEE",
                "AAA EEE",
                "BKN100A EEE",
            };
        }

        #endregion TestCaseSources
    }
}