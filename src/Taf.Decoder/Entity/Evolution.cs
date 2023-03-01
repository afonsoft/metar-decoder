using System;

namespace Taf.Decoder.entity
{
    public class Evolution : AbstractEntity, ICloneable
    {
        /// <summary>
        /// annotation corresponding to the type of evolution (FM, BECMG or TEMPO)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// day when the evolution occurs (FM) or starts (BECMG/TEMPO)
        /// </summary>
        public int? FromDay { get; set; }

        /// <summary>
        /// hour and minute UTC (as string) when the evolution occurs (FM)
        /// or hour UTC (as string) when the evolution starts (BECMG/TEMPO)
        /// </summary>
        public string FromTime { get; set; } = string.Empty;

        /// <summary>
        /// day when the evolution ends (BECMG/tEMPO)
        /// </summary>
        public int? ToDay { get; set; }

        /// <summary>
        /// hour UTC (as string) when the evolution ends (BECMG/TEMPO)
        /// </summary>
        public string ToTime { get; set; } = string.Empty;

        public object Entity { get; set; }

        /// <summary>
        /// weather entity (i.e. SurfaceWind, Temperature, Visibility, etc.)
        /// public Entity entity { get; set; }
        /// </summary>
        public bool Cavok { get; set; }

        /// <summary>
        /// optional annotation corresponding to the probability (PROBnn)
        /// </summary>
        public string Probability { get; set; } = string.Empty;

        public object Clone()
        {
            return new Evolution()
            {
                Type = Type,
                FromDay = FromDay,
                FromTime = FromTime,
                ToDay = ToDay,
                ToTime = ToTime,
                Cavok = Cavok,
                Probability = Probability,
                Entity = Entity,
                Evolutions = Evolutions,
            };
        }
    }
}