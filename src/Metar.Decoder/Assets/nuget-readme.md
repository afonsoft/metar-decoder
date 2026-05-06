# Metar.Decoder

A .NET library to decode METAR (Meteorological Aerodrome Report) strings into structured objects.

Compatible with **.NET Standard 2.0**, **.NET 8.0**, **.NET 10.0**, and **.NET Framework 4.8**.

---

## Quick Start

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z 24004G09KT 2500 +FZRA FEW015 17/10 Q1009");

if (decoded.IsValid)
{
    Console.WriteLine($"ICAO: {decoded.ICAO}");                    // "LFPO"
    Console.WriteLine($"Wind: {decoded.SurfaceWind.MeanSpeed.ActualValue} {decoded.SurfaceWind.MeanSpeed.ActualUnit}");
    Console.WriteLine($"Visibility: {decoded.Visibility.PrevailingVisibility.ActualValue}m");
    Console.WriteLine($"Temperature: {decoded.AirTemperature.ActualValue}C");
    Console.WriteLine($"Pressure: {decoded.Pressure.ActualValue} {decoded.Pressure.ActualUnit}");
}
```

## Features

- **Full METAR parsing**: Report type, ICAO, datetime, surface wind, visibility, RVR, present weather, clouds, temperature, dew point, pressure, recent weather, windshear, trend, remarks
- **Strict and non-strict modes**: Continue parsing on errors or stop at first error
- **Unit conversion**: Built-in conversion between speed, distance, and pressure units
- **Null-safe**: All optional fields are nullable with safe access patterns
- **Cross-platform**: Works on Windows, Linux, macOS

## Decoded Properties

| Property | Description |
|----------|-------------|
| `ICAO` | ICAO 4-letter airport code |
| `SurfaceWind` | Wind direction, speed, gusts |
| `Visibility` | Prevailing and minimum visibility |
| `RunwaysVisualRange` | RVR per runway |
| `PresentWeather` | Current weather phenomena |
| `Clouds` | Cloud layers (amount, height, type) |
| `AirTemperature` | Air temperature |
| `DewPointTemperature` | Dew point temperature |
| `Pressure` | QNH pressure |
| `RecentWeather` | Recent weather |
| `WindshearRunways` | Runways with windshear |
| `TrendType` | Trend (NOSIG, BECMG, TEMPO) |
| `Remark` | Remarks section (after RMK) |
| `SeaLevelPressure` | SLP from remarks |

## Unit Conversion

```csharp
// Convert wind speed to knots
float knots = decoded.SurfaceWind.MeanSpeed.GetConvertedValue(Value.Unit.Knot);

// Convert visibility to feet
float feet = decoded.Visibility.PrevailingVisibility.GetConvertedValue(Value.Unit.Feet);

// Convert pressure to inHg
float inHg = decoded.Pressure.GetConvertedValue(Value.Unit.MercuryInch);
```

## Parsing Modes

```csharp
// Non-strict (default) - continues on errors
var decoded = MetarDecoder.ParseWithMode(rawMetar);

// Strict - stops at first error
var decoded = MetarDecoder.ParseWithMode(rawMetar, isStrict: true);

// Instance-based with global mode
var decoder = new MetarDecoder();
decoder.SetStrictParsing(true);
var result = decoder.Parse(rawMetar);
```

## Documentation & Source

- **Full Documentation**: [https://github.com/afonsoft/metar-decoder](https://github.com/afonsoft/metar-decoder)
- **Usage Guide**: [https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/usage-guide.md](https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/usage-guide.md)
- **API Reference**: [https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/api-reference.md](https://github.com/afonsoft/metar-decoder/blob/main/docs/en-US/api-reference.md)
- **Issues**: [https://github.com/afonsoft/metar-decoder/issues](https://github.com/afonsoft/metar-decoder/issues)
- **License**: MIT
