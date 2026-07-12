using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Taf.Decoder.Entity;

namespace Taf.Decoder.ChunkDecoder
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
                Remaining = ConsumeOneChunk(remainingTaf);
                return;
            }

            var evolutionType = found[1].Value.Trim();
            var evolutionPeriod = found[2].Value.Trim();
            var evolution = new Evolution() { Type = evolutionType };
            SetProbabilityFromRemaining(evolution, newRemainingTaf);
            SetEvolutionPeriod(evolution, evolutionType, evolutionPeriod);

            Remaining = ParseEntitiesChunk(evolution, found[3].Value, decodedTaf);
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
            var remainingEvolutions = chunk;
            var tries = 0;

            foreach (var chunkDecoder in _decoderChain)
            {
                remainingEvolutions = TryParseEntity(
                    chunkDecoder,
                    evolution,
                    remainingEvolutions,
                    decodedTaf,
                    chunk,
                    ref tries);
            }

            return remainingEvolutions;
        }

        private string TryParseEntity(
            TafChunkDecoder chunkDecoder,
            Evolution evolution,
            string remainingEvolutions,
            DecodedTaf decodedTaf,
            string originalChunk,
            ref int tries)
        {
            try
            {
                remainingEvolutions = ProbabilityChunkDecoder(evolution, remainingEvolutions, decodedTaf);
                _withCavok = false;

                var decoded = chunkDecoder.Parse(remainingEvolutions, _withCavok);
                ApplyDecodedResult(evolution, decodedTaf, decoded, originalChunk, remainingEvolutions);
                return (string)decoded[TafDecoder.RemainingTafKey];
            }
            catch (TafChunkDecoderException)
            {
                tries++;
                return HandleEntityFailure(tries, remainingEvolutions, originalChunk);
            }
        }

        private string HandleEntityFailure(int tries, string remainingEvolutions, string originalChunk)
        {
            if (tries != _decoderChain.Count)
            {
                return remainingEvolutions;
            }

            if (IsStrict)
            {
                throw new TafChunkDecoderException(originalChunk, remainingEvolutions, TafChunkDecoderException.Messages.EvolutionInformationBadFormat, this);
            }

            return ConsumeOneChunk(remainingEvolutions);
        }

        private void ApplyDecodedResult(
            Evolution evolution,
            DecodedTaf decodedTaf,
            Dictionary<string, object> decoded,
            string originalChunk,
            string remainingEvolutions)
        {
            var result = decoded[TafDecoder.ResultKey] as Dictionary<string, object>;
            if (result == null)
            {
                throw new TafChunkDecoderException(
                    originalChunk,
                    remainingEvolutions,
                    TafChunkDecoderException.Messages.WeatherEvolutionBadFormat,
                    this);
            }

            if (result.Count == 0)
            {
                throw new TafChunkDecoderException(
                    originalChunk,
                    remainingEvolutions,
                    TafChunkDecoderException.Messages.WeatherEvolutionBadFormat,
                    this);
            }

            var entityName = GetEntityName(result);
            if (string.IsNullOrEmpty(entityName))
            {
                throw new TafChunkDecoderException(
                    originalChunk,
                    remainingEvolutions,
                    TafChunkDecoderException.Messages.WeatherEvolutionBadFormat,
                    this);
            }

            var entity = result[entityName];

            if (entity == null && entityName != VisibilityChunkDecoder.VisibilityParameterName)
            {
                throw new TafChunkDecoderException(
                    originalChunk,
                    remainingEvolutions,
                    TafChunkDecoderException.Messages.WeatherEvolutionBadFormat,
                    this);
            }

            if (IsTemperature(entityName))
            {
                AddEvolution(decodedTaf, evolution, result, TemperatureChunkDecoder.MaximumTemperatureParameterName);
                AddEvolution(decodedTaf, evolution, result, TemperatureChunkDecoder.MinimumTemperatureParameterName);
                return;
            }

            AddEvolution(decodedTaf, evolution, result, entityName);
        }

        private string GetEntityName(Dictionary<string, object> result)
        {
            var entityName = result.Keys.FirstOrDefault();
            if (string.IsNullOrEmpty(entityName))
            {
                return null;
            }

            if (entityName == VisibilityChunkDecoder.CavokParameterName)
            {
                _withCavok = (bool)result[entityName];
                return VisibilityChunkDecoder.VisibilityParameterName;
            }

            return entityName;
        }

        private static bool IsTemperature(string entityName)
        {
            return entityName == TemperatureChunkDecoder.MaximumTemperatureParameterName
                || entityName == TemperatureChunkDecoder.MinimumTemperatureParameterName;
        }

        private void AddEvolution(DecodedTaf decodedTaf, Evolution evolution, Dictionary<string, object> result, string entityName)
        {
            // clone the evolution entity
            var newEvolution = evolution.Clone() as Evolution;
            if (newEvolution == null)
            {
                throw new TafChunkDecoderException(TafChunkDecoderException.Messages.UnknownEntity + entityName);
            }

            // add the new entity to it
            newEvolution.Entity = result[entityName];

            if (entityName == VisibilityChunkDecoder.VisibilityParameterName && _withCavok)
            {
                newEvolution.Cavok = true;
            }

            // get the original entity from the decoded taf or a new one decoded taf doesn't contain it yet
            var property = typeof(DecodedTaf).GetProperty(entityName);
            if (property == null)
            {
                throw new TafChunkDecoderException(TafChunkDecoderException.Messages.UnknownEntity + entityName);
            }

            var decodedEntity = property.GetValue(decodedTaf) as AbstractEntity;

            if (decodedEntity == null || entityName == CloudChunkDecoder.CloudsParameterName || entityName == WeatherChunkDecoder.WeatherPhenomenonParameterName)
            {
                // that entity is not in the decoded_taf yet, or it's a cloud layer which is a special case
                decodedEntity = InstantiateEntity(entityName);
            }

            decodedEntity.Evolutions = decodedEntity.Evolutions ?? new List<Evolution>();

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

        private static void SetProbabilityFromRemaining(Evolution evolution, string remainingTaf)
        {
            if (remainingTaf.StartsWith("PROB"))
            {
                evolution.Probability = remainingTaf.Trim();
            }
        }

        private static void SetEvolutionPeriod(Evolution evolution, string evolutionType, string evolutionPeriod)
        {
            if (evolutionType == "BECMG" || evolutionType == "TEMPO")
            {
                SetBoundedPeriod(evolution, evolutionPeriod);
                return;
            }

            evolution.FromDay = Convert.ToInt32(evolutionPeriod.Substring(0, 2));
            evolution.FromTime = evolutionPeriod.Substring(2, 2) + ':' + evolutionPeriod.Substring(4, 2) + " UTC";
        }

        private static void SetBoundedPeriod(Evolution evolution, string evolutionPeriod)
        {
            var period = evolutionPeriod.Split('/');
            evolution.FromDay = Convert.ToInt32(period[0].Substring(0, 2));
            evolution.FromTime = period[0].Substring(2, 2) + ":00 UTC";
            evolution.ToDay = Convert.ToInt32(period[1].Substring(0, 2));
            evolution.ToTime = period[1].Substring(2, 2) + ":00 UTC";
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
                var embeddedEvolution = new Evolution()
                {
                    Type = !string.IsNullOrEmpty(type) ? type : Probability,
                };
                SetBoundedPeriod(embeddedEvolution, period);

                evolution.Evolutions = evolution.Evolutions ?? new List<Evolution>();
                evolution.Evolutions.Add(embeddedEvolution);
                chunk = ParseEntitiesChunk(evolution, remaining, decodedTaf);
            }

            return string.Empty;
        }
    }
}