# Taf.Decoder - Guia de Uso (PT-BR)

## Visao Geral

**Taf.Decoder** e uma biblioteca .NET para decodificacao de strings TAF (Terminal Aerodrome Forecast / Previsao de Aerodromo Terminal) em objetos estruturados e fortemente tipados. Suporta multiplos frameworks: `.NET Standard 2.0`, `.NET 8.0`, `.NET 10.0` e `.NET Framework 4.8`.

**Versao:** 1.0.7

## Instalacao

```bash
dotnet add package Taf.Decoder
```

Ou pelo NuGet Package Manager:

```
Install-Package Taf.Decoder
```

## Inicio Rapido

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;

var decoder = new TafDecoder();
var taf = decoder.Parse("TAF SBGR 031100Z 0312/0418 35008KT 9999 FEW040 SCT100 TX30/0318Z TN20/0409Z");

Console.WriteLine($"ICAO: {taf.Icao}");
Console.WriteLine($"Dia: {taf.Day}");
Console.WriteLine($"Hora: {taf.Time}");
Console.WriteLine($"Periodo: {taf.ForecastPeriod.FromDay}/{taf.ForecastPeriod.FromHour} ate {taf.ForecastPeriod.ToDay}/{taf.ForecastPeriod.ToHour}");
Console.WriteLine($"Vento: {taf.SurfaceWind.MeanDirection.ActualValue} graus a {taf.SurfaceWind.MeanSpeed.ActualValue} {taf.SurfaceWind.MeanSpeed.ActualUnit}");
Console.WriteLine($"Visibilidade: {taf.Visibility.ActualVisibility.ActualValue} {taf.Visibility.ActualVisibility.ActualUnit}");
```

## Modos de Decodificacao

### Decodificacao Padrao
```csharp
var taf = decoder.Parse("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030");
```

### Decodificacao Estrita (Strict)
Para na primeira falha de decodificacao:
```csharp
var taf = decoder.ParseStrict("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030");
```

### Decodificacao Nao-Estrita (Not Strict)
Continua a decodificacao mesmo encontrando erros:
```csharp
var taf = decoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 INVALIDO 9999 FEW030");
```

### Decodificacao Estatica
```csharp
var taf = TafDecoder.ParseWithMode("TAF LFPO 231100Z 2312/2418 24005KT 9999 FEW030", isStrict: false);
```

## Campos Decodificados

### Tipo de Relatorio
```csharp
taf.Type // TafType.TAF, TafType.TAFAMD (emendado), TafType.TAFCOR (corrigido), TafType.RTD
```

### Codigo ICAO
```csharp
taf.Icao // "SBGR", "LFPO", "KJFK", etc.
```

### Data/Hora
```csharp
taf.Day            // Dia do mes (1-31)
taf.Time           // "11:00 UTC"
taf.OriginDateTime // Objeto DateTime
```

### Periodo de Previsao
```csharp
var periodo = taf.ForecastPeriod;
periodo.FromDay   // Dia inicial
periodo.FromHour  // Hora inicial
periodo.ToDay     // Dia final
periodo.ToHour    // Hora final
periodo.IsValid   // Verificacao de validade
```

### Vento de Superficie
```csharp
var vento = taf.SurfaceWind;
vento.MeanDirection.ActualValue   // Direcao em graus (0-360)
vento.MeanSpeed.ActualValue       // Valor da velocidade
vento.MeanSpeed.ActualUnit        // Unit.Knot, Unit.MeterPerSecond, Unit.KilometerPerHour
vento.SpeedVariations?.ActualValue // Rajada (se presente)
vento.VariableDirection           // true se VRB (variavel)
vento.DirectionVariations         // [min, max] variacao de direcao
```

### Visibilidade
```csharp
var vis = taf.Visibility;
vis.ActualVisibility.ActualValue  // Valor da visibilidade
vis.ActualVisibility.ActualUnit   // Unit.Meter ou Unit.StatuteMile
vis.Greater                       // true se prefixo P (maior que)
taf.Cavok                         // CAVOK (teto e visibilidade OK)
```

### Fenomenos Meteorologicos
```csharp
foreach (var fm in taf.WeatherPhenomenons)
{
    Console.WriteLine($"Intensidade: {fm.IntensityProximity}"); // +, -, VC
    Console.WriteLine($"Descritor: {fm.Descriptor}");            // TS, FZ, SH, etc.
    foreach (var p in fm.Phenomena)
    {
        Console.WriteLine($"Fenomeno: {p}"); // RA (chuva), SN (neve), FG (nevoeiro), etc.
    }
}
```

### Nuvens
```csharp
foreach (var nuvem in taf.Clouds)
{
    Console.WriteLine($"Quantidade: {nuvem.Amount}");     // FEW, SCT, BKN, OVC, VV
    Console.WriteLine($"Altura: {nuvem.BaseHeight?.ActualValue} pes");
    Console.WriteLine($"Tipo: {nuvem.Type}");             // CB, TCU, ou NULL
}
```

### Previsao de Temperatura
```csharp
// Temperatura maxima
var tempMax = taf.MaximumTemperature;
if (tempMax != null)
{
    Console.WriteLine($"Tipo: {tempMax.Type}");              // "TX"
    Console.WriteLine($"Valor: {tempMax.TemperatureValue.ActualValue} C");
    Console.WriteLine($"Dia: {tempMax.Day}, Hora: {tempMax.Hour}");
}

