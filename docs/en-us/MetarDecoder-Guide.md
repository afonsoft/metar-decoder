# Metar.Decoder - Usage Guide (EN-US)

## Overview

**Metar.Decoder** is a .NET library for parsing METAR (Meteorological Aerodrome Report) strings into structured, strongly-typed objects. It supports multiple .NET frameworks: `.NET Standard 2.0`, `.NET 8.0`, `.NET 10.0`, and `.NET Framework 4.8`.

**Version:** 1.0.9

## Installation

```bash
dotnet add package Metar.Decoder
```

Or via NuGet Package Manager:

```
Install-Package Metar.Decoder
```

## Quick Start

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

var decoder = new MetarDecoder();
var metar = decoder.Parse("METAR SBGR 031000Z 35006KT 9999 FEW040 SCT100 27/18 Q1020");

Console.WriteLine($"ICAO: {metar.ICAO}");
Console.WriteLine($"Day: {metar.Day}");
Console.WriteLine($"Time: {metar.Time}");
Console.WriteLine($"Wind: {metar.SurfaceWind.MeanDirection.ActualValue} at {metar.SurfaceWind.MeanSpeed.ActualValue} {metar.SurfaceWind.MeanSpeed.ActualUnit}");
Console.WriteLine($"Visibility: {metar.Visibility.PrevailingVisibility.ActualValue} {metar.Visibility.PrevailingVisibility.ActualUnit}");
Console.WriteLine($"Temperature: {metar.AirTemperature.ActualValue} C");
Console.WriteLine($"Dew Point: {metar.DewPointTemperature.ActualValue} C");
Console.WriteLine($"Pressure: {metar.Pressure.ActualValue} {metar.Pressure.ActualUnit}");
```

## Parsing Modes

### Default Parsing
```csharp
var metar = decoder.Parse("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009");
```

### Strict Parsing
Stops at the first decoding error:
```csharp
var metar = decoder.ParseStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009");
```

### Non-Strict Parsing
Continues decoding even if errors are encountered:
```csharp
var metar = decoder.ParseNotStrict("METAR LFPO 231027Z 24004KT BADVALUE FEW020 17/10 Q1009");
// metar.IsValid == false, but other fields are still decoded
```

### Static Parsing
```csharp
var metar = MetarDecoder.ParseWithMode("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009", isStrict: false);
```

## Decoded Fields

### Report Type
```csharp
metar.Type // MetarType.METAR, MetarType.SPECI, MetarType.METAR_COR, MetarType.SPECI_COR
```

### ICAO Code
```csharp
metar.ICAO // "SBGR", "LFPO", "KJFK", etc.
```

### Date/Time
```csharp
metar.Day              // Day of month (1-31)
metar.Time             // "10:00 UTC"
metar.ObservationDateTime // DateTime object
```

### Report Status
```csharp
metar.Status // "AUTO", "NIL", or empty
```

### Surface Wind
```csharp
var wind = metar.SurfaceWind;
wind.MeanDirection.ActualValue   // Direction in degrees (0-360)
wind.MeanSpeed.ActualValue       // Speed value
wind.MeanSpeed.ActualUnit        // Unit.Knot, Unit.MeterPerSecond, Unit.KilometerPerHour
wind.SpeedVariations?.ActualValue // Gust speed (if present)
wind.VariableDirection           // true if VRB
wind.DirectionVariations         // [min, max] direction variation values
```

### Visibility
```csharp
var vis = metar.Visibility;
vis.PrevailingVisibility.ActualValue  // Main visibility value
vis.PrevailingVisibility.ActualUnit   // Unit.Meter or Unit.StatuteMile
vis.MinimumVisibility?.ActualValue    // Minimum visibility (if present)
vis.MinimumVisibilityDirection        // "NW", "NE", "S", etc.
vis.NDV                               // No Directional Variation flag
metar.Cavok                           // CAVOK flag
```

### Runway Visual Range (RVR)
```csharp
foreach (var rvr in metar.RunwaysVisualRange)
{
    Console.WriteLine($"Runway: {rvr.Runway}");
    Console.WriteLine($"Range: {rvr.VisualRange?.ActualValue}");
    Console.WriteLine($"Variable: {rvr.Variable}");
    if (rvr.Variable)
    {
        Console.WriteLine($"Min: {rvr.VisualRangeInterval[0].ActualValue}");
        Console.WriteLine($"Max: {rvr.VisualRangeInterval[1].ActualValue}");
    }
    Console.WriteLine($"Tendency: {rvr.PastTendency}"); // U (up), D (down), N (no change)
}
```

### Present Weather
```csharp
foreach (var pw in metar.PresentWeather)
{
    Console.WriteLine($"Intensity: {pw.IntensityProximity}"); // +, -, VC
    Console.WriteLine($"Descriptor: {pw.Characteristics}");    // TS, FZ, SH, etc.
    foreach (var type in pw.Types)
    {
        Console.WriteLine($"Phenomenon: {type}"); // RA, SN, FG, BR, etc.
    }
}
```

### Clouds
```csharp
foreach (var cloud in metar.Clouds)
{
    Console.WriteLine($"Amount: {cloud.Amount}");         // FEW, SCT, BKN, OVC, VV
    Console.WriteLine($"Height: {cloud.BaseHeight?.ActualValue} feet");
    Console.WriteLine($"Type: {cloud.Type}");             // CB, TCU, or NULL
}
```

### Temperature
```csharp
metar.AirTemperature.ActualValue       // Air temperature in Celsius
metar.DewPointTemperature.ActualValue  // Dew point in Celsius
```

### Pressure
```csharp
metar.Pressure.ActualValue   // Pressure value
metar.Pressure.ActualUnit    // Unit.HectoPascal (Q) or Unit.MercuryInch (A)
```

### Recent Weather
```csharp
var rw = metar.RecentWeather;
rw?.Characteristics  // Weather descriptor
rw?.Types            // Weather phenomena list
```

### Wind Shear
```csharp
metar.WindshearAllRunways  // true if WS ALL RWY
metar.WindshearRunways     // List of affected runways
```

### Trend Forecast (NOSIG/BECMG/TEMPO)
```csharp
metar.TrendType      // "NOSIG", "BECMG", or "TEMPO"
metar.TrendForecast  // Raw trend content
```

### Remarks (RMK)
```csharp
metar.Remark           // Raw remarks content
metar.SeaLevelPressure // Parsed SLPnnn value (if present)
```

## Value Conversion

The `Value` class supports unit conversion:

```csharp
var speed = metar.SurfaceWind.MeanSpeed;
double knots = speed.GetConvertedValue(Value.Unit.Knot);
double mps = speed.GetConvertedValue(Value.Unit.MeterPerSecond);
double kph = speed.GetConvertedValue(Value.Unit.KilometerPerHour);
```

## Error Handling

```csharp
var metar = decoder.ParseNotStrict("METAR LFPO 231027Z BAD_DATA");

