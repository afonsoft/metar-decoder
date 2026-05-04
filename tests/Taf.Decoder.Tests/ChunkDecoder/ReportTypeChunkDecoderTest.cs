using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.ChunkDecoder;

namespace Taf.Decoder.Tests.ChunkDecoder
{
    [TestFixture, Category("ReportTypeChunkDecoder")]
    public class ReportTypeChunkDecoderTest
    {
        private static readonly ReportTypeChunkDecoder chunkDecoder = new ReportTypeChunkDecoder();

        [Test, TestCaseSource("ValidChunks")]
        public void TestParse(Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string> chunk)
        {
            var decoded = chunkDecoder.Parse(chunk.Item1);
            ClassicAssert.AreEqual(chunk.Item2, (decoded[TafDecoder.ResultKey] as Dictionary<string, object>)[ReportTypeChunkDecoder.TypeParameterName]);
            ClassicAssert.AreEqual(chunk.Item3, decoded[TafDecoder.RemainingTafKey]);
        }

        public static List<Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>> ValidChunks => new List<Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>>()
        {
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("TAF LFPG",     Taf.Decoder.Entity.DecodedTaf.TafType.TAF,       "LFPG"),
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("TAF TAF LFPG", Taf.Decoder.Entity.DecodedTaf.TafType.TAF,       "LFPG"),
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("TAF AMD LFPO", Taf.Decoder.Entity.DecodedTaf.TafType.TAFAMD,    "LFPO"),
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("TA LFPG",      Taf.Decoder.Entity.DecodedTaf.TafType.NULL,      "TA LFPG"),
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("123 LFPO",     Taf.Decoder.Entity.DecodedTaf.TafType.NULL,      "123 LFPO"),
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("TAF COR LFPO", Taf.Decoder.Entity.DecodedTaf.TafType.TAFCOR,    "LFPO"),
            new Tuple<string, Taf.Decoder.Entity.DecodedTaf.TafType, string>("RTD EKEB",     Taf.Decoder.Entity.DecodedTaf.TafType.RTD,       "EKEB"),
        };
    }
}