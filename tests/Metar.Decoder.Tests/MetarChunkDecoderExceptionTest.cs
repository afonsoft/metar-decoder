using Metar.Decoder;
using Metar.Decoder.ChunkDecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;

namespace Metar.Decoder.Tests
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

        /// <summary>
        /// GetObjectData serializes remaining metar fields.
        /// </summary>
        [Test]
        public void GetObjectDataSerializesRemainingMetarFields()
        {
            var decoder = new IcaoChunkDecoder();
            var dto = new MetarChunkDecoderException("REMAINING", "NEW", "Test", decoder);
            var info = new SerializationInfo(typeof(MetarChunkDecoderException), new FormatterConverter());

            dto.GetObjectData(info, new StreamingContext());

            ClassicAssert.That(info.GetString("RemainingMetar"), Is.EqualTo("NEW"));
            ClassicAssert.That(info.GetString("NewRemainingMetar"), Is.EqualTo("REMAINING"));
        }

        /// <summary>
        /// GetObjectData throws ArgumentNullException when info is null.
        /// </summary>
        [Test]
        public void GetObjectDataThrowsWhenInfoIsNull()
        {
            var dto = new MetarChunkDecoderException("REMAINING", "NEW", "Test", new IcaoChunkDecoder());
            ClassicAssert.Throws(typeof(ArgumentNullException), () =>
            {
                dto.GetObjectData(null, new StreamingContext());
            });
        }

        /// <summary>
        /// Private serialization constructor restores remaining metar fields.
        /// </summary>
        [Test]
        public void SerializationConstructorRestoresFields()
        {
            var info = new SerializationInfo(typeof(MetarChunkDecoderException), new FormatterConverter());
            info.AddValue("RemainingMetar", "REMAINING");
            info.AddValue("NewRemainingMetar", "NEW");
            info.AddValue("Message", "Test");
            info.AddValue("ClassName", typeof(MetarChunkDecoderException).FullName);
            info.AddValue("InnerException", null);
            info.AddValue("StackTraceString", null);
            info.AddValue("RemoteStackTraceString", null);
            info.AddValue("RemoteStackIndex", 0);
            info.AddValue("ExceptionMethod", null);
            info.AddValue("HResult", -2146233088);
            info.AddValue("Source", null);
            info.AddValue("HelpURL", null);

            var ctor = typeof(MetarChunkDecoderException).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(SerializationInfo), typeof(StreamingContext) },
                null);

            var dto = (MetarChunkDecoderException)ctor.Invoke(new object[] { info, new StreamingContext() });

            ClassicAssert.That(dto.RemainingMetar, Is.EqualTo("REMAINING"));
            ClassicAssert.That(dto.NewRemainingMetar, Is.EqualTo("NEW"));
        }
    }
}