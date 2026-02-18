using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Taf.Decoder.entity
{
    public sealed class DecodedTaf : AbstractEntity
    {
        public enum TafType
        {
            NULL,
            TAF,
            TAFAMD,
            TAFCOR,
            RTD,
        }

        private string _rawTaf;

        /// <summary>
        /// Raw TAF
        /// </summary>
        public string RawTaf
        {
            get
            {
                return _rawTaf.Trim();
            }
            set
            {
                _rawTaf = value;
            }
        }

        /// <summary>
        /// Decoding exceptions, if any
        /// </summary>
        private List<TafChunkDecoderException> _decodingExceptions = new List<TafChunkDecoderException>();

        /// <summary>
        /// If the decoded taf is invalid, get all the exceptions that occurred during decoding
        /// Note that in strict mode, only the first encountered exception will be reported as parsing stops on error
        /// Else return null;.
        /// </summary>
        public ReadOnlyCollection<TafChunkDecoderException> DecodingExceptions
        {
            get
            {
                return new ReadOnlyCollection<TafChunkDecoderException>(_decodingExceptions);
            }
        }

        /// <summary>
        /// Report type
        /// </summary>
        public TafType Type { get; set; } = TafType.NULL;

        /// <summary>
        /// ICAO code of the airport where the forecast has been made
        /// </summary>
        public string Icao { get; set; } = string.Empty;

        /// <summary>
        /// Day of origin
        /// </summary>
        public int? Day { get; set; }

        /// <summary>
        /// Time of origin, as string
        /// </summary>
        public string Time { get; set; } = string.Empty;

        /// <summary>
        /// Date and time of origin
        /// </summary>
        public DateTime? OriginDateTime { get; set; }

        /// <summary>
        /// Forecast period
        /// </summary>
        public ForecastPeriod ForecastPeriod { get; set; }

        /// <summary>
        /// Report status (AUTO or NIL)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Surface wind information
        /// </summary>
        public SurfaceWind SurfaceWind { get; set; }

        /// <summary>
        /// Visibility information
        /// </summary>
        public Visibility Visibility { get; set; }

        public bool Cavok { get; set; } = false;

        /// <summary>
        /// Weather phenomenon
        /// </summary>
        public List<WeatherPhenomenon> WeatherPhenomenons { get; set; } = new List<WeatherPhenomenon>();

        /// <summary>
        /// Cloud layers information
        /// </summary>
        public List<CloudLayer> Clouds { get; set; } = new List<CloudLayer>();

        /// <summary>
        /// Temperature information
        /// </summary>
        public Temperature MinimumTemperature { get; set; }

        /// <summary>
        /// Temperature information
        /// </summary>
        public Temperature MaximumTemperature { get; set; }

        internal DecodedTaf(string rawTaf = "")
        {
            RawTaf = rawTaf;
        }

        /// <summary>
        /// Reset the whole list of Decoding Exceptions
        /// </summary>
        public void ResetDecodingExceptions()
        {
            _decodingExceptions = new List<TafChunkDecoderException>();
        }

        /// <summary>
        /// Check if the decoded taf is valid, i.e. if there was no error during decoding.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return DecodingExceptions.Count == 0;
            }
        }

        /// <summary>
        /// Add an exception that occured during taf decoding.
        /// </summary>
        /// <param name="ex"></param>
        public void AddDecodingException(TafChunkDecoderException ex)
        {
            _decodingExceptions.Add(ex);
        }
    }
}