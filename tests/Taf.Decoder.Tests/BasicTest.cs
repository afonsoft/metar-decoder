using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Taf.Decoder;
using Taf.Decoder.entity;

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
            ClassicAssert.IsNotNull(DecodedTafs[0]);
        }

        [Test, Category("Basic")]
        public void CheckRawTafNotNull()
        {
            ClassicAssert.That(TestTafSource[0], Is.EqualTo(DecodedTafs[0].RawTaf));
        }
    }
}