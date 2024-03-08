using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using Taf.Decoder;
using Taf.Decoder.chunkdecoder;
using Taf.Decoder.entity;

namespace Taf.Decoder_tests.ChunkDecoder
{
    [TestFixture, Category("EvolutionChunkDecoder")]
    public class EvolutionChunkDecoderTest
    {
        private static readonly EvolutionChunkDecoder evoDecoder = new EvolutionChunkDecoder(true, false);

        //  Test parsing of evolution chunks
        [Test, TestCaseSource("Chunks")]
        public void TestParse(EvolutionChunkDecoderTester chunk)
        {
            var decodedTaf = TafDecoder.ParseStrict(chunk.Base);
            evoDecoder.IsStrict = chunk.Strict;
            evoDecoder.Parse(chunk.EvoChunk + " END", decodedTaf);

            var windEvolutions = decodedTaf?.SurfaceWind?.Evolutions;
            ClassicAssert.IsNotNull(windEvolutions, "No surface winds!");
            if (windEvolutions.Count == 0)
            {
                ClassicAssert.Fail("No wind evolution!");
            }
            // global evolution attributes (no point testing them in each evolution as they never change)
            ClassicAssert.AreEqual(chunk.Type, windEvolutions[0].Type);
            ClassicAssert.AreEqual(chunk.Probability, windEvolutions[0].Probability);
            ClassicAssert.AreEqual(chunk.FromDay, windEvolutions[0].FromDay);
            ClassicAssert.AreEqual(chunk.FromTime, windEvolutions[0].FromTime);
            ClassicAssert.AreEqual(chunk.ToDay, windEvolutions[0].ToDay);
            ClassicAssert.AreEqual(chunk.ToTime, windEvolutions[0].ToTime);
            if (!string.IsNullOrEmpty(chunk.Element.EmbEvolutionType))
            {
                // it's embedded in the second evolution
                ClassicAssert.LessOrEqual(2, windEvolutions.Count);
                var embEvolutions = windEvolutions[1].Evolutions;
                ClassicAssert.AreEqual(chunk.Element.EmbEvolutionType, embEvolutions[0].Type);
            }
            // surfaceWind attributes
            ClassicAssert.AreEqual(chunk.Element.WindDirection, (windEvolutions[0].Entity as SurfaceWind).MeanDirection.ActualValue);
            ClassicAssert.AreEqual(chunk.Element.WindSpeed, (windEvolutions[0].Entity as SurfaceWind).MeanSpeed.ActualValue);

            var visiEvolutions = decodedTaf.Visibility.Evolutions;
            ClassicAssert.IsNotNull(visiEvolutions);
            ClassicAssert.AreEqual(chunk.Element.Cavok, visiEvolutions[0].Cavok);
            if (!visiEvolutions[0].Cavok)
            {
                // cavok and visibility are mutually exclusive
                ClassicAssert.AreEqual(chunk.Element.Visibility, (visiEvolutions[0].Entity as Visibility).ActualVisibility.ActualValue);
                ClassicAssert.AreEqual(chunk.Element.Greater, (visiEvolutions[0].Entity as Visibility).Greater);
            }
            if (chunk.Element.WeatherPhenomena.Count > 0)
            {
                if (decodedTaf.WeatherPhenomenons.Count > 0)
                {
                    var proxyWeatherPhenomena = decodedTaf.WeatherPhenomenons;
                    var weatherPhenomena = proxyWeatherPhenomena[0].Evolutions;
                    var entity = weatherPhenomena[0].Entity as List<WeatherPhenomenon>;
                    ClassicAssert.AreEqual(chunk.Element.WeatherIntensity, entity[0].IntensityProximity);
                    ClassicAssert.AreEqual(chunk.Element.WeatherDesc, entity[0].Descriptor);
                    ClassicAssert.AreEqual(chunk.Element.WeatherPhenomena, entity[0].Phenomena);
                }
            }
            var clouds = decodedTaf.Clouds;
            if (chunk.Element.CloudsBaseHeight.HasValue)
            {
                // 1 instead of 0 because each evo is considered a new layer
                var cloudsEvolutions = clouds[1].Evolutions;
                ClassicAssert.AreEqual(chunk.Type, cloudsEvolutions[0].Type);
                var cloudsLayers = cloudsEvolutions[0].Entity as List<CloudLayer>;
                ClassicAssert.AreEqual(chunk.Element.CloudsAmount, cloudsLayers[0].Amount);
                ClassicAssert.AreEqual(chunk.Element.CloudsBaseHeight, cloudsLayers[0].BaseHeight.ActualValue);
            }
            if (chunk.Element.MinimumTemperatureValue.HasValue)
            {
                ClassicAssert.IsNotNull(decodedTaf.MinimumTemperature);
                ClassicAssert.IsNotNull(decodedTaf.MaximumTemperature);
                var minTemps = decodedTaf.MinimumTemperature.Evolutions;
                var maxTemps = decodedTaf.MaximumTemperature.Evolutions;
                ClassicAssert.AreEqual(chunk.Element.MinimumTemperatureValue, (minTemps[0].Entity as Temperature).TemperatureValue.ActualValue);
                ClassicAssert.AreEqual(chunk.Element.MaximumTemperatureValue, (maxTemps[0].Entity as Temperature).TemperatureValue.ActualValue);
            }
        }

