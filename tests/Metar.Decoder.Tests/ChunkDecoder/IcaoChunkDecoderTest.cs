using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;

namespace Metar.Decoder_tests.chunkdecoder
{
    /// <summary>
    /// IcaoChunkDecoderTest
    /// </summary>
    [TestFixture, Category("IcaoChunkDecoder")]
    public class IcaoChunkDecoderTest
    {
        private readonly IcaoChunkDecoder chunkDecoder = new IcaoChunkDecoder();

        /// <summary>
        /// Test parsing of valid icao chunks.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="icao"></param>
        /// <param name="remaining"></param>
        [Test, TestCaseSource("ValidChunks")]
        public void TestParseIcaoChunk(Tuple<string, string, string> chunk)
        {
            var decoded = new Dictionary<string, object>();
            ClassicAssert.DoesNotThrow(() =>
             {
                 decoded = chunkDecoder.Parse(chunk.Item1);
             });

            ClassicAssert.IsTrue(decoded.ContainsKey(MetarDecoder.ResultKey));

            //check ICAO
            ClassicAssert.That(((Dictionary<string, object>)decoded[MetarDecoder.ResultKey])[IcaoChunkDecoder.ICAOParameterName] as string, Is.EqualTo(chunk.Item2));

            //check RemainingMetar
            ClassicAssert.That(decoded[MetarDecoder.RemainingMetarKey] as string, Is.EqualTo(chunk.Item3));
        }

        /// <summary>
        /// Test parsing of invalid icao chunks.
        /// </summary>
        /// <param name="chunk"></param>
        ///
        [Test, TestCaseSource("InvalidChunks")]
        public void TestParseInvalidChunk(string chunk)
        {
            var decoded = new Dictionary<string, object>();
            var ex = ClassicAssert.Throws(typeof(MetarChunkDecoderException), () =>
            {
                decoded = chunkDecoder.Parse(chunk);
            }) as MetarChunkDecoderException;
            ClassicAssert.That(decoded.ContainsKey(MetarDecoder.ResultKey), Is.False);
            ClassicAssert.That(ex.RemainingMetar, Is.EqualTo(chunk));
        }

        #region TestCaseSources

        public static List<Tuple<string, string, string>> ValidChunks()
        {
            return new List<Tuple<string, string, string>>()
            {
                new Tuple<string, string, string>("LFPG AAA", "LFPG", "AAA"),
                new Tuple<string, string, string>("LFPO BBB", "LFPO", "BBB"),
                new Tuple<string, string, string>("LFIO CCC", "LFIO", "CCC"),
            };
        }

        public static List<string> InvalidChunks()
        {
            return new List<string>() {
                "LFA AAA",
                "L AAA",
                "LFP BBB",
                "LF8 CCC" };
        }

        #endregion TestCaseSources
    }
}