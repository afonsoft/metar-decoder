# Metar.Decoder - Guia de Uso (PT-BR)

## Visao Geral

**Metar.Decoder** e uma biblioteca .NET para decodificacao de strings METAR (Meteorological Aerodrome Report / Relatorio Meteorologico de Aerodromo) em objetos estruturados e fortemente tipados. Suporta multiplos frameworks: `.NET Standard 2.0`, `.NET 8.0`, `.NET 10.0` e `.NET Framework 4.8`.

**Versao:** 1.0.9

## Instalacao

```bash
dotnet add package Metar.Decoder
```

Ou pelo NuGet Package Manager:

```
Install-Package Metar.Decoder
```

## Inicio Rapido

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;

var decoder = new MetarDecoder();
var metar = decoder.Parse("METAR SBGR 031000Z 35006KT 9999 FEW040 SCT100 27/18 Q1020");

Console.WriteLine($"ICAO: {metar.ICAO}");
Console.WriteLine($"Dia: {metar.Day}");
Console.WriteLine($"Hora: {metar.Time}");
Console.WriteLine($"Vento: {metar.SurfaceWind.MeanDirection.ActualValue} graus a {metar.SurfaceWind.MeanSpeed.ActualValue} {metar.SurfaceWind.MeanSpeed.ActualUnit}");
Console.WriteLine($"Visibilidade: {metar.Visibility.PrevailingVisibility.ActualValue} {metar.Visibility.PrevailingVisibility.ActualUnit}");
Console.WriteLine($"Temperatura: {metar.AirTemperature.ActualValue} C");
Console.WriteLine($"Ponto de Orvalho: {metar.DewPointTemperature.ActualValue} C");
Console.WriteLine($"Pressao: {metar.Pressure.ActualValue} {metar.Pressure.ActualUnit}");
```

## Modos de Decodificacao

### Decodificacao Padrao
```csharp
var metar = decoder.Parse("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009");
```

### Decodificacao Estrita (Strict)
Para na primeira falha de decodificacao:
```csharp
var metar = decoder.ParseStrict("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009");
```

### Decodificacao Nao-Estrita (Not Strict)
Continua a decodificacao mesmo encontrando erros:
```csharp
var metar = decoder.ParseNotStrict("METAR LFPO 231027Z 24004KT INVALIDO FEW020 17/10 Q1009");
// metar.IsValid == false, mas os demais campos sao decodificados normalmente
```

### Decodificacao Estatica
```csharp
var metar = MetarDecoder.ParseWithMode("METAR LFPO 231027Z 24004KT 9999 FEW020 17/10 Q1009", isStrict: false);
```

## Campos Decodificados

### Tipo de Relatorio
```csharp
metar.Type // MetarType.METAR, MetarType.SPECI, MetarType.METAR_COR, MetarType.SPECI_COR
```

### Codigo ICAO
```csharp
metar.ICAO // "SBGR", "LFPO", "KJFK", etc.
```

### Data/Hora
```csharp
metar.Day              // Dia do mes (1-31)
metar.Time             // "10:00 UTC"
metar.ObservationDateTime // Objeto DateTime
```

### Status do Relatorio
```csharp
metar.Status // "AUTO", "NIL", ou vazio
```

### Vento de Superficie
```csharp
var vento = metar.SurfaceWind;
vento.MeanDirection.ActualValue   // Direcao em graus (0-360)
vento.MeanSpeed.ActualValue       // Valor da velocidade
vento.MeanSpeed.ActualUnit        // Unit.Knot, Unit.MeterPerSecond, Unit.KilometerPerHour
vento.SpeedVariations?.ActualValue // Rajada (se presente)
vento.VariableDirection           // true se VRB (variavel)
vento.DirectionVariations         // [min, max] variacao de direcao
```

### Visibilidade
```csharp
var vis = metar.Visibility;
vis.PrevailingVisibility.ActualValue  // Valor da visibilidade predominante
vis.PrevailingVisibility.ActualUnit   // Unit.Meter ou Unit.StatuteMile
vis.MinimumVisibility?.ActualValue    // Visibilidade minima (se presente)
vis.MinimumVisibilityDirection        // "NW", "NE", "S", etc.
vis.NDV                               // Sem Variacao Direcional
metar.Cavok                           // CAVOK (teto e visibilidade OK)
```

### Alcance Visual de Pista (RVR)
```csharp
foreach (var rvr in metar.RunwaysVisualRange)
{
    Console.WriteLine($"Pista: {rvr.Runway}");
    Console.WriteLine($"Alcance: {rvr.VisualRange?.ActualValue}");
    Console.WriteLine($"Variavel: {rvr.Variable}");
    if (rvr.Variable)
    {
        Console.WriteLine($"Min: {rvr.VisualRangeInterval[0].ActualValue}");
        Console.WriteLine($"Max: {rvr.VisualRangeInterval[1].ActualValue}");
    }
    Console.WriteLine($"Tendencia: {rvr.PastTendency}"); // U (subindo), D (descendo), N (sem mudanca)
}
```

### Tempo Presente
```csharp
foreach (var tp in metar.PresentWeather)
{
    Console.WriteLine($"Intensidade: {tp.IntensityProximity}"); // +, -, VC
    Console.WriteLine($"Descritor: {tp.Characteristics}");       // TS, FZ, SH, etc.
    foreach (var tipo in tp.Types)
    {
        Console.WriteLine($"Fenomeno: {tipo}"); // RA (chuva), SN (neve), FG (nevoeiro), etc.
    }
}
```

### Nuvens
```csharp
foreach (var nuvem in metar.Clouds)
{
    Console.WriteLine($"Quantidade: {nuvem.Amount}");     // FEW (poucas), SCT (esparsas), BKN (nublado), OVC (encoberto), VV (visib. vertical)
    Console.WriteLine($"Altura: {nuvem.BaseHeight?.ActualValue} pes");
    Console.WriteLine($"Tipo: {nuvem.Type}");             // CB (cumulonimbus), TCU (cumulus torre), ou NULL
}
```

### Temperatura
```csharp
metar.AirTemperature.ActualValue       // Temperatura do ar em Celsius
metar.DewPointTemperature.ActualValue  // Ponto de orvalho em Celsius
```

### Pressao
```csharp
metar.Pressure.ActualValue   // Valor da pressao
metar.Pressure.ActualUnit    // Unit.HectoPascal (QNH) ou Unit.MercuryInch (altimetro)
```

### Tempo Recente
```csharp
var tr = metar.RecentWeather;
tr?.Characteristics  // Descritor do tempo
tr?.Types            // Lista de fenomenos
```

### Cisalhamento de Vento (Wind Shear)
```csharp
metar.WindshearAllRunways  // true se WS ALL RWY (todas as pistas)
metar.WindshearRunways     // Lista de pistas afetadas
```

### Tendencia (NOSIG/BECMG/TEMPO)
```csharp
metar.TrendType      // "NOSIG" (sem mudanca significativa), "BECMG" (tornando-se), ou "TEMPO" (temporario)
metar.TrendForecast  // Conteudo bruto da tendencia
```

### Observacoes (RMK)
```csharp
metar.Remark           // Conteudo das observacoes
metar.SeaLevelPressure // Pressao ao nivel do mar (SLPnnn)
```

## Conversao de Valores

A classe `Value` suporta conversao de unidades:

```csharp
var velocidade = metar.SurfaceWind.MeanSpeed;
double nos = velocidade.GetConvertedValue(Value.Unit.Knot);
double mps = velocidade.GetConvertedValue(Value.Unit.MeterPerSecond);
double kph = velocidade.GetConvertedValue(Value.Unit.KilometerPerHour);
```

## Tratamento de Erros

```csharp
var metar = decoder.ParseNotStrict("METAR LFPO 231027Z DADOS_INVALIDOS");

