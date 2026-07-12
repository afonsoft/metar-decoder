using Decoder.Shared;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Taf.Decoder.Entity;

namespace Taf.Decoder.ChunkDecoder
{
    public sealed class CloudChunkDecoder : TafChunkDecoder
    {
        public const string CloudsParameterName = "Clouds";

        private const string NoCloudRegexPattern = "(NSC|NCD|CLR|SKC)";
        private const string LayerRegexPattern = "(VV|FEW|SCT|BKN|OVC|///)([0-9]{3}|///)(CB|TCU|///)?";

        private static readonly Dictionary<string, CloudLayer.CloudAmount> AmountMap = new()
        {
            { "FEW", CloudLayer.CloudAmount.FEW },
            { "SCT", CloudLayer.CloudAmount.SCT },
            { "BKN", CloudLayer.CloudAmount.BKN },
            { "OVC", CloudLayer.CloudAmount.OVC },
            { "VV", CloudLayer.CloudAmount.VV },
        };

        private static readonly Dictionary<string, CloudLayer.CloudType> TypeMap = new()
        {
            { "CB", CloudLayer.CloudType.CB },
            { "TCU", CloudLayer.CloudType.TCU },
            { "///", CloudLayer.CloudType.CannotMeasure },
        };

        public override string GetRegex()
        {
            // vertical visibility VV is handled as a regular cloud layer
            return $"^({NoCloudRegexPattern}|({LayerRegexPattern})( {LayerRegexPattern})?( {LayerRegexPattern})?( {LayerRegexPattern})?)( )";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);
            var result = new Dictionary<string, object>();

            if (found.Count <= 1 && !withCavok)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.CloudsInformationBadFormat, this);
            }

            var layers = found.Count > 2 && string.IsNullOrEmpty(found[2].Value)
                ? ParseCloudLayers(found)
                : new List<CloudLayer>();

            result.Add(CloudsParameterName, layers);

            return GetResults(newRemainingTaf, result);
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
                Amount = AmountMap.GetValueOrDefault(found[index + 1].Value, CloudLayer.CloudAmount.NULL),
                Type = TypeMap.GetValueOrDefault(found[index + 3].Value, CloudLayer.CloudType.NULL),
            };

            var layerHeight = Value.ToInt(found[index + 2].Value);
            if (layerHeight.HasValue)
            {
                layer.BaseHeight = new Value(layerHeight.Value * 100.0, Value.Unit.Feet);
            }

            return layer;
        }
    }
}