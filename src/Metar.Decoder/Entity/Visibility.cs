﻿namespace Metar.Decoder.Entity
{
    /// <summary>
    /// Visibility
    /// </summary>
    public sealed class Visibility
    {
        /// <summary>
        /// Prevailing visibility
        /// </summary>
        public Value PrevailingVisibility { get; set; }

        /// <summary>
        /// Minimum visibility
        /// </summary>
        public Value MinimumVisibility { get; set; }

        /// <summary>
        /// Direction of minimum visibility
        /// </summary>
        public string MinimumVisibilityDirection { get; set; }

        /// <summary>
        /// No Direction Visibility
        /// </summary>
        public bool NDV { get; set; } = false;
    }
}