if (!metar.IsValid)
{
    foreach (var ex in metar.DecodingExceptions)
    {
        Console.WriteLine($"Erro no decodificador: {ex.ChunkDecoder.GetType().Name}");
        Console.WriteLine($"Mensagem: {ex.Message}");
        Console.WriteLine($"METAR restante: {ex.RemainingMetar}");
    }
}

// Limpar erros
metar.ResetDecodingExceptions();
```

## Arquitetura

A biblioteca segue os principios de Arquitetura Limpa (Clean Architecture) e SOLID:

- **Padrao ChunkDecoder**: Cadeia de Responsabilidade - cada decodificador trata uma secao do METAR
- **Segregacao de Interface**: `IMetarChunkDecoder` define o contrato
- **Responsabilidade Unica**: Cada classe decodificadora trata exatamente um elemento do METAR
- **Aberto/Fechado**: Novos decodificadores podem ser adicionados sem modificar os existentes

### Ordem da Cadeia de Decodificacao
1. `ReportTypeChunkDecoder` - METAR/SPECI/COR
2. `IcaoChunkDecoder` - Codigo ICAO do aerodromo
3. `DatetimeChunkDecoder` - Dia/hora da observacao
4. `ReportStatusChunkDecoder` - Status AUTO/NIL
5. `SurfaceWindChunkDecoder` - Direcao/velocidade/rajadas do vento
6. `VisibilityChunkDecoder` - CAVOK ou valores de visibilidade
7. `RunwayVisualRangeChunkDecoder` - RVR das pistas
8. `PresentWeatherChunkDecoder` - Fenomenos meteorologicos atuais
9. `CloudChunkDecoder` - Camadas de nuvens
10. `TemperatureChunkDecoder` - Temperatura do ar/ponto de orvalho
11. `PressureChunkDecoder` - Ajuste de altimetro (QNH)
12. `RecentWeatherChunkDecoder` - Tempo recente (RE)
13. `WindShearChunkDecoder` - Cisalhamento de vento
14. `TrendChunkDecoder` - Tendencia (NOSIG/BECMG/TEMPO)
15. `RemarkChunkDecoder` - Secao de observacoes (RMK)

## Elementos METAR Suportados

| Elemento | Exemplo | Suportado |
|----------|---------|-----------|
| Tipo de Relatorio | METAR, SPECI, COR | Sim |
| Codigo ICAO | SBGR, KJFK | Sim |
| Data/Hora | 231027Z | Sim |
| Status | AUTO, NIL | Sim |
| Vento de Superficie | 24004KT, VRB02MPS, 24015G25KT | Sim |
| Variacao de Direcao | 180V270 | Sim |
| CAVOK | CAVOK | Sim |
| Visibilidade (ICAO) | 9999, 2500 | Sim |
| Visibilidade (EUA) | 3SM, 1 1/2SM | Sim |
| Visibilidade Minima | 1000NW | Sim |
| NDV | 9999NDV | Sim |
| RVR | R32/0400, R06L/0200V0600U | Sim |
| Tempo Presente | +TSRA, -SN, FZFG, VCSH | Sim |
| Nuvens | FEW020, SCT030CB, OVC005 | Sim |
| Visibilidade Vertical | VV003 | Sim |
| Ceu Claro | SKC, NSC, NCD, CLR | Sim |
| Temperatura | 17/10, M02/M05 | Sim |
| Pressao (QNH) | Q1009 | Sim |
| Pressao (Altimetro) | A2992 | Sim |
| Tempo Recente | REFZRA | Sim |
| Cisalhamento de Vento | WS R03, WS ALL RWY | Sim |
| Tendencia | NOSIG, BECMG, TEMPO | Sim |
| Observacoes | RMK AO2 SLP013 | Sim |
| Pressao ao Nivel do Mar | SLPnnn (no RMK) | Sim |

## Glossario de Termos METAR

| Codigo | Significado |
|--------|-------------|
| METAR | Relatorio Meteorologico de Aerodromo |
| SPECI | Relatorio Especial |
| CAVOK | Teto e Visibilidade OK (vis >10km, sem nuvens abaixo de 5000ft, sem tempo significativo) |
| NOSIG | Sem Mudanca Significativa |
| BECMG | Tornando-se (mudanca gradual esperada) |
| TEMPO | Temporario (flutuacao temporaria) |
| FEW | Poucas nuvens (1-2 oitavos) |
| SCT | Esparsas (3-4 oitavos) |
| BKN | Nublado (5-7 oitavos) |
| OVC | Encoberto (8 oitavos) |
| VV | Visibilidade Vertical |
| CB | Cumulonimbus |
| TCU | Cumulus em Torre |
| RA | Chuva |
| SN | Neve |
| FG | Nevoeiro |
| BR | Nevoa |
| TS | Trovoada |
| FZ | Congelante |
| SH | Pancadas |
| VRB | Variavel |
| RVR | Alcance Visual de Pista |

## Licenca

MIT License
