using Taf.Decoder;
using Taf.Decoder.ChunkDecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Taf.Decoder.Tests.Entity
{
    [TestFixture, Category("TafChunkDecoderException")]
    public class TafExceptionExtendedTest
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var ex = new TafChunkDecoderException();
            ClassicAssert.IsNotNull(ex);
            ClassicAssert.IsNull(ex.RemainingTaf);
            ClassicAssert.IsNull(ex.NewRemainingTaf);
            ClassicAssert.IsNull(ex.ChunkDecoder);
        }

        [Test]
        public void TestMessageConstructor()
        {
            var ex = new TafChunkDecoderException("test message");
            ClassicAssert.AreEqual("test message", ex.Message);
            ClassicAssert.IsNull(ex.RemainingTaf);
            ClassicAssert.IsNull(ex.NewRemainingTaf);
            ClassicAssert.IsNull(ex.ChunkDecoder);
        }

        [Test]
        public void TestFullConstructor()
        {
            var decoder = new IcaoChunkDecoder();
            var ex = new TafChunkDecoderException("remaining", "newRemaining", "error message", decoder);
            ClassicAssert.AreEqual("error message", ex.Message);
            ClassicAssert.AreEqual("remaining", ex.RemainingTaf);
            ClassicAssert.AreEqual("newRemaining", ex.NewRemainingTaf);
            ClassicAssert.AreEqual(decoder, ex.ChunkDecoder);
        }

        [Test]
        public void TestGetObjectDataWithNullInfoThrows()
        {
            var decoder = new IcaoChunkDecoder();
            var ex = new TafChunkDecoderException("remaining", "newRemaining", "error", decoder);
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
            var ex = new TafChunkDecoderException("remaining", "newRemaining", "error message", decoder);
            var info = new SerializationInfo(typeof(TafChunkDecoderException), new FormatterConverter());
            var context = new StreamingContext();

            ClassicAssert.DoesNotThrow(() =>
            {
                ex.GetObjectData(info, context);
            });

            ClassicAssert.AreEqual("remaining", info.GetString("NewRemainingTaf"));
            ClassicAssert.AreEqual("newRemaining", info.GetString("RemainingTaf"));
        }
#pragma warning restore SYSLIB0051

        [Test]
        public void TestSerializationConstructorRestoresFields()
        {
#pragma warning disable SYSLIB0051
            var info = new SerializationInfo(typeof(TafChunkDecoderException), new FormatterConverter());
            info.AddValue("RemainingTaf", "REMAINING");
            info.AddValue("NewRemainingTaf", "NEW");
            info.AddValue("Message", "Test");
            info.AddValue("ClassName", typeof(TafChunkDecoderException).FullName);
            info.AddValue("InnerException", null);
            info.AddValue("StackTraceString", null);
            info.AddValue("RemoteStackTraceString", null);
            info.AddValue("RemoteStackIndex", 0);
            info.AddValue("ExceptionMethod", null);
            info.AddValue("HResult", -2146233088);
            info.AddValue("Source", null);
            info.AddValue("HelpURL", null);

            var ctor = typeof(TafChunkDecoderException).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(SerializationInfo), typeof(StreamingContext) },
                null);

            var ex = (TafChunkDecoderException)ctor.Invoke(new object[] { info, new StreamingContext() });

            ClassicAssert.AreEqual("REMAINING", ex.RemainingTaf);
            ClassicAssert.AreEqual("NEW", ex.NewRemainingTaf);
#pragma warning restore SYSLIB0051
        }

        [Test]
        public void TestMessagesConstants()
        {
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.CloudsInformationBadFormat);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.BadDayHourMinuteInformation);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidDayHourMinuteRanges);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.ICAONotFound);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.AtmosphericPressureNotFound);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidReportStatus);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.NoInformationExpectedAfterNILStatus);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidRunwayQFURunwayVisualRangeInformation);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.SurfaceWindInformationBadFormat);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.NoSurfaceWindInformationMeasured);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidWindDirectionInterval);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidWindDirectionVariationsInterval);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.ForVisibilityInformationBadFormat);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidRunwayQFURunwaVisualRangeInformation);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidForecastPeriodInformation);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InvalidValuesForTheForecastPeriod);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.InconsistentValuesForTemperatureInformation);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.WeatherEvolutionBadFormat);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.EvolutionInformationBadFormat);
            ClassicAssert.IsNotNull(TafChunkDecoderException.Messages.UnknownEntity);

            ClassicAssert.That(TafChunkDecoderException.Messages.CloudsInformationBadFormat, Does.Contain("clouds"));
            ClassicAssert.That(TafChunkDecoderException.Messages.UnknownEntity, Does.Contain("Unknown"));
        }
    }
}
