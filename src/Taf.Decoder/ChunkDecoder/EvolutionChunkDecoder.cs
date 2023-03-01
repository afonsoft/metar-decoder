using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Taf.Decoder.entity;

namespace Taf.Decoder.chunkdecoder
{
    public sealed class EvolutionChunkDecoder : TafChunkDecoder
    {
        public const string Probability = "Probability";

        private const string TypePattern = @"(BECMG\s+|TEMPO\s+|FM|PROB[34]0\s+){1}";
        private const string PeriodPattern = @"([0-9]{4}/[0-9]{4}\s+|[0-9]{6}\s+){1}";
        private const string RemainderPattern = @"(.*)";

        private const string ProbabilityPattern = @"^(PROB[34]0\s+){1}(TEMPO\s+){0,1}([0-9]{4}/[0-9]{4}){0,1}(.*)";

        public override string GetRegex()
        {
            return $"{TypePattern}{PeriodPattern}{RemainderPattern}";
        }

        private static ReadOnlyCollection<TafChunkDecoder> _decoderChain = new ReadOnlyCollection<TafChunkDecoder>(new List<TafChunkDecoder>()
        {
            new SurfaceWindChunkDecoder(),
            new VisibilityChunkDecoder(),
            new WeatherChunkDecoder(),
            new CloudChunkDecoder(),
            new TemperatureChunkDecoder(),
        });

        public bool IsStrict { private get; set; }
        private bool _withCavok;

        public string Remaining { get; private set; }

        public EvolutionChunkDecoder(bool strict, bool withCavok)
        {
            IsStrict = strict;
            _withCavok = withCavok;
        }

        /// <summary>
        /// Not implemented because EvolutionChunkDecoder is not part of the decoder chain
        /// </summary>
        /// <param name="remainingTaf"></param>
        /// <param name="withCavok"></param>
        /// <returns></returns>
        public override Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false)
        {
            throw new NotImplementedException();
        }

        public void Parse(string remainingTaf, DecodedTaf decodedTaf)
        {
            string newRemainingTaf;
            var found = Consume(remainingTaf, out newRemainingTaf);

            if (found.Count <= 1)
            {
                // the first chunk didn't match anything, so we remove it to avoid an infinite loop
                Remaining = ConsumeOneChunk(remainingTaf);
                return;
            }

            var evolutionType = found[1].Value.Trim();
            var evolutionPeriod = found[2].Value.Trim();
            var remaining = found[3].Value;

            var evolution = new Evolution() { Type = evolutionType };
            if (newRemainingTaf.StartsWith("PROB"))
            {
                // if the line started with PROBnn it won't have been consumed and we'll find it in remaining
                evolution.Probability = newRemainingTaf.Trim();
            }

            // period
            if (evolutionType == "BECMG" || evolutionType == "TEMPO")
            {
                var evolutionPeriodArray = evolutionPeriod.Split('/');
                evolution.FromDay = Convert.ToInt32(evolutionPeriodArray[0].Substring(0, 2));
                evolution.FromTime = evolutionPeriodArray[0].Substring(2, 2) + ":00 UTC";
                evolution.ToDay = Convert.ToInt32(evolutionPeriodArray[1].Substring(0, 2));
                evolution.ToTime = evolutionPeriodArray[1].Substring(2, 2) + ":00 UTC";
            }
            else
            {
                evolution.FromDay = Convert.ToInt32(evolutionPeriod.Substring(0, 2));
                evolution.FromTime = evolutionPeriod.Substring(2, 2) + ':' + evolutionPeriod.Substring(4, 2) + " UTC";
            }

            remaining = ParseEntitiesChunk(evolution, remaining, decodedTaf);
        }

