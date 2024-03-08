using Metar.Decoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[0]));
            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("CYFB"));
            ClassicAssert.That(decodedMetar.Day, Is.EqualTo(27));
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo("15:15 UTC"));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(320));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(17));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(3));
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.StatuteMile));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("DR"));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types[0], Is.EqualTo("SN"));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(4000));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(-29));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(-34));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(29.57));
            ClassicAssert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.MercuryInch));

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// EETU271450ZNotStrictTest
        /// </summary>
        [Test]
        public void EETU271450ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[1];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[1]));
            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("EETU"));
            ClassicAssert.That(decodedMetar.Day, Is.EqualTo(27));
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo("14:50 UTC"));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(50));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(5));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(9000));
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.OVC));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(600));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1019));
            ClassicAssert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU271400ZNotStrictTest
        /// </summary>
        [Test]
        public void SBGU271400ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[2];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[2]));
            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            ClassicAssert.That(decodedMetar.Day, Is.EqualTo(27));
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo("14:00 UTC"));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(270));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(6));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(8000));
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(2));

            ClassicAssert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("TS"));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(0));

            ClassicAssert.That(decodedMetar.PresentWeather[1].IntensityProximity, Is.EqualTo("VC"));
            ClassicAssert.That(decodedMetar.PresentWeather[1].Characteristics, Is.EqualTo("SH"));
            ClassicAssert.That(decodedMetar.PresentWeather[1].Types.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(3));

            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(2000));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            ClassicAssert.That(decodedMetar.Clouds[1].Amount, Is.EqualTo(CloudAmount.FEW));
            ClassicAssert.That(decodedMetar.Clouds[1].BaseHeight.ActualValue, Is.EqualTo(3000));
            ClassicAssert.That(decodedMetar.Clouds[1].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[1].Type, Is.EqualTo(CloudType.CB));

            ClassicAssert.That(decodedMetar.Clouds[2].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[2].BaseHeight.ActualValue, Is.EqualTo(8000));
            ClassicAssert.That(decodedMetar.Clouds[2].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[2].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(25));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(20));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1017));
            ClassicAssert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU321400ZNotStrictTest
        /// </summary>
        [Test]
        public void SBGU321400ZNotStrictTest()
        {
            var decodedMetar = DecodedMetarsNotStrict[3];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[3]));
            //PHP version says there are 0, and reports 2 in the "Invalid format" message at top of page
            //issues probably lies with strict/no strict error handling

            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(3));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            ClassicAssert.That(decodedMetar.Day, Is.Null);
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            //needs fixing
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(270));
            //needs fixing
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            //needs fixing
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(6));
            //needs fixing
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            //needs fixing
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(8000));
            //needs fixing
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            //needs fixing
            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(2));

            ClassicAssert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("TS"));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(0));

            ClassicAssert.That(decodedMetar.PresentWeather[1].IntensityProximity, Is.EqualTo("VC"));
            ClassicAssert.That(decodedMetar.PresentWeather[1].Characteristics, Is.EqualTo("SH"));
            ClassicAssert.That(decodedMetar.PresentWeather[1].Types.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            //needs fixing
            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(3));

            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(2000));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            ClassicAssert.That(decodedMetar.Clouds[1].Amount, Is.EqualTo(CloudAmount.FEW));
            ClassicAssert.That(decodedMetar.Clouds[1].BaseHeight.ActualValue, Is.EqualTo(3000));
            ClassicAssert.That(decodedMetar.Clouds[1].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[1].Type, Is.EqualTo(CloudType.CB));

            ClassicAssert.That(decodedMetar.Clouds[2].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[2].BaseHeight.ActualValue, Is.EqualTo(8000));
            ClassicAssert.That(decodedMetar.Clouds[2].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[2].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            //needs fixing
            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(25));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(20));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure, Is.Null);

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// CYFB271515ZStrictTest
        /// </summary>
        [Test]
        public void CYFB271515ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[0];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[0]));
            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("CYFB"));
            ClassicAssert.That(decodedMetar.Day, Is.EqualTo(27));
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo("15:15 UTC"));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(320));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(17));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(3));
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.StatuteMile));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("DR"));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types[0], Is.EqualTo("SN"));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(4000));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(-29));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(-34));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(29.57));
            ClassicAssert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.MercuryInch));

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// EETU271450ZStrictTest
        /// </summary>
        [Test]
        public void EETU271450ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[1];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[1]));
            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("EETU"));
            ClassicAssert.That(decodedMetar.Day, Is.EqualTo(27));
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo("14:50 UTC"));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(50));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(5));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(9000));
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.OVC));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(600));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1019));
            ClassicAssert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU271400ZStrictTest
        /// </summary>
        [Test]
        public void SBGU271400ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[2];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[2]));
            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(0));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            ClassicAssert.That(decodedMetar.Day, Is.EqualTo(27));
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo("14:00 UTC"));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Not.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualValue, Is.EqualTo(270));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanDirection.ActualUnit, Is.EqualTo(Value.Unit.Degree));
            ClassicAssert.That(decodedMetar.SurfaceWind.VariableDirection, Is.False);
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualValue, Is.EqualTo(6));
            ClassicAssert.That(decodedMetar.SurfaceWind.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.Knot));
            ClassicAssert.That(decodedMetar.SurfaceWind.SpeedVariations, Is.Null);
            ClassicAssert.That(decodedMetar.SurfaceWind.DirectionVariations, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Not.Null);
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualValue, Is.EqualTo(8000));
            ClassicAssert.That(decodedMetar.Visibility.PrevailingVisibility.ActualUnit, Is.EqualTo(Value.Unit.Meter));
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibility, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.MinimumVisibilityDirection, Is.Null);
            ClassicAssert.That(decodedMetar.Visibility.NDV, Is.False);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(2));

            ClassicAssert.That(decodedMetar.PresentWeather[0].IntensityProximity, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Characteristics, Is.EqualTo("TS"));
            ClassicAssert.That(decodedMetar.PresentWeather[0].Types.Count, Is.EqualTo(0));

            ClassicAssert.That(decodedMetar.PresentWeather[1].IntensityProximity, Is.EqualTo("VC"));
            ClassicAssert.That(decodedMetar.PresentWeather[1].Characteristics, Is.EqualTo("SH"));
            ClassicAssert.That(decodedMetar.PresentWeather[1].Types.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(3));

            ClassicAssert.That(decodedMetar.Clouds[0].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualValue, Is.EqualTo(2000));
            ClassicAssert.That(decodedMetar.Clouds[0].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[0].Type, Is.EqualTo(CloudType.NULL));

            ClassicAssert.That(decodedMetar.Clouds[1].Amount, Is.EqualTo(CloudAmount.FEW));
            ClassicAssert.That(decodedMetar.Clouds[1].BaseHeight.ActualValue, Is.EqualTo(3000));
            ClassicAssert.That(decodedMetar.Clouds[1].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[1].Type, Is.EqualTo(CloudType.CB));

            ClassicAssert.That(decodedMetar.Clouds[2].Amount, Is.EqualTo(CloudAmount.BKN));
            ClassicAssert.That(decodedMetar.Clouds[2].BaseHeight.ActualValue, Is.EqualTo(8000));
            ClassicAssert.That(decodedMetar.Clouds[2].BaseHeight.ActualUnit, Is.EqualTo(Value.Unit.Feet));
            ClassicAssert.That(decodedMetar.Clouds[2].Type, Is.EqualTo(CloudType.NULL));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature.ActualValue, Is.EqualTo(25));
            ClassicAssert.That(decodedMetar.AirTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualValue, Is.EqualTo(20));
            ClassicAssert.That(decodedMetar.DewPointTemperature.ActualUnit, Is.EqualTo(Value.Unit.DegreeCelsius));

            ClassicAssert.That(decodedMetar.Pressure.ActualValue, Is.EqualTo(1017));
            ClassicAssert.That(decodedMetar.Pressure.ActualUnit, Is.EqualTo(Value.Unit.HectoPascal));

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// SBGU321400ZStrictTest
        /// </summary>
        [Test]
        public void SBGU321400ZStrictTest()
        {
            var decodedMetar = DecodedMetarsStrict[3];
            ClassicAssert.That(decodedMetar.RawMetar, Is.EqualTo(TestMetarSource[3]));
            //PHP version says there are 0, and reports 2 in the "Invalid format" message at top of page
            //issues probably lies with strict/no strict error handling

            ClassicAssert.That(decodedMetar.DecodingExceptions.Count, Is.EqualTo(1));
            ClassicAssert.That(decodedMetar.Type, Is.EqualTo(MetarType.NULL));
            ClassicAssert.That(decodedMetar.ICAO, Is.EqualTo("SBGU"));
            ClassicAssert.That(decodedMetar.Day, Is.Null);
            ClassicAssert.That(decodedMetar.Time, Is.EqualTo(string.Empty));
            ClassicAssert.That(decodedMetar.Status, Is.EqualTo(string.Empty));

            #region SurfaceWind

            ClassicAssert.That(decodedMetar.SurfaceWind, Is.Null);

            #endregion SurfaceWind

            #region Visibility

            ClassicAssert.That(decodedMetar.Visibility, Is.Null);

            #endregion Visibility

            ClassicAssert.That(decodedMetar.Cavok, Is.False);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange, Is.Not.Null);
            ClassicAssert.That(decodedMetar.RunwaysVisualRange.Count, Is.EqualTo(0));

            #region PresentWeather

            ClassicAssert.That(decodedMetar.PresentWeather.Count, Is.EqualTo(0));

            #endregion PresentWeather

            #region Clouds

            ClassicAssert.That(decodedMetar.Clouds.Count, Is.EqualTo(0));

            #endregion Clouds

            #region Temperatures & pressure

            ClassicAssert.That(decodedMetar.AirTemperature, Is.Null);
            ClassicAssert.That(decodedMetar.DewPointTemperature, Is.Null);
            ClassicAssert.That(decodedMetar.Pressure, Is.Null);

            #endregion Temperatures & pressure

            ClassicAssert.That(decodedMetar.RecentWeather, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearAllRunways, Is.Null);
            ClassicAssert.That(decodedMetar.WindshearRunways, Is.Null);
        }
    }
}