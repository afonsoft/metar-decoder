using Metar.Decoder.ChunkDecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder.Tests.ChunkDecoder
{
    [TestFixture, Category("TrendChunkDecoder")]
    public class TrendChunkDecoderTest
    {
        private TrendChunkDecoder decoder;

        [SetUp]
        public void Setup()
        {
            decoder = new TrendChunkDecoder();
        }

        [Test]
        public void TestParseNosig()
        {
            var result = decoder.Parse("NOSIG ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.AreEqual("NOSIG", decoded[TrendChunkDecoder.TrendTypeParameterName]);
            ClassicAssert.AreEqual("NOSIG", decoded[TrendChunkDecoder.TrendForecastParameterName]);
        }

        [Test]
        public void TestParseBecmg()
        {
            var result = decoder.Parse("BECMG 24010KT ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.AreEqual("BECMG", decoded[TrendChunkDecoder.TrendTypeParameterName]);
        }

        [Test]
        public void TestParseTempo()
        {
            var result = decoder.Parse("TEMPO 3000 BR ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.AreEqual("TEMPO", decoded[TrendChunkDecoder.TrendTypeParameterName]);
        }

        [Test]
        public void TestParseNoTrend()
        {
            var result = decoder.Parse("SOMETHING ");
            var decoded = result[MetarDecoder.ResultKey] as Dictionary<string, object>;
            ClassicAssert.IsFalse(decoded.ContainsKey(TrendChunkDecoder.TrendTypeParameterName));
        }
    }
}
