using Metar.Decoder.ChunkDecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder.Tests.ChunkDecoder
{
    [TestFixture, Category("RemarkChunkDecoder")]
    public class RemarkChunkDecoderTest
    {
        private RemarkChunkDecoder decoder;

        [SetUp]
        public void Setup()
        {
            decoder = new RemarkChunkDecoder();
        }

        [Test]
        public void TestParseRemark()
        {
            var result = decoder.Parse("RMK AO2 SLP013 T00720044 ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.IsTrue(decoded.ContainsKey(RemarkChunkDecoder.RemarkParameterName));
            ClassicAssert.IsTrue(((string)decoded[RemarkChunkDecoder.RemarkParameterName]).Contains("AO2"));
        }

        [Test]
        public void TestParseSeaLevelPressure()
        {
            var result = decoder.Parse("RMK AO2 SLP013 T00720044 ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.IsTrue(decoded.ContainsKey(RemarkChunkDecoder.SeaLevelPressureParameterName));
            var slp = (Value)decoded[RemarkChunkDecoder.SeaLevelPressureParameterName];
            ClassicAssert.AreEqual(1001.3, slp.ActualValue, 0.01);
            ClassicAssert.AreEqual(Value.Unit.HectoPascal, slp.ActualUnit);
        }

        [Test]
        public void TestParseSeaLevelPressureHigh()
        {
            var result = decoder.Parse("RMK SLP982 ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            var slp = (Value)decoded[RemarkChunkDecoder.SeaLevelPressureParameterName];
            ClassicAssert.AreEqual(998.2, slp.ActualValue, 0.01);
        }

        [Test]
        public void TestParseNoRemark()
        {
            var result = decoder.Parse("SOMETHING ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.IsFalse(decoded.ContainsKey(RemarkChunkDecoder.RemarkParameterName));
        }
    }
}
