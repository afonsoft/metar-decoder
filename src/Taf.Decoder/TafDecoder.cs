using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;

namespace Taf.Decoder
{
    public sealed class TafDecoder
    {
        public const string ResultKey = "Result";
        public const string RemainingTafKey = "RemainingTaf";
        public const string ExceptionKey = "Exception";

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
            // prepare decoding inputs/outputs: (trim, remove linefeeds and returns, no more than one space)
            var cleanTaf = rawTaf.Trim();
            cleanTaf = Regex.Replace(cleanTaf, "\n+", string.Empty);
            cleanTaf = Regex.Replace(cleanTaf, "\r+", string.Empty);
            cleanTaf = Regex.Replace(cleanTaf, "[ ]{2,}", " ") + " ";
            cleanTaf = cleanTaf.ToUpper();

            var remainingTaf = cleanTaf;
            if (!cleanTaf.Contains("CNL"))
            {
                // appending END to it is necessary to detect the last line of evolution
                // but only when the TAF wasn't cancelled (CNL)
                remainingTaf = cleanTaf.Trim() + " END";
            }
            else
            {
                remainingTaf = cleanTaf;
            }

            var decodedTaf = new DecodedTaf(cleanTaf);
            var withCavok = false;

            // call each decoder in the chain and use results to populate decoded taf
            foreach (var chunkDecoder in _decoderChain)
            {
                try
                {
                    // try to parse a chunk with current chunk decoder
                    var decodedData = chunkDecoder.Parse(remainingTaf, withCavok);

                    // map obtained fields (if any) to the final decoded object
                    if (decodedData.ContainsKey(ResultKey) && decodedData[ResultKey] is Dictionary<string, object>)
                    {
                        var result = decodedData[ResultKey] as Dictionary<string, object>;
                        foreach (var obj in result)
                        {
                            if (obj.Value != null)
                            {
                                typeof(DecodedTaf).GetProperty(obj.Key).SetValue(decodedTaf, obj.Value);
                            }
                        }
                    }

                    // update remaining taf for next round
                    remainingTaf = decodedData[RemainingTafKey] as string;
                }
                catch (TafChunkDecoderException tafChunkDecoderException)
                {
                    // log error in decoded taf and abort decoding if in strict mode
                    decodedTaf.AddDecodingException(tafChunkDecoderException);
                    // abort decoding if strict mode is activated, continue otherwise
                    if (isStrict)
                    {
                        break;
                    }
                    // update remaining taf for next round
                    remainingTaf = tafChunkDecoderException.RemainingTaf;
                }

                // hook for CAVOK decoder, keep CAVOK information in memory
                if (chunkDecoder is VisibilityChunkDecoder)
                {
                    withCavok = decodedTaf.Cavok;
                }
            }

            // weather evolutions
            var evolutionDecoder = new EvolutionChunkDecoder(isStrict, withCavok);
            while (!string.IsNullOrEmpty(remainingTaf) && remainingTaf.Trim() != "END")
            {
                evolutionDecoder.Parse(remainingTaf, decodedTaf);
                remainingTaf = evolutionDecoder.Remaining;
            }

            return decodedTaf;
        }
    }
}