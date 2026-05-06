# Metar.Decoder & Taf.Decoder - Guia de Uso (pt-BR)

Biblioteca .NET para decodificacao de strings de relatorios meteorologicos METAR e TAF. Compativel com **.NET Standard 2.0**, **.NET 8.0**, **.NET 10.0** e **.NET Framework 4.8**.

## Indice

- [Instalacao](#instalacao)
- [Decodificador METAR](#decodificador-metar)
  - [Uso Basico](#uso-basico-metar)
  - [Exemplo Completo](#exemplo-completo-metar)
  - [Propriedades Decodificadas](#propriedades-decodificadas-metar)
- [Decodificador TAF](#decodificador-taf)
  - [Uso Basico](#uso-basico-taf)
  - [Exemplo Completo](#exemplo-completo-taf)
  - [Suporte RTD](#suporte-rtd)
  - [Evolucoes](#evolucoes)
- [Objetos Value e Conversao de Unidades](#objetos-value-e-conversao-de-unidades)
- [Modos de Analise](#modos-de-analise)
- [Tratamento de Erros](#tratamento-de-erros)

---

## Instalacao

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

## Decodificador METAR

METAR (Meteorological Aerodrome Report) e um formato padronizado pela Organizacao da Aviacao Civil Internacional (ICAO) para relatar condicoes meteorologicas em aerodromos. A classe `MetarDecoder` analisa strings METAR brutas em objetos `DecodedMetar` estruturados.

### Uso Basico METAR

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

// Uso mais simples - metodo estatico com modo nao estrito (padrao)
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z 24004KT 9999 FEW040 17/10 Q1009");

if (decoded.IsValid)
{
    Console.WriteLine($"ICAO: {decoded.ICAO}");
    Console.WriteLine($"Temperatura: {decoded.AirTemperature.ActualValue} {decoded.AirTemperature.ActualUnit}");
}
```

### Exemplo Completo METAR

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

var d = MetarDecoder.ParseWithMode(
    "METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D " +
    "+FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03");

// --- Informacoes de Contexto ---
Console.WriteLine($"Valido: {d.IsValid}");          // true
Console.WriteLine($"METAR Bruto: {d.RawMetar}");
Console.WriteLine($"Tipo: {d.Type}");                // MetarType.METAR
Console.WriteLine($"ICAO: {d.ICAO}");                // "LFPO"
Console.WriteLine($"Dia: {d.Day}");                  // 23
Console.WriteLine($"Hora: {d.Time}");                // "10:27 UTC"
Console.WriteLine($"Status: {d.Status}");            // "AUTO"

// --- Vento de Superficie ---
var sw = d.SurfaceWind;
Console.WriteLine($"Direcao Media: {sw.MeanDirection.ActualValue} {sw.MeanDirection.ActualUnit}");
// 240 Degree
Console.WriteLine($"Velocidade Media: {sw.MeanSpeed.ActualValue} {sw.MeanSpeed.ActualUnit}");
// 4 MeterPerSecond
Console.WriteLine($"Rajadas: {sw.SpeedVariations.ActualValue} {sw.SpeedVariations.ActualUnit}");
// 9 MeterPerSecond
Console.WriteLine($"Vento Variavel: {sw.VariableWind}");
// false (true quando a direcao e VRB)

// --- Visibilidade ---
var v = d.Visibility;
Console.WriteLine($"Prevalecente: {v.PrevailingVisibility.ActualValue} {v.PrevailingVisibility.ActualUnit}");
// 2500 Meter
Console.WriteLine($"Minima: {v.MinimumVisibility.ActualValue} {v.MinimumVisibility.ActualUnit}");
// 1000 Meter
Console.WriteLine($"Direcao Minima: {v.MinimumVisibilityDirection}");
// "NW"
Console.WriteLine($"CAVOK: {d.Cavok}");
// false

// --- Alcance Visual da Pista (RVR) ---
foreach (var rvr in d.RunwaysVisualRange)
{
    Console.WriteLine($"Pista {rvr.Runway}: {rvr.VisualRange.ActualValue} {rvr.VisualRange.ActualUnit}");
    Console.WriteLine($"  Tendencia: {rvr.PastTendency}");
    // PastTendency pode ser: U (subindo), D (descendo), N (sem mudanca), ou vazio
}

// --- Tempo Presente ---
foreach (var pw in d.PresentWeather)
{
    Console.WriteLine($"Tempo: {pw.IntensityProximity}" +
        $"{string.Join("", pw.Characteristics)}" +
        $"{string.Join("", pw.Types)}");
    // "+FZRA" (chuva congelante forte)
    // "VCSN"  (neve nas proximidades)
}

// --- Camadas de Nuvens ---
foreach (var cld in d.Clouds)
{
    Console.WriteLine($"Nuvens: {cld.Amount} a {cld.BaseHeight?.ActualValue} {cld.BaseHeight?.ActualUnit}");
    Console.WriteLine($"  Tipo: {cld.Type}");
    // Amount: FEW, SCT, BKN, OVS
    // Type: CB (cumulonimbus), TCU (cumulus imponente), ou vazio
}

// --- Temperatura ---
Console.WriteLine($"Temperatura do Ar: {d.AirTemperature?.ActualValue} {d.AirTemperature?.ActualUnit}");
// 17 DegreeCelsius
Console.WriteLine($"Ponto de Orvalho: {d.DewPointTemperature?.ActualValue} {d.DewPointTemperature?.ActualUnit}");
// 10 DegreeCelsius

// --- Pressao ---
Console.WriteLine($"Pressao: {d.Pressure?.ActualValue} {d.Pressure?.ActualUnit}");
// 1009 HectoPascal (QNH)

// --- Tempo Recente ---
if (d.RecentWeather != null)
{
    Console.WriteLine($"Recente: {d.RecentWeather.Characteristics}{string.Join("", d.RecentWeather.Types)}");
    // "FZRA" (chuva congelante recente)
}

// --- Windshear ---
Console.WriteLine($"Windshear Todas as Pistas: {d.WindshearAllRunways}");
if (d.WindshearRunways?.Count > 0)
{
    Console.WriteLine($"Windshear Pistas: {string.Join(", ", d.WindshearRunways)}");
    // "03"
}

// --- Tendencia ---
if (!string.IsNullOrEmpty(d.TrendType))
{
    Console.WriteLine($"Tipo de Tendencia: {d.TrendType}"); // NOSIG, BECMG, TEMPO
    Console.WriteLine($"Previsao de Tendencia: {d.TrendForecast}");
}

// --- Observacoes ---
if (!string.IsNullOrEmpty(d.Remark))
{
    Console.WriteLine($"Observacoes: {d.Remark}");
}
```

### Propriedades Decodificadas METAR

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `RawMetar` | `string` | String METAR bruta original |
| `IsValid` | `bool` | Se o METAR foi decodificado sem erros |
| `Type` | `MetarType` | Tipo de relatorio: `METAR`, `METAR_COR`, `SPECI`, `SPECI_COR` |
| `ICAO` | `string` | Codigo ICAO do aeroporto (ex.: "LFPO") |
| `Day` | `int?` | Dia do mes da observacao |
| `Time` | `string` | Hora da observacao (ex.: "10:27 UTC") |
| `ObservationDateTime` | `DateTime?` | Data e hora completas da observacao |
| `Status` | `string` | Status do relatorio: `AUTO`, `NIL`, ou vazio |
| `SurfaceWind` | `SurfaceWind` | Informacoes de vento (direcao, velocidade, rajadas) |
| `Visibility` | `Visibility` | Dados de visibilidade |
| `Cavok` | `bool` | Se CAVOK (Teto e Visibilidade OK) foi reportado |
| `RunwaysVisualRange` | `List<RunwayVisualRange>` | RVR para cada pista |
| `PresentWeather` | `List<WeatherPhenomenon>` | Fenomenos meteorologicos atuais |
| `Clouds` | `List<CloudLayer>` | Camadas de nuvens |
| `AirTemperature` | `Value` | Temperatura do ar em graus Celsius |
| `DewPointTemperature` | `Value` | Temperatura do ponto de orvalho |
| `Pressure` | `Value` | Pressao atmosferica (QNH/QFE) |
| `RecentWeather` | `WeatherPhenomenon` | Fenomenos meteorologicos recentes |
| `WindshearAllRunways` | `bool?` | Se o windshear afeta todas as pistas |
| `WindshearRunways` | `List<string>` | Pistas especificas com windshear |
| `TrendType` | `string` | Tipo de tendencia: `NOSIG`, `BECMG`, `TEMPO` |
| `TrendForecast` | `string` | Conteudo bruto da previsao de tendencia |
| `Remark` | `string` | Conteudo bruto da secao de observacoes (apos RMK) |
| `SeaLevelPressure` | `Value` | Pressao ao nivel do mar extraida das observacoes |
| `DecodingExceptions` | `ReadOnlyCollection<MetarChunkDecoderException>` | Erros de analise |

---

## Decodificador TAF

TAF (Terminal Aerodrome Forecast) e um formato de previsao meteorologica para aerodromos padronizado pela ICAO. A classe `TafDecoder` analisa strings TAF brutas em objetos `DecodedTaf` estruturados.

### Uso Basico TAF

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var decoded = TafDecoder.ParseWithMode("TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025");

if (decoded.IsValid)
{
    Console.WriteLine($"ICAO: {decoded.Icao}");
    Console.WriteLine($"Periodo: {decoded.ForecastPeriod.FromDay}/{decoded.ForecastPeriod.FromHour}Z " +
        $"ate {decoded.ForecastPeriod.ToDay}/{decoded.ForecastPeriod.ToHour}Z");
}
```

### Exemplo Completo TAF

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var d = TafDecoder.ParseWithMode(
    "TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025 TX12/0816Z TN04/0807Z");

// --- Informacoes de Contexto ---
Console.WriteLine($"Valido: {d.IsValid}");
Console.WriteLine($"TAF Bruto: {d.RawTaf}");
Console.WriteLine($"Tipo: {d.Type}");       // TAF, TAFAMD, TAFCOR, RTD
Console.WriteLine($"ICAO: {d.Icao}");       // "LEMD"

// --- Periodo de Previsao ---
var fp = d.ForecastPeriod;
Console.WriteLine($"De: Dia {fp.FromDay} Hora {fp.FromHour}");
Console.WriteLine($"Ate: Dia {fp.ToDay} Hora {fp.ToHour}");

// --- Vento de Superficie ---
var sw = d.SurfaceWind;
Console.WriteLine($"Direcao: {sw.MeanDirection?.ActualValue} {sw.MeanDirection?.ActualUnit}");
Console.WriteLine($"Velocidade: {sw.MeanSpeed?.ActualValue} {sw.MeanSpeed?.ActualUnit}");

// --- Visibilidade ---
Console.WriteLine($"Visibilidade: {d.Visibility.ActualVisibility?.ActualValue} {d.Visibility.ActualVisibility?.ActualUnit}");

// --- Nuvens ---
foreach (var cld in d.Clouds)
{
    Console.WriteLine($"Nuvens: {cld.Amount} a {cld.BaseHeight?.ActualValue} {cld.BaseHeight?.ActualUnit}");
}

// --- Temperaturas ---
if (d.MaximumTemperature != null)
{
    var tx = d.MaximumTemperature;
    Console.WriteLine($"Temp Maxima: {tx.TemperatureValue.ActualValue}{tx.TemperatureValue.ActualUnit} " +
        $"no dia {tx.Day} as {tx.Hour}Z");
}
if (d.MinimumTemperature != null)
{
    var tn = d.MinimumTemperature;
    Console.WriteLine($"Temp Minima: {tn.TemperatureValue.ActualValue}{tn.TemperatureValue.ActualUnit} " +
        $"no dia {tn.Day} as {tn.Hour}Z");
}

// --- Evolucoes ---
foreach (var evo in d.Evolutions)
{
    Console.WriteLine($"Evolucao: {evo.Type} de {evo.FromDay}{evo.FromHour}Z a {evo.ToDay}{evo.ToHour}Z");
}
```

### Suporte RTD

O decodificador suporta relatorios TAF marcados como "RTD" (Report Delayed):

```csharp
string rtdTaf = "RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 " +
    "TEMPO 1905/1907 2000 BR BKN003 BECMG 1907/1909 9000 NSW FEW002";
var decoded = TafDecoder.ParseWithMode(rtdTaf);

Console.WriteLine($"Tipo: {decoded.Type}");     // TafType.RTD
Console.WriteLine($"ICAO: {decoded.Icao}");     // "EKEB"
Console.WriteLine($"Valido: {decoded.IsValid}"); // true
```

### Evolucoes

Relatorios TAF podem conter mudancas de previsao ao longo do tempo:

| Tipo | Palavra-chave | Descricao |
|------|---------------|-----------|
| BECMG | `BECMG` | Tornando-se - condicoes mudarao gradualmente |
| TEMPO | `TEMPO` | Temporario - condicoes mudarao temporariamente |
| PROB | `PROB30/40` | Probabilidade de ocorrencia (30% ou 40%) |
| PROB+TEMPO | `PROB30 TEMPO` | Probabilidade de mudancas temporarias |

```csharp
var d = TafDecoder.ParseWithMode(
    "TAF EGLL 080500Z 0806/0906 24015G25KT 9999 BKN040 " +
    "TEMPO 0806/0812 30018G30KT 3000 +SHRA BKN012CB " +
    "BECMG 0812/0814 25010KT " +
    "PROB40 0818/0824 2000 BR");

foreach (var evo in d.Evolutions)
{
    Console.WriteLine($"--- {evo.Type} ---");
    Console.WriteLine($"Periodo: {evo.FromDay}{evo.FromHour}Z-{evo.ToDay}{evo.ToHour}Z");

    if (evo.SurfaceWind != null)
        Console.WriteLine($"Vento: {evo.SurfaceWind.MeanDirection?.ActualValue} a " +
            $"{evo.SurfaceWind.MeanSpeed?.ActualValue}{evo.SurfaceWind.MeanSpeed?.ActualUnit}");

    if (evo.Visibility != null)
        Console.WriteLine($"Visibilidade: {evo.Visibility.ActualVisibility?.ActualValue}m");
}
```

---

## Objetos Value e Conversao de Unidades

Todos os valores meteorologicos numericos sao encapsulados em objetos `Value` que carregam tanto o valor quanto sua unidade.

### Acessando Valores

```csharp
Value temp = decoded.AirTemperature;
if (temp != null)
{
    double valor = temp.ActualValue;   // ex.: 17.0
    Value.Unit unidade = temp.ActualUnit; // ex.: Value.Unit.DegreeCelsius
    Console.WriteLine($"{valor} {unidade}");
}
```

### Conversao de Unidades

Converta valores para diferentes unidades usando `GetConvertedValue()`:

```csharp
// Conversoes de velocidade
Value velVento = decoded.SurfaceWind.MeanSpeed; // ex.: 4 MeterPerSecond
float nos = velVento.GetConvertedValue(Value.Unit.Knot);             // ~7.776
float kmh = velVento.GetConvertedValue(Value.Unit.KilometerPerHour); // ~14.4

// Conversoes de distancia
Value vis = decoded.Visibility.PrevailingVisibility; // ex.: 2500 Meter
float pes = vis.GetConvertedValue(Value.Unit.Feet);          // ~8202.1
float milhas = vis.GetConvertedValue(Value.Unit.StatuteMile); // ~1.553

// Conversoes de pressao
Value press = decoded.Pressure; // ex.: 1009 HectoPascal
float inHg = press.GetConvertedValue(Value.Unit.MercuryInch); // ~29.797
```

### Unidades Disponiveis

| Categoria | Unidade | Descricao |
|-----------|---------|-----------|
| **Velocidade** | `Value.Unit.MeterPerSecond` | Metros por segundo (m/s) |
| | `Value.Unit.KilometerPerHour` | Quilometros por hora (km/h) |
| | `Value.Unit.Knot` | Nos (kt) |
| **Distancia** | `Value.Unit.Meter` | Metros (m) |
| | `Value.Unit.Feet` | Pes (ft) |
| | `Value.Unit.StatuteMile` | Milhas terrestres (SM) |
| **Pressao** | `Value.Unit.HectoPascal` | Hectopascais (hPa) |
| | `Value.Unit.MercuryInch` | Polegadas de mercurio (inHg) |
| **Temperatura** | `Value.Unit.DegreeCelsius` | Graus Celsius |
| **Direcao** | `Value.Unit.Degree` | Graus (0-360) |

### Seguranca contra Nulos

Sempre verifique nulo antes de acessar objetos `Value`, pois nem todos os campos sao obrigatorios:

```csharp
var pontoOrvalho = decoded.DewPointTemperature;
if (pontoOrvalho != null)
{
    Console.WriteLine($"Ponto de Orvalho: {pontoOrvalho.ActualValue} {pontoOrvalho.ActualUnit}");
}
else
{
    Console.WriteLine("Ponto de orvalho nao reportado");
}

// Padrao com valor default
var pressao = decoded.Pressure ?? new Value(1013.25, Value.Unit.HectoPascal);
```

---

## Modos de Analise

Ambos os decodificadores suportam dois modos de analise:

### Modo Nao Estrito (Padrao)

Continua a analise mesmo quando erros sao encontrados:

```csharp
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z BADCHUNK 17/10 Q1009");

// Verificar erros
foreach (var ex in decoded.DecodingExceptions)
{
    Console.WriteLine($"Erro: {ex.Message}");
}

// Dados validos ainda estao disponiveis
Console.WriteLine($"Temperatura: {decoded.AirTemperature?.ActualValue}"); // 17
```

### Modo Estrito

Para a analise no primeiro erro encontrado:

```csharp
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z BADCHUNK 17/10 Q1009", isStrict: true);
Console.WriteLine($"Valido: {decoded.IsValid}"); // false
```

### Sobrescrevendo o Modo por Chamada

```csharp
var decoder = new MetarDecoder();
decoder.SetStrictParsing(true); // estrito global

decoder.ParseNotStrict("...");  // sobrescreve: nao estrito para esta chamada
decoder.ParseStrict("...");     // sobrescreve: estrito para esta chamada
decoder.Parse("...");           // usa configuracao global (estrito)
```

---

## Tratamento de Erros

### Acessando Erros de Analise

```csharp
var decoded = MetarDecoder.ParseWithMode(rawMetar);

if (!decoded.IsValid)
{
    foreach (var exception in decoded.DecodingExceptions)
    {
        Console.WriteLine($"Chunk: {exception.ChunkDecoder}");
        Console.WriteLine($"Mensagem: {exception.Message}");
        Console.WriteLine($"Restante: {exception.RemainingMetar}");
    }
}
```

### Recuperacao em Modo Nao Estrito

No modo nao estrito, o decodificador tenta se recuperar de erros retentando no proximo chunk. Por exemplo, se `AAA 12003KPH ...` e fornecido ao decodificador `SurfaceWind`:

1. Falha em `AAA`
2. Retenta em `12003KPH` e tem sucesso
3. O primeiro erro e registrado, mas `SurfaceWind` e preenchido

```csharp
var decoded = MetarDecoder.ParseWithMode("METAR LFPO 231027Z AAA 12003KT 9999 17/10 Q1009");

Console.WriteLine($"Tem Erros: {decoded.DecodingExceptions.Count > 0}"); // true
Console.WriteLine($"Velocidade do Vento: {decoded.SurfaceWind?.MeanSpeed?.ActualValue}"); // 12
```

---

## Repositorio e Issues

- **Repositorio**: [https://github.com/afonsoft/metar-decoder](https://github.com/afonsoft/metar-decoder)
- **NuGet (Metar)**: [https://www.nuget.org/packages/Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder)
- **NuGet (TAF)**: [https://www.nuget.org/packages/Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder)
- **Issues**: [https://github.com/afonsoft/metar-decoder/issues](https://github.com/afonsoft/metar-decoder/issues)
- **Licenca**: MIT
