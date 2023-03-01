using NUnit.Framework;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("WeatherPhenomenon")]
    public class WeatherPhenomenonDecoderTest
    {
        private static readonly WeatherChunkDecoder chunkDecoder = new WeatherChunkDecoder();

        /// <summary>
        /// Test parsing of valid weather chunks
        /// </summary>
        /// <param name="chunk"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParse(WeatherPhenomenonChunkDecoderTester chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Chunk);

            var weatherPhenomenon = (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[WeatherChunkDecoder.WeatherPhenomenonParameterName] as List<WeatherPhenomenon>;
            for (var i = 0; i < weatherPhenomenon.Count; i++)
            {
                Assert.AreEqual(chunk.WeatherPhenomons[i].IntensityProximity, weatherPhenomenon[i].IntensityProximity);
                Assert.AreEqual(chunk.WeatherPhenomons[i].Descriptor, weatherPhenomenon[i].Descriptor);
                Assert.AreEqual(chunk.WeatherPhenomons[i].Phenomena, weatherPhenomenon[i].Phenomena);
            }
            Assert.AreEqual(chunk.Remaining, decoded[TafDecoder.RemainingTafKey]);
        }

        public static List<WeatherPhenomenonChunkDecoderTester> ValidChunks => new List<WeatherPhenomenonChunkDecoderTester>()
        {
            new WeatherPhenomenonChunkDecoderTester() { Chunk = "VCBLSN AAA",   Remaining = "AAA", WeatherPhenomons = new List<WeatherPhenomenon>() { new WeatherPhenomenon() { IntensityProximity = "VC", Descriptor = "BL", Phenomena = new List<string>() { "SN" } } } },
            new WeatherPhenomenonChunkDecoderTester() { Chunk = "-PL BBB",      Remaining = "BBB", WeatherPhenomons = new List<WeatherPhenomenon>() { new WeatherPhenomenon() { IntensityProximity = "-",  Descriptor =   "", Phenomena = new List<string>() { "PL" } } } },
            new WeatherPhenomenonChunkDecoderTester() { Chunk = "+TSRA CCC",    Remaining = "CCC", WeatherPhenomons = new List<WeatherPhenomenon>() { new WeatherPhenomenon() { IntensityProximity = "+",  Descriptor = "TS", Phenomena = new List<string>() { "RA" } } } },
            new WeatherPhenomenonChunkDecoderTester() { Chunk = "TSRABR DDD",   Remaining = "DDD", WeatherPhenomons = new List<WeatherPhenomenon>() { new WeatherPhenomenon() { IntensityProximity = "",   Descriptor = "TS", Phenomena = new List<string>() { "RA", "BR" } } } },
            new WeatherPhenomenonChunkDecoderTester() { Chunk = "-FZDZ FG EEE", Remaining = "EEE", WeatherPhenomons = new List<WeatherPhenomenon>() { new WeatherPhenomenon() { IntensityProximity = "-",  Descriptor = "FZ", Phenomena = new List<string>() { "DZ" } } , new WeatherPhenomenon() { IntensityProximity = "", Descriptor = "", Phenomena = new List<string>() { "FG" } } } },
        };
    }

    public class WeatherPhenomenonChunkDecoderTester
    {
        public string Chunk { get; set; }
        public List<WeatherPhenomenon> WeatherPhenomons { get; set; }
        public string Remaining { get; set; }

        public override string ToString()
        {
            return Chunk;
        }
    }
}