# Metar.Decoder & Taf.Decoder - API Reference (en-US)

Complete reference for all public classes, properties, methods, and enumerations.

## Table of Contents

- [Metar.Decoder Namespace](#metardecoder-namespace)
  - [MetarDecoder](#metardecoder-class)
  - [DecodedMetar](#decodedmetar-class)
  - [SurfaceWind](#surfacewind-class)
  - [Visibility](#visibility-class)
  - [RunwayVisualRange](#runwayvisualrange-class)
  - [CloudLayer](#cloudlayer-class)
  - [WeatherPhenomenon](#weatherphenomenon-class)
  - [Value](#value-class)
  - [MetarChunkDecoderException](#metarchunkdecoderexception-class)
- [Taf.Decoder Namespace](#tafdecoder-namespace)
  - [TafDecoder](#tafdecoder-class)
  - [DecodedTaf](#decodedtaf-class)
  - [ForecastPeriod](#forecastperiod-class)
  - [Temperature](#temperature-class)
  - [Evolution](#evolution-class)
- [Enumerations](#enumerations)
  - [MetarType](#metartype)
  - [MetarStatus](#metarstatus)
  - [TafType](#taftype)
  - [Value.Unit](#valueunit)

---

## Metar.Decoder Namespace

### MetarDecoder Class

Main class for decoding METAR weather report strings.

**Namespace**: `Metar.Decoder`

#### Constants

| Constant | Type | Value | Description |
|----------|------|-------|-------------|
| `ResultKey` | `string` | `"Result"` | Key used internally for decoded data results |
| `RemainingMetarKey` | `string` | `"RemainingMetar"` | Key used internally for remaining unparsed string |
| `ExceptionKey` | `string` | `"Exception"` | Key used internally for parsing exceptions |

#### Constructors

```csharp
public MetarDecoder()
```

Creates a new instance of the METAR decoder. Sets a process-wide regex timeout of 500ms for safety.

#### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Parse(string rawMetar)` | `DecodedMetar` | Decodes a METAR string using the global strict parsing setting |
| `ParseStrict(string rawMetar)` | `DecodedMetar` | Decodes a METAR string in strict mode (stops on first error) |
| `ParseNotStrict(string rawMetar)` | `DecodedMetar` | Decodes a METAR string in non-strict mode (continues on errors) |
| `ParseWithMode(string rawMetar, bool isStrict = false)` | `DecodedMetar` | **Static**. Decodes a METAR string with the specified parsing mode |
| `SetStrictParsing(bool isStrict)` | `void` | Sets the global parsing mode for this decoder instance |

---

### DecodedMetar Class

Represents a fully decoded METAR weather report.

**Namespace**: `Metar.Decoder.Entity`

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `RawMetar` | `string` | The original raw METAR string (trimmed) |
| `Type` | `MetarType` | Report type (`METAR`, `METAR_COR`, `SPECI`, `SPECI_COR`, `NULL`) |
| `ICAO` | `string` | ICAO 4-letter airport identifier (e.g., `"LFPO"`, `"KJFK"`) |
| `Day` | `int?` | Day of the month of observation (1-31) |
| `Time` | `string` | Time of observation as formatted string (e.g., `"10:27 UTC"`) |
| `ObservationDateTime` | `DateTime?` | Full date/time of observation (computed from day and time) |
| `Status` | `string` | Report status: `"AUTO"` (automated), `"NIL"` (missing), or empty |
| `SurfaceWind` | `SurfaceWind` | Surface wind data (direction, speed, gusts, variability) |
| `Visibility` | `Visibility` | Visibility data (prevailing, minimum, direction, NDV) |
| `Cavok` | `bool` | `true` if CAVOK (Ceiling And Visibility OK) was reported |
| `RunwaysVisualRange` | `List<RunwayVisualRange>` | Runway visual range data for each reported runway |
| `PresentWeather` | `List<WeatherPhenomenon>` | Current weather conditions (rain, snow, fog, etc.) |
| `Clouds` | `List<CloudLayer>` | Cloud layers with amount, base height, and cloud type |
| `AirTemperature` | `Value` | Air temperature (unit: `DegreeCelsius`) |
| `DewPointTemperature` | `Value` | Dew point temperature (unit: `DegreeCelsius`) |
| `Pressure` | `Value` | Atmospheric pressure, QNH or altimeter setting |
| `RecentWeather` | `WeatherPhenomenon` | Weather that occurred recently but is no longer occurring |
| `WindshearAllRunways` | `bool?` | `true` if windshear reported on all runways (WS ALL RWY) |
| `WindshearRunways` | `List<string>` | Specific runway identifiers with windshear (e.g., `["03", "21"]`) |
| `TrendType` | `string` | Trend forecast type: `"NOSIG"`, `"BECMG"`, `"TEMPO"`, or empty |
| `TrendForecast` | `string` | Raw content of the trend forecast section |
| `Remark` | `string` | Raw content of the remarks section (after RMK keyword) |
| `SeaLevelPressure` | `Value` | Sea-level pressure from remarks (SLPnnn format) |
| `DecodingExceptions` | `ReadOnlyCollection<MetarChunkDecoderException>` | List of all parsing errors |

#### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `IsValid` | `bool` | Returns `true` if no decoding exceptions occurred |
| `AddDecodingException(MetarChunkDecoderException ex)` | `void` | Adds a decoding exception to the list |
| `ResetDecodingExceptions()` | `void` | Clears all decoding exceptions |

---

### SurfaceWind Class

Represents surface wind information from a METAR report.

**Namespace**: `Metar.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `MeanDirection` | `Value` | Mean wind direction (unit: `Degree`, 0-360) |
| `VariableWind` | `bool` | `true` if wind direction is variable (VRB) |
| `MeanSpeed` | `Value` | Mean wind speed (unit depends on report: `Knot`, `MeterPerSecond`, or `KilometerPerHour`) |
| `SpeedVariations` | `Value` | Gust speed, if reported (same unit as `MeanSpeed`) |
| `DirectionVariations` | `Value[]` | Array of 2 values [min, max] for variable direction range (e.g., 180V240) |

---

### Visibility Class

Represents visibility information from a METAR report.

**Namespace**: `Metar.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `PrevailingVisibility` | `Value` | Main visibility (unit: `Meter` or `StatuteMile`) |
| `MinimumVisibility` | `Value` | Minimum directional visibility, if reported |
| `MinimumVisibilityDirection` | `string` | Direction of minimum visibility (e.g., `"NW"`, `"SE"`) |
| `NDV` | `bool` | No Directional Variation - `true` if visibility is the same in all directions |
| `HasCavok` | `bool` | Whether CAVOK was set for this visibility report |

---

### RunwayVisualRange Class

Represents runway visual range (RVR) information.

**Namespace**: `Metar.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `Runway` | `string` | Runway designator (e.g., `"32"`, `"08C"`, `"26L"`) |
| `VisualRange` | `Value` | Visual range value (unit: `Meter` or `Feet`) |
| `PastTendency` | `string` | Trend indicator: `"U"` (up), `"D"` (down), `"N"` (no change), or empty |
| `VisualRangeInterval` | `Value[]` | Min/max range values if an interval was reported |
| `Variable` | `bool` | Whether a variable range was reported |

---

### CloudLayer Class

Represents a single cloud layer.

**Namespace**: `Metar.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `Amount` | `string` | Coverage amount: `"FEW"` (1-2 oktas), `"SCT"` (3-4), `"BKN"` (5-7), `"OVS"` (8) |
| `BaseHeight` | `Value` | Cloud base height above ground level (unit: `Feet`) |
| `Type` | `string` | Cloud type: `"CB"` (Cumulonimbus), `"TCU"` (Towering Cumulus), or empty |

---

### WeatherPhenomenon Class

Represents a weather phenomenon (present or recent).

**Namespace**: `Metar.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `IntensityProximity` | `string` | `"+"` (heavy), `"-"` (light), `"VC"` (vicinity), or empty (moderate) |
| `Characteristics` | `List<string>` | Descriptor codes: `"MI"`, `"BC"`, `"PR"`, `"DR"`, `"BL"`, `"SH"`, `"TS"`, `"FZ"` |
| `Types` | `List<string>` | Precipitation/obscuration types: `"RA"`, `"SN"`, `"DZ"`, `"FG"`, `"BR"`, etc. |

#### Common Weather Codes

| Code | Description | Category |
|------|-------------|----------|
| `RA` | Rain | Precipitation |
| `SN` | Snow | Precipitation |
| `DZ` | Drizzle | Precipitation |
| `GR` | Hail | Precipitation |
| `GS` | Small hail / Snow pellets | Precipitation |
| `PL` | Ice pellets | Precipitation |
| `SG` | Snow grains | Precipitation |
| `IC` | Ice crystals | Precipitation |
| `UP` | Unknown precipitation | Precipitation |
| `FG` | Fog (visibility < 1000m) | Obscuration |
| `BR` | Mist (visibility 1000-5000m) | Obscuration |
| `HZ` | Haze | Obscuration |
| `FU` | Smoke | Obscuration |
| `SA` | Sand | Obscuration |
| `DU` | Dust | Obscuration |
| `VA` | Volcanic ash | Obscuration |
| `TS` | Thunderstorm | Descriptor |
| `SH` | Showers | Descriptor |
| `FZ` | Freezing | Descriptor |
| `BL` | Blowing | Descriptor |
| `DR` | Drifting | Descriptor |
| `MI` | Shallow | Descriptor |
| `BC` | Patches | Descriptor |
| `PR` | Partial | Descriptor |
| `SQ` | Squall | Other |
| `FC` | Funnel cloud / Tornado | Other |
| `SS` | Sandstorm | Other |
| `DS` | Duststorm | Other |

---

### Value Class

Represents a numeric value with its associated unit. Supports unit conversions.

**Namespace**: `Metar.Decoder.Entity`

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `ActualValue` | `double` | The numeric value in the original unit |
| `ActualUnit` | `Value.Unit` | The unit of measurement |

#### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `GetConvertedValue(Value.Unit unitTo)` | `float` | Converts the value to the specified unit. Throws `ArgumentException` if conversion is not supported |
| `ToString()` | `string` | Returns `"{ActualValue} {ActualUnit}"` |

#### Static Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `ToInt(string value)` | `int?` | Converts METAR-encoded string to integer. Handles `P` (+), `M` (-), `/` (null) |

#### Conversion Table

| From | To | Supported |
|------|----|-----------|
| `MeterPerSecond` | `KilometerPerHour`, `Knot` | Yes |
| `KilometerPerHour` | `MeterPerSecond`, `Knot` | Yes |
| `Knot` | `MeterPerSecond`, `KilometerPerHour` | Yes |
| `Meter` | `Feet`, `StatuteMile` | Yes |
| `Feet` | `Meter`, `StatuteMile` | Yes |
| `StatuteMile` | `Meter`, `Feet` | Yes |
| `HectoPascal` | `MercuryInch` | Yes |
| `MercuryInch` | `HectoPascal` | Yes |

---

### MetarChunkDecoderException Class

Exception thrown when a METAR chunk cannot be decoded.

**Namespace**: `Metar.Decoder`

| Property | Type | Description |
|----------|------|-------------|
| `Message` | `string` | Description of the parsing error |
| `RemainingMetar` | `string` | The remaining unparsed METAR string at the point of failure |
| `ChunkDecoder` | `string` | Name of the chunk decoder that encountered the error |
| `NewRemainingMetar` | `string` | The adjusted remaining string after error recovery |

---

## Taf.Decoder Namespace

### TafDecoder Class

Main class for decoding TAF weather forecast strings.

**Namespace**: `Taf.Decoder`

#### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Parse(string rawTaf)` | `DecodedTaf` | Decodes a TAF string using the global strict parsing setting |
| `ParseStrict(string rawTaf)` | `DecodedTaf` | Decodes a TAF string in strict mode |
| `ParseNotStrict(string rawTaf)` | `DecodedTaf` | Decodes a TAF string in non-strict mode |
| `ParseWithMode(string rawTaf, bool isStrict = false)` | `DecodedTaf` | **Static**. Decodes a TAF string with the specified parsing mode |
| `SetStrictParsing(bool isStrict)` | `void` | Sets the global parsing mode for this decoder instance |

---

### DecodedTaf Class

Represents a fully decoded TAF weather forecast.

**Namespace**: `Taf.Decoder.Entity`

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `RawTaf` | `string` | The original raw TAF string (trimmed) |
| `Type` | `TafType` | Report type (`TAF`, `TAFAMD`, `TAFCOR`, `RTD`, `NULL`) |
| `Icao` | `string` | ICAO 4-letter airport identifier |
| `Day` | `int?` | Day of origin (1-31) |
| `Time` | `string` | Time of origin as formatted string |
| `OriginDateTime` | `DateTime?` | Full date/time of origin |
| `ForecastPeriod` | `ForecastPeriod` | Valid forecast period |
| `Status` | `string` | Report status |
| `SurfaceWind` | `SurfaceWind` | Surface wind data |
| `Visibility` | `Visibility` | Visibility data |
| `Cavok` | `bool` | Whether CAVOK was reported |
| `WeatherPhenomenons` | `List<WeatherPhenomenon>` | Weather phenomena |
| `Clouds` | `List<CloudLayer>` | Cloud layers |
| `MinimumTemperature` | `Temperature` | Forecast minimum temperature |
| `MaximumTemperature` | `Temperature` | Forecast maximum temperature |
| `Evolutions` | `List<Evolution>` | Forecast changes (BECMG, TEMPO, PROB) |
| `DecodingExceptions` | `ReadOnlyCollection<TafChunkDecoderException>` | Parsing errors |

#### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `IsValid` | `bool` | Returns `true` if no decoding exceptions occurred |
| `AddDecodingException(TafChunkDecoderException ex)` | `void` | Adds a decoding exception |
| `ResetDecodingExceptions()` | `void` | Clears all decoding exceptions |

---

### ForecastPeriod Class

Represents the valid period of a TAF forecast.

**Namespace**: `Taf.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `FromDay` | `int?` | Start day of the forecast period |
| `FromHour` | `int?` | Start hour of the forecast period (UTC) |
| `ToDay` | `int?` | End day of the forecast period |
| `ToHour` | `int?` | End hour of the forecast period (UTC) |

---

### Temperature Class

Represents a forecast temperature with its associated time.

**Namespace**: `Taf.Decoder.Entity`

| Property | Type | Description |
|----------|------|-------------|
| `TemperatureValue` | `Value` | Temperature value (unit: `DegreeCelsius`) |
| `Day` | `int?` | Day when the temperature is forecast |
| `Hour` | `int?` | Hour (UTC) when the temperature is forecast |

---

### Evolution Class

Represents a forecast change (evolution) within a TAF.

**Namespace**: `Taf.Decoder.Entity`

Inherits from `AbstractEntity`, which provides weather-related properties.

| Property | Type | Description |
|----------|------|-------------|
| `Type` | `string` | Evolution type: `"BECMG"`, `"TEMPO"` |
| `FromDay` | `int?` | Start day of the evolution period |
| `FromHour` | `int?` | Start hour of the evolution period |
| `ToDay` | `int?` | End day of the evolution period |
| `ToHour` | `int?` | End hour of the evolution period |
| `Probability` | `int?` | Probability percentage (30 or 40) if PROB was specified |
| `SurfaceWind` | `SurfaceWind` | Wind changes (if any) |
| `Visibility` | `Visibility` | Visibility changes (if any) |
| `Cavok` | `bool` | Whether CAVOK applies to this evolution |
| `WeatherPhenomenons` | `List<WeatherPhenomenon>` | Weather changes |
| `Clouds` | `List<CloudLayer>` | Cloud changes |

---

## Enumerations

### MetarType

| Value | Description |
|-------|-------------|
| `NULL` | Not determined |
| `METAR` | Standard METAR report |
| `METAR_COR` | Corrected METAR report |
| `SPECI` | Special METAR report (issued for significant changes) |
| `SPECI_COR` | Corrected special METAR report |

### MetarStatus

| Value | Description |
|-------|-------------|
| `NULL` | Not determined |
| `AUTO` | Automated observation (no human intervention) |
| `NIL` | Missing report (no data available) |

### TafType

| Value | Description |
|-------|-------------|
| `NULL` | Not determined |
| `TAF` | Standard TAF forecast |
| `TAFAMD` | Amended TAF forecast |
| `TAFCOR` | Corrected TAF forecast |
| `RTD` | Report Delayed |

### Value.Unit

| Value | Description | Abbreviation |
|-------|-------------|-------------|
| `None` | No unit | |
| `DegreeCelsius` | Temperature in Celsius | deg C |
| `Degree` | Direction in degrees | deg |
| `Knot` | Speed in knots | kt |
| `MeterPerSecond` | Speed in m/s | m/s |
| `KilometerPerHour` | Speed in km/h | km/h |
| `Meter` | Distance in meters | m |
| `Feet` | Distance in feet | ft |
| `StatuteMile` | Distance in statute miles | SM |
| `HectoPascal` | Pressure in hectopascals | hPa |
| `MercuryInch` | Pressure in inches of mercury | inHg |
| `UnknownUnit` | Unknown/unrecognized unit | N/A |

---

## Repository & Issues

- **Repository**: [https://github.com/afonsoft/metar-decoder](https://github.com/afonsoft/metar-decoder)
- **NuGet (Metar)**: [https://www.nuget.org/packages/Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder)
- **NuGet (TAF)**: [https://www.nuget.org/packages/Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder)
- **Issues**: [https://github.com/afonsoft/metar-decoder/issues](https://github.com/afonsoft/metar-decoder/issues)
- **License**: MIT
