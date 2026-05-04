# Taf.Decoder - Usage Guide (EN-US)

## Overview

**Taf.Decoder** is a .NET library for parsing TAF (Terminal Aerodrome Forecast) strings into structured, strongly-typed objects. It supports multiple .NET frameworks: `.NET Standard 2.0`, `.NET 8.0`, `.NET 10.0`, and `.NET Framework 4.8`.

**Version:** 1.0.7

## Installation

```bash
dotnet add package Taf.Decoder
```

Or via NuGet Package Manager:

```
Install-Package Taf.Decoder
```

## Quick Start

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var decoder = new TafDecoder();
var taf = decoder.Parse("TAF SBGR 031100Z 0312/0418 35008KT 9999 FEW040 SCT100 TX30/0318Z TN20/0409Z");

Console.WriteLine($"ICAO: {taf.Icao}");
Console.WriteLine($"Day: {taf.Day}");
Console.WriteLine($"Time: {taf.Time}");
Console.WriteLine($"Forecast Period: {taf.ForecastPeriod.FromDay}/{taf.ForecastPeriod.FromHour} to {taf.ForecastPeriod.ToDay}/{taf.ForecastPeriod.ToHour}");
Console.WriteLine($"Wind: {taf.SurfaceWind.MeanDirection.ActualValue} at {taf.SurfaceWind.MeanSpeed.ActualValue} {taf.SurfaceWind.MeanSpeed.ActualUnit}");
Console.WriteLine($"Visibility: {taf.Visibility.ActualVisibility.ActualValue} {taf.Visibility.ActualVisibility.ActualUnit}");
```

## Parsing Modes

### Default Parsing
```csharp
var taf = decoder.Parse("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030");
```

### Strict Parsing
Stops at the first decoding error:
```csharp
var taf = decoder.ParseStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030");
```

### Non-Strict Parsing
Continues decoding even if errors are encountered:
```csharp
var taf = decoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 BADVALUE 9999 FEW030");
```

### Static Parsing
```csharp
var taf = TafDecoder.ParseWithMode("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030", isStrict: false);
```

## Decoded Fields

### Report Type
```csharp
taf.Type // TafType.TAF, TafType.TAFAMD, TafType.TAFCOR, TafType.RTD
```

### ICAO Code
```csharp
taf.Icao // "SBGR", "LFPO", "KJFK", etc.
```

### Date/Time
```csharp
taf.Day            // Day of month (1-31)
taf.Time           // "11:00 UTC"
taf.OriginDateTime // DateTime object
```

### Forecast Period
```csharp
var period = taf.ForecastPeriod;
period.FromDay   // Start day
period.FromHour  // Start hour
period.ToDay     // End day
period.ToHour    // End hour
period.IsValid   // Validity check
```

### Surface Wind
```csharp
var wind = taf.SurfaceWind;
wind.MeanDirection.ActualValue   // Direction in degrees (0-360)
wind.MeanSpeed.ActualValue       // Speed value
wind.MeanSpeed.ActualUnit        // Unit.Knot, Unit.MeterPerSecond, Unit.KilometerPerHour
wind.SpeedVariations?.ActualValue // Gust speed (if present)
wind.VariableDirection           // true if VRB
wind.DirectionVariations         // [min, max] direction variation values
```

### Visibility
```csharp
var vis = taf.Visibility;
vis.ActualVisibility.ActualValue  // Visibility value
vis.ActualVisibility.ActualUnit   // Unit.Meter or Unit.StatuteMile
vis.Greater                       // true if P (greater than) prefix
taf.Cavok                         // CAVOK flag
```

### Weather Phenomena
```csharp
foreach (var wp in taf.WeatherPhenomenons)
{
    Console.WriteLine($"Intensity: {wp.IntensityProximity}"); // +, -, VC
    Console.WriteLine($"Descriptor: {wp.Descriptor}");         // TS, FZ, SH, etc.
    foreach (var p in wp.Phenomena)
    {
        Console.WriteLine($"Phenomenon: {p}"); // RA, SN, FG, BR, etc.
    }
}
```

### Clouds
```csharp
foreach (var cloud in taf.Clouds)
{
    Console.WriteLine($"Amount: {cloud.Amount}");         // FEW, SCT, BKN, OVC, VV
    Console.WriteLine($"Height: {cloud.BaseHeight?.ActualValue} feet");
    Console.WriteLine($"Type: {cloud.Type}");             // CB, TCU, or NULL
}
```

### Temperature Forecast
```csharp
// Maximum temperature
var maxTemp = taf.MaximumTemperature;
if (maxTemp != null)
{
    Console.WriteLine($"Type: {maxTemp.Type}");              // "TX"
    Console.WriteLine($"Value: {maxTemp.TemperatureValue.ActualValue} C");
    Console.WriteLine($"Day: {maxTemp.Day}, Hour: {maxTemp.Hour}");
}

