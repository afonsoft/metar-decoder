using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;
using static Taf.Decoder.entity.DecodedTaf;

namespace Taf.Decoder_tests
{
    [TestFixture, Category("TafDecoder")]
    public class TafDecoderTest
    {
        /// <summary>
        /// Test parsing of a valid TAF
        /// </summary>
        [Test]
        public void TestParse()
        {
            var rawTaf = "TAF TAF LIRU 032244Z 0318/0406 23010KT P6SM -SHDZRA BKN020CB TX05/0318Z TNM03/0405Z";
            var decoderTaf = TafDecoder.ParseWithMode(rawTaf);
            ClassicAssert.IsTrue(decoderTaf.IsValid);
            ClassicAssert.AreEqual("TAF TAF LIRU 032244Z 0318/0406 23010KT P6SM -SHDZRA BKN020CB TX05/0318Z TNM03/0405Z", decoderTaf.RawTaf);
            ClassicAssert.AreEqual(TafType.TAF, decoderTaf.Type);
            ClassicAssert.AreEqual("LIRU", decoderTaf.Icao);
            ClassicAssert.AreEqual(3, decoderTaf.Day);
            ClassicAssert.AreEqual("22:44 UTC", decoderTaf.Time);

            var forecastPeriod = decoderTaf.ForecastPeriod;
            ClassicAssert.AreEqual(3, forecastPeriod.FromDay);
            ClassicAssert.AreEqual(18, forecastPeriod.FromHour);
            ClassicAssert.AreEqual(4, forecastPeriod.ToDay);
            ClassicAssert.AreEqual(6, forecastPeriod.ToHour);

            var surfaceWind = decoderTaf.SurfaceWind;
            ClassicAssert.IsFalse(surfaceWind.VariableDirection);
            ClassicAssert.AreEqual(230, surfaceWind.MeanDirection.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.Degree, surfaceWind.MeanDirection.ActualUnit);
            ClassicAssert.IsNull(surfaceWind.DirectionVariations);
            ClassicAssert.AreEqual(10, surfaceWind.MeanSpeed.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.Knot, surfaceWind.MeanSpeed.ActualUnit);
            ClassicAssert.Null(surfaceWind.SpeedVariations);

            var visibility = decoderTaf.Visibility;
            ClassicAssert.AreEqual(6, visibility.ActualVisibility.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.StatuteMile, visibility.ActualVisibility.ActualUnit);
            ClassicAssert.True(visibility.Greater);

            var weatherPhenomenon = decoderTaf.WeatherPhenomenons;
            ClassicAssert.AreEqual("-", weatherPhenomenon[0].IntensityProximity);
            ClassicAssert.AreEqual("SH", weatherPhenomenon[0].Descriptor);
            var phenomena = weatherPhenomenon[0].Phenomena;
            ClassicAssert.AreEqual("DZ", phenomena[0]);
            ClassicAssert.AreEqual("RA", phenomena[1]);

            var cloud = decoderTaf.Clouds.FirstOrDefault();
            ClassicAssert.AreEqual(CloudLayer.CloudAmount.BKN, cloud.Amount);
            ClassicAssert.AreEqual(2000, cloud.BaseHeight.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.Feet, cloud.BaseHeight.ActualUnit);
            ClassicAssert.AreEqual(CloudLayer.CloudType.CB, cloud.Type);

            var minimumTemperature = decoderTaf.MinimumTemperature;
            ClassicAssert.AreEqual(-3, minimumTemperature.TemperatureValue.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.DegreeCelsius, minimumTemperature.TemperatureValue.ActualUnit);
            ClassicAssert.AreEqual(4, minimumTemperature.Day);
            ClassicAssert.AreEqual(5, minimumTemperature.Hour);

            var maximumTemperature = decoderTaf.MaximumTemperature;
            ClassicAssert.AreEqual(5, maximumTemperature.TemperatureValue.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.DegreeCelsius, maximumTemperature.TemperatureValue.ActualUnit);
            ClassicAssert.AreEqual(3, maximumTemperature.Day);
            ClassicAssert.AreEqual(18, maximumTemperature.Hour);
        }

