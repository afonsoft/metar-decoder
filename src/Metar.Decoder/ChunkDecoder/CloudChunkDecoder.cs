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

            // default case: CAVOK or clear sky, no cloud layer
            var layers = new List<CloudLayer>();

            // handle the case where nothing has been found and metar is not cavok
            if (found.Count <= 1 && !withCavok)
            {
                throw new MetarChunkDecoderException(remainingMetar, newRemainingMetar, MetarChunkDecoderException.Messages.CloudsInformationBadFormat, this);
            }

            // there are clouds, handle cloud layers and visibility
            if (found.Count > 2 && string.IsNullOrEmpty(found[2].Value))
            {
                for (var i = 3; i <= 15; i += 4)
                {
                    if (!string.IsNullOrEmpty(found[i].Value))
                    {
                        layers.Add(ParseLayer(found, i));
                    }
                }
            }

            result.Add(CloudsParameterName, layers);

            return GetResults(newRemainingMetar, result);
        }

        private static CloudLayer ParseLayer(List<Group> found, int i)
        {
            var layer = new CloudLayer();
            var layerHeight = Value.ToInt(found[i + 2].Value);
            int? layerHeightFeet = null;
            if (layerHeight.HasValue)
            {
                layerHeightFeet = layerHeight * 100;
            }

            layer.Amount = ParseAmount(found[i + 1].Value);

            if (layerHeightFeet.HasValue)
            {
                layer.BaseHeight = new Value(layerHeightFeet.Value, Value.Unit.Feet);
            }

            layer.Type = ParseType(found[i + 3].Value);

            return layer;
        }

        private static CloudLayer.CloudAmount ParseAmount(string amount)
        {
            switch (amount)
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

        private static CloudLayer.CloudType ParseType(string type)
        {
            switch (type)
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
