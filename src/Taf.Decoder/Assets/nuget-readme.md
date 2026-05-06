# Taf.Decoder

A .NET library to decode TAF (Terminal Aerodrome Forecast) strings into structured objects.

Compatible with **.NET Standard 2.0**, **.NET 8.0**, **.NET 10.0**, and **.NET Framework 4.8**.

---

## Quick Start

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var decoded = TafDecoder.ParseWithMode(
    "TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025 TX12/0816Z TN04/0807Z");

if (decoded.IsValid)
{
    Console.WriteLine($"ICAO: {decoded.Icao}");    // "LEMD"
    Console.WriteLine($"Wind: {decoded.SurfaceWind.MeanSpeed.ActualValue} {decoded.SurfaceWind.MeanSpeed.ActualUnit}");
    Console.WriteLine($"Visibility: {decoded.Visibility.ActualVisibility.ActualValue}m");
    Console.WriteLine($"Max Temp: {decoded.MaximumTemperature?.TemperatureValue.ActualValue}C");
    Console.WriteLine($"Min Temp: {decoded.MinimumTemperature?.TemperatureValue.ActualValue}C");
}
```

## Features

- **Full TAF parsing**: Report type, ICAO, datetime, forecast period, surface wind, visibility, weather, clouds, temperatures, evolutions
- **RTD Support**: Decodes "Report Delayed" TAF messages
- **Evolution handling**: Parses BECMG, TEMPO, and PROB forecast changes
- **Strict and non-strict modes**: Continue parsing on errors or stop at first error
- **Unit conversion**: Built-in conversion between speed, distance, and pressure units
- **Cross-platform**: Works on Windows, Linux, macOS

## Decoded Properties

| Property | Description |
|----------|-------------|
| `Icao` | ICAO 4-letter airport code |
| `Type` | Report type: TAF, TAFAMD, TAFCOR, RTD |
| `ForecastPeriod` | Valid period (FromDay/FromHour to ToDay/ToHour) |
| `SurfaceWind` | Wind direction, speed, gusts |
| `Visibility` | Prevailing visibility |
| `Cavok` | CAVOK indicator |
| `WeatherPhenomenons` | Weather phenomena |
| `Clouds` | Cloud layers (amount, height, type) |
| `MaximumTemperature` | Forecast maximum temperature with time |
| `MinimumTemperature` | Forecast minimum temperature with time |
| `Evolutions` | Forecast changes (BECMG, TEMPO, PROB) |

## Supported Report Types

| Type | Description |
|------|-------------|
| `TAF` | Standard TAF forecast |
| `TAFAMD` | Amended TAF forecast |
| `TAFCOR` | Corrected TAF forecast |
| `RTD` | Report Delayed |

## Evolutions (Forecast Changes)

```csharp
var decoded = TafDecoder.ParseWithMode(
    "TAF EGLL 080500Z 0806/0906 24015G25KT 9999 BKN040 " +
    "TEMPO 0806/0812 30018G30KT 3000 +SHRA BKN012CB " +
    "BECMG 0812/0814 25010KT");

foreach (var evo in decoded.Evolutions)
{
    Console.WriteLine($"{evo.Type}: {evo.FromDay}{evo.FromHour}Z-{evo.ToDay}{evo.ToHour}Z");
    if (evo.SurfaceWind != null)
        Console.WriteLine($"  Wind: {evo.SurfaceWind.MeanSpeed?.ActualValue} {evo.SurfaceWind.MeanSpeed?.ActualUnit}");
}
```

## Parsing Modes

```csharp
// Non-strict (default) - continues on errors
var decoded = TafDecoder.ParseWithMode(rawTaf);

// Strict - stops at first error
var decoded = TafDecoder.ParseWithMode(rawTaf, isStrict: true);

// Instance-based with global mode
var decoder = new TafDecoder();
decoder.SetStrictParsing(true);
var result = decoder.Parse(rawTaf);
```

## Documentation & Source

- **Full Documentation**: [https://github.com/afonsoft/metar-decoder](https://github.com/afonsoft/metar-decoder)
- **Usage Guide**: [https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/usage-guide.md](https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/usage-guide.md)
- **API Reference**: [https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/api-reference.md](https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/api-reference.md)
- **Issues**: [https://github.com/afonsoft/metar-decoder/issues](https://github.com/afonsoft/metar-decoder/issues)
- **License**: MIT
