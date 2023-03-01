using Taf.Decoder;
using Taf.Decoder.entity;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Taf.Decoder_tests
{
    [TestFixture]
    public class BasicTest
    {
        public readonly ReadOnlyCollection<string> TestTafSource = new ReadOnlyCollection<string>(new List<string>() {
            "LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03",
        });

        private List<DecodedTaf> DecodedTafs;

        [SetUp]
        public void Setup()
        {
            DecodedTafs = TestTafSource.Select(taf => TafDecoder.ParseWithMode(taf)).ToList();
        }

        [Test, Category("Basic")]
        public void RunToCompletionTest()
        {
            Assert.IsNotNull(DecodedTafs[0]);
        }

        [Test, Category("Basic")]
        public void CheckRawTafNotNull()
        {
            Assert.AreEqual(TestTafSource[0], DecodedTafs[0].RawTaf);
        }
    }
}