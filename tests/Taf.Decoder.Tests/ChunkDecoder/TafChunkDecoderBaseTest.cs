using Taf.Decoder.ChunkDecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Taf.Decoder.Tests.ChunkDecoder
{
    [TestFixture, Category("TafChunkDecoder")]
    public class TafChunkDecoderBaseTest
    {
        [Test]
        public void TestConsumeOneChunkWithSpace()
        {
            var result = TafChunkDecoder.ConsumeOneChunk("FIRST SECOND THIRD");
            ClassicAssert.AreEqual("SECOND THIRD", result);
        }

        [Test]
        public void TestConsumeOneChunkNoSpace()
        {
            var result = TafChunkDecoder.ConsumeOneChunk("NOSPACEHERE");
            ClassicAssert.AreEqual("NOSPACEHERE", result);
        }

        [Test]
        public void TestConsumeOneChunkSingleWord()
        {
            var result = TafChunkDecoder.ConsumeOneChunk("SINGLE");
            ClassicAssert.AreEqual("SINGLE", result);
        }

        [Test]
        public void TestConsumeOneChunkEmpty()
        {
            var result = TafChunkDecoder.ConsumeOneChunk("");
            ClassicAssert.AreEqual("", result);
        }

        [Test]
        public void TestConsumeOneChunkTrailingSpace()
        {
            var result = TafChunkDecoder.ConsumeOneChunk("FIRST ");
            ClassicAssert.AreEqual("", result);
        }
    }
}
