using Metar.Decoder.Entity;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Metar.Decoder.ChunkDecoder
{
    public sealed class CloudChunkDecoder : MetarChunkDecoder
    {
        public const string CloudsParameterName = "Clouds";

        private const string NoCloudRegexPattern = "(NSC|NCD|CLR|SKC)";
        private const string LayerRegexPattern = "(VV|FEW|SCT|BKN|OVC|///)([0-9]{3}|///)(CB|TCU|///)?";

        /// <summary>
        /// vertical visibility VV is handled as a regular cloud layer
        /// </summary>
        /// <returns>string</returns>
        public override string GetRegex()
        {
            // vertical visibility VV is handled as a regular cloud layer
            return $"^({NoCloudRegexPattern}|({LayerRegexPattern})( {LayerRegexPattern})?( {LayerRegexPattern})?( {LayerRegexPattern})?)( )";
        }

        /// <summary>
        /// Parse
        /// </summary>
        /// <param name="remainingMetar"></param>
        /// <param name="withCavok"></param>
        /// <returns></returns>
        /// <exception cref="MetarChunkDecoderException"></exception>
        public override Dictionary<string, object> Parse(string remainingMetar, bool withCavok = false)
        {
            var consumed = Consume(remainingMetar);
            var found = consumed.Value;
            var newRemainingMetar = consumed.Key;
            var result = new Dictionary<string, object>();

            if (found.Count <= 1 && !withCavok)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.CloudsInformationBadFormat, this);
            }

            var layers = found.Count > 2 && string.IsNullOrEmpty(found[2].Value)
                ? ParseCloudLayers(found)
                : new List<CloudLayer>();

            result.Add(CloudsParameterName, layers);

            return GetResults(newRemainingMetar, result);
        }

        private static List<CloudLayer> ParseCloudLayers(List<Group> found)
        {
            var layers = new List<CloudLayer>();

            for (var i = 3; i <= 15; i += 4)
            {
                if (!string.IsNullOrEmpty(found[i].Value))
                {
                    layers.Add(ParseCloudLayer(found, i));
                }
            }

            return layers;
        }

        private static CloudLayer ParseCloudLayer(List<Group> found, int index)
        {
            var layer = new CloudLayer
            {
                Amount = ParseCloudAmount(found[index + 1].Value),
                Type = ParseCloudType(found[index + 3].Value),
            };

            var layerHeight = Value.ToInt(found[index + 2].Value);
            if (layerHeight.HasValue)
            {
                layer.BaseHeight = new Value(layerHeight.Value * 100.0, Value.Unit.Feet);
            }

            return layer;
        }

        private static CloudLayer.CloudAmount ParseCloudAmount(string value)
        {
            return value switch
            {
                "FEW" => CloudLayer.CloudAmount.FEW,
                "SCT" => CloudLayer.CloudAmount.SCT,
                "BKN" => CloudLayer.CloudAmount.BKN,
                "OVC" => CloudLayer.CloudAmount.OVC,
                "VV" => CloudLayer.CloudAmount.VV,
                _ => CloudLayer.CloudAmount.NULL,
            };
        }

        private static CloudLayer.CloudType ParseCloudType(string value)
        {
            return value switch
            {
                "CB" => CloudLayer.CloudType.CB,
                "TCU" => CloudLayer.CloudType.TCU,
                "///" => CloudLayer.CloudType.CannotMeasure,
                _ => CloudLayer.CloudType.NULL,
            };
        }
    }
}