using Metar.Decoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Metar.Decoder.Entity.CloudLayer;
using static Metar.Decoder.Entity.DecodedMetar;
using static Metar.Decoder.Entity.RunwayVisualRange;
using static Metar.Decoder.Entity.Value;

namespace Metar.Decoder_tests
{
    /// <summary>
    /// MetarDecoderTest
    /// </summary>
    [TestFixture, Category("MetarDecoderTest")]
    public class MetarDecoderTest
    {
        private MetarDecoder decoder;

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            decoder = new MetarDecoder();
        }

        /// <summary>
        /// Test parsing of a complete, valid METAR.
        /// </summary>
        ///         [Test]
        [Test]
        public void TestParse()
        {
            // launch decoding
            var raw_metar = "METAR  LFPO 231027Z   AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN // FEW015 17/10 Q1009 REFZRA WS R03";
            var d = decoder.ParseStrict(raw_metar);
            // compare results
            ClassicAssert.IsTrue(d.IsValid);
            ClassicAssert.That(d.RawMetar, Is.EqualTo("METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN // FEW015 17/10 Q1009 REFZRA WS R03"));
            ClassicAssert.That(d.Type, Is.EqualTo(MetarType.METAR));
            ClassicAssert.That(d.ICAO, Is.EqualTo("LFPO"));
            ClassicAssert.That(d.Day, Is.EqualTo(23));
            ClassicAssert.That(d.Time, Is.EqualTo("10:27 UTC"));
            ClassicAssert.That(d.Status, Is.EqualTo("AUTO"));
            var w = d.SurfaceWind;
            ClassicAssert.That(w.MeanDirection.ActualValue, Is.EqualTo(240));
            ClassicAssert.That(w.MeanSpeed.ActualValue, Is.EqualTo(4));
            ClassicAssert.That(w.SpeedVariations.ActualValue, Is.EqualTo(9));
            ClassicAssert.That(w.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.MeterPerSecond));
            var v = d.Visibility;
            ClassicAssert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(2500));
            ClassicAssert.That(v.MinimumVisibility.ActualValue, Is.EqualTo(1000));
            ClassicAssert.That(v.MinimumVisibilityDirection, Is.EqualTo("NW"));
            var rs = d.RunwaysVisualRange;
            var r1 = rs[0];
            ClassicAssert.That(r1.Runway, Is.EqualTo("32"));
            ClassicAssert.That(r1.VisualRange.ActualValue, Is.EqualTo(400));
            ClassicAssert.That(r1.PastTendency, Is.EqualTo(Tendency.NONE));
            var r2 = rs[1];
            ClassicAssert.That(r2.Runway, Is.EqualTo("08C"));
            ClassicAssert.That(r2.VisualRange.ActualValue, Is.EqualTo(4));
            ClassicAssert.That(r2.PastTendency, Is.EqualTo(Tendency.D));
            var pw = d.PresentWeather;
            ClassicAssert.That(pw.Count, Is.EqualTo(2));
            var pw1 = pw[0];
            ClassicAssert.That(pw1.IntensityProximity, Is.EqualTo("+"));
            ClassicAssert.That(pw1.Characteristics, Is.EqualTo("FZ"));
            ClassicAssert.That(pw1.Types, Is.EqualTo(new ReadOnlyCollection<string>(new List<string>() { "RA" })));
            var pw2 = pw[1];
            ClassicAssert.That(pw2.IntensityProximity, Is.EqualTo("VC"));
            ClassicAssert.That(pw2.Characteristics, Is.EqualTo(string.Empty));
            ClassicAssert.That(pw2.Types, Is.EqualTo(new ReadOnlyCollection<string>(new List<string>() { "SN" })));
            var cs = d.Clouds;
            var c = cs[0];
            ClassicAssert.That(c.Amount, Is.EqualTo(CloudAmount.FEW));
            ClassicAssert.That(c.BaseHeight.ActualValue, Is.EqualTo(1500));
            ClassicAssert.That(c.BaseHeight.ActualUnit, Is.EqualTo(Unit.Feet));
            ClassicAssert.That(d.AirTemperature.ActualValue, Is.EqualTo(17));
            ClassicAssert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(10));
            ClassicAssert.That(d.Pressure.ActualValue, Is.EqualTo(1009));
            ClassicAssert.That(d.Pressure.ActualUnit, Is.EqualTo(Unit.HectoPascal));
            var rw = d.RecentWeather;
            ClassicAssert.That(rw.Characteristics, Is.EqualTo("FZ"));
            ClassicAssert.That(rw.Types[0], Is.EqualTo("RA"));
            ClassicAssert.That(d.WindshearRunways, Is.EqualTo(new string[] { "03" }));
            ClassicAssert.That(d.WindshearAllRunways, Is.False);
        }

        /// <summary>
        /// Test parsing of a short, valid METAR.
        /// </summary>
        [Test]
        public void TestParseShort()
        {
            // launch decoding
            var d = decoder.ParseStrict("METAR LFPB 190730Z AUTO 17005KT 6000 OVC024 02/00 Q1032 ");
            // compare results
            ClassicAssert.IsTrue(d.IsValid);
            ClassicAssert.That(d.Type, Is.EqualTo(MetarType.METAR));
            ClassicAssert.That(d.ICAO, Is.EqualTo("LFPB"));
            ClassicAssert.That(d.Day, Is.EqualTo(19));
            ClassicAssert.That(d.Time, Is.EqualTo("07:30 UTC"));
            ClassicAssert.That(d.Status, Is.EqualTo("AUTO"));
            var w = d.SurfaceWind;
            ClassicAssert.That(w.MeanDirection.ActualValue, Is.EqualTo(170));
            ClassicAssert.That(w.MeanSpeed.ActualValue, Is.EqualTo(5));
            ClassicAssert.That(w.MeanSpeed.ActualUnit, Is.EqualTo(Unit.Knot));
            var v = d.Visibility;
            ClassicAssert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(6000));
            var cs = d.Clouds;
            var c = cs[0];
            ClassicAssert.That(c.Amount, Is.EqualTo(CloudAmount.OVC));
            ClassicAssert.That(c.BaseHeight.ActualValue, Is.EqualTo(2400));
            ClassicAssert.That(d.AirTemperature.ActualValue, Is.EqualTo(2));
            ClassicAssert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(0));
            ClassicAssert.That(d.Pressure.ActualValue, Is.EqualTo(1032));
            ClassicAssert.That(d.Pressure.ActualUnit, Is.EqualTo(Unit.HectoPascal));
        }

        /// <summary>
        /// Test parsing of a short, invalid METAR, without strict option activated.
        /// </summary>
        [Test]
        public void TestParseInvalid()
        {
            // launch decoding
            var d = decoder.ParseNotStrict("METAR LFPB 190730Z AUTOPP 17005KT 6000 OVC024 02/00 Q10032 ");
            //                                                 here ^                              ^ and here
            // compare results
            ClassicAssert.That(d.IsValid, Is.False);
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(2));
            ClassicAssert.That(d.Type, Is.EqualTo(MetarType.METAR));
            ClassicAssert.That(d.ICAO, Is.EqualTo("LFPB"));
            ClassicAssert.That(d.Day, Is.EqualTo(19));
            ClassicAssert.That(d.Time, Is.EqualTo("07:30 UTC"));
            ClassicAssert.That(d.Status, Is.EqualTo(string.Empty));
            var w = d.SurfaceWind;
            ClassicAssert.That(w.MeanDirection.ActualValue, Is.EqualTo(170));
            ClassicAssert.That(w.MeanSpeed.ActualValue, Is.EqualTo(5));
            ClassicAssert.That(w.MeanSpeed.ActualUnit, Is.EqualTo(Unit.Knot));
            var v = d.Visibility;
            ClassicAssert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(6000));
            var cs = d.Clouds;
            var c = cs[0];
            ClassicAssert.That(c.Amount, Is.EqualTo(CloudAmount.OVC));
            ClassicAssert.That(c.BaseHeight.ActualValue, Is.EqualTo(2400));
            ClassicAssert.That(d.AirTemperature.ActualValue, Is.EqualTo(2));
            ClassicAssert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(0));
            ClassicAssert.That(d.Pressure, Is.Null);
        }

        /// <summary>
        /// Test parsing of an invalid METAR, where parsing can continue normally without strict option activated.
        /// </summary>
        [Test]
        public void TestParseInvalidPart()
        {
            // launch decoding
            var d = decoder.ParseNotStrict("METAR LFPB 190730Z AUTOP X17005KT 6000 OVC024 02/00 Q1032 ");
            //                                                here ^ ^ and here
            // compare results
            ClassicAssert.That(d.IsValid, Is.False);
            // 3 errors because visibility decoder will choke once before finding the right piece of metar
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(3));
            ClassicAssert.That(d.Type, Is.EqualTo(MetarType.METAR));
            ClassicAssert.That(d.ICAO, Is.EqualTo("LFPB"));
            ClassicAssert.That(d.Day, Is.EqualTo(19));
            ClassicAssert.That(d.Time, Is.EqualTo("07:30 UTC"));
            ClassicAssert.That(d.Status, Is.EqualTo(string.Empty));
            ClassicAssert.That(d.SurfaceWind, Is.Null);
            var v = d.Visibility;
            ClassicAssert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(6000));
            var cs = d.Clouds;
            var c = cs[0];
            ClassicAssert.That(c.Amount, Is.EqualTo(CloudAmount.OVC));
            ClassicAssert.That(c.BaseHeight.ActualValue, Is.EqualTo(2400));
            ClassicAssert.That(d.AirTemperature.ActualValue, Is.EqualTo(2));
            ClassicAssert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(0));
            ClassicAssert.That(d.Pressure.ActualValue, Is.EqualTo(1032));
        }

        /// <summary>
        /// TestParseNoClouds
        /// </summary>
        [Test]
        public void TestParseNoClouds()
        {
            var metar = "PAWI 140753Z AUTO 08034G41KT 1/4SM SN FZFG M18/M21 A2951 RMK PK WND 08041/0752 SLP995 P0000 T11831206 TSNO  VIA AUTODIAL";
            var d = decoder.ParseStrict(metar);
            ClassicAssert.That(d.IsValid, Is.False);
            d = decoder.ParseNotStrict(metar);
            ClassicAssert.That(d.IsValid, Is.False);
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(1));
            var errors = d.DecodingExceptions;
            var error = errors[0];
            ClassicAssert.That(error.ChunkDecoder.GetType().Name, Is.EqualTo("CloudChunkDecoder"));
            ClassicAssert.That(d.Clouds.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test parsing of an empty METAR, which is valid.
        /// </summary>
        [Test]
        public void TestParseNil()
        {
            var d = decoder.ParseStrict("METAR LFPO 231027Z NIL");
            ClassicAssert.That(d.Status, Is.EqualTo("NIL"));
        }

        /// <summary>
        /// Test parsing of METAR with trailing end-of-message.
        /// </summary>
        [Test]
        public void TestParseEOM()
        {
            var d = decoder.ParseStrict("METAR LFPB 190730Z AUTO 17005KT 6000 OVC024 02/00 Q1032=");
            ClassicAssert.IsTrue(d.IsValid);
            ClassicAssert.That(d.RawMetar, Is.EqualTo("METAR LFPB 190730Z AUTO 17005KT 6000 OVC024 02/00 Q1032"));
        }

        /// <summary>
        /// Test parsing of a METAR with CAVOK.
        /// </summary>
        [Test]
        public void testParseCAVOK()
        {
            var d = decoder.ParseStrict("METAR LFPO 231027Z AUTO 24004KT CAVOK 02/M08 Q0995");
            ClassicAssert.IsTrue(d.Cavok);
            ClassicAssert.That(d.Visibility, Is.Null);
            ClassicAssert.That(d.Clouds.Count, Is.EqualTo(0));
            // check that we went to the end of the decoding though
            ClassicAssert.That(d.Pressure.ActualValue, Is.EqualTo(995));
        }

        /// <summary>
        /// Test parsing of invalid METARs
        /// TODO improve this now that strict option exists.
        /// </summary>
        /// [Test]
        [Test, TestCaseSource("ErrorDataset")]
        public void TestParseErrors(Tuple<string, string, string> metar_error)
        {
            // launch decoding
            var d = decoder.ParseNotStrict(metar_error.Item1);
            // check the error triggered
            ClassicAssert.That(d.IsValid, Is.False);
            var errors = d.DecodingExceptions;
            var first_error = errors[0];
            ClassicAssert.That(first_error.ChunkDecoder.GetType().Name, Is.EqualTo(metar_error.Item2));
            ClassicAssert.That(first_error.RemainingMetar, Is.EqualTo(metar_error.Item3));
        }

        public static List<Tuple<string, string, string>> ErrorDataset()
        {
            return new List<Tuple<string, string, string>>()
            {
                new Tuple<string, string, string>("LFPG aaa bbb cccc", "DatetimeChunkDecoder", "AAA BBB CCCC "),
                new Tuple<string, string, string>("METAR LFPO 231027Z NIL 1234", "ReportStatusChunkDecoder", "NIL 1234 "),
                new Tuple<string, string, string>("METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D FZRAA FEW015", "CloudChunkDecoder", "FZRAA FEW015 "),
            };
        }

        /// <summary>
        /// Test object-wide strict option.
        /// </summary>
        [Test]
        public void TestParseDefaultStrictMode()
        {
            // strict mode, max 1 error triggered
            decoder.SetStrictParsing(true);
            var d = decoder.Parse("LFPG aaa bbb cccc");
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(1));
            // not strict: several errors triggered
            decoder.SetStrictParsing(false);
            d = decoder.Parse("LFPG aaa bbb cccc");
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Test error reset
        /// </summary>
        [Test]
        public void TestErrorReset()
        {
            var d = decoder.Parse("LFPG aaa bbb cccc");
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(5));
            d.ResetDecodingExceptions();
            ClassicAssert.That(d.DecodingExceptions.Count, Is.EqualTo(0));
        }
    }
}