// Temperatura minima
var tempMin = taf.MinimumTemperature;
if (tempMin != null)
{
    Console.WriteLine($"Tipo: {tempMin.Type}");              // "TN"
    Console.WriteLine($"Valor: {tempMin.TemperatureValue.ActualValue} C");
    Console.WriteLine($"Dia: {tempMin.Day}, Hora: {tempMin.Hour}");
}
```

## Evolucoes Meteorologicas (TEMPO/BECMG/FM/PROB)

O TAF inclui secoes de evolucao meteorologica que modificam a previsao base. As evolucoes sao armazenadas em cada entidade:

```csharp
// Verificar evolucoes de vento
var evoVento = taf.SurfaceWind.Evolutions;
foreach (var evo in evoVento)
{
    Console.WriteLine($"Tipo: {evo.Type}");         // "TEMPO", "BECMG", "FM"
    Console.WriteLine($"De: Dia {evo.FromDay} {evo.FromTime}");
    Console.WriteLine($"Ate: Dia {evo.ToDay} {evo.ToTime}");
    Console.WriteLine($"Probabilidade: {evo.Probability}"); // "PROB30", "PROB40", ou null

    var ventoEvo = evo.Entity as SurfaceWind;
    if (ventoEvo != null)
    {
        Console.WriteLine($"Novo Vento: {ventoEvo.MeanDirection?.ActualValue} a {ventoEvo.MeanSpeed?.ActualValue}");
    }
}

// Verificar evolucoes de visibilidade
var evoVis = taf.Visibility?.Evolutions;
// ... mesmo padrao

// Verificar evolucoes de nuvens
foreach (var nuvem in taf.Clouds)
{
    var evoNuvem = nuvem.Evolutions;
    // ... mesmo padrao
}
```

### Tipos de Evolucao
- **TEMPO**: Flutuacao temporaria com duracao esperada inferior a 1 hora
- **BECMG**: Mudanca gradual esperada durante o periodo especificado
- **FM**: A partir de um horario especifico, as condicoes substituem a previsao anterior
- **PROB30/PROB40**: Probabilidade de ocorrencia (30% ou 40%)

## Conversao de Valores

A classe `Value` suporta conversao de unidades:

```csharp
var velocidade = taf.SurfaceWind.MeanSpeed;
double nos = velocidade.GetConvertedValue(Value.Unit.Knot);
double mps = velocidade.GetConvertedValue(Value.Unit.MeterPerSecond);
double kph = velocidade.GetConvertedValue(Value.Unit.KilometerPerHour);
```

## Tratamento de Erros

```csharp
var taf = decoder.ParseNotStrict("TAF LFPO 231100Z 2312/2418 DADOS_INVALIDOS");

if (!taf.IsValid)
{
    foreach (var ex in taf.DecodingExceptions)
    {
        Console.WriteLine($"Erro no decodificador: {ex.ChunkDecoder.GetType().Name}");
        Console.WriteLine($"Mensagem: {ex.Message}");
        Console.WriteLine($"TAF restante: {ex.RemainingTaf}");
    }
}

