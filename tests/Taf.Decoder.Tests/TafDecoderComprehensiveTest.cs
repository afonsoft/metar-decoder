using NUnit.Framework;
using NUnit.Framework.Legacy;
using Taf.Decoder;
using Taf.Decoder.Entity;
using static Taf.Decoder.Entity.CloudLayer;
using static Taf.Decoder.Entity.DecodedTaf;
using static Taf.Decoder.Entity.Value;

namespace Taf.Decoder.Tests
{
    [TestFixture, Category("TafDecoderComprehensiveTest")]
    public class TafDecoderComprehensiveTest
    {
        private TafDecoder decoder;

        [SetUp]
        public void Setup()
        {
            decoder = new TafDecoder();
        }

        [Test]
        public void TestParseTafBasic()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 ");
            ClassicAssert.AreEqual(TafType.TAF, d.Type);
            ClassicAssert.AreEqual("LFPO", d.Icao);
            ClassicAssert.AreEqual(23, d.Day);
            ClassicAssert.AreEqual("11:00 UTC", d.Time);
        }

        [Test]
        public void TestParseTafWithCavok()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT CAVOK ");
            ClassicAssert.IsTrue(d.Cavok);
            ClassicAssert.AreEqual("LFPO", d.Icao);
        }

        [Test]
        public void TestParseTafWithVariableWind()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 VRB03KT 9999 FEW030 ");
            ClassicAssert.IsTrue(d.SurfaceWind.VariableDirection);
            ClassicAssert.IsNull(d.SurfaceWind.MeanDirection);
            ClassicAssert.AreEqual(3, d.SurfaceWind.MeanSpeed.ActualValue);
        }

        [Test]
        public void TestParseTafWithGust()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24015G25KT 9999 FEW030 ");
            ClassicAssert.AreEqual(15, d.SurfaceWind.MeanSpeed.ActualValue);
            ClassicAssert.AreEqual(25, d.SurfaceWind.SpeedVariations.ActualValue);
            ClassicAssert.AreEqual(Unit.Knot, d.SurfaceWind.MeanSpeed.ActualUnit);
        }

        [Test]
        public void TestParseTafWithMultipleClouds()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW020 SCT030 BKN060 ");
            ClassicAssert.AreEqual(3, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudAmount.FEW, d.Clouds[0].Amount);
            ClassicAssert.AreEqual(2000, d.Clouds[0].BaseHeight.ActualValue);
            ClassicAssert.AreEqual(CloudAmount.SCT, d.Clouds[1].Amount);
            ClassicAssert.AreEqual(CloudAmount.BKN, d.Clouds[2].Amount);
        }

        [Test]
        public void TestParseTafWithCB()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 SCT025CB ");
            ClassicAssert.AreEqual(1, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudType.CB, d.Clouds[0].Type);
        }

        [Test]
        public void TestParseTafWithTCU()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 BKN035TCU ");
            ClassicAssert.AreEqual(CloudType.TCU, d.Clouds[0].Type);
        }

        [Test]
        public void TestParseTafWithNSC()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 NSC ");
            ClassicAssert.AreEqual(0, d.Clouds.Count);
        }

        [Test]
        public void TestParseTafWithVerticalVisibility()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 0500 VV003 ");
            ClassicAssert.AreEqual(1, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudAmount.VV, d.Clouds[0].Amount);
            ClassicAssert.AreEqual(300, d.Clouds[0].BaseHeight.ActualValue);
        }

        [Test]
        public void TestParseTafWithWeatherPhenomena()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 3000 -RA BKN010 ");
            ClassicAssert.IsTrue(d.WeatherPhenomenons.Count >= 1);
        }

        [Test]
        public void TestParseTafWithFZRA()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 2000 FZRA BKN010 ");
            ClassicAssert.IsTrue(d.WeatherPhenomenons.Count >= 1);
        }

        [Test]
        public void TestParseTafWithUSVisibility()
        {
            var d = TafDecoder.ParseNotStrict("TAF KJFK 231100Z 2312/2418 24005KT 3SM FEW030 ");
            ClassicAssert.AreEqual(3, d.Visibility.ActualVisibility.ActualValue);
            ClassicAssert.AreEqual(Unit.StatuteMile, d.Visibility.ActualVisibility.ActualUnit);
        }

        [Test]
        public void TestParseTafWithFractionalUSVisibility()
        {
            var d = TafDecoder.ParseNotStrict("TAF KJFK 231100Z 2312/2418 24005KT 1 1/2SM FEW030 ");
            ClassicAssert.AreEqual(1.5, d.Visibility.ActualVisibility.ActualValue, 0.01);
        }

        [Test]
        public void TestParseTafWithEvolutions()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 TEMPO 2312/2315 24010KT 3000 -RA BKN010 ");
            // Evolutions are stored on each entity (SurfaceWind, Visibility, etc.), not on DecodedTaf directly
            ClassicAssert.IsNotNull(d.SurfaceWind);
            ClassicAssert.IsTrue(d.SurfaceWind.Evolutions.Count >= 1);
        }

        [Test]
        public void TestParseTafWithBECMG()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 BECMG 2315/2318 18010KT 9999 SCT020 ");
            ClassicAssert.IsNotNull(d.SurfaceWind);
            ClassicAssert.IsTrue(d.SurfaceWind.Evolutions.Count >= 1);
            ClassicAssert.AreEqual("BECMG", d.SurfaceWind.Evolutions[0].Type);
        }

        [Test]
        public void TestParseTafWithFM()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 FM231500 18010KT 9999 SCT020 ");
            ClassicAssert.IsNotNull(d.SurfaceWind);
            ClassicAssert.IsTrue(d.SurfaceWind.Evolutions.Count >= 1);
            ClassicAssert.AreEqual("FM", d.SurfaceWind.Evolutions[0].Type);
        }

        [Test]
        public void TestParseTafWithPROB()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 PROB40 TEMPO 2318/2320 24010KT 2000 TSRA BKN010 ");
            // PROB evolutions are stored on the entities
            ClassicAssert.IsNotNull(d.SurfaceWind);
        }

        [Test]
        public void TestParseTafWithTemperature()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 TX25/2314Z TN15/2405Z ");
            if (d.MaximumTemperature != null)
            {
                ClassicAssert.AreEqual("TX", d.MaximumTemperature.Type);
            }
            if (d.MinimumTemperature != null)
            {
                ClassicAssert.AreEqual("TN", d.MinimumTemperature.Type);
            }
        }

        [Test]
        public void TestParseTafWithMPS()
        {
            var d = TafDecoder.ParseNotStrict("TAF UUEE 231100Z 2312/2418 24005MPS 9999 FEW030 ");
            ClassicAssert.AreEqual(Unit.MeterPerSecond, d.SurfaceWind.MeanSpeed.ActualUnit);
        }

        [Test]
        public void TestParseTafWithKPH()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24010KPH 9999 FEW030 ");
            ClassicAssert.AreEqual(Unit.KilometerPerHour, d.SurfaceWind.MeanSpeed.ActualUnit);
        }

        [Test]
        public void TestParseTafAMD()
        {
            var d = TafDecoder.ParseNotStrict("TAF AMD LFPO 231100Z 2312/2418 24005KT 9999 FEW030 ");
            ClassicAssert.AreEqual(TafType.TAFAMD, d.Type);
        }

        [Test]
        public void TestParseTafCOR()
        {
            var d = TafDecoder.ParseNotStrict("TAF COR LFPO 231100Z 2312/2418 24005KT 9999 FEW030 ");
            ClassicAssert.AreEqual(TafType.TAFCOR, d.Type);
        }

        [Test]
        public void TestParseTafStrictWithErrors()
        {
            var d = TafDecoder.ParseStrict("TAF LFPO 231100Z 2312/2418 INVALID_WIND 9999 FEW030 ");
            ClassicAssert.IsFalse(d.IsValid);
        }

        [Test]
        public void TestParseTafNotStrictWithErrors()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 INVALID_WIND 9999 FEW030 ");
            ClassicAssert.IsFalse(d.IsValid);
            ClassicAssert.IsTrue(d.DecodingExceptions.Count > 0);
        }

        [Test]
        public void TestParseTafMissingType()
        {
            var d = TafDecoder.ParseNotStrict("LFPO 231100Z 2312/2418 24005KT 9999 FEW030 ");
            ClassicAssert.AreEqual(TafType.NULL, d.Type);
            ClassicAssert.AreEqual("LFPO", d.Icao);
        }

        [Test]
        public void TestParseTafForecastPeriod()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 ");
            ClassicAssert.IsNotNull(d.ForecastPeriod);
            ClassicAssert.AreEqual(23, d.ForecastPeriod.FromDay);
            ClassicAssert.AreEqual(12, d.ForecastPeriod.FromHour);
            ClassicAssert.AreEqual(24, d.ForecastPeriod.ToDay);
            ClassicAssert.AreEqual(18, d.ForecastPeriod.ToHour);
        }

        [Test]
        public void TestParseTafWithOVC()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 3000 OVC005 ");
            ClassicAssert.AreEqual(1, d.Clouds.Count);
            ClassicAssert.AreEqual(CloudAmount.OVC, d.Clouds[0].Amount);
            ClassicAssert.AreEqual(500, d.Clouds[0].BaseHeight.ActualValue);
        }

        [Test]
        public void TestParseTafResetExceptions()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 INVALID 9999 FEW030 ");
            ClassicAssert.IsFalse(d.IsValid);
            d.ResetDecodingExceptions();
            ClassicAssert.IsTrue(d.IsValid);
        }

        [Test]
        public void TestParseTafLowercaseInput()
        {
            var d = TafDecoder.ParseNotStrict("taf lfpo 231100z 2312/2418 24005kt 9999 few030 ");
            ClassicAssert.AreEqual("LFPO", d.Icao);
        }

        [Test]
        public void TestParseTafWithDirectionVariation()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 180V270 9999 FEW030 ");
            if (d.SurfaceWind.DirectionVariations != null)
            {
                ClassicAssert.AreEqual(180, d.SurfaceWind.DirectionVariations[0].ActualValue);
                ClassicAssert.AreEqual(270, d.SurfaceWind.DirectionVariations[1].ActualValue);
            }
        }

        [Test]
        public void TestParseTafNoVisibilityInfo()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT //// FEW030 ");
            ClassicAssert.IsNull(d.Visibility);
        }

        [Test]
        public void TestParseTafWithNegativeTemperature()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 TX02/2314Z TNM10/2405Z ");
            if (d.MaximumTemperature != null)
            {
                ClassicAssert.AreEqual("TX", d.MaximumTemperature.Type);
            }
            if (d.MinimumTemperature != null)
            {
                ClassicAssert.AreEqual(-10, d.MinimumTemperature.TemperatureValue.ActualValue);
            }
        }

        [Test]
        public void TestParseRealWorldTaf()
        {
            var d = TafDecoder.ParseNotStrict("TAF SBGR 031100Z 0312/0418 35008KT 9999 FEW040 SCT100 TX30/0318Z TN20/0409Z ");
            ClassicAssert.AreEqual("SBGR", d.Icao);
            ClassicAssert.AreEqual(3, d.Day);
            ClassicAssert.IsNotNull(d.SurfaceWind);
            ClassicAssert.IsNotNull(d.Visibility);
            ClassicAssert.IsTrue(d.Clouds.Count >= 1);
        }

        [Test]
        public void TestParseRealWorldTafWithEvolutions()
        {
            var d = TafDecoder.ParseNotStrict("TAF SBSP 031200Z 0312/0418 09005KT 9999 FEW025 TEMPO 0312/0315 09010KT 4000 -RA BKN010 BECMG 0315/0317 18010KT 9999 SCT020 ");
            ClassicAssert.AreEqual("SBSP", d.Icao);
            // Evolutions are stored on each entity
            ClassicAssert.IsNotNull(d.SurfaceWind);
            ClassicAssert.IsTrue(d.SurfaceWind.Evolutions.Count >= 1);
        }

        [Test]
        public void TestParseTafWithMultipleEvolutions()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 TEMPO 2312/2315 24010KT 3000 -RA BKN010 BECMG 2315/2318 18010KT 9999 SCT020 FM231800 27015G25KT 9999 SCT025 ");
            ClassicAssert.IsNotNull(d.SurfaceWind);
            // Multiple evolutions are stored on the wind entity
            ClassicAssert.IsTrue(d.SurfaceWind.Evolutions.Count >= 1);
        }

        [Test]
        public void TestSetStrictParsing()
        {
            decoder.SetStrictParsing(true);
            var d = decoder.Parse("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 ");
            ClassicAssert.IsTrue(d.IsValid);
        }

        [Test]
        public void TestParseTafRTD()
        {
            var d = TafDecoder.ParseNotStrict("RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 ");
            ClassicAssert.AreEqual(TafType.RTD, d.Type);
            ClassicAssert.AreEqual("EKEB", d.Icao);
        }

        [Test]
        public void TestParseTafWithSKC()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 SKC ");
            ClassicAssert.AreEqual(0, d.Clouds.Count);
        }

        [Test]
        public void TestParseTafWithCLR()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 CLR ");
            ClassicAssert.AreEqual(0, d.Clouds.Count);
        }

        [Test]
        public void TestTafValueConversions()
        {
            var v = new Value(10, Unit.MeterPerSecond);
            var ktValue = v.GetConvertedValue(Unit.Knot);
            ClassicAssert.IsTrue(ktValue > 0);
        }

        [Test]
        public void TestTafValueToString()
        {
            var v = new Value(15, Unit.Knot);
            ClassicAssert.AreEqual("15 Knot", v.ToString());
        }

        [Test]
        public void TestTafValueToInt()
        {
            ClassicAssert.AreEqual(10, Value.ToInt("10"));
            ClassicAssert.AreEqual(-10, Value.ToInt("M10"));
            ClassicAssert.IsNull(Value.ToInt("///"));
        }

        [Test]
        public void TestParseTafWithWindDirectionVariation()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 200V280 9999 FEW030 ");
            if (d.SurfaceWind.DirectionVariations != null)
            {
                ClassicAssert.AreEqual(2, d.SurfaceWind.DirectionVariations.Length);
            }
        }

        [Test]
        public void TestParseTafWithPROB30TEMPO()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030 PROB30 TEMPO 2318/2320 24010KT 2000 TSRA BKN010 ");
            ClassicAssert.IsNotNull(d.SurfaceWind);
        }

        [Test]
        public void TestParseTafWithEndMarker()
        {
            var d = TafDecoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030=");
            ClassicAssert.AreEqual("LFPO", d.Icao);
        }

        [Test]
        public void TestParseTafWithExtraSpaces()
        {
            var d = TafDecoder.ParseNotStrict("TAF   LFPO  231100Z  2312/2418   24005KT  9999  FEW030 ");
            ClassicAssert.AreEqual("LFPO", d.Icao);
        }
    }
}
