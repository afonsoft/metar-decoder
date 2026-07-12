using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Taf.Decoder.ChunkDecoder;
using Taf.Decoder.Entity;

namespace Taf.Decoder
{
    /// <summary>
    /// Taf Decoder
    /// </summary>
    public sealed class TafDecoder
    {
        public const string ResultKey = "Result";
        public const string RemainingTafKey = "RemainingTaf";
        public const string ExceptionKey = "Exception";

        /// <summary>
        /// Taf Decoder
        /// </summary>
        public TafDecoder()
        {
            //csharpsquid:S6444
            AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(500)); // process-wide setting
        }

        private static readonly ReadOnlyCollection<TafChunkDecoder> _decoderChain = new ReadOnlyCollection<TafChunkDecoder>(new List<TafChunkDecoder>()
        {
            new ReportTypeChunkDecoder(),
            new IcaoChunkDecoder(),
            new DatetimeChunkDecoder(),
            new ForecastPeriodChunkDecoder(),
            new SurfaceWindChunkDecoder(),
            new VisibilityChunkDecoder(),
            new WeatherChunkDecoder(),
            new CloudChunkDecoder(),
            new TemperatureChunkDecoder(),
        });

        private bool _globalStrictParsing = false;

        /// <summary>
        /// Set global parsing mode (strict/not strict) for the whole object.
        /// </summary>
        /// <param name="isStrict"></param>
        public void SetStrictParsing(bool isStrict)
        {
            _globalStrictParsing = isStrict;
        }

        /// <summary>
        /// Decode a full taf string into a complete taf object
        /// while using global strict option.
        /// </summary>
        /// <param name="rawTaf"></param>
        public DecodedTaf Parse(string rawTaf)
        {
            return ParseWithMode(rawTaf, _globalStrictParsing);
        }

        /// <summary>
        /// Decode a full taf string into a complete taf object
        /// with strict option, meaning decoding will stop as soon as
        /// a non-compliance is detected.
        /// </summary>
        /// <param name="rawTaf"></param>
        /// <returns></returns>
        [SuppressMessage("Major Code Smell", "S2325", Justification = "Public API instance method kept for backward compatibility")]
        public DecodedTaf ParseStrict(string rawTaf)
        {
            return ParseWithMode(rawTaf, true);
        }

        /// <summary>
        /// Decode a full taf string into a complete taf object
        /// with strict option disabled, meaning that decoding will
        /// continue even if taf is not compliant.
        /// </summary>
        /// <param name="rawTaf"></param>
        /// <returns></returns>
        [SuppressMessage("Major Code Smell", "S2325", Justification = "Public API instance method kept for backward compatibility")]
        public DecodedTaf ParseNotStrict(string rawTaf)
        {
            return ParseWithMode(rawTaf, false);
        }

        /// <summary>
        /// Decode a full taf string into a complete taf object.
        /// </summary>
        /// <param name="rawTaf">raw Taf</param>
        /// <param name="isStrict"></param>
        /// <returns></returns>
        public static DecodedTaf ParseWithMode(string rawTaf, bool isStrict = false)
        {
            return DecodeTaf(rawTaf, isStrict);
        }

        private static DecodedTaf DecodeTaf(string rawTaf, bool isStrict)
        {
            var cleanTaf = NormalizeTaf(rawTaf);
            var remainingTaf = PrepareRemainingTaf(cleanTaf);
            var decodedTaf = new DecodedTaf(cleanTaf);
            var withCavok = false;

            DecodeChunks(decodedTaf, isStrict, ref remainingTaf, ref withCavok);

            DecodeEvolutions(decodedTaf, isStrict, withCavok, ref remainingTaf);

            return decodedTaf;
        }

        private static string NormalizeTaf(string rawTaf)
        {
            var cleanTaf = rawTaf.Trim();
            cleanTaf = Regex.Replace(cleanTaf, "\n+", string.Empty);
            cleanTaf = Regex.Replace(cleanTaf, "\r+", string.Empty);
            cleanTaf = Regex.Replace(cleanTaf, "[ ]{2,}", " ") + " ";
            return cleanTaf.ToUpper();
        }

        private static string PrepareRemainingTaf(string cleanTaf)
        {
            if (cleanTaf.Contains("CNL"))
            {
                return cleanTaf;
            }

            return cleanTaf.Trim() + " END";
        }

        private static void DecodeChunks(DecodedTaf decodedTaf, bool isStrict, ref string remainingTaf, ref bool withCavok)
        {
            foreach (var chunkDecoder in _decoderChain)
            {
                try
                {
                    var decodedData = chunkDecoder.Parse(remainingTaf, withCavok);
                    ApplyDecodedData(decodedTaf, decodedData);
                    remainingTaf = decodedData[RemainingTafKey] as string;
                }
                catch (TafChunkDecoderException tafChunkDecoderException)
                {
                    decodedTaf.AddDecodingException(tafChunkDecoderException);
                    if (isStrict)
                    {
                        break;
                    }

                    remainingTaf = tafChunkDecoderException.RemainingTaf;
                }

                if (chunkDecoder is VisibilityChunkDecoder)
                {
                    withCavok = decodedTaf.Cavok;
                }
            }
        }

        private static void DecodeEvolutions(DecodedTaf decodedTaf, bool isStrict, bool withCavok, ref string remainingTaf)
        {
            var evolutionDecoder = new EvolutionChunkDecoder(isStrict, withCavok);
            while (!string.IsNullOrEmpty(remainingTaf) && remainingTaf.Trim() != "END")
            {
                evolutionDecoder.Parse(remainingTaf, decodedTaf);
                remainingTaf = evolutionDecoder.Remaining;
            }
        }

        private static void ApplyDecodedData(DecodedTaf decodedTaf, Dictionary<string, object> decodedData)
        {
            if (decodedData.ContainsKey(ResultKey) && decodedData[ResultKey] is Dictionary<string, object> result)
            {
                foreach (var obj in result)
                {
                    if (obj.Value != null)
                    {
                        var property = typeof(DecodedTaf).GetProperty(obj.Key);
                        if (property == null)
                        {
                            throw new TafChunkDecoderException($"Unknown property: {obj.Key}");
                        }
                        property.SetValue(decodedTaf, obj.Value);
                    }
                }
            }
        }
    }
}
