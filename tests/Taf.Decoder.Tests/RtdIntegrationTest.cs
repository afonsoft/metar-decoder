using NUnit.Framework;
using NUnit.Framework.Legacy;
using Taf.Decoder;

namespace Taf.Decoder.Tests
{
    [TestFixture, Category("RTD Integration")]
    public class RtdIntegrationTest
    {
        [Test]
        public void TestParseRtdTaf()
        {
            // Arrange
            string rtdTaf = "RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 TEMPO 1905/1907 2000 BR BKN003 BECMG 1907/1909 9000 NSW FEW002 PROB40 1909/1911 0400 FZFG BKN002=";
            var decoder = new TafDecoder();
            
            // Act
            var result = decoder.Parse(rtdTaf);
            
            // Assert - Most important: RTD type is recognized
            ClassicAssert.AreEqual(entity.DecodedTaf.TafType.RTD, result.Type);
            ClassicAssert.AreEqual("EKEB", result.Icao);
            ClassicAssert.AreEqual(19, result.Day);
            ClassicAssert.AreEqual(rtdTaf, result.RawTaf);
            
            // Check that parsing worked (basic elements)
            ClassicAssert.IsNotNull(result.SurfaceWind);
            ClassicAssert.IsNotNull(result.Visibility);
            ClassicAssert.IsNotNull(result.ForecastPeriod);
        }
        
        [Test]
        public void TestParseRtdTafNotStrict()
        {
            // Arrange
            string rtdTaf = "RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 TEMPO 1905/1907 2000 BR BKN003 BECMG 1907/1909 9000 NSW FEW002 PROB40 1909/1911 0400 FZFG BKN002=";
            var decoder = new TafDecoder();
            
            // Act
            var result = decoder.ParseNotStrict(rtdTaf);
            
            // Assert
            ClassicAssert.IsTrue(result.IsValid);
            ClassicAssert.AreEqual(entity.DecodedTaf.TafType.RTD, result.Type);
            ClassicAssert.AreEqual("EKEB", result.Icao);
        }
    }
}
