using Metar.Decoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
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
            Assert.IsTrue(d.IsValid);
            Assert.That(d.RawMetar, Is.EqualTo("METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN // FEW015 17/10 Q1009 REFZRA WS R03"));
            Assert.That(d.Type, Is.EqualTo(MetarType.METAR));
            Assert.That(d.ICAO, Is.EqualTo("LFPO"));
            Assert.That(d.Day, Is.EqualTo(23));
            Assert.That(d.Time, Is.EqualTo("10:27 UTC"));
            Assert.That(d.Status, Is.EqualTo("AUTO"));
            var w = d.SurfaceWind;
            Assert.That(w.MeanDirection.ActualValue, Is.EqualTo(240));
            Assert.That(w.MeanSpeed.ActualValue, Is.EqualTo(4));
            Assert.That(w.SpeedVariations.ActualValue, Is.EqualTo(9));
            Assert.That(w.MeanSpeed.ActualUnit, Is.EqualTo(Value.Unit.MeterPerSecond));
            var v = d.Visibility;
            Assert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(2500));
            Assert.That(v.MinimumVisibility.ActualValue, Is.EqualTo(1000));
            Assert.That(v.MinimumVisibilityDirection, Is.EqualTo("NW"));
            var rs = d.RunwaysVisualRange;
            var r1 = rs[0];
            Assert.That(r1.Runway, Is.EqualTo("32"));
            Assert.That(r1.VisualRange.ActualValue, Is.EqualTo(400));
            Assert.That(r1.PastTendency, Is.EqualTo(Tendency.NONE));
            var r2 = rs[1];
            Assert.That(r2.Runway, Is.EqualTo("08C"));
            Assert.That(r2.VisualRange.ActualValue, Is.EqualTo(4));
            Assert.That(r2.PastTendency, Is.EqualTo(Tendency.D));
            var pw = d.PresentWeather;
            Assert.That(pw.Count, Is.EqualTo(2));
            var pw1 = pw[0];
            Assert.That(pw1.IntensityProximity, Is.EqualTo("+"));
            Assert.That(pw1.Characteristics, Is.EqualTo("FZ"));
            Assert.That(pw1.Types, Is.EqualTo(new ReadOnlyCollection<string>(new List<string>() { "RA" })));
            var pw2 = pw[1];
            Assert.That(pw2.IntensityProximity, Is.EqualTo("VC"));
            Assert.That(pw2.Characteristics, Is.EqualTo(string.Empty));
            Assert.That(pw2.Types, Is.EqualTo(new ReadOnlyCollection<string>(new List<string>() { "SN" })));
            var cs = d.Clouds;
            var c = cs[0];
            Assert.That(c.Amount, Is.EqualTo(CloudAmount.FEW));
            Assert.That(c.BaseHeight.ActualValue, Is.EqualTo(1500));
            Assert.That(c.BaseHeight.ActualUnit, Is.EqualTo(Unit.Feet));
            Assert.That(d.AirTemperature.ActualValue, Is.EqualTo(17));
            Assert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(10));
            Assert.That(d.Pressure.ActualValue, Is.EqualTo(1009));
            Assert.That(d.Pressure.ActualUnit, Is.EqualTo(Unit.HectoPascal));
            var rw = d.RecentWeather;
            Assert.That(rw.Characteristics, Is.EqualTo("FZ"));
            Assert.That(rw.Types[0], Is.EqualTo("RA"));
            Assert.That(d.WindshearRunways, Is.EqualTo(new string[] { "03" }));
            Assert.IsFalse(d.WindshearAllRunways);
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
            Assert.IsTrue(d.IsValid);
            Assert.That(d.Type, Is.EqualTo(MetarType.METAR));
            Assert.That(d.ICAO, Is.EqualTo("LFPB"));
            Assert.That(d.Day, Is.EqualTo(19));
            Assert.That(d.Time, Is.EqualTo("07:30 UTC"));
            Assert.That(d.Status, Is.EqualTo("AUTO"));
            var w = d.SurfaceWind;
            Assert.That(w.MeanDirection.ActualValue, Is.EqualTo(170));
            Assert.That(w.MeanSpeed.ActualValue, Is.EqualTo(5));
            Assert.That(w.MeanSpeed.ActualUnit, Is.EqualTo(Unit.Knot));
            var v = d.Visibility;
            Assert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(6000));
            var cs = d.Clouds;
            var c = cs[0];
            Assert.That(c.Amount, Is.EqualTo(CloudAmount.OVC));
            Assert.That(c.BaseHeight.ActualValue, Is.EqualTo(2400));
            Assert.That(d.AirTemperature.ActualValue, Is.EqualTo(2));
            Assert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(0));
            Assert.That(d.Pressure.ActualValue, Is.EqualTo(1032));
            Assert.That(d.Pressure.ActualUnit, Is.EqualTo(Unit.HectoPascal));
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
            Assert.IsFalse(d.IsValid);
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(2));
            Assert.That(d.Type, Is.EqualTo(MetarType.METAR));
            Assert.That(d.ICAO, Is.EqualTo("LFPB"));
            Assert.That(d.Day, Is.EqualTo(19));
            Assert.That(d.Time, Is.EqualTo("07:30 UTC"));
            Assert.That(d.Status, Is.EqualTo(string.Empty));
            var w = d.SurfaceWind;
            Assert.That(w.MeanDirection.ActualValue, Is.EqualTo(170));
            Assert.That(w.MeanSpeed.ActualValue, Is.EqualTo(5));
            Assert.That(w.MeanSpeed.ActualUnit, Is.EqualTo(Unit.Knot));
            var v = d.Visibility;
            Assert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(6000));
            var cs = d.Clouds;
            var c = cs[0];
            Assert.That(c.Amount, Is.EqualTo(CloudAmount.OVC));
            Assert.That(c.BaseHeight.ActualValue, Is.EqualTo(2400));
            Assert.That(d.AirTemperature.ActualValue, Is.EqualTo(2));
            Assert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(0));
            Assert.IsNull(d.Pressure);
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
            Assert.IsFalse(d.IsValid);
            // 3 errors because visibility decoder will choke once before finding the right piece of metar
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(3));
            Assert.That(d.Type, Is.EqualTo(MetarType.METAR));
            Assert.That(d.ICAO, Is.EqualTo("LFPB"));
            Assert.That(d.Day, Is.EqualTo(19));
            Assert.That(d.Time, Is.EqualTo("07:30 UTC"));
            Assert.That(d.Status, Is.EqualTo(string.Empty));
            Assert.IsNull(d.SurfaceWind);
            var v = d.Visibility;
            Assert.That(v.PrevailingVisibility.ActualValue, Is.EqualTo(6000));
            var cs = d.Clouds;
            var c = cs[0];
            Assert.That(c.Amount, Is.EqualTo(CloudAmount.OVC));
            Assert.That(c.BaseHeight.ActualValue, Is.EqualTo(2400));
            Assert.That(d.AirTemperature.ActualValue, Is.EqualTo(2));
            Assert.That(d.DewPointTemperature.ActualValue, Is.EqualTo(0));
            Assert.That(d.Pressure.ActualValue, Is.EqualTo(1032));
        }

        /// <summary>
        /// TestParseNoClouds
        /// </summary>
        [Test]
        public void TestParseNoClouds()
        {
            var metar = "PAWI 140753Z AUTO 08034G41KT 1/4SM SN FZFG M18/M21 A2951 RMK PK WND 08041/0752 SLP995 P0000 T11831206 TSNO  VIA AUTODIAL";
            var d = decoder.ParseStrict(metar);
            Assert.IsFalse(d.IsValid);
            d = decoder.ParseNotStrict(metar);
            Assert.IsFalse(d.IsValid);
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(1));
            var errors = d.DecodingExceptions;
            var error = errors[0];
            Assert.That(error.ChunkDecoder.GetType().Name, Is.EqualTo("CloudChunkDecoder"));
            Assert.That(d.Clouds.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test parsing of an empty METAR, which is valid.
        /// </summary>
        [Test]
        public void TestParseNil()
        {
            var d = decoder.ParseStrict("METAR LFPO 231027Z NIL");
            Assert.That(d.Status, Is.EqualTo("NIL"));
        }

        /// <summary>
        /// Test parsing of METAR with trailing end-of-message.
        /// </summary>
        [Test]
        public void TestParseEOM()
        {
            var d = decoder.ParseStrict("METAR LFPB 190730Z AUTO 17005KT 6000 OVC024 02/00 Q1032=");
            Assert.IsTrue(d.IsValid);
            Assert.That(d.RawMetar, Is.EqualTo("METAR LFPB 190730Z AUTO 17005KT 6000 OVC024 02/00 Q1032"));
        }

        /// <summary>
        /// Test parsing of a METAR with CAVOK.
        /// </summary>
        [Test]
        public void testParseCAVOK()
        {
            var d = decoder.ParseStrict("METAR LFPO 231027Z AUTO 24004KT CAVOK 02/M08 Q0995");
            Assert.IsTrue(d.Cavok);
            Assert.IsNull(d.Visibility);
            Assert.That(d.Clouds.Count, Is.EqualTo(0));
            // check that we went to the end of the decoding though
            Assert.That(d.Pressure.ActualValue, Is.EqualTo(995));
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
            Assert.IsFalse(d.IsValid);
            var errors = d.DecodingExceptions;
            var first_error = errors[0];
            Assert.That(first_error.ChunkDecoder.GetType().Name, Is.EqualTo(metar_error.Item2));
            Assert.That(first_error.RemainingMetar, Is.EqualTo(metar_error.Item3));
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
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(1));
            // not strict: several errors triggered
            decoder.SetStrictParsing(false);
            d = decoder.Parse("LFPG aaa bbb cccc");
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Test error reset
        /// </summary>
        [Test]
        public void TestErrorReset()
        {
            var d = decoder.Parse("LFPG aaa bbb cccc");
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(5));
            d.ResetDecodingExceptions();
            Assert.That(d.DecodingExceptions.Count, Is.EqualTo(0));
        }
    }
}