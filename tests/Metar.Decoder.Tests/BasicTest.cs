using Metar.Decoder;
using Metar.Decoder.Chunkdecoder;
using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Metar.Decoder_tests
{
    /// <summary>
    /// BasicTest
    /// </summary>
    [TestFixture]
    public class BasicTest
    {
        public readonly ReadOnlyCollection<string> TestMetarSource = new ReadOnlyCollection<string>(new List<string>() {
            "LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03",
        });

        private List<DecodedMetar> DecodedMetars;

        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            DecodedMetars = TestMetarSource.Select(metar => MetarDecoder.ParseWithMode(metar)).ToList();
        }

        /// <summary>
        /// RunToCompletionTest
        /// </summary>
        [Test, Category("Basic")]
        public void RunToCompletionTest()
        {
            ClassicAssert.That(DecodedMetars[0], Is.Not.Null);
        }

        /// <summary>
        /// CheckRawMetarNotNull
        /// </summary>
        [Test, Category("Basic")]
        public void CheckRawMetarNotNull()
        {
            ClassicAssert.That(DecodedMetars[0].RawMetar, Is.EqualTo(TestMetarSource[0]));
        }

        /// <summary>
        /// ConsumeOneChunkTest
        /// </summary>
        [Test, Category("Basic")]
        public void ConsumeOneChunkTest()
        {
            var result = MetarChunkDecoder.ConsumeOneChunk(TestMetarSource[0]);

            ClassicAssert.That(result, Is.EqualTo("231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03"));
        }
    }
}