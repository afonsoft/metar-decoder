﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Metar.Decoder.Entity
{
    public sealed class PresentWeather
    {
        private readonly List<int> _precipitations = new List<int>();

        /// <summary>
        /// Precipitations phenomenon
        /// </summary>
        public ReadOnlyCollection<int> Precipitations
        {
            get
            {
                return new ReadOnlyCollection<int>(_precipitations);
            }
        }

        private readonly List<int> _obscurations = new List<int>();

        /// <summary>
        /// Obscurations phenomenon
        /// </summary>
        public ReadOnlyCollection<int> Obscurations
        {
            get
            {
                return new ReadOnlyCollection<int>(_obscurations);
            }
        }

        private readonly List<int> _vicinities = new List<int>();

        /// <summary>
        /// Obscurations phenomenon
        /// </summary>
        public ReadOnlyCollection<int> Vicinities
        {
            get
            {
                return new ReadOnlyCollection<int>(_vicinities);
            }
        }

        /// <summary>
        /// AddPrecipitation
        /// </summary>
        /// <param name="precipitation">precipitation</param>
        public void AddPrecipitation(int precipitation)
        {
            _precipitations.Add(precipitation);
        }

        /// <summary>
        /// AddObscuration
        /// </summary>
        /// <param name="obscurationPhenomenon">obscurationPhenomenon</param>
        public void AddObscuration(int obscurationPhenomenon)
        {
            _obscurations.Add(obscurationPhenomenon);
        }

        /// <summary>
        /// AddVicinity
        /// </summary>
        /// <param name="vicinity">vicinity</param>
        public void AddVicinity(int vicinity)
        {
            _vicinities.Add(vicinity);
        }
    }
}