        /// <summary>
        /// Test parsing of a valid TAF
        /// </summary>
        [Test]
        public void TestParseSecond()
        {
            var rawTaf = "TAF TAF LIRU 032244Z 0318/0406 23010KT P6SM +TSRA FG BKN020CB TX05/0318Z TNM03/0405Z";
            var decoderTaf = TafDecoder.ParseWithMode(rawTaf);
            ClassicAssert.True(decoderTaf.IsValid);
            ClassicAssert.AreEqual("TAF TAF LIRU 032244Z 0318/0406 23010KT P6SM +TSRA FG BKN020CB TX05/0318Z TNM03/0405Z", decoderTaf.RawTaf);
            ClassicAssert.AreEqual(TafType.TAF, decoderTaf.Type);
            ClassicAssert.AreEqual("LIRU", decoderTaf.Icao);
            ClassicAssert.AreEqual(3, decoderTaf.Day);
            ClassicAssert.AreEqual("22:44 UTC", decoderTaf.Time);

            var forecastPeriod = decoderTaf.ForecastPeriod;
            ClassicAssert.AreEqual(3, forecastPeriod.FromDay);
            ClassicAssert.AreEqual(18, forecastPeriod.FromHour);
            ClassicAssert.AreEqual(4, forecastPeriod.ToDay);
            ClassicAssert.AreEqual(6, forecastPeriod.ToHour);

            var surfaceWind = decoderTaf.SurfaceWind;
            ClassicAssert.False(surfaceWind.VariableDirection);
            ClassicAssert.AreEqual(230, surfaceWind.MeanDirection.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.Degree, surfaceWind.MeanDirection.ActualUnit);
            ClassicAssert.Null(surfaceWind.DirectionVariations);
            ClassicAssert.AreEqual(10, surfaceWind.MeanSpeed.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.Knot, surfaceWind.MeanSpeed.ActualUnit);
            ClassicAssert.Null(surfaceWind.SpeedVariations);

            var visibility = decoderTaf.Visibility;
            ClassicAssert.AreEqual(6, visibility.ActualVisibility.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.StatuteMile, visibility.ActualVisibility.ActualUnit);
            ClassicAssert.True(visibility.Greater);

            var weatherPhenomenons = decoderTaf.WeatherPhenomenons;
            ClassicAssert.AreEqual("+", weatherPhenomenons[0].IntensityProximity);
            ClassicAssert.AreEqual("TS", weatherPhenomenons[0].Descriptor);
            var phenomena = weatherPhenomenons[0].Phenomena;
            ClassicAssert.AreEqual("RA", phenomena[0]);
            phenomena = weatherPhenomenons[1].Phenomena;
            ClassicAssert.AreEqual("FG", phenomena[0]);

            var cloud = decoderTaf.Clouds[0];
            ClassicAssert.AreEqual(CloudLayer.CloudAmount.BKN, cloud.Amount);
            ClassicAssert.AreEqual(2000, cloud.BaseHeight.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.Feet, cloud.BaseHeight.ActualUnit);
            ClassicAssert.AreEqual(CloudLayer.CloudType.CB, cloud.Type);

            var minimumTemperature = decoderTaf.MinimumTemperature;
            ClassicAssert.AreEqual(-3, minimumTemperature.TemperatureValue.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.DegreeCelsius, minimumTemperature.TemperatureValue.ActualUnit);
            ClassicAssert.AreEqual(4, minimumTemperature.Day);
            ClassicAssert.AreEqual(5, minimumTemperature.Hour);

            var maximumTemperature = decoderTaf.MaximumTemperature;
            ClassicAssert.AreEqual(5, maximumTemperature.TemperatureValue.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.DegreeCelsius, maximumTemperature.TemperatureValue.ActualUnit);
            ClassicAssert.AreEqual(3, maximumTemperature.Day);
            ClassicAssert.AreEqual(18, maximumTemperature.Hour);
        }

        /// <summary>
        /// Test parsing of a short, invalid TAF, without strict option activated
        /// </summary>
        [Test]
        public void TestParseInvalid()
        {
            // launch decoding (forecast was cancelled)
            var d = TafDecoder.ParseNotStrict("TAF LFMT 032244Z 0318/0206 CNL");
            ClassicAssert.IsFalse(d.IsValid);
            // launch decoding (surface wind is invalid)
            d = TafDecoder.ParseNotStrict("TAF TAF LIRU 032244Z 0318/0420 2300ABKT PSSM\nBKN020CB TX05/0318Z TNM03/0405Z\n");
            ClassicAssert.IsFalse(d.IsValid);
        }

        /// <summary>
        /// Test object-wide strict option
        /// </summary>
        [Test]
        public void TestParseDefaultStrictMode()
        {
            // strict mode, max 1 error triggered
            TafDecoder.SetStrictParsing(true);
            var d = TafDecoder.Parse("TAF TAF LIR 032244Z 0318/0206 23010KT P6SM BKN020CB TX05/0318Z TNM03/0405Z\n");
            ClassicAssert.AreEqual(1, d.DecodingExceptions.Count);
            // not strict: several errors triggered (6 because the icao failure causes the next ones to fail too)
            TafDecoder.SetStrictParsing(false);
            d = TafDecoder.Parse("TAF TAF LIR 032244Z 0318/0206 23010KT\n");
            ClassicAssert.AreEqual(6, d.DecodingExceptions.Count);
        }

        /// <summary>
        /// Test parsing of invalid TAFs
        /// </summary>
        [Test, TestCaseSource("ErrorChunks")]
        public void TestParseErrors(Tuple<string, Type, string> source)
        {
            // launch decoding
            DecodedTaf decodedTaf = TafDecoder.ParseNotStrict(source.Item1);

            // check the error triggered
            ClassicAssert.NotNull(decodedTaf);
            ClassicAssert.False(decodedTaf.IsValid, "DecodedTaf should be invalid.");
            var errors = decodedTaf.DecodingExceptions;
            ClassicAssert.AreEqual(source.Item2, errors.FirstOrDefault().ChunkDecoder.GetType(), "ChunkDecoder type is incorrect.");
            ClassicAssert.AreEqual(source.Item3, errors.FirstOrDefault().RemainingTaf, "RemainingTaf is incorrect.");
            decodedTaf.ResetDecodingExceptions();
            ClassicAssert.AreEqual(0, decodedTaf.DecodingExceptions.Count, "DecodingExceptions should be empty.");
        }

        public static List<Tuple<string, Type, string>> ErrorChunks()
        {
            return new List<Tuple<string, Type, string>>() {
                new Tuple<string, Type, string>("TAF LFPG aaa bbb cccc",                typeof(DatetimeChunkDecoder),       "AAA BBB CCCC END"),
                new Tuple<string, Type, string>("TAF LFPO 231027Z NIL 1234",            typeof(ForecastPeriodChunkDecoder), "NIL 1234 END"),
                new Tuple<string, Type, string>("TAF LFPO 231027Z 2310/2411 NIL 12345", typeof(SurfaceWindChunkDecoder),    "NIL 12345 END"),
       };
        }
    }
}