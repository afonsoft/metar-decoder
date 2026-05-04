using Metar.Decoder;
using Metar.Decoder.ChunkDecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Metar.Decoder.Tests.Entity
{
    [TestFixture, Category("Entity")]
    public class DecodedMetarExtendedTest
    {
        [Test]
        public void TestDefaultValues()
        {
            var metar = new DecodedMetar();
            ClassicAssert.AreEqual(string.Empty, metar.RawMetar);
            ClassicAssert.AreEqual(DecodedMetar.MetarType.NULL, metar.Type);
            ClassicAssert.AreEqual(string.Empty, metar.ICAO);
            ClassicAssert.IsNull(metar.Day);
            ClassicAssert.AreEqual(string.Empty, metar.Time);
            ClassicAssert.IsNull(metar.ObservationDateTime);
            ClassicAssert.AreEqual(string.Empty, metar.Status);
            ClassicAssert.IsNull(metar.SurfaceWind);
            ClassicAssert.IsNull(metar.Visibility);
            ClassicAssert.IsFalse(metar.Cavok);
            ClassicAssert.IsNotNull(metar.RunwaysVisualRange);
            ClassicAssert.AreEqual(0, metar.RunwaysVisualRange.Count);
            ClassicAssert.IsNotNull(metar.PresentWeather);
            ClassicAssert.AreEqual(0, metar.PresentWeather.Count);
            ClassicAssert.IsNotNull(metar.Clouds);
            ClassicAssert.AreEqual(0, metar.Clouds.Count);
            ClassicAssert.IsNull(metar.AirTemperature);
            ClassicAssert.IsNull(metar.DewPointTemperature);
            ClassicAssert.IsNull(metar.Pressure);
            ClassicAssert.IsNull(metar.RecentWeather);
            ClassicAssert.IsNull(metar.WindshearAllRunways);
            ClassicAssert.IsNull(metar.WindshearRunways);
            ClassicAssert.AreEqual(string.Empty, metar.TrendType);
            ClassicAssert.AreEqual(string.Empty, metar.TrendForecast);
            ClassicAssert.AreEqual(string.Empty, metar.Remark);
            ClassicAssert.IsNull(metar.SeaLevelPressure);
        }

        [Test]
        public void TestIsValidWithNoExceptions()
        {
            var metar = new DecodedMetar("METAR SBGL");
            ClassicAssert.IsTrue(metar.IsValid);
            ClassicAssert.AreEqual(0, metar.DecodingExceptions.Count);
        }

        [Test]
        public void TestIsValidWithExceptions()
        {
            var metar = new DecodedMetar("METAR SBGL");
            metar.AddDecodingException(new MetarChunkDecoderException("test"));
            ClassicAssert.IsFalse(metar.IsValid);
            ClassicAssert.AreEqual(1, metar.DecodingExceptions.Count);
        }

        [Test]
        public void TestResetDecodingExceptions()
        {
            var metar = new DecodedMetar("METAR SBGL");
            metar.AddDecodingException(new MetarChunkDecoderException("test1"));
            metar.AddDecodingException(new MetarChunkDecoderException("test2"));
            ClassicAssert.AreEqual(2, metar.DecodingExceptions.Count);
            ClassicAssert.IsFalse(metar.IsValid);

            metar.ResetDecodingExceptions();
            ClassicAssert.AreEqual(0, metar.DecodingExceptions.Count);
            ClassicAssert.IsTrue(metar.IsValid);
        }

        [Test]
        public void TestRawMetarTrimsWhitespace()
        {
            var metar = new DecodedMetar("  METAR SBGL  ");
            ClassicAssert.AreEqual("METAR SBGL", metar.RawMetar);
        }

        [Test]
        public void TestSetProperties()
        {
            var metar = new DecodedMetar("METAR SBGL 041200Z");
            metar.Type = DecodedMetar.MetarType.METAR;
            metar.ICAO = "SBGL";
            metar.Day = 4;
            metar.Time = "12:00 UTC";
            metar.Cavok = true;
            metar.Status = "AUTO";
            metar.TrendType = "NOSIG";
            metar.TrendForecast = "NOSIG";
            metar.Remark = "RMK AO2";

            ClassicAssert.AreEqual(DecodedMetar.MetarType.METAR, metar.Type);
            ClassicAssert.AreEqual("SBGL", metar.ICAO);
            ClassicAssert.AreEqual(4, metar.Day);
            ClassicAssert.AreEqual("12:00 UTC", metar.Time);
            ClassicAssert.IsTrue(metar.Cavok);
            ClassicAssert.AreEqual("AUTO", metar.Status);
            ClassicAssert.AreEqual("NOSIG", metar.TrendType);
            ClassicAssert.AreEqual("NOSIG", metar.TrendForecast);
            ClassicAssert.AreEqual("RMK AO2", metar.Remark);
        }

        [Test]
        public void TestSurfaceWindProperty()
        {
            var metar = new DecodedMetar();
            var wind = new SurfaceWind
            {
                MeanSpeed = new Value(10, Value.Unit.Knot),
                MeanDirection = new Value(180, Value.Unit.Degree),
                VariableDirection = false
            };
            metar.SurfaceWind = wind;
            ClassicAssert.IsNotNull(metar.SurfaceWind);
            ClassicAssert.AreEqual(10, metar.SurfaceWind.MeanSpeed.ActualValue);
        }

        [Test]
        public void TestCloudLayersProperty()
        {
            var metar = new DecodedMetar();
            metar.Clouds.Add(new CloudLayer());
            metar.Clouds.Add(new CloudLayer());
            ClassicAssert.AreEqual(2, metar.Clouds.Count);
        }

        [Test]
        public void TestMetarTypeEnum()
        {
            ClassicAssert.AreEqual(DecodedMetar.MetarType.NULL, (DecodedMetar.MetarType)0);
            ClassicAssert.AreEqual(DecodedMetar.MetarType.METAR, (DecodedMetar.MetarType)1);
            ClassicAssert.AreEqual(DecodedMetar.MetarType.METAR_COR, (DecodedMetar.MetarType)2);
            ClassicAssert.AreEqual(DecodedMetar.MetarType.SPECI, (DecodedMetar.MetarType)3);
            ClassicAssert.AreEqual(DecodedMetar.MetarType.SPECI_COR, (DecodedMetar.MetarType)4);
        }

        [Test]
        public void TestMetarStatusEnum()
        {
            ClassicAssert.AreEqual(DecodedMetar.MetarStatus.NULL, (DecodedMetar.MetarStatus)0);
            ClassicAssert.AreEqual(DecodedMetar.MetarStatus.AUTO, (DecodedMetar.MetarStatus)1);
            ClassicAssert.AreEqual(DecodedMetar.MetarStatus.NIL, (DecodedMetar.MetarStatus)2);
        }
    }
}