if (!metar.IsValid)
{
    foreach (var ex in metar.DecodingExceptions)
    {
        Console.WriteLine($"Error in: {ex.ChunkDecoder.GetType().Name}");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Remaining: {ex.RemainingMetar}");
    }
}

// Reset errors
metar.ResetDecodingExceptions();
```

## Architecture

The library follows Clean Architecture with SOLID principles:

- **ChunkDecoder Pattern**: Chain of Responsibility - each decoder handles one METAR section
- **Interface Segregation**: `IMetarChunkDecoder` defines the contract
- **Single Responsibility**: Each decoder class handles exactly one METAR element
- **Open/Closed**: New decoders can be added without modifying existing ones

### Decoder Chain Order
1. `ReportTypeChunkDecoder` - METAR/SPECI/COR
2. `IcaoChunkDecoder` - Airport ICAO code
3. `DatetimeChunkDecoder` - Day/time of observation
4. `ReportStatusChunkDecoder` - AUTO/NIL status
5. `SurfaceWindChunkDecoder` - Wind direction/speed/gusts
6. `VisibilityChunkDecoder` - CAVOK or visibility values
7. `RunwayVisualRangeChunkDecoder` - RVR for runways
8. `PresentWeatherChunkDecoder` - Current weather phenomena
9. `CloudChunkDecoder` - Cloud layers
10. `TemperatureChunkDecoder` - Air/dew point temperatures
11. `PressureChunkDecoder` - QNH/altimeter setting
12. `RecentWeatherChunkDecoder` - Recent weather (RE)
13. `WindShearChunkDecoder` - Wind shear information
14. `TrendChunkDecoder` - Trend forecast (NOSIG/BECMG/TEMPO)
15. `RemarkChunkDecoder` - Remarks section (RMK)

## Supported METAR Elements

| Element | Example | Supported |
|---------|---------|-----------|
| Report Type | METAR, SPECI, COR | Yes |
| ICAO Code | SBGR, KJFK | Yes |
| Date/Time | 231027Z | Yes |
| Status | AUTO, NIL | Yes |
| Surface Wind | 24004KT, VRB02MPS, 24015G25KT | Yes |
| Direction Variation | 180V270 | Yes |
| CAVOK | CAVOK | Yes |
| Visibility (ICAO) | 9999, 2500 | Yes |
| Visibility (US) | 3SM, 1 1/2SM | Yes |
| Minimum Visibility | 1000NW | Yes |
| NDV | 9999NDV | Yes |
| RVR | R32/0400, R06L/0200V0600U | Yes |
| Present Weather | +TSRA, -SN, FZFG, VCSH | Yes |
| Clouds | FEW020, SCT030CB, OVC005 | Yes |
| Vertical Visibility | VV003 | Yes |
| Clear Sky | SKC, NSC, NCD, CLR | Yes |
| Temperature | 17/10, M02/M05 | Yes |
| Pressure (QNH) | Q1009 | Yes |
| Pressure (Altimeter) | A2992 | Yes |
| Recent Weather | REFZRA | Yes |
| Wind Shear | WS R03, WS ALL RWY | Yes |
| Trend Forecast | NOSIG, BECMG, TEMPO | Yes |
| Remarks | RMK AO2 SLP013 | Yes |
| Sea Level Pressure | SLPnnn (in RMK) | Yes |

## License

MIT License
