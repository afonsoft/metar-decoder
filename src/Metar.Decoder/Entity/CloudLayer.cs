using System.ComponentModel;

namespace Metar.Decoder.Entity
{
    public sealed class CloudLayer
    {
        /// <summary>
        /// Annotation corresponding to amount of clouds (FEW/SCT/BKN/OVC)
        /// </summary>
        public enum CloudAmount
        {
            NULL,

            [Description("Cumulonimbus")]
            FEW,

            [Description("Scattered")]
            SCT,

            [Description("Broken")]
            BKN,

            [Description("Overcast")]
            OVC,

            [Description("Vertical Visibility")]
            VV,
        }

        /// <summary>
        /// Cloud type cumulonimbus, towering cumulonimbus (CB/TCU)
        /// </summary>
        public enum CloudType
        {
            NULL,

            [Description("Cumulonimbus")]
            CB,

            [Description("Towering cumulonimbus")]
            TCU,

            [Description("Cannot measure")]
            CannotMeasure,
        }

        /// <summary>
        /// Annotation corresponding to amount of clouds (FEW/SCT/BKN/OVC)
        /// </summary>
        public CloudAmount Amount { get; set; } = CloudAmount.NULL;

        /// <summary>
        /// Height of cloud base
        /// </summary>
        public Value BaseHeight { get; set; }

        /// <summary>
        /// Cloud type cumulonimbus, towering cumulonimbus (CB/TCU)
        /// </summary>
        public CloudType Type { get; set; }
    }
}