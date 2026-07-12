using Metar.Decoder.ChunkDecoder;
using Metar.Decoder.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Metar.Decoder
{
    /// <summary>
    /// Metar Decoder
    /// </summary>
    public sealed class MetarDecoder
    {
        public const string ResultKey = "Result";
        public const string RemainingMetarKey = "RemainingMetar";
        public const string ExceptionKey = "Exception";

        /// <summary>
        /// Metar Decoder
        /// </summary>
        public MetarDecoder()
        {
            //csharpsquid:S6444
            AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(500)); // process-wide setting
        }

        private static readonly ReadOnlyCollection<MetarChunkDecoder> _decoderChain = new(new List<MetarChunkDecoder>()
        {
            new ReportTypeChunkDecoder(),
            new IcaoChunkDecoder(),
            new DatetimeChunkDecoder(),
            new ReportStatusChunkDecoder(),
            new SurfaceWindChunkDecoder(),
            new VisibilityChunkDecoder(),
            new RunwayVisualRangeChunkDecoder(),
            new PresentWeatherChunkDecoder(),
            new CloudChunkDecoder(),
            new TemperatureChunkDecoder(),
            new PressureChunkDecoder(),
            new RecentWeatherChunkDecoder(),
            new WindShearChunkDecoder(),
            new TrendChunkDecoder(),
            new RemarkChunkDecoder(),
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
        /// Decode a full metar string into a complete metar object
        /// while using global strict option.
        /// </summary>
        /// <param name="rawMetar">Metar String</param>
        public DecodedMetar Parse(string rawMetar)
        {
            return ParseWithMode(rawMetar, _globalStrictParsing);
        }

        /// <summary>
        /// Decode a full metar string into a complete metar object
        /// with strict option, meaning decoding will stop as soon as
        /// a non-compliance is detected.
        /// </summary>
        /// <param name="rawMetar"></param>
        /// <returns></returns>
        [SuppressMessage("Major Code Smell", "S2325", Justification = "Public API instance method kept for backward compatibility")]
        public DecodedMetar ParseStrict(string rawMetar)
        {
            return ParseWithMode(rawMetar, true);
        }

        /// <summary>
        /// Decode a full metar string into a complete metar object
        /// ith strict option disabled, meaning that decoding will
        /// continue even if metar is not compliant.
        /// </summary>
        /// <param name="rawMetar"></param>
        /// <returns></returns>
        [SuppressMessage("Major Code Smell", "S2325", Justification = "Public API instance method kept for backward compatibility")]
        public DecodedMetar ParseNotStrict(string rawMetar)
        {
            return ParseWithMode(rawMetar, false);
        }

        /// <summary>
        /// Decode a full metar string into a complete metar object.
        /// </summary>
        /// <param name="rawMetar">Metar String</param>
        /// <param name="isStrict">false</param>
        /// <returns></returns>
        public static DecodedMetar ParseWithMode(string rawMetar, bool isStrict = false)
        {
            return DecodeMetar(rawMetar, isStrict);
        }

        private static DecodedMetar DecodeMetar(string rawMetar, bool isStrict)
        {
            var cleanMetar = NormalizeMetar(rawMetar);
            var remainingMetar = cleanMetar;
            var decodedMetar = new DecodedMetar(cleanMetar);
            var withCavok = false;

            DecodeChunks(decodedMetar, isStrict, ref remainingMetar, ref withCavok);

            return decodedMetar;
        }

        private static string NormalizeMetar(string rawMetar)
        {
            var cleanMetar = rawMetar.ToUpper().Trim();
            cleanMetar = Regex.Replace(cleanMetar, "=$", string.Empty);
            return Regex.Replace(cleanMetar, "[ ]{2,}", " ") + " ";
        }

        private static void DecodeChunks(DecodedMetar decodedMetar, bool isStrict, ref string remainingMetar, ref bool withCavok)
        {
            foreach (var chunkDecoder in _decoderChain)
            {
                try
                {
                    var decodedData = TryParsing(chunkDecoder, isStrict, remainingMetar, withCavok);
                    ApplyDecodedData(decodedMetar, decodedData);
                    remainingMetar = decodedData[RemainingMetarKey] as string;
                }
                catch (MetarChunkDecoderException metarChunkDecoderException)
                {
                    decodedMetar.AddDecodingException(metarChunkDecoderException);
                    if (isStrict)
                    {
                        break;
                    }

                    remainingMetar = metarChunkDecoderException.RemainingMetar;
                }

                if (chunkDecoder is ReportStatusChunkDecoder && decodedMetar.Status == "NIL")
                {
                    break;
                }

                if (chunkDecoder is VisibilityChunkDecoder)
                {
                    withCavok = decodedMetar.Cavok;
                }
            }
        }

        private static void ApplyDecodedData(DecodedMetar decodedMetar, Dictionary<string, object> decodedData)
        {
            if (decodedData.ContainsKey(ExceptionKey))
            {
                decodedMetar.AddDecodingException((MetarChunkDecoderException)decodedData[ExceptionKey]);
            }

            if (decodedData.ContainsKey(ResultKey) && decodedData[ResultKey] is Dictionary<string, object> result)
            {
                foreach (var obj in result)
                {
                    if (obj.Value != null)
                    {
                        var property = typeof(DecodedMetar).GetProperty(obj.Key);
                        if (property == null)
                        {
                            throw new MetarChunkDecoderException($"Unknown property: {obj.Key}");
                        }
                        property.SetValue(decodedMetar, obj.Value, null);
                    }
                }
            }
        }

        private static Dictionary<string, object> TryParsing(IMetarChunkDecoder chunkDecoder, bool strict, string remainingMetar, bool withCavok)
        {
            Dictionary<string, object> decoded;
            try
            {
                decoded = chunkDecoder.Parse(remainingMetar, withCavok);
            }
            catch (MetarChunkDecoderException primaryException)
            {
                if (strict)
                {
                    throw;
                }
                else
                {
                    try
                    {
                        //the PHP version of ConsumeOneChunk implements an additional, unused strict flag
                        var alternativeRemainingMetar = MetarChunkDecoder.ConsumeOneChunk(remainingMetar);
                        decoded = chunkDecoder.Parse(alternativeRemainingMetar, withCavok);
                        decoded.Add(ExceptionKey, primaryException);
                    }
                    catch (MetarChunkDecoderException)
                    {
                        throw primaryException;
                    }
                }
            }
            return decoded;
        }
    }
}