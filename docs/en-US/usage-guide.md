# Metar.Decoder & Taf.Decoder - Usage Guide (en-US)

.NET library for decoding METAR and TAF weather report strings. Compatible with **.NET Standard 2.0**, **.NET 8.0**, **.NET 10.0**, and **.NET Framework 4.8**.

## Table of Contents

- [Installation](#installation)
- [METAR Decoder](#metar-decoder)
  - [Basic Usage](#basic-metar-usage)
  - [Full Example](#full-metar-example)
  - [Decoded Properties](#decoded-metar-properties)
- [TAF Decoder](#taf-decoder)
  - [Basic Usage](#basic-taf-usage)
  - [Full Example](#full-taf-example)
  - [RTD Support](#rtd-support)
  - [Evolutions](#evolutions)
- [Value Objects & Unit Conversion](#value-objects--unit-conversion)
- [Parsing Modes](#parsing-modes)
- [Error Handling](#error-handling)

---

## Installation

### .NET CLI

```bash
dotnet add package Metar.Decoder
dotnet add package Taf.Decoder
```

### NuGet Package Manager

```bash
Install-Package Metar.Decoder
Install-Package Taf.Decoder
```

### PackageReference

```xml
<PackageReference Include="Metar.Decoder" Version="1.0.9" />
<PackageReference Include="Taf.Decoder" Version="1.0.7" />
```

---

## METAR Decoder

METAR (Meteorological Aerodrome Report) is a standardized format by the International Civil Aviation Organization (ICAO) for reporting meteorological conditions at aerodromes. The `MetarDecoder` class parses raw METAR strings into structured `DecodedMetar` objects.

### Basic METAR Usage

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

// Simplest usage - static method with non-strict mode (default)
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z 24004KT 9999 FEW040 17/10 Q1009");

if (decoded.IsValid)
{
    Console.WriteLine($"ICAO: {decoded.ICAO}");
    Console.WriteLine($"Temperature: {decoded.AirTemperature.ActualValue} {decoded.AirTemperature.ActualUnit}");
}
```

### Full METAR Example

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

var d = MetarDecoder.ParseWithMode(
    "METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D " +
    "+FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03");

// --- Context Information ---
Console.WriteLine($"Valid: {d.IsValid}");          // true
Console.WriteLine($"Raw METAR: {d.RawMetar}");
Console.WriteLine($"Type: {d.Type}");              // MetarType.METAR
Console.WriteLine($"ICAO: {d.ICAO}");              // "LFPO"
Console.WriteLine($"Day: {d.Day}");                // 23
Console.WriteLine($"Time: {d.Time}");              // "10:27 UTC"
Console.WriteLine($"Status: {d.Status}");          // "AUTO"

// --- Surface Wind ---
var sw = d.SurfaceWind;
Console.WriteLine($"Mean Direction: {sw.MeanDirection.ActualValue} {sw.MeanDirection.ActualUnit}");
// 240 Degree
Console.WriteLine($"Mean Speed: {sw.MeanSpeed.ActualValue} {sw.MeanSpeed.ActualUnit}");
// 4 MeterPerSecond
Console.WriteLine($"Speed Variations (gusts): {sw.SpeedVariations.ActualValue} {sw.SpeedVariations.ActualUnit}");
// 9 MeterPerSecond
Console.WriteLine($"Variable Wind: {sw.VariableWind}");
// false (true when direction is VRB)

// --- Visibility ---
var v = d.Visibility;
Console.WriteLine($"Prevailing: {v.PrevailingVisibility.ActualValue} {v.PrevailingVisibility.ActualUnit}");
// 2500 Meter
Console.WriteLine($"Minimum: {v.MinimumVisibility.ActualValue} {v.MinimumVisibility.ActualUnit}");
// 1000 Meter
Console.WriteLine($"Min Direction: {v.MinimumVisibilityDirection}");
// "NW"
Console.WriteLine($"NDV (No Directional Variation): {v.NDV}");
// false
Console.WriteLine($"CAVOK: {d.Cavok}");
// false

// --- Runway Visual Range (RVR) ---
foreach (var rvr in d.RunwaysVisualRange)
{
    Console.WriteLine($"Runway {rvr.Runway}: {rvr.VisualRange.ActualValue} {rvr.VisualRange.ActualUnit}");
    Console.WriteLine($"  Trend: {rvr.PastTendency}");
    // PastTendency can be: U (Up), D (Down), N (No change), or empty
}

// --- Present Weather ---
foreach (var pw in d.PresentWeather)
{
    Console.WriteLine($"Weather: {pw.IntensityProximity}" +
        $"{string.Join("", pw.Characteristics)}" +
        $"{string.Join("", pw.Types)}");
    // "+FZRA" (heavy freezing rain)
    // "VCSN"  (vicinity snow)
}

// --- Cloud Layers ---
foreach (var cld in d.Clouds)
{
    Console.WriteLine($"Clouds: {cld.Amount} at {cld.BaseHeight?.ActualValue} {cld.BaseHeight?.ActualUnit}");
    Console.WriteLine($"  Type: {cld.Type}");
    // Amount: FEW, SCT, BKN, OVS
    // Type: CB (cumulonimbus), TCU (towering cumulus), or empty
}

// --- Temperature ---
Console.WriteLine($"Air Temperature: {d.AirTemperature?.ActualValue} {d.AirTemperature?.ActualUnit}");
// 17 DegreeCelsius
Console.WriteLine($"Dew Point: {d.DewPointTemperature?.ActualValue} {d.DewPointTemperature?.ActualUnit}");
// 10 DegreeCelsius

// --- Pressure ---
Console.WriteLine($"Pressure: {d.Pressure?.ActualValue} {d.Pressure?.ActualUnit}");
// 1009 HectoPascal (QNH)

// --- Recent Weather ---
if (d.RecentWeather != null)
{
    Console.WriteLine($"Recent: {d.RecentWeather.Characteristics}{string.Join("", d.RecentWeather.Types)}");
    // "FZRA" (recent freezing rain)
}

// --- Windshear ---
Console.WriteLine($"Windshear All Runways: {d.WindshearAllRunways}");
if (d.WindshearRunways?.Count > 0)
{
    Console.WriteLine($"Windshear Runways: {string.Join(", ", d.WindshearRunways)}");
    // "03"
}

// --- Trend Forecast ---
if (!string.IsNullOrEmpty(d.TrendType))
{
    Console.WriteLine($"Trend Type: {d.TrendType}"); // NOSIG, BECMG, TEMPO
    Console.WriteLine($"Trend Forecast: {d.TrendForecast}");
}

// --- Remarks ---
if (!string.IsNullOrEmpty(d.Remark))
{
    Console.WriteLine($"Remarks: {d.Remark}");
}

// --- Sea-Level Pressure (from remarks) ---
if (d.SeaLevelPressure != null)
{
    Console.WriteLine($"SLP: {d.SeaLevelPressure.ActualValue} {d.SeaLevelPressure.ActualUnit}");
}
```

### Decoded METAR Properties

| Property | Type | Description |
|----------|------|-------------|
| `RawMetar` | `string` | The original raw METAR string |
| `IsValid` | `bool` | Whether the METAR was decoded without errors |
| `Type` | `MetarType` | Report type: `METAR`, `METAR_COR`, `SPECI`, `SPECI_COR` |
| `ICAO` | `string` | ICAO airport code (e.g., "LFPO") |
| `Day` | `int?` | Day of the month of observation |
| `Time` | `string` | Time of observation (e.g., "10:27 UTC") |
| `ObservationDateTime` | `DateTime?` | Parsed observation date and time |
| `Status` | `string` | Report status: `AUTO`, `NIL`, or empty |
| `SurfaceWind` | `SurfaceWind` | Wind information (direction, speed, gusts, variable) |
| `Visibility` | `Visibility` | Visibility data (prevailing, minimum, direction) |
| `Cavok` | `bool` | Whether CAVOK (Ceiling And Visibility OK) was reported |
| `RunwaysVisualRange` | `List<RunwayVisualRange>` | RVR for each runway |
| `PresentWeather` | `List<WeatherPhenomenon>` | Current weather phenomena |
| `Clouds` | `List<CloudLayer>` | Cloud layers with amount, height, and type |
| `AirTemperature` | `Value` | Air temperature in degrees Celsius |
| `DewPointTemperature` | `Value` | Dew point temperature in degrees Celsius |
| `Pressure` | `Value` | Atmospheric pressure (QNH/QFE) |
| `RecentWeather` | `WeatherPhenomenon` | Recent weather phenomena |
| `WindshearAllRunways` | `bool?` | Whether windshear affects all runways |
| `WindshearRunways` | `List<string>` | Specific runways with windshear |
| `TrendType` | `string` | Trend forecast type: `NOSIG`, `BECMG`, `TEMPO` |
| `TrendForecast` | `string` | Raw content of the trend forecast |
| `Remark` | `string` | Raw content of the remarks section (after RMK) |
| `SeaLevelPressure` | `Value` | Sea-level pressure extracted from remarks (SLPnnn) |
| `DecodingExceptions` | `ReadOnlyCollection<MetarChunkDecoderException>` | All parsing errors encountered |

---

## TAF Decoder

TAF (Terminal Aerodrome Forecast) is a weather forecast format for aerodromes standardized by ICAO. The `TafDecoder` class parses raw TAF strings into structured `DecodedTaf` objects.

### Basic TAF Usage

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var decoded = TafDecoder.ParseWithMode("TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025");

if (decoded.IsValid)
{
    Console.WriteLine($"ICAO: {decoded.Icao}");
    Console.WriteLine($"Period: {decoded.ForecastPeriod.FromDay}/{decoded.ForecastPeriod.FromHour}Z " +
        $"to {decoded.ForecastPeriod.ToDay}/{decoded.ForecastPeriod.ToHour}Z");
}
```

### Full TAF Example

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var d = TafDecoder.ParseWithMode(
    "TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025 TX12/0816Z TN04/0807Z");

// --- Context Information ---
Console.WriteLine($"Valid: {d.IsValid}");
Console.WriteLine($"Raw TAF: {d.RawTaf}");
Console.WriteLine($"Type: {d.Type}");       // TAF, TAFAMD, TAFCOR, RTD
Console.WriteLine($"ICAO: {d.Icao}");       // "LEMD"
Console.WriteLine($"Day: {d.Day}");         // 8
Console.WriteLine($"Time: {d.Time}");       // "05:00 UTC"

// --- Forecast Period ---
var fp = d.ForecastPeriod;
Console.WriteLine($"From: Day {fp.FromDay} Hour {fp.FromHour}");
Console.WriteLine($"To: Day {fp.ToDay} Hour {fp.ToHour}");

// --- Surface Wind ---
var sw = d.SurfaceWind;
Console.WriteLine($"Direction: {sw.MeanDirection?.ActualValue} {sw.MeanDirection?.ActualUnit}");
Console.WriteLine($"Speed: {sw.MeanSpeed?.ActualValue} {sw.MeanSpeed?.ActualUnit}");
if (sw.SpeedVariations != null)
    Console.WriteLine($"Gusts: {sw.SpeedVariations.ActualValue} {sw.SpeedVariations.ActualUnit}");

// --- Visibility ---
var vis = d.Visibility;
Console.WriteLine($"Visibility: {vis.ActualVisibility?.ActualValue} {vis.ActualVisibility?.ActualUnit}");
Console.WriteLine($"CAVOK: {d.Cavok}");

// --- Clouds ---
foreach (var cld in d.Clouds)
{
    Console.WriteLine($"Clouds: {cld.Amount} at {cld.BaseHeight?.ActualValue} {cld.BaseHeight?.ActualUnit}");
}

// --- Temperatures ---
if (d.MaximumTemperature != null)
{
    var tx = d.MaximumTemperature;
    Console.WriteLine($"Max Temp: {tx.TemperatureValue.ActualValue}{tx.TemperatureValue.ActualUnit} " +
        $"on day {tx.Day} at {tx.Hour}Z");
}
if (d.MinimumTemperature != null)
{
    var tn = d.MinimumTemperature;
    Console.WriteLine($"Min Temp: {tn.TemperatureValue.ActualValue}{tn.TemperatureValue.ActualUnit} " +
        $"on day {tn.Day} at {tn.Hour}Z");
}

// --- Weather Phenomena ---
foreach (var wp in d.WeatherPhenomenons)
{
    Console.WriteLine($"Weather: {wp.IntensityProximity}" +
        $"{string.Join("", wp.Characteristics)}" +
        $"{string.Join("", wp.Types)}");
}

// --- Evolutions ---
foreach (var evo in d.Evolutions)
{
    Console.WriteLine($"Evolution: {evo.Type}");
    Console.WriteLine($"  Period: {evo.FromDay}{evo.FromHour}Z to {evo.ToDay}{evo.ToHour}Z");
    Console.WriteLine($"  Probability: {evo.Probability}");
    // Access evolution-specific weather data: evo.SurfaceWind, evo.Visibility, evo.Clouds, etc.
}
```

### RTD Support

The decoder supports TAF reports marked as "RTD" (Report Delayed):

```csharp
string rtdTaf = "RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 " +
    "TEMPO 1905/1907 2000 BR BKN003 BECMG 1907/1909 9000 NSW FEW002";
var decoded = TafDecoder.ParseWithMode(rtdTaf);

Console.WriteLine($"Type: {decoded.Type}");     // TafType.RTD
Console.WriteLine($"ICAO: {decoded.Icao}");     // "EKEB"
Console.WriteLine($"Valid: {decoded.IsValid}");  // true
```

### Evolutions

TAF reports can contain forecast changes over time:

| Type | Keyword | Description |
|------|---------|-------------|
| BECMG | `BECMG` | Becoming - conditions will gradually change |
| TEMPO | `TEMPO` | Temporary - conditions will temporarily change |
| PROB | `PROB30/40` | Probability of occurrence (30% or 40%) |
| PROB+TEMPO | `PROB30 TEMPO` | Probability of temporary changes |

```csharp
var d = TafDecoder.ParseWithMode(
    "TAF EGLL 080500Z 0806/0906 24015G25KT 9999 BKN040 " +
    "TEMPO 0806/0812 30018G30KT 3000 +SHRA BKN012CB " +
    "BECMG 0812/0814 25010KT " +
    "PROB40 0818/0824 2000 BR");

foreach (var evo in d.Evolutions)
{
    Console.WriteLine($"--- {evo.Type} ---");
    Console.WriteLine($"Period: {evo.FromDay}{evo.FromHour}Z-{evo.ToDay}{evo.ToHour}Z");

    if (evo.SurfaceWind != null)
        Console.WriteLine($"Wind: {evo.SurfaceWind.MeanDirection?.ActualValue} at " +
            $"{evo.SurfaceWind.MeanSpeed?.ActualValue}{evo.SurfaceWind.MeanSpeed?.ActualUnit}");

    if (evo.Visibility != null)
        Console.WriteLine($"Visibility: {evo.Visibility.ActualVisibility?.ActualValue}m");

    foreach (var wp in evo.WeatherPhenomenons)
        Console.WriteLine($"Weather: {wp.IntensityProximity}" +
            $"{string.Join("", wp.Characteristics)}{string.Join("", wp.Types)}");

    foreach (var cld in evo.Clouds)
        Console.WriteLine($"Clouds: {cld.Amount} at {cld.BaseHeight?.ActualValue}ft");
}
```

### Decoded TAF Properties

| Property | Type | Description |
|----------|------|-------------|
| `RawTaf` | `string` | The original raw TAF string |
| `IsValid` | `bool` | Whether the TAF was decoded without errors |
| `Type` | `TafType` | Report type: `TAF`, `TAFAMD`, `TAFCOR`, `RTD` |
| `Icao` | `string` | ICAO airport code |
| `Day` | `int?` | Day of origin |
| `Time` | `string` | Time of origin |
| `OriginDateTime` | `DateTime?` | Parsed origin date and time |
| `ForecastPeriod` | `ForecastPeriod` | Valid period (FromDay/FromHour to ToDay/ToHour) |
| `SurfaceWind` | `SurfaceWind` | Wind information |
| `Visibility` | `Visibility` | Visibility data |
| `Cavok` | `bool` | Whether CAVOK was reported |
| `WeatherPhenomenons` | `List<WeatherPhenomenon>` | Weather phenomena |
| `Clouds` | `List<CloudLayer>` | Cloud layers |
| `MinimumTemperature` | `Temperature` | Forecast minimum temperature (TNnn/ddhhZ) |
| `MaximumTemperature` | `Temperature` | Forecast maximum temperature (TXnn/ddhhZ) |
| `Evolutions` | `List<Evolution>` | Forecast changes (BECMG, TEMPO, PROB) |
| `DecodingExceptions` | `ReadOnlyCollection<TafChunkDecoderException>` | All parsing errors |

---

## Value Objects & Unit Conversion

All numeric meteorological values (speed, distance, pressure, temperature) are encapsulated in `Value` objects that carry both the value and its unit.

### Accessing Values

```csharp
Value temp = decoded.AirTemperature;
if (temp != null)
{
    double value = temp.ActualValue;  // e.g., 17.0
    Value.Unit unit = temp.ActualUnit; // e.g., Value.Unit.DegreeCelsius
    Console.WriteLine($"{value} {unit}");
}
```

### Unit Conversion

Convert values to different units using `GetConvertedValue()`:

```csharp
// Speed conversions
Value windSpeed = decoded.SurfaceWind.MeanSpeed; // e.g., 4 MeterPerSecond
float knots = windSpeed.GetConvertedValue(Value.Unit.Knot);           // ~7.776
float kph = windSpeed.GetConvertedValue(Value.Unit.KilometerPerHour); // ~14.4

// Distance conversions
Value vis = decoded.Visibility.PrevailingVisibility; // e.g., 2500 Meter
float feet = vis.GetConvertedValue(Value.Unit.Feet);         // ~8202.1
float miles = vis.GetConvertedValue(Value.Unit.StatuteMile); // ~1.553

// Pressure conversions
Value press = decoded.Pressure; // e.g., 1009 HectoPascal
float inHg = press.GetConvertedValue(Value.Unit.MercuryInch); // ~29.797
```

### Available Units

| Category | Units | Description |
|----------|-------|-------------|
| **Speed** | `Value.Unit.MeterPerSecond` | Meters per second (m/s) |
| | `Value.Unit.KilometerPerHour` | Kilometers per hour (km/h) |
| | `Value.Unit.Knot` | Knots (kt) |
| **Distance** | `Value.Unit.Meter` | Meters (m) |
| | `Value.Unit.Feet` | Feet (ft) |
| | `Value.Unit.StatuteMile` | Statute miles (SM) |
| **Pressure** | `Value.Unit.HectoPascal` | Hectopascals (hPa) |
| | `Value.Unit.MercuryInch` | Inches of mercury (inHg) |
| **Temperature** | `Value.Unit.DegreeCelsius` | Degrees Celsius |
| **Direction** | `Value.Unit.Degree` | Degrees (0-360) |

### Null Safety

Always check for null before accessing `Value` objects, as not all fields are mandatory:

```csharp
// Safe pattern
var dewPoint = decoded.DewPointTemperature;
if (dewPoint != null)
{
    Console.WriteLine($"Dew Point: {dewPoint.ActualValue} {dewPoint.ActualUnit}");
}
else
{
    Console.WriteLine("Dew point not reported");
}

// Default value pattern
var pressure = decoded.Pressure ?? new Value(1013.25, Value.Unit.HectoPascal);
Console.WriteLine($"Pressure: {pressure.ActualValue} {pressure.ActualUnit}");
```

---

## Parsing Modes

Both decoders support two parsing modes:

### Non-Strict Mode (Default)

Continues parsing even when errors are found. Errors are logged in `DecodingExceptions`:

```csharp
// Using static method (default: non-strict)
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z BADCHUNK 17/10 Q1009");

// Or using instance method
var decoder = new MetarDecoder();
decoder.SetStrictParsing(false);
var result = decoder.Parse("METAR LFPO 231027Z BADCHUNK 17/10 Q1009");

// Check for errors
foreach (var ex in decoded.DecodingExceptions)
{
    Console.WriteLine($"Error in chunk: {ex.Message}");
}

// Valid data is still available
Console.WriteLine($"Temperature: {decoded.AirTemperature?.ActualValue}"); // 17
```

### Strict Mode

Stops parsing at the first error encountered:

```csharp
// Using static method
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z BADCHUNK 17/10 Q1009", isStrict: true);

// Or using instance method
var decoder = new MetarDecoder();
decoder.SetStrictParsing(true);
var result = decoder.Parse("METAR LFPO 231027Z BADCHUNK 17/10 Q1009");

// Only first error will be reported
Console.WriteLine($"Valid: {decoded.IsValid}"); // false
```

### Override Modes per Call

```csharp
var decoder = new MetarDecoder();
decoder.SetStrictParsing(true); // global strict

decoder.ParseNotStrict("...");  // override: non-strict for this call
decoder.ParseStrict("...");     // override: strict for this call
decoder.Parse("...");           // uses global setting (strict)
```

---

## Error Handling

### Accessing Parsing Errors

```csharp
var decoded = MetarDecoder.ParseWithMode(rawMetar);

if (!decoded.IsValid)
{
    foreach (var exception in decoded.DecodingExceptions)
    {
        Console.WriteLine($"Chunk: {exception.ChunkDecoder}");
        Console.WriteLine($"Message: {exception.Message}");
        Console.WriteLine($"Remaining: {exception.RemainingMetar}");
    }
}
```

### Non-Strict Recovery

In non-strict mode, the decoder attempts to recover from errors by retrying on the next chunk. For example, if `AAA 12003KPH ...` is provided to the `SurfaceWind` chunk decoder:

1. It fails on `AAA`
2. Retries on `12003KPH` and succeeds
3. The first error is logged, but `SurfaceWind` is populated

```csharp
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z AAA 12003KT 9999 17/10 Q1009");

// Errors occurred but data was recovered
Console.WriteLine($"Has Errors: {decoded.DecodingExceptions.Count > 0}"); // true
Console.WriteLine($"Wind Speed: {decoded.SurfaceWind?.MeanSpeed?.ActualValue}"); // 12
```

---

## Repository & Issues

- **Repository**: [https://github.com/afonsoft/metar-decoder](https://github.com/afonsoft/metar-decoder)
- **NuGet (Metar)**: [https://www.nuget.org/packages/Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder)
- **NuGet (TAF)**: [https://www.nuget.org/packages/Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder)
- **Issues**: [https://github.com/afonsoft/metar-decoder/issues](https://github.com/afonsoft/metar-decoder/issues)
- **License**: MIT