        /// <summary>
        /// Extract the weather elements (surface winds, visibility, etc) between 2 evolution tags (BECMG, TEMPO or FM)
        /// </summary>
        /// <param name="evolution"></param>
        /// <param name="chunk"></param>
        /// <param name="decodedTaf"></param>
        /// <returns></returns>
        private string ParseEntitiesChunk(Evolution evolution, string chunk, DecodedTaf decodedTaf)
        {
            // For each value we detect, we'll clone the evolution object, complete the clone,
            // and add it to the corresponding entity of the decoded taf

            var remainingEvolutions = chunk;
            var tries = 0;

            // call each decoder in the chain and use results to populate the decoded taf
            foreach (var chunk_decoder in _decoderChain)
            {
                try
                {
                    // we check for probability in each loop, as it can be anywhere
                    remainingEvolutions = ProbabilityChunkDecoder(evolution, remainingEvolutions, decodedTaf);

                    // reset cavok
                    _withCavok = false;

                    // try to parse the chunk with the current chunk decoder
                    var decoded = chunk_decoder.Parse(remainingEvolutions, _withCavok);

                    // map the obtained fields (if any) to a original entity in the decoded_taf
                    var result = decoded[TafDecoder.ResultKey] as Dictionary<string, object>;
                    var entityName = result.Keys.FirstOrDefault();
                    if (entityName == VisibilityChunkDecoder.CavokParameterName)
                    {
                        if ((bool)result[entityName])
                        {
                            _withCavok = true;
                        }
                        entityName = VisibilityChunkDecoder.VisibilityParameterName;
                    }
                    var entity = result.Count > 0 ? result[entityName] : null;

                    if (entity == null && entityName != VisibilityChunkDecoder.VisibilityParameterName)
                    {
                        // visibility will be null if cavok is true but we still want to add the evolution
                        throw new TafChunkDecoderException(chunk, remainingEvolutions, TafChunkDecoderException.Messages.WeatherEvolutionBadFormat, this);
                    }
                    if (entityName == TemperatureChunkDecoder.MaximumTemperatureParameterName || entityName == TemperatureChunkDecoder.MinimumTemperatureParameterName)
                    {
                        AddEvolution(decodedTaf, evolution, result, TemperatureChunkDecoder.MaximumTemperatureParameterName);
                        AddEvolution(decodedTaf, evolution, result, TemperatureChunkDecoder.MinimumTemperatureParameterName);
                    }
                    else
                    {
                        AddEvolution(decodedTaf, evolution, result, entityName);
                    }

                    // update remaining evo for the next round
                    remainingEvolutions = (string)decoded[TafDecoder.RemainingTafKey];
                }
                catch (Exception)
                {
                    if (++tries == _decoderChain.Count)
                    {
                        if (IsStrict)
                        {
                            throw new TafChunkDecoderException(chunk, remainingEvolutions, TafChunkDecoderException.Messages.EvolutionInformationBadFormat, this);
                        }
                        else
                        {
                            // we tried all the chunk decoders on the first chunk and none of them got a match,
                            // so we drop it
                            remainingEvolutions = ConsumeOneChunk(remainingEvolutions);
                        }
                    }
                }
            }
            return remainingEvolutions;
        }

