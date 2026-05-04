using Metar.Decoder;
using Metar.Decoder.ChunkDecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Metar.Decoder.Tests.Entity
{
    [TestFixture, Category("MetarChunkDecoderException")]
    public class MetarExceptionExtendedTest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var ex = new MetarChunkDecoderException();
            ClassicAssert.IsNotNull(ex);
            ClassicAssert.IsNull(ex.RemainingMetar);
            ClassicAssert.IsNull(ex.NewRemainingMetar);
            ClassicAssert.IsNull(ex.ChunkDecoder);
        }

        [Test]
        public void TestMessageConstructor()
        {
            var ex = new MetarChunkDecoderException("test message");
            ClassicAssert.AreEqual("test message", ex.Message);
            ClassicAssert.IsNull(ex.RemainingMetar);
            ClassicAssert.IsNull(ex.NewRemainingMetar);
            ClassicAssert.IsNull(ex.ChunkDecoder);
        }

        [Test]
        public void TestFullConstructor()
        {
            var decoder = new IcaoChunkDecoder();
            var ex = new MetarChunkDecoderException("remaining", "newRemaining", "error message", decoder);
            ClassicAssert.AreEqual("error message", ex.Message);
            ClassicAssert.AreEqual("remaining", ex.RemainingMetar);
            ClassicAssert.AreEqual("newRemaining", ex.NewRemainingMetar);
            ClassicAssert.AreEqual(decoder, ex.ChunkDecoder);
        }

        [Test]
        public void TestGetObjectDataWithNullInfoThrows()
        {
            var ex = new MetarChunkDecoderException("remaining", "newRemaining", "error", new IcaoChunkDecoder());
            ClassicAssert.Throws<ArgumentNullException>(() =>
            {
                ex.GetObjectData(null, new StreamingContext());
            });
        }

#pragma warning disable SYSLIB0051
        [Test]
        public void TestGetObjectDataSerializes()
        {
            var decoder = new IcaoChunkDecoder();
            var ex = new MetarChunkDecoderException("remaining", "newRemaining", "error message", decoder);
            var info = new SerializationInfo(typeof(MetarChunkDecoderException), new FormatterConverter());
            var context = new StreamingContext();

            ClassicAssert.DoesNotThrow(() =>
            {
                ex.GetObjectData(info, context);
            });

            ClassicAssert.AreEqual("remaining", info.GetString("NewRemainingMetar"));
            ClassicAssert.AreEqual("newRemaining", info.GetString("RemainingMetar"));
        }
#pragma warning restore SYSLIB0051

        [Test]
        public void TestMessagesConstants()
        {
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.CloudsInformationBadFormat);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.BadDayHourMinuteInformation);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.InvalidDayHourMinuteRanges);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.ICAONotFound);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.AtmosphericPressureNotFound);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.InvalidReportStatus);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.NoInformationExpectedAfterNILStatus);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.InvalidRunwayQFURunwayVisualRangeInformation);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.SurfaceWindInformationBadFormat);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.NoSurfaceWindInformationMeasured);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.InvalidWindDirectionInterval);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.InvalidWindDirectionVariationsInterval);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.ForVisibilityInformationBadFormat);
            ClassicAssert.IsNotNull(MetarChunkDecoderException.Messages.InvalidRunwayQFURunwaVisualRangeInformation);

            ClassicAssert.That(MetarChunkDecoderException.Messages.CloudsInformationBadFormat, Does.Contain("clouds"));
            ClassicAssert.That(MetarChunkDecoderException.Messages.SurfaceWindInformationBadFormat, Does.Contain("surface wind"));
        }
    }
}
