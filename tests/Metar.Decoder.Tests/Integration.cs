using Metar.Decoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Metar.Decoder.Entity.CloudLayer;
using static Metar.Decoder.Entity.DecodedMetar;

namespace Metar.Decoder_tests
{
    /// <summary>
    /// Integration
    /// </summary>
    [TestFixture, Category("Integration")]
    public class Integration
    {
        public readonly ReadOnlyCollection<string> TestMetarSource = new(new List<string>() {
            "CYFB 271515Z 32017KT 3SM DRSN BKN040 M29/M34 A2957 RMK SC7 SLP019",
            "EETU 271450Z 05005KT 9000 OVC006 01/M00 Q1019",
            "SBGU 271400Z 27006KT 8000 TS VCSH BKN020 FEW030CB BKN080 25/20 Q1017",
            "SBGU 321400Z 27006KT 8000 TS VCSH BKN020 FEW030CB BKN080 25/20"  // this one has errors, on purpose !
        });

        private List<DecodedMetar> DecodedMetarsNotStrict;
        private List<DecodedMetar> DecodedMetarsStrict;

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            DecodedMetarsNotStrict = TestMetarSource.Select(metar =>
                MetarDecoder.ParseWithMode(metar, false)
            ).ToList();
            DecodedMetarsStrict = TestMetarSource.Select(metar =>
                MetarDecoder.ParseWithMode(metar, true)
            ).ToList();
        }

