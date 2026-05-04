using System.Collections.Generic;

namespace Metar.Decoder.ChunkDecoder
{
    /// <summary>
    /// Chunk decoder for METAR remarks section (RMK).
    /// Per ICAO Annex 3, the remarks section contains supplementary information
    /// including sea-level pressure (SLPnnn), hourly precipitation, etc.
    /// </summary>
    public sealed class RemarkChunkDecoder : MetarChunkDecoder
    {
        public const string RemarkParameterName = "Remark";
        public const string SeaLevelPressureParameterName = "SeaLevelPressure";

        public override string GetRegex()
        {
            return @"^RMK (.*)";
        }

        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            if (found.Count > 1)
            {
                var remarkContent = found[1].Value.Trim();
                result.Add(RemarkParameterName, remarkContent);

                // Try to extract sea-level pressure (SLPnnn)
                var slpMatch = System.Text.RegularExpressions.Regex.Match(
                    remarkContent,
                    @"SLP(\d{3})",
                    System.Text.RegularExpressions.RegexOptions.None,
                    System.TimeSpan.FromMilliseconds(500));
                if (slpMatch.Success)
                {
                    var slpValue = int.Parse(slpMatch.Groups[1].Value);
                    double pressureHpa = slpValue >= 500 ? (900.0 + slpValue / 10.0) : (1000.0 + slpValue / 10.0);
                    result.Add(SeaLevelPressureParameterName, new Entity.Value(pressureHpa, Entity.Value.Unit.HectoPascal));
                }
            }

            return GetResults(newRemainingMetar, result);
        }
    }
}