        public static List<EvolutionChunkDecoderTester> Chunks => new List<EvolutionChunkDecoderTester>()
        {
            new EvolutionChunkDecoderTester() {
                Description = "1. Common cases",
                Strict = true,
                Base = "TAF KJFK 080500Z 0806/0910 23010KT 6 1/4SM BKN020",
                EvoChunk = "BECMG 0807/0810 23024KT P6SM +SHRA BKN025 TX08/0910Z TNM01/0904",
                Type = "BECMG",
                Probability = string.Empty,
                FromDay = 8,
                FromTime = "07:00 UTC",
                ToDay = 8,
                ToTime = "10:00 UTC",
                Element = new Element()
                {
                    WindDirection = 230,
                    WindSpeed = 24,
                    Visibility = 6,
                    Cavok = false,
                    Greater = true,
                    WeatherPhenomena = new List<string>() { "RA" },
                    WeatherIntensity = "+",
                    WeatherDesc = "SH",
                    CloudsAmount = CloudLayer.CloudAmount.BKN,
                    CloudsBaseHeight = 2500,
                    MinimumTemperatureValue = -1,
                    MaximumTemperatureValue = 8,
                    EmbEvolutionType = null,
                }
            },

            new EvolutionChunkDecoderTester()
            {
                Description = "2. Line starting with PROB",
                Strict = true,
                Base = "TAF KJFK 080500Z 0806/0910 23010KT 6 1/4SM BKN020",
                EvoChunk = "PROB40 TEMPO 0807/0810 23024KT CAVOK BKN025",
                Type = "TEMPO",
                Probability = "PROB40",
                FromDay = 8,
                FromTime = "07:00 UTC",
                ToDay = 8,
                ToTime = "10:00 UTC",
                Element = new Element()
                {
                    WindDirection = 230,
                    WindSpeed = 24,
                    Visibility = null,
                    Cavok = true,
                    Greater = false,
                    WeatherPhenomena = new List<string>(),
                    WeatherIntensity = string.Empty,
                    WeatherDesc = string.Empty,
                    CloudsAmount = CloudLayer.CloudAmount.BKN,
                    CloudsBaseHeight = 2500,
                    MinimumTemperatureValue = null,
                    MaximumTemperatureValue = null,
                    EmbEvolutionType = string.Empty,
                }
            },

            new EvolutionChunkDecoderTester()
            {
                Description = "3. Embedded evolutions",
                Strict = true,
                Base = "TAF KJFK 080500Z 0806/0910 23010KT",
                EvoChunk = "BECMG 0807/0810 23024KT CAVOK -RA PROB40 TEMPO 0808/0809 18020KT",
                Type = "BECMG",
                Probability = string.Empty,
                FromDay = 8,
                FromTime = "07:00 UTC",
                ToDay = 8,
                ToTime = "10:00 UTC",
                Element = new Element()
                {
                    WindDirection = 230,
                    WindSpeed = 24,
                    Visibility = null,
                    Cavok = true,
                    Greater = false,
                    WeatherPhenomena = new List<string>(),
                    WeatherIntensity = string.Empty,
                    WeatherDesc = string.Empty,
                    CloudsAmount = CloudLayer.CloudAmount.NULL,
                    CloudsBaseHeight = null,
                    MinimumTemperatureValue = null,
                    MaximumTemperatureValue = null,
                    EmbEvolutionType = "TEMPO",
                }
            },

            new EvolutionChunkDecoderTester()
            {
                Description = "4. SurfaceWind and visibility entities",
                Strict = false,
                Base = "TAF BAH KJFK 080500Z 0806/0910 TX10/0807Z TN05/0904Z",
                EvoChunk = "BECMG 0810/0812 27010KT 4000 -RA BKN025",
                Type = "BECMG",
                Probability = string.Empty,
                FromDay = 8,
                FromTime = "10:00 UTC",
                ToDay = 8,
                ToTime = "12:00 UTC",
                Element = new Element()
                {
                    WindDirection = 270,
                    WindSpeed = 10,
                    Visibility = 4000,
                    Cavok = false,
                    Greater = false,
                    WeatherPhenomena = new List<string>(),
                    WeatherIntensity = string.Empty,
                    WeatherDesc = string.Empty,
                    CloudsAmount = CloudLayer.CloudAmount.NULL,
                    CloudsBaseHeight = null,
                    MinimumTemperatureValue = null,
                    MaximumTemperatureValue = null,
                    EmbEvolutionType = null,
                }
            },

            new EvolutionChunkDecoderTester()
            {
                Description = "5. Drop a chunk that doesn't match with any decoder",
                Strict = false,
                Base = "TAF KJFK 081009Z 0810/0912 03017G28KT 9000 BKN020",
                EvoChunk = "FM081100 03018G27KT 6000 -SN OVC015 PROB40 0811/0912 AAA",
                Type = "FM",
                Probability = string.Empty,
                FromDay = 8,
                FromTime = "11:00 UTC",
                ToDay = null,
                ToTime = string.Empty,
                Element = new Element()
                {
                    WindDirection = 30,
                    WindSpeed = 18,
                    Visibility = 6000,
                    Cavok = false,
                    Greater = false,
                    WeatherPhenomena = new List<string>() { "SN" },
                    WeatherIntensity = "-",
                    WeatherDesc = string.Empty,
                    CloudsAmount = CloudLayer.CloudAmount.OVC,
                    CloudsBaseHeight = 1500,
                    MinimumTemperatureValue = null,
                    MaximumTemperatureValue = null,
                    EmbEvolutionType = null,
                }
            },

            new EvolutionChunkDecoderTester()
            {
                Description = "6. Trigger a ChunkDecoderException",
                Strict = true,
                Base = "TAF KJFK 081009Z 0810/0912 03017G28KT 9000 BKN020",
                EvoChunk = "FM081200 03018G27KT 7000 -SN OVC015 PROB40 0810/0910 BK025",
                Type = "FM",
                Probability = string.Empty,
                FromDay = 8,
                FromTime = "12:00 UTC",
                ToDay = null,
                ToTime = string.Empty,
                Element = new Element()
                {
                    WindDirection = 30,
                    WindSpeed = 18,
                    Visibility = 7000,
                    Cavok = false,
                    Greater = false,
                    WeatherPhenomena = new List<string>() { "SN" },
                    WeatherIntensity = "-",
                    WeatherDesc = string.Empty,
                    CloudsAmount = CloudLayer.CloudAmount.OVC,
                    CloudsBaseHeight = 1500,
                    MinimumTemperatureValue = null,
                    MaximumTemperatureValue = null,
                    EmbEvolutionType = null,
                }
            },
        };
    }

    public class EvolutionChunkDecoderTester
    {
        public string Description { get; set; }
        public bool Strict { get; set; }
        public string Base { get; set; }
        public string EvoChunk { get; set; }
        public string Type { get; set; }
        public string Probability { get; set; }
        public int? FromDay { get; set; }
        public string FromTime { get; set; }
        public int? ToDay { get; set; }
        public string ToTime { get; set; }
        public Element Element { get; set; }

        public override string ToString()
        {
            return $"{Description}"; ;
        }
    }

    public class Element
    {
        public int WindDirection { get; set; }
        public int WindSpeed { get; set; }
        public int? Visibility { get; set; }
        public bool Cavok { get; set; }
        public bool Greater { get; set; }
        public List<string> WeatherPhenomena { get; set; } = new List<string>();
        public string WeatherIntensity { get; set; }
        public string WeatherDesc { get; set; }
        public CloudLayer.CloudAmount CloudsAmount { get; set; }
        public int? CloudsBaseHeight { get; set; }
        public int? MinimumTemperatureValue { get; set; }
        public int? MaximumTemperatureValue { get; set; }
        public string EmbEvolutionType { get; set; }
    }
}