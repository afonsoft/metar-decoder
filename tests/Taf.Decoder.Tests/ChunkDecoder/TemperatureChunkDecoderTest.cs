using NUnit.Framework;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("TemperatureChunkDecoder")]
    public class TemperatureChunkDecoderTest
    {
        private static readonly TemperatureChunkDecoder chunkDecoder = new TemperatureChunkDecoder();

        /// <summary>
        /// Test parsing valid temperature chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParse(Tuple<string, int, int, int, int, int, int> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);
            var minimumTemperature = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[TemperatureChunkDecoder.MinimumTemperatureParameterName] as Temperature;
            var maximumTemperature = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[TemperatureChunkDecoder.MaximumTemperatureParameterName] as Temperature;

            Assert.AreEqual("TN", minimumTemperature.Type);
            Assert.AreEqual(chunk.Item2, minimumTemperature.TemperatureValue.ActualValue);
            Assert.AreEqual(Value.Unit.DegreeCelsius, minimumTemperature.TemperatureValue.ActualUnit);
            Assert.AreEqual(chunk.Item3, minimumTemperature.Day);
            Assert.AreEqual(chunk.Item4, minimumTemperature.Hour);
            Assert.AreEqual("TX", maximumTemperature.Type);
            Assert.AreEqual(chunk.Item5, maximumTemperature.TemperatureValue.ActualValue);
            Assert.AreEqual(Value.Unit.DegreeCelsius, maximumTemperature.TemperatureValue.ActualUnit);
            Assert.AreEqual(chunk.Item6, maximumTemperature.Day);
            Assert.AreEqual(chunk.Item7, maximumTemperature.Hour);
        }

        /// <summary>
        /// Test parsing of invalid temperature chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("InvalidChunks")]
        public static void TestParseInvalidChunk(string chunk)
        {
            var exception = Assert.Throws(typeof(TafChunkDecoderException), () =>
            {
                chunkDecoder.Parse(chunk);
            });
            Assert.AreEqual("Inconsistent values for temperature information", exception.Message);
        }

        public static List<Tuple<string, int, int, int, int, int, int>> ValidChunks => new List<Tuple<string, int, int, int, int, int, int>>()
        {
            new Tuple<string, int, int, int, int, int, int>("TX20/1012Z TN16/1206Z",  16, 12, 6, 20, 10, 12),
            new Tuple<string, int, int, int, int, int, int>("TX03/1012Z TNM05/1206Z", -5, 12, 6,  3, 10, 12),
        };

        public static List<string> InvalidChunks => new List<string>()
        {
            "TX04/0102Z TN05/0203Z",
        };
    }
}