        /// <summary>
        /// CYFB271515ZNotStrictTest
        /// </summary>
        [Test]
        public void CYFB271515ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[0];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[0]));
            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("CYFB"));
            Assert.That(decodedMetar.Day, Is.EqualTo(27));
            Assert.That(decodedMetar.Time, Is.EqualTo("15:15 UTC"));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(320));
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(17));
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(3));
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.StatuteMile));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("DR"));
            Assert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.PresentWeather[0].Types[0], Is.EqualTo("SN"));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(4000));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(-29));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(-34));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(29.57));
            Assert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.MercuryInch));

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// EETU271450ZNotStrictTest
        /// </summary>
        [Test]
        public void EETU271450ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[1];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[1]));
            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("EETU"));
            Assert.That(decodedMetar.Day, Is.EqualTo(27));
            Assert.That(decodedMetar.Time, Is.EqualTo("14:50 UTC"));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(50));
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(5));
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(9000));
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.OVC));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(600));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(1));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(0));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1019));
            Assert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU271400ZNotStrictTest
        /// </summary>
        [Test]
        public void SBGU271400ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[2];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[2]));
            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            Assert.That(decodedMetar.Day, Is.EqualTo(27));
            Assert.That(decodedMetar.Time, Is.EqualTo("14:00 UTC"));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(270));
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(6));
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(8000));
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(2));

            Assert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("TS"));
            Assert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(0));

            Assert.That(decodedMetar.PresentWeather[1].IntensityProximity, Is.EqualTo("VC"));
            Assert.That(decodedMetar.PresentWeather[1].Characteristics, Is.EqualTo("SH"));
            Assert.That(decodedMetar.PresentWeather[1].Types.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(3));

            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(2000));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            Assert.That(decodedMetar.Clouds[1].Amount, Is.EqualTo(CloudAmount.FEW));
            Assert.That(decodedMetar.Clouds[1].BaseHeight.ActualValue, Is.EqualTo(3000));
            Assert.That(decodedMetar.Clouds[1].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[1].Type, Is.EqualTo(CloudType.CB));

            Assert.That(decodedMetar.Clouds[2].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[2].BaseHeight.ActualValue, Is.EqualTo(8000));
            Assert.That(decodedMetar.Clouds[2].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[2].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(25));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(20));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1017));
            Assert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU321400ZNotStrictTest
        /// </summary>
        [Test]
        public void SBGU321400ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[3];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[3]));
            //PHP version says there are 0, and reports 2 in the "Invalid format" message at top of page
            //issues probably lies with strict/no strict error handling

            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(3));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            Assert.That(decodedMetar.Day, Is.Null);
            Assert.That(decodedMetar.Time, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            //needs fixing
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(270));
            //needs fixing
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            //needs fixing
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(6));
            //needs fixing
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            //needs fixing
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(8000));
            //needs fixing
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            //needs fixing
            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(2));

            Assert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("TS"));
            Assert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(0));

            Assert.That(decodedMetar.PresentWeather[1].IntensityProximity, Is.EqualTo("VC"));
            Assert.That(decodedMetar.PresentWeather[1].Characteristics, Is.EqualTo("SH"));
            Assert.That(decodedMetar.PresentWeather[1].Types.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            //needs fixing
            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(3));

            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(2000));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            Assert.That(decodedMetar.Clouds[1].Amount, Is.EqualTo(CloudAmount.FEW));
            Assert.That(decodedMetar.Clouds[1].BaseHeight.ActualValue, Is.EqualTo(3000));
            Assert.That(decodedMetar.Clouds[1].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[1].Type, Is.EqualTo(CloudType.CB));

            Assert.That(decodedMetar.Clouds[2].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[2].BaseHeight.ActualValue, Is.EqualTo(8000));
            Assert.That(decodedMetar.Clouds[2].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[2].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            //needs fixing
            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(25));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(20));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure, Is.Null);

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// CYFB271515ZStrictTest
        /// </summary>
        [Test]
        public void CYFB271515ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[0];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[0]));
            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("CYFB"));
            Assert.That(decodedMetar.Day, Is.EqualTo(27));
            Assert.That(decodedMetar.Time, Is.EqualTo("15:15 UTC"));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(320));
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(17));
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(3));
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.StatuteMile));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("DR"));
            Assert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.PresentWeather[0].Types[0], Is.EqualTo("SN"));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(4000));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(-29));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(-34));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(29.57));
            Assert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.MercuryInch));

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// EETU271450ZStrictTest
        /// </summary>
        [Test]
        public void EETU271450ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[1];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[1]));
            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("EETU"));
            Assert.That(decodedMetar.Day, Is.EqualTo(27));
            Assert.That(decodedMetar.Time, Is.EqualTo("14:50 UTC"));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(50));
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(5));
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(9000));
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.OVC));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(600));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(1));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(0));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1019));
            Assert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU271400ZStrictTest
        /// </summary>
        [Test]
        public void SBGU271400ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[2];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[2]));
            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            Assert.That(decodedMetar.Day, Is.EqualTo(27));
            Assert.That(decodedMetar.Time, Is.EqualTo("14:00 UTC"));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(270));
            Assert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            Assert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(6));
            Assert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            Assert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            Assert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Not.Null);
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(8000));
            Assert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            Assert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            Assert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            Assert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(2));

            Assert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("TS"));
            Assert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(0));

            Assert.That(decodedMetar.PresentWeather[1].IntensityProximity, Is.EqualTo("VC"));
            Assert.That(decodedMetar.PresentWeather[1].Characteristics, Is.EqualTo("SH"));
            Assert.That(decodedMetar.PresentWeather[1].Types.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(3));

            Assert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(2000));
            Assert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            Assert.That(decodedMetar.Clouds[1].Amount, Is.EqualTo(CloudAmount.FEW));
            Assert.That(decodedMetar.Clouds[1].BaseHeight.ActualValue, Is.EqualTo(3000));
            Assert.That(decodedMetar.Clouds[1].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[1].Type, Is.EqualTo(CloudType.CB));

            Assert.That(decodedMetar.Clouds[2].Amount, Is.EqualTo(CloudAmount.BKN));
            Assert.That(decodedMetar.Clouds[2].BaseHeight.ActualValue, Is.EqualTo(8000));
            Assert.That(decodedMetar.Clouds[2].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            Assert.That(decodedMetar.Clouds[2].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(25));
            Assert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(20));
            Assert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            Assert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1017));
            Assert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU321400ZStrictTest
        /// </summary>
        [Test]
        public void SBGU321400ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[3];
            Assert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[3]));
            //PHP version says there are 0, and reports 2 in the "Invalid format" message at top of page
            //issues probably lies with strict/no strict error handling

            Assert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(1));
            Assert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            Assert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            Assert.That(decodedMetar.Day, Is.Null);
            Assert.That(decodedMetar.Time, Is.EqualTo(string.Empty));
            Assert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            Assert.That(decodedMetar.SurfaceWind, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            Assert.That(decodedMetar.Visibility, Is.Null);

            #endregion Visibility

            Assert.That(decodedMetar.Cavok, Is.False);
            Assert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            Assert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            Assert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            Assert.That(decodedMetar.Clouds.Count, Is.EqualTo(0));

            #endregion Clouds

            #region Temperatures & pressure

            Assert.That(decodedMetar.AirTemperature, Is.Null);
            Assert.That(decodedMetar.DewPointTemperature, Is.Null);
            Assert.That(decodedMetar.Pressure, Is.Null);

            #endregion Temperatures & pressure

            Assert.That(decodedMetar.RecentWeather, Is.Null);
            Assert.That(decodedMetar.WindshearAllRunways, Is.Null);
            Assert.That(decodedMetar.WindshearRunways, Is.Null);
        }
    }
}