        private void AddEvolution(DecodedTaf decodedTaf, Evolution evolution, Dictionary<string, object> result, string entityName)
        {
            // clone the evolution entity
            var newEvolution = evolution.Clone() as Evolution;

            // add the new entity to it
            newEvolution.Entity = result[entityName];

            if (entityName == VisibilityChunkDecoder.VisibilityParameterName && _withCavok)
            {
                newEvolution.Cavok = true;
            }

            // get the original entity from the decoded taf or a new one decoded taf doesn't contain it yet
            var decodedEntity = typeof(DecodedTaf).GetProperty(entityName).GetValue(decodedTaf) as AbstractEntity;

            if (decodedEntity == null || entityName == CloudChunkDecoder.CloudsParameterName || entityName == WeatherChunkDecoder.WeatherPhenomenonParameterName)
            {
                // that entity is not in the decoded_taf yet, or it's a cloud layer which is a special case
                decodedEntity = InstantiateEntity(entityName);
            }

            // add the new evolution to that entity
            decodedEntity.Evolutions.Add(newEvolution);

            // update the decoded taf's entity or add the new one to it
            switch (entityName)
            {
                case CloudChunkDecoder.CloudsParameterName:
                    decodedTaf.Clouds.Add(decodedEntity as CloudLayer);
                    break;

                case WeatherChunkDecoder.WeatherPhenomenonParameterName:
                    decodedTaf.WeatherPhenomenons.Add(decodedEntity as WeatherPhenomenon);
                    break;

                case VisibilityChunkDecoder.VisibilityParameterName:
                    decodedTaf.Visibility = decodedEntity as Visibility;
                    break;

                case SurfaceWindChunkDecoder.SurfaceWindParameterName:
                    decodedTaf.SurfaceWind = decodedEntity as SurfaceWind;
                    break;

                case TemperatureChunkDecoder.MaximumTemperatureParameterName:
                    decodedTaf.MaximumTemperature = decodedEntity as Temperature;
                    break;

                case TemperatureChunkDecoder.MinimumTemperatureParameterName:
                    decodedTaf.MinimumTemperature = decodedEntity as Temperature;
                    break;

                default:
                    throw new TafChunkDecoderException(TafChunkDecoderException.Messages.UnknownEntity + decodedEntity.ToString());
            }
        }

        private AbstractEntity InstantiateEntity(string entityName)
        {
            switch (entityName)
            {
                case WeatherChunkDecoder.WeatherPhenomenonParameterName:
                    return new WeatherPhenomenon();

                case TemperatureChunkDecoder.MinimumTemperatureParameterName:
                case TemperatureChunkDecoder.MaximumTemperatureParameterName:
                    return new Temperature();

                case CloudChunkDecoder.CloudsParameterName:
                    return new CloudLayer();

                case SurfaceWindChunkDecoder.SurfaceWindParameterName:
                    return new SurfaceWind();

                case VisibilityChunkDecoder.VisibilityParameterName:
                    return new Visibility();

                default:
                    throw new TafChunkDecoderException(TafChunkDecoderException.Messages.UnknownEntity + entityName);
            }
        }

        /// <summary>
        /// Look recursively for probability (PROBnn) attributes and embed a new evolution object one level deeper for each
        /// </summary>
        /// <param name="evolution"></param>
        /// <param name="chunk"></param>
        /// <param name="decodedTaf"></param>
        /// <returns></returns>
        private string ProbabilityChunkDecoder(Evolution evolution, string chunk, DecodedTaf decodedTaf)
        {
            var found = Regex.Matches(chunk, ProbabilityPattern).Cast<Match>().ToList();

            if (found.Count < 1)
            {
                return chunk;
            }

            var probability = found[0].Groups[1].Value.Trim();
            var type = found[0].Groups[2].Value.Trim();
            var period = found[0].Groups[3].Value.Trim();
            var remaining = found[0].Groups[4].Value.Trim();

            if (probability.StartsWith("PROB"))
            {
                evolution.Probability = probability;
                var embeddedEvolutionPeriodArray = period.Split('/');
                var embeddedEvolution = new Evolution()
                {
                    Type = !string.IsNullOrEmpty(type) ? type : Probability,
                    FromDay = Convert.ToInt32(embeddedEvolutionPeriodArray[0].Substring(0, 2)),
                    FromTime = embeddedEvolutionPeriodArray[0].Substring(2, 2) + ":00 UTC",
                    ToDay = Convert.ToInt32(embeddedEvolutionPeriodArray[1].Substring(0, 2)),
                    ToTime = embeddedEvolutionPeriodArray[1].Substring(2, 2) + ":00 UTC",
                };

                evolution.Evolutions.Add(embeddedEvolution);
                // recurse on the remaining chunk to extract the weather elements it contains
                chunk = ParseEntitiesChunk(evolution, remaining, decodedTaf);
            }

            return string.Empty;
        }
    }
}