// Minimum temperature
var minTemp = taf.MinimumTemperature;
if (minTemp != null)
{
    Console.WriteLine($"Type: {minTemp.Type}");              // "TN"
    Console.WriteLine($"Value: {minTemp.TemperatureValue.ActualValue} C");
    Console.WriteLine($"Day: {minTemp.Day}, Hour: {minTemp.Hour}");
}
```

## Weather Evolutions (TEMPO/BECMG/FM/PROB)

TAF includes weather evolution sections that modify the base forecast. Evolutions are stored on each entity:

```csharp
// Check wind evolutions
var windEvos = taf.SurfaceWind.Evolutions;
foreach (var evo in windEvos)
{
    Console.WriteLine($"Type: {evo.Type}");         // "TEMPO", "BECMG", "FM"
    Console.WriteLine($"From: Day {evo.FromDay} {evo.FromTime}");
    Console.WriteLine($"To: Day {evo.ToDay} {evo.ToTime}");
    Console.WriteLine($"Probability: {evo.Probability}"); // "PROB30", "PROB40", or null

    var windEntity = evo.Entity as SurfaceWind;
    if (windEntity != null)
    {
        Console.WriteLine($"New Wind: {windEntity.MeanDirection?.ActualValue} at {windEntity.MeanSpeed?.ActualValue}");
    }
}

// Check visibility evolutions
var visEvos = taf.Visibility?.Evolutions;
// ... similar pattern

// Check cloud evolutions
foreach (var cloud in taf.Clouds)
{
    var cloudEvos = cloud.Evolutions;
    // ... similar pattern
}
```

### Evolution Types
- **TEMPO**: Temporary fluctuation expected to last less than 1 hour
- **BECMG**: Gradual change expected to occur during the specified period
- **FM**: From a specific time, conditions replace the previous forecast
- **PROB30/PROB40**: Probability of occurrence (30% or 40%)

## Value Conversion

The `Value` class supports unit conversion:

```csharp
var speed = taf.SurfaceWind.MeanSpeed;
double knots = speed.GetConvertedValue(Value.Unit.Knot);
double mps = speed.GetConvertedValue(Value.Unit.MeterPerSecond);
double kph = speed.GetConvertedValue(Value.Unit.KilometerPerHour);
```

## Error Handling

```csharp
var taf = decoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 BAD_DATA");

if (!taf.IsValid)
{
    foreach (var ex in taf.DecodingExceptions)
    {
        Console.WriteLine($"Error in: {ex.ChunkDecoder.GetType().Name}");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Remaining: {ex.RemainingTaf}");
    }
}

// Reset errors
taf.ResetDecodingExceptions();
```

## Architecture

The library follows Clean Architecture with SOLID principles:

- **ChunkDecoder Pattern**: Chain of Responsibility - each decoder handles one TAF section
- **Interface Segregation**: `ITafChunkDecoder` defines the contract
- **Single Responsibility**: Each decoder class handles exactly one TAF element
- **Open/Closed**: New decoders can be added without modifying existing ones

### Main Decoder Chain
1. `ReportTypeChunkDecoder` - TAF/TAF AMD/TAF COR/RTD
2. `IcaoChunkDecoder` - Airport ICAO code
3. `DatetimeChunkDecoder` - Day/time of issue
4. `ForecastPeriodChunkDecoder` - Valid period (e.g., 0312/0418)
5. `SurfaceWindChunkDecoder` - Wind direction/speed/gusts
6. `VisibilityChunkDecoder` - CAVOK or visibility values
7. `WeatherChunkDecoder` - Weather phenomena
8. `CloudChunkDecoder` - Cloud layers
9. `TemperatureChunkDecoder` - TX/TN temperatures

### Evolution Decoder
After the main chain, `EvolutionChunkDecoder` processes:
- `TEMPO` - Temporary changes
- `BECMG` - Becoming changes
- `FM` - From time changes
- `PROB30/PROB40` - Probability groups

## Supported TAF Elements

| Element | Example | Supported |
|---------|---------|-----------|
| Report Type | TAF, TAF AMD, TAF COR, RTD | Yes |
| ICAO Code | SBGR, KJFK | Yes |
| Date/Time | 231100Z | Yes |
| Forecast Period | 2312/2418 | Yes |
| Surface Wind | 24005KT, VRB03MPS, 24015G25KT | Yes |
| Direction Variation | 180V270 | Yes |
| CAVOK | CAVOK | Yes |
| Visibility (ICAO) | 9999, 2500 | Yes |
| Visibility (US) | 3SM, 1 1/2SM, P6SM | Yes |
| No Visibility Info | //// | Yes |
| Weather Phenomena | -RA, +TSRA, FZFG | Yes |
| Clouds | FEW020, SCT030CB, OVC005 | Yes |
| Vertical Visibility | VV003 | Yes |
| Clear Sky | SKC, NSC, NCD, CLR | Yes |
| Temperature Max | TX25/0318Z | Yes |
| Temperature Min | TNM03/0405Z | Yes |
| TEMPO | TEMPO 0312/0315 ... | Yes |
| BECMG | BECMG 0315/0317 ... | Yes |
| FM | FM031500 ... | Yes |
| PROB30/PROB40 | PROB40 TEMPO ... | Yes |

## License

MIT License
