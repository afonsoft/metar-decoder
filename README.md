# METAR and TAF Decoder for C#

[![GitHub license](https://img.shields.io/github/license/afonsoft/metar-decoder)](https://github.com/afonsoft/metar-decoder/blob/main/LICENSE)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=code_smells)](https://sonarcloud.io/dashboard?id=afonsoft_metar-decoder)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=bugs)](https://sonarcloud.io/dashboard?id=afonsoft_metar-decoder)
![GitHub top language](https://img.shields.io/github/languages/top/afonsoft/metar-decoder)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=alert_status)](https://sonarcloud.io/dashboard?id=afonsoft_metar-decoder)
[![GitHub issues](https://img.shields.io/github/issues/afonsoft/metar-decoder)](https://github.com/afonsoft/metar-decoder/issues)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=afonsoft_metar-decoder)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=coverage)](https://sonarcloud.io/summary/new_code?id=afonsoft_metar-decoder)
[![GitHub all releases](https://img.shields.io/github/downloads/afonsoft/metar-decoder/total)](https://github.com/afonsoft/metar-decoder/releases)

> **[Leia em Portugues (pt-BR)](README.pt-br.md)**

## Documentation / Documentacao

- **[Usage Guide (English)](docs/en-US/usage-guide.md)** - Complete guide with examples
- **[API Reference (English)](docs/en-US/api-reference.md)** - All classes, properties and methods
- **[Guia de Uso (Portugues)](docs/pt-BR/guia-de-uso.md)** - Guia completo com exemplos
- **[Referencia da API (Portugues)](docs/pt-BR/referencia-api.md)** - Todas as classes, propriedades e metodos

## Project Description

This repository contains a .NET library for decoding METAR (Meteorological Aerodrome Report) and TAF (Terminal Aerodrome Forecast) strings. Both libraries are compatible with .NET Standard 2.0, .NET 8.0, .NET 10.0, and .NET Framework 4.8, allowing use in .NET Core and .NET Framework projects.

### METAR Decoder

The METAR decoder is a robust tool for parsing and interpreting raw METAR weather reports. METAR is a standardized format by the International Civil Aviation Organization (ICAO) for reporting weather information, widely used by pilots and meteorologists. The library processes METAR strings and transforms them into a structured `DecodedMetar` object containing all decoded meteorological properties such as surface wind, visibility, runway visual range, present weather, cloud layers, air temperature, dew point temperature, and pressure.

### TAF Decoder

The TAF decoder is a .NET library for decoding TAF (Terminal Aerodrome Forecast) messages, which are weather forecasts for aerodromes. Like METAR, the TAF format is highly standardized by ICAO and is crucial for flight planning. The library parses TAF strings and converts them into a `DecodedTaf` object, which includes information such as report type, ICAO code, origin date and time, forecast period, surface wind, visibility, weather phenomena, cloud layers, and minimum/maximum temperatures. The TAF decoder can also interpret "evolutions" (forecast changes over time), such as `BECMG` (becoming) and `TEMPO` (temporary).

Both decoders offer "strict" and "non-strict" parsing modes. In non-strict mode, parsing continues even when format errors are encountered, and exceptions are logged in the `DecodingExceptions` property. Numeric values with units (such as speed, distance, and pressure) are encapsulated in `Value` objects, allowing flexible unit conversions.

This project is largely based on the implementations of [SafranCassiopee/csharp-metar-decoder](https://github.com/SafranCassiopee/csharp-metar-decoder) and [SafranCassiopee/csharp-taf-decoder](https://github.com/SafranCassiopee/csharp-taf-decoder).

## Project Status

**Active and Under Development** - With modern CI/CD pipelines

### Recent Changes

- **Test Coverage ~98%** - 421 unit tests with comprehensive coverage
- **DatetimeChunkDecoder Fix** - Day/month rollover bug fix
- **RTD Support** - Full support for TAF reports with "Report Delayed"
- **.NET 10.0** - Compatibility with the latest .NET version
- **Modern Workflows** - Automated CI/CD with GitHub Actions

## CI/CD and Workflows

This project uses modern GitHub Actions pipelines to ensure quality and automation:

### Available Workflows

- **CI Build & Test** (`ci-build-test.yml`) - Complete continuous integration pipeline
  - Automated build for .NET 8.0
  - Unit tests with coverage
  - Security scans and performance tests
  - Automatic PR creation

- **Code Quality** (`code-quality.yml`) - Code quality analysis
  - Qodana analysis
  - SonarQube integration
  - Snyk security scanning
  - Quality metrics

- **Security Scan** (`security-scan.yml`) - Security scans
  - CodeQL analysis
  - Vulnerability scanning
  - Automatic weekly scans

- **Publish NuGet** (`publish-all.yml`) - Automated publishing
  - Publishing to GitHub Packages
  - Publishing to NuGet.org
  - Automatic release creation

- **Auto Dependency Update** (`auto-pr-from-main.yml`) - Automatic updates
  - Check for outdated dependencies
  - Automatic security updates
  - Automatic PRs for updates

### Quality Badges

[![CI/CD Pipeline](https://github.com/afonsoft/metar-decoder/actions/workflows/ci-build-test.yml/badge.svg)](https://github.com/afonsoft/metar-decoder/actions/workflows/ci-build-test.yml)
[![Code Quality](https://github.com/afonsoft/metar-decoder/actions/workflows/code-quality.yml/badge.svg)](https://github.com/afonsoft/metar-decoder/actions/workflows/code-quality.yml)
[![Security Scan](https://github.com/afonsoft/metar-decoder/actions/workflows/security-scan.yml/badge.svg)](https://github.com/afonsoft/metar-decoder/actions/workflows/security-scan.yml)

## NuGet Packages

Official NuGet packages are available for easy integration into your projects:

| Package | Version | NuGet |
| ------- | ------- | ----- |
| [Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder/) | 1.0.9 | [![NuGet version](https://badge.fury.io/nu/Metar.Decoder.svg)](https://badge.fury.io/nu/Metar.Decoder) |
| [Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder/) | 1.0.7 | [![NuGet version](https://badge.fury.io/nu/Taf.Decoder.svg)](https://badge.fury.io/nu/Taf.Decoder) |

## Prerequisites

This library is compatible with multiple .NET versions:

- **.NET Standard 2.0** - Maximum compatibility
- **.NET 8.0** - Recommended LTS
- **.NET 10.0** - Latest version
- **.NET Framework 4.8** - Legacy support

## Installation

### NuGet Package Manager (recommended)

In the Package Manager Console in Visual Studio:

```shell
Install-Package Metar.Decoder
Install-Package Taf.Decoder
```

### .NET CLI

```shell
dotnet add package Metar.Decoder
dotnet add package Taf.Decoder
```

### PackageReference

```xml
<PackageReference Include="Metar.Decoder" Version="1.0.9" />
<PackageReference Include="Taf.Decoder" Version="1.0.7" />
```

Add a reference to the library and then add the following `using` directives:

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;
```

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;
```

### Manual Installation

Download the latest version from [GitHub Releases](https://github.com/afonsoft/metar-decoder/releases).

Extract the contents wherever you want in your project. The library itself is in the `Metar.Decoder/` and `Taf.Decoder/` directories; the other directories are not required for the library to function.

## Quick Start

### METAR Decoder

Instantiate the decoder and run it on a METAR string. The returned object is a `DecodedMetar` object from which you can retrieve all decoded meteorological properties.

All values that have a unit are based on the `Value` object, which provides the `ActualValue` and `ActualUnit` properties.

See the [`DecodedMetar`](src/Metar.Decoder/Entity/DecodedMetar.cs) class for the resulting object structure.

```csharp
var d = MetarDecoder.ParseWithMode("METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03");

// Context information
Console.WriteLine($"Valid: {d.IsValid}"); // true
Console.WriteLine($"Raw METAR: {d.RawMetar}");
Console.WriteLine($"Type: {d.Type}"); // MetarType.METAR
Console.WriteLine($"ICAO: {d.ICAO}"); // "LFPO"
Console.WriteLine($"Day: {d.Day}"); // 23
Console.WriteLine($"Time: {d.Time}"); // "10:27 UTC"
Console.WriteLine($"Status: {d.Status}"); // "AUTO"

// Surface Wind
var sw = d.SurfaceWind;
Console.WriteLine($"Wind - Mean Direction: {sw.MeanDirection.ActualValue}"); // 240
Console.WriteLine($"Wind - Mean Speed: {sw.MeanSpeed.ActualValue} {sw.MeanSpeed.ActualUnit}"); // 4 MeterPerSecond
Console.WriteLine($"Wind - Speed Variations: {sw.SpeedVariations.ActualValue}"); // 9

// Visibility
var v = d.Visibility;
Console.WriteLine($"Prevailing Visibility: {v.PrevailingVisibility.ActualValue} {v.PrevailingVisibility.ActualUnit}"); // 2500 Meter
Console.WriteLine($"Minimum Visibility: {v.MinimumVisibility.ActualValue}"); // 1000
Console.WriteLine($"Minimum Visibility Direction: {v.MinimumVisibilityDirection}"); // "NW"

// Runway Visual Range (RVR)
var rvr = d.RunwaysVisualRange;
Console.WriteLine($"RVR Runway 32: {rvr[0].VisualRange.ActualValue}"); // 400
Console.WriteLine($"RVR Runway 08C: {rvr[1].VisualRange.ActualValue}"); // 4

// Present Weather
var pw = d.PresentWeather;
Console.WriteLine($"Present Weather 1: {pw[0].IntensityProximity}{string.Join("", pw[0].Characteristics)}{string.Join("", pw[0].Types)}"); // "+FZRA"

// Clouds
var cld = d.Clouds;
Console.WriteLine($"Clouds - Amount: {cld[0].Amount}"); // FEW
Console.WriteLine($"Clouds - Base Height: {cld[0].BaseHeight.ActualValue} {cld[0].BaseHeight.ActualUnit}"); // 1500 Feet

// Temperature
Console.WriteLine($"Air Temperature: {d.AirTemperature.ActualValue} {d.AirTemperature.ActualUnit}"); // 17 DegreeCelsius
Console.WriteLine($"Dew Point Temperature: {d.DewPointTemperature.ActualValue}"); // 10

// Pressure
Console.WriteLine($"Pressure: {d.Pressure.ActualValue} {d.Pressure.ActualUnit}"); // 1009 HectoPascal

// Recent Weather
var rw = d.RecentWeather;
Console.WriteLine($"Recent Weather - Characteristics: {rw.Characteristics}"); // "FZ"
Console.WriteLine($"Recent Weather - Types: {string.Join(", ", rw.Types)}"); // "RA"

// Windshear
Console.WriteLine($"Windshear All Runways: {d.WindshearAllRunways}");
Console.WriteLine($"Windshear Runways: {string.Join(", ", d.WindshearRunways)}"); // "03"
```

### TAF Decoder

Instantiate the decoder and run it on a TAF string. The returned object is a `DecodedTaf` object from which you can retrieve all decoded forecast properties.

See the [`DecodedTaf`](src/Taf.Decoder/Entity/DecodedTaf.cs) class for the resulting object structure.

#### RTD Support (Report Delayed)

The decoder now supports TAF reports marked as "RTD" (Report Delayed), which indicate delayed reports:

```csharp
string rtdTaf = "RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 TEMPO 1905/1907 2000 BR BKN003 BECMG 1907/1909 9000 NSW FEW002 PROB40 1909/1911 0400 FZFG BKN002=";
var decoder = new TafDecoder();
var result = decoder.Parse(rtdTaf);

Console.WriteLine($"Type: {result.Type}"); // Output: RTD
Console.WriteLine($"ICAO: {result.Icao}"); // Output: EKEB
Console.WriteLine($"Valid: {result.IsValid}"); // Output: True
```

#### Full TAF Usage Example

```csharp
var d = TafDecoder.ParseWithMode("TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025 TX12/0816Z TN04/0807Z");

// Context information
Console.WriteLine($"Valid: {d.IsValid}");
Console.WriteLine($"Raw TAF: {d.RawTaf}");
Console.WriteLine($"Type: {d.Type}"); // Can be: TAF, TAFAMD, TAFCOR, RTD
Console.WriteLine($"ICAO: {d.Icao}");
Console.WriteLine($"Day: {d.Day}");
Console.WriteLine($"Time: {d.Time}");

// Forecast Period
var fp = d.ForecastPeriod;
Console.WriteLine($"Forecast Period - From Day: {fp.FromDay}");
Console.WriteLine($"Forecast Period - From Hour: {fp.FromHour}");
Console.WriteLine($"Forecast Period - To Day: {fp.ToDay}");
Console.WriteLine($"Forecast Period - To Hour: {fp.ToHour}");

// Surface Wind
var swTaf = d.SurfaceWind;
Console.WriteLine($"TAF Wind - Mean Direction: {swTaf.MeanDirection.ActualValue}");
Console.WriteLine($"TAF Wind - Mean Speed: {swTaf.MeanSpeed.ActualValue} {swTaf.MeanSpeed.ActualUnit}");

// Visibility
var vTaf = d.Visibility;
Console.WriteLine($"TAF Prevailing Visibility: {vTaf.ActualVisibility.ActualValue} {vTaf.ActualVisibility.ActualUnit}");
Console.WriteLine($"TAF CAVOK: {d.Cavok}");

// Clouds
var cldTaf = d.Clouds;
if (cldTaf.Count > 0)
{
    Console.WriteLine($"TAF Clouds - Amount: {cldTaf[0].Amount}");
    Console.WriteLine($"TAF Clouds - Base Height: {cldTaf[0].BaseHeight.ActualValue} {cldTaf[0].BaseHeight.ActualUnit}");
}

// Temperatures (Minimum and Maximum)
var minTemp = d.MinimumTemperature;
if (minTemp != null)
{
    Console.WriteLine($"Minimum Temperature: {minTemp.TemperatureValue.ActualValue} {minTemp.TemperatureValue.ActualUnit} on day {minTemp.Day} at {minTemp.Hour}Z");
}
var maxTemp = d.MaximumTemperature;
if (maxTemp != null)
{
    Console.WriteLine($"Maximum Temperature: {maxTemp.TemperatureValue.ActualValue} {maxTemp.TemperatureValue.ActualUnit} on day {maxTemp.Day} at {maxTemp.Hour}Z");
}

// Weather Phenomena
var wpTaf = d.WeatherPhenomenons;
if (wpTaf.Count > 0)
{
    Console.WriteLine($"TAF Weather Phenomenon: {wpTaf[0].IntensityProximity}{string.Join("", wpTaf[0].Characteristics)}{string.Join("", wpTaf[0].Types)}");
}

// Evolutions (BECMG, TEMPO, etc.)
foreach (var evolution in d.Evolutions)
{
    Console.WriteLine($"Evolution: {evolution.Type} from {evolution.FromDay}{evolution.FromHour}Z to {evolution.ToDay}{evolution.ToHour}Z");
}
```

#### Supported TAF Report Types

| Type | Description | Example |
|------|-------------|---------|
| `TAF` | Standard TAF report | `TAF LEMD 080500Z...` |
| `TAFAMD` | Amended TAF report | `TAF AMD LEMD 080500Z...` |
| `TAFCOR` | Corrected TAF report | `TAF COR LEMD 080500Z...` |
| `RTD` | Delayed TAF report | `RTD EKEB 190416Z...` |

### About Value Objects

In the example above, it is assumed that all requested parameters are available. In the real world, some fields are not mandatory, so it is important to check if the `Value` object (containing both the value and its unit) is not null before using it.

Here is an example:

```csharp
var dew_point = d.DewPointTemperature;
if (dew_point == null)
{
    dew_point = new Value(999, Value.Unit.DegreeCelsius);
}

Console.WriteLine(dew_point.ActualValue);
Console.WriteLine(dew_point.ActualUnit);
```

`Value` objects also contain their unit, which can be accessed with the `ActualUnit` property. When accessing the `ActualValue` property, you get the value in this unit.

If you want to get the value directly in another unit, you can call `GetConvertedValue(unit)`. Supported values are speed, distance, and pressure.

Here are all available units for conversion:

```csharp
// Speed units:
// Value.Unit.MeterPerSecond
// Value.Unit.KilometerPerHour
// Value.Unit.Knot

// Distance units:
// Value.Unit.Meter
// Value.Unit.Feet
// Value.Unit.StatuteMile

// Pressure units:
// Value.Unit.HectoPascal
// Value.Unit.MercuryInch

// Using real-time conversion
var distance_in_sm = visibility.GetConvertedValue(Value.Unit.StatuteMile);
var speed_kph = speed.GetConvertedValue(Value.Unit.KilometerPerHour);
```

### About Parsing Errors

When an unexpected format is found for a part of the METAR/TAF, the parsing error is logged in the `DecodedMetar` or `DecodedTaf` object itself.

All parsing errors for a METAR/TAF can be accessed through the `DecodingExceptions` property.

By default, parsing will continue when an incorrect format is found. However, the parser also offers a "strict" mode where parsing stops as soon as a non-compliance is detected. The mode can be set globally for a `MetarDecoder`/`TafDecoder` object, or just once, as shown in this example:

```csharp
var decoder = new MetarDecoder();

// Change global parsing mode to "strict"
decoder.SetStrictParsing(true);

// This parse will be done in strict mode
decoder.Parse("...");

// But this will ignore the global mode and parse in non-strict mode
decoder.ParseNotStrict("...");

// Change global parsing mode to "non-strict"
decoder.SetStrictParsing(false);

// This parse will be done in non-strict mode
decoder.Parse("...");

// But this will ignore the global mode and parse in strict mode
decoder.ParseStrict("...");
```

### About Parsing Errors (Advanced)

In non-strict mode, it is possible to get a parsing error for a given chunk decoder while still getting the decoded information for that chunk. How is this possible?

This happens because non-strict mode not only continues decoding where there is an error but also retries parsing on the "next chunk" (based on the whitespace separator). However, all errors from the first attempt will remain logged, even if the second attempt was successful.

For example, if you have the chunk `AAA 12003KPH ...` fed to the `SurfaceWind` chunk decoder, it will fail on `AAA`, then try to decode `12003KPH` and succeed. The first exception for the surface wind decoder will be kept, but the `SurfaceWind` object will be populated with some information.

This does not apply to strict mode, as parsing is stopped at the first parsing error in that case.

## Contributing

### Development Process

1. **Create a branch** from `main`:
   ```bash
   git checkout -b feature/your-feature
   ```

2. **Make your changes** following best practices

3. **Automated workflows** will run:
   - **CI Build & Test** - Validates your code
   - **Code Quality** - Analyzes quality
   - **Security Scan** - Checks security

4. **Automatic Pull Request**: If on `feature/*`, `bug/*`, or `hotfix/*` branches, a PR will be created automatically to `main`

5. **Review and Merge**: After approval, your code will be merged

## Repository Structure

```
.
├── CHANGELOG.md                # History of all notable changes in the project.
├── EAF.ico                     # Project icon.
├── EAF.png                     # Project image.
├── LICENSE                     # Project license file.
├── MetarDecoder.sln            # Main Visual Studio solution for the project.
├── README.md                   # This project documentation file (en-US).
├── README.pt-br.md             # Project documentation in Portuguese (pt-BR).
├── docs/                       # Documentation with guides and API reference.
│   ├── en-US/                  # English documentation.
│   │   ├── usage-guide.md      # Complete usage guide with examples.
│   │   └── api-reference.md    # API reference for all classes and methods.
│   └── pt-BR/                  # Portuguese documentation.
│       ├── guia-de-uso.md      # Guia completo de uso com exemplos.
│       └── referencia-api.md   # Referencia da API para todas as classes e metodos.
├── src/                        # Main source code of the project.
│   ├── Metar.Decoder/          # Library project for METAR decoding.
│   │   ├── ChunkDecoder/       # Individual METAR "chunk" decoders.
│   │   │   ├── Abstract/       # Abstract classes and interfaces.
│   │   │   ├── CloudChunkDecoder.cs
│   │   │   ├── DatetimeChunkDecoder.cs
│   │   │   ├── IcaoChunkDecoder.cs
│   │   │   ├── PresentWeatherChunkDecoder.cs
│   │   │   ├── PressureChunkDecoder.cs
│   │   │   ├── RecentWeatherChunkDecoder.cs
│   │   │   ├── ReportStatusChunkDecoder.cs
│   │   │   ├── ReportTypeChunkDecoder.cs
│   │   │   ├── RunwayVisualRangeChunkDecoder.cs
│   │   │   ├── SurfaceWindChunkDecoder.cs
│   │   │   ├── TemperatureChunkDecoder.cs
│   │   │   ├── TrendChunkDecoder.cs
│   │   │   ├── VisibilityChunkDecoder.cs
│   │   │   └── WindShearChunkDecoder.cs
│   │   ├── Entity/             # Entity classes representing decoded METAR data.
│   │   │   ├── CloudLayer.cs
│   │   │   ├── DecodedMetar.cs
│   │   │   ├── PresentWeather.cs
│   │   │   ├── RunwayVisualRange.cs
│   │   │   ├── SurfaceWind.cs
│   │   │   ├── Value.cs
│   │   │   ├── Visibility.cs
│   │   │   └── WeatherPhenomenon.cs
│   │   └── Exception/          # Custom exception classes.
│   │       └── MetarChunkDecoderException.cs
│   └── Taf.Decoder/            # Library project for TAF decoding.
│       ├── ChunkDecoder/       # Individual TAF "chunk" decoders.
│       ├── Entity/             # Entity classes representing decoded TAF data.
│       └── Exception/          # Custom exception classes.
├── tests/                      # Unit tests.
│   ├── Metar.Decoder.Tests/    # Tests for METAR decoder.
│   └── Taf.Decoder.Tests/      # Tests for TAF decoder.
└── .github/workflows/          # GitHub Actions workflows.
```

## Star History

See the [star history chart](https://www.star-history.com/?repos=afonsoft%2Fmetar-decoder&type=date&legend=top-left) for this repository.

## StarMapper

[![StarMapper Map](https://starmapper.bruniaux.com/afonsoft/metar-decoder/opengraph-image)](https://starmapper.bruniaux.com/afonsoft/metar-decoder)

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
