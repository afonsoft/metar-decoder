using Metar.Decoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;

namespace Metar.Decoder_tests
{
    /// <summary>
    /// MetarChunkDecoderExceptionTest
    /// </summary>
    [TestFixture, Category("MetarChunkDecoderException")]
    public class MetarChunkDecoderExceptionTest
    {
        /// <summary>
        /// SerializationWithNoMessageTest
        /// </summary>
        [Test]
        public void SerializationWithNoMessageTest()
        {
            var dto = new MetarChunkDecoderException();
            var mem = new MemoryStream();
            ClassicAssert.DoesNotThrow(() =>
             {
                 JsonSerializer.Serialize(mem, dto);
             });
        }

        /// <summary>
        /// SerializationWithMessageTest
        /// </summary>
        [Test]
        public void SerializationWithMessageTest()
        {
            var dto = new MetarChunkDecoderException("Test");
            var mem = new MemoryStream();
            ClassicAssert.DoesNotThrow(() =>
             {
                 JsonSerializer.Serialize(mem, dto);
             });
        }

        /// <summary>
        /// SerializationWithSerializationInfoTest
        /// </summary>
        [Test]
        public void SerializationWithSerializationInfoTest()
        {
            var dto = new MetarChunkDecoderException("Test");
            var mem = new MemoryStream();
            ClassicAssert.DoesNotThrow(() =>
             {
                 JsonSerializer.Serialize(mem, dto);
             });
        }
    }
}