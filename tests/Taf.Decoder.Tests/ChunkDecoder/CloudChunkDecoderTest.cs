using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("CloudChunkDecoder")]
    public class CloudChunkDecoderTest
    {
        private static readonly CloudChunkDecoder chunkDecoder = new CloudChunkDecoder();

        /// <summary>
        /// Test parsing of valid cloud chunks
        /// </summary>
        [Test, TestCaseSource("ValidChunks")]
        public static void TestParse(Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);
            var clouds = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[CloudChunkDecoder.CloudsParameterName] as List<CloudLayer>;
            Assert.AreEqual(chunk.Item2, clouds.Count);
            if (clouds.Count > 0)
            {
                var cloud = clouds[0];
                Assert.AreEqual(chunk.Item3, cloud.Amount);
                if (chunk.Item4.HasValue)
                {
                    Assert.AreEqual(chunk.Item4, cloud.BaseHeight.ActualValue);
                    Assert.AreEqual(Value.Unit.Feet, cloud.BaseHeight.ActualUnit);
                }
                else
                {
                    Assert.IsNull(cloud.BaseHeight);
                }
                Assert.AreEqual(chunk.Item5, cloud.Type);
            }
            Assert.AreEqual(chunk.Item6, decoded[TafDecoder.RemainingTafKey]);
        }

        /// <summary>
        /// Test parsing with invalid cloud chunks but with CAVOK earlier in the TAF
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public static void TestParseCAVOKChunk(string chunk)
        {
            Dictionary<string, object> decoded = null;
            Assert.DoesNotThrow(() =>
            {
                decoded = chunkDecoder.Parse(chunk, true);
            });
            var clouds = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[CloudChunkDecoder.CloudsParameterName] as List<CloudLayer>;
            Assert.AreEqual(0, clouds.Count);
        }

        /// <summary>
        /// Test parsing of invalid cloud chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public static void TestParseInvalidChunk(string chunk)
        {
            Assert.Throws(typeof(TafChunkDecoderException), () =>
            {
                chunkDecoder.Parse(chunk);
            });
        }

        public static List<Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>> ValidChunks => new List<Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>>()
        {
            new Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>("VV085 AAA", 1, CloudLayer.CloudAmount.VV, 8500, CloudLayer.CloudType.NULL, "AAA"),
            new Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>("BKN200TCU OVC250 VV/// BBB", 3, CloudLayer.CloudAmount.BKN, 20000, CloudLayer.CloudType.TCU, "BBB"),
            new Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>("OVC////// FEW250 CCC", 2, CloudLayer.CloudAmount.OVC, null, CloudLayer.CloudType.CannotMeasure, "CCC"),
            new Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>("NSC DDD", 0, CloudLayer.CloudAmount.NULL, null, CloudLayer.CloudType.NULL, "DDD"),
            new Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>("SKC EEE", 0, CloudLayer.CloudAmount.NULL, 1, CloudLayer.CloudType.CannotMeasure, "EEE"),
            new Tuple<string, int, CloudLayer.CloudAmount, int?, CloudLayer.CloudType, string>("NCD FFF", 0, CloudLayer.CloudAmount.NULL, 1, CloudLayer.CloudType.CannotMeasure, "FFF"),
        };

        public static List<string> InvalidChunks => new List<string>()
        {
            "FEW10 EEE" , "AAA EEE", "BKN100A EEE",
        };
    }
}