// Limpar erros
taf.ResetDecodingExceptions();
```

## Arquitetura

A biblioteca segue os principios de Arquitetura Limpa (Clean Architecture) e SOLID:

- **Padrao ChunkDecoder**: Cadeia de Responsabilidade - cada decodificador trata uma secao do TAF
- **Segregacao de Interface**: `ITafChunkDecoder` define o contrato
- **Responsabilidade Unica**: Cada classe decodificadora trata exatamente um elemento do TAF
- **Aberto/Fechado**: Novos decodificadores podem ser adicionados sem modificar os existentes

### Cadeia Principal de Decodificacao
1. `ReportTypeChunkDecoder` - TAF/TAF AMD/TAF COR/RTD
2. `IcaoChunkDecoder` - Codigo ICAO do aerodromo
3. `DatetimeChunkDecoder` - Dia/hora de emissao
4. `ForecastPeriodChunkDecoder` - Periodo de validade (ex: 0312/0418)
5. `SurfaceWindChunkDecoder` - Direcao/velocidade/rajadas do vento
6. `VisibilityChunkDecoder` - CAVOK ou valores de visibilidade
7. `WeatherChunkDecoder` - Fenomenos meteorologicos
8. `CloudChunkDecoder` - Camadas de nuvens
9. `TemperatureChunkDecoder` - Temperaturas TX/TN

### Decodificador de Evolucao
Apos a cadeia principal, `EvolutionChunkDecoder` processa:
- `TEMPO` - Mudancas temporarias
- `BECMG` - Mudancas graduais (tornando-se)
- `FM` - A partir de determinado horario
- `PROB30/PROB40` - Grupos de probabilidade

## Elementos TAF Suportados

| Elemento | Exemplo | Suportado |
|----------|---------|-----------|
| Tipo de Relatorio | TAF, TAF AMD, TAF COR, RTD | Sim |
| Codigo ICAO | SBGR, KJFK | Sim |
| Data/Hora | 231100Z | Sim |
| Periodo de Previsao | 2312/2418 | Sim |
| Vento de Superficie | 24005KT, VRB03MPS, 24015G25KT | Sim |
| Variacao de Direcao | 180V270 | Sim |
| CAVOK | CAVOK | Sim |
| Visibilidade (ICAO) | 9999, 2500 | Sim |
| Visibilidade (EUA) | 3SM, 1 1/2SM, P6SM | Sim |
| Sem Info Visibilidade | //// | Sim |
| Fenomenos Meteorologicos | -RA, +TSRA, FZFG | Sim |
| Nuvens | FEW020, SCT030CB, OVC005 | Sim |
| Visibilidade Vertical | VV003 | Sim |
| Ceu Claro | SKC, NSC, NCD, CLR | Sim |
| Temperatura Maxima | TX25/0318Z | Sim |
| Temperatura Minima | TNM03/0405Z | Sim |
| TEMPO | TEMPO 0312/0315 ... | Sim |
| BECMG | BECMG 0315/0317 ... | Sim |
| FM | FM031500 ... | Sim |
| PROB30/PROB40 | PROB40 TEMPO ... | Sim |

## Glossario de Termos TAF

| Codigo | Significado |
|--------|-------------|
| TAF | Previsao de Aerodromo Terminal |
| AMD | Emenda (forecast emendado) |
| COR | Correcao (forecast corrigido) |
| RTD | Retardado (forecast atrasado) |
| CAVOK | Teto e Visibilidade OK |
| TEMPO | Temporario (flutuacao temporaria) |
| BECMG | Tornando-se (mudanca gradual) |
| FM | A partir de (mudanca completa) |
| PROB | Probabilidade |
| FEW | Poucas nuvens (1-2 oitavos) |
| SCT | Esparsas (3-4 oitavos) |
| BKN | Nublado (5-7 oitavos) |
| OVC | Encoberto (8 oitavos) |
| VV | Visibilidade Vertical |
| CB | Cumulonimbus |
| TCU | Cumulus em Torre |
| TX | Temperatura Maxima |
| TN | Temperatura Minima |
| RA | Chuva |
| SN | Neve |
| FG | Nevoeiro |
| BR | Nevoa |
| TS | Trovoada |
| FZ | Congelante |
| SH | Pancadas |
| VRB | Variavel |

## Licenca

MIT License
