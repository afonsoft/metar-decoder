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

        public override string GetRegex()
        {
            // vertical visibility VV is handled as a regular cloud layer
            return $"^({NoCloudRegexPattern}|({LayerRegexPattern})( {LayerRegexPattern})?( {LayerRegexPattern})?( {LayerRegexPattern})?)( )";
        }

        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);

            ValidateCloudMatch(found, withCavok, remainingTaf, newRemainingTaf);

            var result = new Dictionary<string, object>
            {
                { CloudsParameterName, DecodeLayers(found) }
            };

            return GetResults(newRemainingTaf, result);
        }

        private void ValidateCloudMatch(
            List<Group> found,
            bool withCavok,
            string remainingTaf,
            string newRemainingTaf)
        {
            if (found.Count <= 1 && !withCavok)
            {
                throw new TafChunkDecoderException(remainingTaf, newRemainingTaf, TafChunkDecoderException.Messages.CloudsInformationBadFormat, this);
            }
        }

        private static List<CloudLayer> DecodeLayers(List<Group> found)
        {
            var layers = new List<CloudLayer>();
            if (found.Count <= 2 || !string.IsNullOrEmpty(found[2].Value))
            {
                return layers;
            }

            for (var i = 3; i <= 15; i += 4)
            {
                if (!string.IsNullOrEmpty(found[i].Value))
                {
                    layers.Add(CreateLayer(found, i));
                }
            }

            return layers;
        }

        private static CloudLayer CreateLayer(List<Group> found, int index)
        {
            var layer = new CloudLayer
            {
                Amount = GetCloudAmount(found[index + 1].Value),
                Type = GetCloudType(found[index + 3].Value)
            };

            var layerHeight = Value.ToInt(found[index + 2].Value);
            if (layerHeight.HasValue)
            {
                layer.BaseHeight = new Value(layerHeight.Value * 100d, Value.Unit.Feet);
            }

            return layer;
        }

        private static CloudLayer.CloudAmount GetCloudAmount(string value)
        {
            switch (value)
            {
                case "FEW":
                    return CloudLayer.CloudAmount.FEW;
                case "SCT":
                    return CloudLayer.CloudAmount.SCT;
                case "BKN":
                    return CloudLayer.CloudAmount.BKN;
                case "OVC":
                    return CloudLayer.CloudAmount.OVC;
                case "VV":
                    return CloudLayer.CloudAmount.VV;
                default:
                    return CloudLayer.CloudAmount.NULL;
            }
        }

        private static CloudLayer.CloudType GetCloudType(string value)
        {
            switch (value)
            {
                case "CB":
                    return CloudLayer.CloudType.CB;
                case "TCU":
                    return CloudLayer.CloudType.TCU;
                case "///":
                    return CloudLayer.CloudType.CannotMeasure;
                default:
                    return CloudLayer.CloudType.NULL;
            }
        }
    }
}