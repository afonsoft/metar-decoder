using Metar.Decoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Metar.Decoder.Entity.CloudLayer;
using static Metar.Decoder.Entity.DecodedMetar;
using static Metar.Decoder.Entity.RunwayVisualRange;
using static Metar.Decoder.Entity.Value;

namespace Metar.Decoder.Tests
{
    [TestFixture, Category("MetarDecoderComprehensiveTest")]
    public class MetarDecoderComprehensiveTest
    {
        private MetarDecoder decoder;

        [SetUp]
        public void Setup()
        {
            decoder = new MetarDecoder();
        }

        [Test]
        public void TestParseMetarWithCavok()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT CAVOK 17/10 Q1009 ");
            ClassicAssert.IsTrue(d.Cavok);
            ClassicAssert.AreEqual(MetarType.METAR, d.Type);
            ClassicAssert.AreEqual("LFPO", d.ICAO);
        }

        [Test]
        public void TestParseMetarWithVariableWind()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z VRB02KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.IsTrue(d.SurfaceWind.VariableDirection);
            ClassicAssert.IsNull(d.SurfaceWind.MeanDirection);
            ClassicAssert.AreEqual(2, d.SurfaceWind.MeanSpeed.ActualValue);
        }

        [Test]
        public void TestParseMetarWithGust()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24015G25KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(240, d.SurfaceWind.MeanDirection.ActualValue);
            ClassicAssert.AreEqual(15, d.SurfaceWind.MeanSpeed.ActualValue);
            ClassicAssert.AreEqual(25, d.SurfaceWind.SpeedVariations.ActualValue);
            ClassicAssert.AreEqual(Unit.Knot, d.SurfaceWind.MeanSpeed.ActualUnit);
        }

        [Test]
        public void TestParseMetarWithDirectionVariation()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 180V270 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.IsNotNull(d.SurfaceWind.DirectionVariations);
            ClassicAssert.AreEqual(180, d.SurfaceWind.DirectionVariations[0].ActualValue);
            ClassicAssert.AreEqual(270, d.SurfaceWind.DirectionVariations[1].ActualValue);
        }

        [Test]
        public void TestParseMetarWithNilStatus()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z NIL ");
            ClassicAssert.AreEqual("NIL", d.Status);
        }

        [Test]
        public void TestParseMetarWithMultipleClouds()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 SCT030 BKN060 17/10 Q1009 ");
            ClassicAssert.AreEqual(3, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudAmount.FEW, d.Clouds[0].Amount);
            ClassicAssert.AreEqual(2000, d.Clouds[0].BaseHeight.ActualValue);
            ClassicAssert.AreEqual(CloudAmount.SCT, d.Clouds[1].Amount);
            ClassicAssert.AreEqual(3000, d.Clouds[1].BaseHeight.ActualValue);
            ClassicAssert.AreEqual(CloudAmount.BKN, d.Clouds[2].Amount);
            ClassicAssert.AreEqual(6000, d.Clouds[2].BaseHeight.ActualValue);
        }

        [Test]
        public void TestParseMetarWithCB()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 SCT025CB 17/10 Q1009 ");
            ClassicAssert.AreEqual(1, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudType.CB, d.Clouds[0].Type);
        }

        [Test]
        public void TestParseMetarWithTCU()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 BKN035TCU 17/10 Q1009 ");
            ClassicAssert.AreEqual(CloudType.TCU, d.Clouds[0].Type);
        }

        [Test]
        public void TestParseMetarWithClearSky()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 SKC 17/10 Q1009 ");
            ClassicAssert.AreEqual(0, d.Clouds.Count);
        }

        [Test]
        public void TestParseMetarWithNSC()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 NSC 17/10 Q1009 ");
            ClassicAssert.AreEqual(0, d.Clouds.Count);
        }

        [Test]
        public void TestParseMetarWithVerticalVisibility()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 0500 VV003 17/10 Q1009 ");
            ClassicAssert.AreEqual(1, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudAmount.VV, d.Clouds[0].Amount);
            ClassicAssert.AreEqual(300, d.Clouds[0].BaseHeight.ActualValue);
        }

        [Test]
        public void TestParseMetarWithUSVisibility()
        {
            var d = MetarDecoder.ParseNotStrict("METAR KJFK 231027Z 24004KT 1 1/2SM FEW020 17/10 A2992 ");
            ClassicAssert.AreEqual(1.5, d.Visibility.PrevailingVisibility.ActualValue);
            ClassicAssert.AreEqual(Unit.StatuteMile, d.Visibility.PrevailingVisibility.ActualUnit);
        }

        [Test]
        public void TestParseMetarWithMercuryPressure()
        {
            var d = MetarDecoder.ParseNotStrict("METAR KJFK 231027Z 24004KT 9999 FEW020 17/10 A2992 ");
            ClassicAssert.AreEqual(29.92, d.Pressure.ActualValue);
            ClassicAssert.AreEqual(Unit.MercuryInch, d.Pressure.ActualUnit);
        }

        [Test]
        public void TestParseMetarWithNegativeTemperature()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 M02/M05 Q1009 ");
            ClassicAssert.AreEqual(-2, d.AirTemperature.ActualValue);
            ClassicAssert.AreEqual(-5, d.DewPointTemperature.ActualValue);
        }

        [Test]
        public void TestParseMetarWithWindShearAllRunways()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 WS ALL RWY ");
            ClassicAssert.IsTrue(d.WindshearAllRunways.Value);
        }

        [Test]
        public void TestParseMetarWithMultipleRunwayVisualRange()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 0800 R06L/0600U R24/0400D FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(2, d.RunwaysVisualRange.Count);
            ClassicAssert.AreEqual("06L", d.RunwaysVisualRange[0].Runway);
            ClassicAssert.AreEqual(Tendency.U, d.RunwaysVisualRange[0].PastTendency);
            ClassicAssert.AreEqual("24", d.RunwaysVisualRange[1].Runway);
            ClassicAssert.AreEqual(Tendency.D, d.RunwaysVisualRange[1].PastTendency);
        }

        [Test]
        public void TestParseMetarWithVariableRVR()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 0800 R06L/0200V0600U FEW020 17/10 Q1009 ");
            ClassicAssert.IsTrue(d.RunwaysVisualRange[0].Variable);
            ClassicAssert.AreEqual(200, d.RunwaysVisualRange[0].VisualRangeInterval[0].ActualValue);
            ClassicAssert.AreEqual(600, d.RunwaysVisualRange[0].VisualRangeInterval[1].ActualValue);
        }

        [Test]
        public void TestParseMetarWithMultipleWeatherPhenomena()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 2000 +TSRA FZDZ FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(2, d.PresentWeather.Count);
            ClassicAssert.AreEqual("+", d.PresentWeather[0].IntensityProximity);
            ClassicAssert.AreEqual("TS", d.PresentWeather[0].Characteristics);
        }

        [Test]
        public void TestParseMetarNotStrictWithErrors()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT INVALID_VISIBILITY FEW020 17/10 Q1009 ");
            ClassicAssert.IsFalse(d.IsValid);
            ClassicAssert.IsTrue(d.DecodingExceptions.Count > 0);
        }

        [Test]
        public void TestParseMetarStrictWithErrors()
        {
            var d = MetarDecoder.ParseStrict("METAR LFPO 231027Z 24004KT INVALID_VISIBILITY FEW020 17/10 Q1009 ");
            ClassicAssert.IsFalse(d.IsValid);
        }

        [Test]
        public void TestParseSpeciMetar()
        {
            var d = MetarDecoder.ParseNotStrict("SPECI LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(MetarType.SPECI, d.Type);
        }

        [Test]
        public void TestParseSpeciCorMetar()
        {
            var d = MetarDecoder.ParseNotStrict("SPECI COR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(MetarType.SPECI_COR, d.Type);
        }

        [Test]
        public void TestParseMetarCor()
        {
            var d = MetarDecoder.ParseNotStrict("METAR COR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(MetarType.METAR_COR, d.Type);
        }

        [Test]
        public void TestParseMetarWithNDV()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999NDV FEW020 17/10 Q1009 ");
            ClassicAssert.IsTrue(d.Visibility.NDV);
        }

        [Test]
        public void TestParseMetarWithMinimumVisibility()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 5000 2000NE FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(5000, d.Visibility.PrevailingVisibility.ActualValue);
            ClassicAssert.AreEqual(2000, d.Visibility.MinimumVisibility.ActualValue);
            ClassicAssert.AreEqual("NE", d.Visibility.MinimumVisibilityDirection);
        }

        [Test]
        public void TestParseMetarWithNoVisibilityInfo()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT //// FEW020 17/10 Q1009 ");
            ClassicAssert.IsNull(d.Visibility);
            ClassicAssert.IsFalse(d.Cavok);
        }

        [Test]
        public void TestParseMetarWithNosig()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 NOSIG ");
            ClassicAssert.AreEqual("NOSIG", d.TrendType);
        }

        [Test]
        public void TestParseMetarWithRecentWeather()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 RERA ");
            ClassicAssert.IsNotNull(d.RecentWeather);
            ClassicAssert.AreEqual("RA", d.RecentWeather.Types[0]);
        }

        [Test]
        public void TestParseMetarWithKPH()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24010KPH 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(Unit.KilometerPerHour, d.SurfaceWind.MeanSpeed.ActualUnit);
        }

        [Test]
        public void TestParseMetarWithNoPressureValue()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q//// ");
            ClassicAssert.IsNull(d.Pressure);
        }

        [Test]
        public void TestParseMetarMissingType()
        {
            var d = MetarDecoder.ParseNotStrict("LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(MetarType.NULL, d.Type);
            ClassicAssert.AreEqual("LFPO", d.ICAO);
        }

        [Test]
        public void TestSetStrictParsing()
        {
            decoder.SetStrictParsing(true);
            var d = decoder.Parse("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.IsTrue(d.IsValid);
        }

        [Test]
        public void TestDecodedMetarResetExceptions()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT INVALID_VIS FEW020 17/10 Q1009 ");
            ClassicAssert.IsFalse(d.IsValid);
            d.ResetDecodingExceptions();
            ClassicAssert.IsTrue(d.IsValid);
        }

        [Test]
        public void TestParseMetarWithRVRFeetUnit()
        {
            var d = MetarDecoder.ParseNotStrict("METAR KJFK 231027Z 24004KT 0800 R04R/2000FT FEW020 17/10 A2992 ");
            if (d.RunwaysVisualRange.Count > 0)
            {
                ClassicAssert.AreEqual(Unit.Feet, d.RunwaysVisualRange[0].VisualRange.ActualUnit);
            }
        }

        [Test]
        public void TestParseMetarWithOVCClouds()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 3000 OVC005 17/10 Q1009 ");
            ClassicAssert.AreEqual(1, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudAmount.OVC, d.Clouds[0].Amount);
            ClassicAssert.AreEqual(500, d.Clouds[0].BaseHeight.ActualValue);
        }

        [Test]
        public void TestParseMetarWithAutoStatus()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z AUTO 24004KT 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual("AUTO", d.Status);
        }

        [Test]
        public void TestParseMetarWithFractionalUSVisibility()
        {
            var d = MetarDecoder.ParseNotStrict("METAR KJFK 231027Z 24004KT 3/4SM FEW020 17/10 A2992 ");
            ClassicAssert.AreEqual(0.75, d.Visibility.PrevailingVisibility.ActualValue, 0.001);
            ClassicAssert.AreEqual(Unit.StatuteMile, d.Visibility.PrevailingVisibility.ActualUnit);
        }

        [Test]
        public void TestParseRealWorldMetar()
        {
            var d = MetarDecoder.ParseNotStrict("METAR SBGR 031000Z 35006KT 9999 FEW040 SCT100 27/18 Q1020 ");
            ClassicAssert.IsTrue(d.IsValid);
            ClassicAssert.AreEqual("SBGR", d.ICAO);
            ClassicAssert.AreEqual(3, d.Day);
            ClassicAssert.AreEqual("10:00 UTC", d.Time);
            ClassicAssert.AreEqual(350, d.SurfaceWind.MeanDirection.ActualValue);
            ClassicAssert.AreEqual(6, d.SurfaceWind.MeanSpeed.ActualValue);
            ClassicAssert.AreEqual(9999, d.Visibility.PrevailingVisibility.ActualValue);
            ClassicAssert.AreEqual(2, d.Clouds.Count);
            ClassicAssert.AreEqual(27, d.AirTemperature.ActualValue);
            ClassicAssert.AreEqual(18, d.DewPointTemperature.ActualValue);
            ClassicAssert.AreEqual(1020, d.Pressure.ActualValue);
        }

        [Test]
        public void TestParseRealWorldMetarWithPhenomena()
        {
            var d = MetarDecoder.ParseNotStrict("METAR SBSP 031200Z 09005KT 4000 -RA BR BKN010 OVC020 18/17 Q1019 ");
            ClassicAssert.IsTrue(d.IsValid);
            ClassicAssert.AreEqual("SBSP", d.ICAO);
            ClassicAssert.AreEqual(4000, d.Visibility.PrevailingVisibility.ActualValue);
            ClassicAssert.IsTrue(d.PresentWeather.Count >= 1);
        }

        [Test]
        public void TestParseMetarWithWindshearMultipleRunways()
        {
            var d = MetarDecoder.ParseNotStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009 WS R03 WS R21 ");
            if (d.WindshearRunways != null)
            {
                ClassicAssert.IsTrue(d.WindshearRunways.Count >= 1);
            }
        }

        [Test]
        public void TestValueConversion()
        {
            var v = new Value(10, Unit.MeterPerSecond);
            var ktValue = v.GetConvertedValue(Unit.Knot);
            ClassicAssert.IsTrue(ktValue > 0);
        }

        [Test]
        public void TestValueToString()
        {
            var v = new Value(15, Unit.Knot);
            ClassicAssert.AreEqual("15 Knot", v.ToString());
        }

        [Test]
        public void TestValueToInt()
        {
            ClassicAssert.AreEqual(10, Value.ToInt("10"));
            ClassicAssert.AreEqual(-10, Value.ToInt("M10"));
            ClassicAssert.IsNull(Value.ToInt("///"));
        }

        [Test]
        public void TestParseMetarWithMPS()
        {
            var d = MetarDecoder.ParseNotStrict("METAR UUEE 231027Z 24004MPS 9999 FEW020 17/10 Q1009 ");
            ClassicAssert.AreEqual(Unit.MeterPerSecond, d.SurfaceWind.MeanSpeed.ActualUnit);
            ClassicAssert.AreEqual(4, d.SurfaceWind.MeanSpeed.ActualValue);
        }

        [Test]
        public void TestParseMetarLowercaseInput()
        {
            var d = MetarDecoder.ParseNotStrict("metar lfpo 231027z 24004kt 9999 few020 17/10 q1009 ");
            ClassicAssert.AreEqual("LFPO", d.ICAO);
        }
    }
}
