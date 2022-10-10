using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Metar.Decoder.Entity.Value;

namespace Metar.Decoder_tests.chunkdecoder
{
    [TestFixture, Category("PressureChunkDecoder")]
    public class PressureChunkDecoderTest
    {
        private readonly PressureChunkDecoder chunkDecoder = new PressureChunkDecoder();

        /// <summary>
        /// Test parsing of valid icao chunks.
        /// </summary>
        /// <param name="chunkToTest"></param>
        /// <param name="icao"></param>
        /// <param name="remaining"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParsePressureChunk(Tuple<string, double?, Unit, string> chunkToTest)
        {
            var decoded = chunkDecoder.Parse(chunkToTest.Item1);

            if (chunkToTest.Item2.HasValue)
            {
                Assert.That((((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[PressureChunkDecoder.PressureParameterName] as Value).ActualValue, Is.EqualTo(chunkToTest.Item2));
                Assert.That((((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[PressureChunkDecoder.PressureParameterName] as Value).ActualUnit, Is.EqualTo(chunkToTest.Item3));
            }
            else
            {
                Assert.IsNull((((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[PressureChunkDecoder.PressureParameterName] as Value));
            }

            //check RemainingMetar
            Assert.That(decoded[MetarDecoder.RemainingMetarKey], Is.EqualTo(chunkToTest.Item4));
        }

        /// <summary>
        /// Test parsing of invalid pressure chunks.
        /// </summary>
        /// <param name="chunk"></param>
        ///
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseInvalidChunk(string chunk)
        {
            var decoded = new Dictionary<string, object>();
            var ex = Assert.Throws(typeof(MetarChunkDecoderException), () =>
            {
                decoded = chunkDecoder.Parse(chunk);
            }) as MetarChunkDecoderException;
            Assert.IsFalse(decoded.ContainsKey(MetarDecoder.ResultKey));
            Assert.That(ex.RemainingMetar, Is.EqualTo(chunk));
        }

        #region TestCaseSources

        public static List<Tuple<string, double?, Unit, string>> ValidChunks()
        {
            return new List<Tuple<string, double?, Unit, string>>()
            {
                new Tuple<string, double?, Unit, string>("Q1000 AAA", 1000, Unit.HectoPascal, "AAA"),
                new Tuple<string, double?, Unit, string>("A0202 BBB", 2.02d, Unit.MercuryInch, "BBB"),
                new Tuple<string, double?, Unit, string>("Q//// CCC", null, Unit.None, "CCC"),
            };
        }

        public static List<string> InvalidChunks()
        {
            return new List<string>() {
                "Q123 AAA",
                "R1234 BBB",
                "Q12345 CCC",
            };
        }

        #endregion TestCaseSources
    }
}