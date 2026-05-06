# Metar.Decoder & Taf.Decoder - Referencia da API (pt-BR)

Referencia completa de todas as classes publicas, propriedades, metodos e enumeracoes.

Para a versao em ingles, consulte: **[API Reference (en-US)](../en-US/api-reference.md)**

## Indice

- [Namespace Metar.Decoder](#namespace-metardecoder)
  - [MetarDecoder](#classe-metardecoder)
  - [DecodedMetar](#classe-decodedmetar)
  - [SurfaceWind](#classe-surfacewind)
  - [Visibility](#classe-visibility)
  - [RunwayVisualRange](#classe-runwayvisualrange)
  - [CloudLayer](#classe-cloudlayer)
  - [WeatherPhenomenon](#classe-weatherphenomenon)
  - [Value](#classe-value)
- [Namespace Taf.Decoder](#namespace-tafdecoder)
  - [TafDecoder](#classe-tafdecoder)
  - [DecodedTaf](#classe-decodedtaf)
  - [ForecastPeriod](#classe-forecastperiod)
  - [Temperature](#classe-temperature)
  - [Evolution](#classe-evolution)
- [Enumeracoes](#enumeracoes)

---

## Namespace Metar.Decoder

### Classe MetarDecoder

Classe principal para decodificacao de strings METAR.

**Namespace**: `Metar.Decoder`

#### Construtores

```csharp
public MetarDecoder()
```

Cria uma nova instancia do decodificador METAR. Define um timeout de regex de 500ms para seguranca.

#### Metodos

| Metodo | Retorno | Descricao |
|--------|---------|-----------|
| `Parse(string rawMetar)` | `DecodedMetar` | Decodifica uma string METAR usando a configuracao global de modo estrito |
| `ParseStrict(string rawMetar)` | `DecodedMetar` | Decodifica no modo estrito (para no primeiro erro) |
| `ParseNotStrict(string rawMetar)` | `DecodedMetar` | Decodifica no modo nao estrito (continua em erros) |
| `ParseWithMode(string rawMetar, bool isStrict = false)` | `DecodedMetar` | **Estatico**. Decodifica com o modo especificado |
| `SetStrictParsing(bool isStrict)` | `void` | Define o modo de analise global |

---

### Classe DecodedMetar

Representa um relatorio METAR completamente decodificado.

**Namespace**: `Metar.Decoder.Entity`

#### Propriedades

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `RawMetar` | `string` | String METAR bruta original (sem espacos extras) |
| `Type` | `MetarType` | Tipo de relatorio: `METAR`, `METAR_COR`, `SPECI`, `SPECI_COR`, `NULL` |
| `ICAO` | `string` | Identificador ICAO de 4 letras do aeroporto (ex.: `"LFPO"`, `"SBGR"`) |
| `Day` | `int?` | Dia do mes da observacao (1-31) |
| `Time` | `string` | Hora da observacao (ex.: `"10:27 UTC"`) |
| `ObservationDateTime` | `DateTime?` | Data/hora completa da observacao |
| `Status` | `string` | Status: `"AUTO"` (automatizado), `"NIL"` (ausente), ou vazio |
| `SurfaceWind` | `SurfaceWind` | Dados de vento de superficie |
| `Visibility` | `Visibility` | Dados de visibilidade |
| `Cavok` | `bool` | `true` se CAVOK foi reportado |
| `RunwaysVisualRange` | `List<RunwayVisualRange>` | RVR para cada pista reportada |
| `PresentWeather` | `List<WeatherPhenomenon>` | Condicoes meteorologicas atuais |
| `Clouds` | `List<CloudLayer>` | Camadas de nuvens |
| `AirTemperature` | `Value` | Temperatura do ar (unidade: `DegreeCelsius`) |
| `DewPointTemperature` | `Value` | Temperatura do ponto de orvalho |
| `Pressure` | `Value` | Pressao atmosferica (QNH) |
| `RecentWeather` | `WeatherPhenomenon` | Tempo recente |
| `WindshearAllRunways` | `bool?` | Se windshear em todas as pistas |
| `WindshearRunways` | `List<string>` | Pistas com windshear |
| `TrendType` | `string` | Tipo de tendencia: `"NOSIG"`, `"BECMG"`, `"TEMPO"` |
| `TrendForecast` | `string` | Conteudo bruto da previsao de tendencia |
| `Remark` | `string` | Conteudo bruto das observacoes (apos RMK) |
| `SeaLevelPressure` | `Value` | Pressao ao nivel do mar (SLPnnn) |
| `DecodingExceptions` | `ReadOnlyCollection<MetarChunkDecoderException>` | Erros de analise |

#### Metodos

| Metodo | Retorno | Descricao |
|--------|---------|-----------|
| `IsValid` | `bool` | Retorna `true` se nenhuma excecao de decodificacao ocorreu |
| `AddDecodingException(...)` | `void` | Adiciona uma excecao de decodificacao |
| `ResetDecodingExceptions()` | `void` | Limpa todas as excecoes |

---

### Classe SurfaceWind

Informacoes de vento de superficie.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `MeanDirection` | `Value` | Direcao media do vento (unidade: `Degree`, 0-360) |
| `VariableWind` | `bool` | `true` se a direcao do vento e variavel (VRB) |
| `MeanSpeed` | `Value` | Velocidade media do vento |
| `SpeedVariations` | `Value` | Velocidade de rajada, se reportada |
| `DirectionVariations` | `Value[]` | Array de 2 valores [min, max] para faixa de direcao variavel |

---

### Classe Visibility

Informacoes de visibilidade.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `PrevailingVisibility` | `Value` | Visibilidade principal (unidade: `Meter` ou `StatuteMile`) |
| `MinimumVisibility` | `Value` | Visibilidade direcional minima |
| `MinimumVisibilityDirection` | `string` | Direcao da visibilidade minima (ex.: `"NW"`, `"SE"`) |
| `NDV` | `bool` | Sem Variacao Direcional |
| `HasCavok` | `bool` | Se CAVOK foi definido |

---

### Classe RunwayVisualRange

Alcance visual da pista (RVR).

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `Runway` | `string` | Designador da pista (ex.: `"32"`, `"08C"`, `"26L"`) |
| `VisualRange` | `Value` | Valor do alcance visual |
| `PastTendency` | `string` | Indicador de tendencia: `"U"`, `"D"`, `"N"`, ou vazio |
| `VisualRangeInterval` | `Value[]` | Valores min/max se um intervalo foi reportado |
| `Variable` | `bool` | Se um intervalo variavel foi reportado |

---

### Classe CloudLayer

Camada de nuvens individual.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `Amount` | `string` | Cobertura: `"FEW"` (1-2 oitavos), `"SCT"` (3-4), `"BKN"` (5-7), `"OVS"` (8) |
| `BaseHeight` | `Value` | Altura da base da nuvem acima do solo (unidade: `Feet`) |
| `Type` | `string` | Tipo: `"CB"` (Cumulonimbus), `"TCU"` (Cumulus imponente), ou vazio |

---

### Classe WeatherPhenomenon

Fenomeno meteorologico (presente ou recente).

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `IntensityProximity` | `string` | `"+"` (forte), `"-"` (leve), `"VC"` (vizinhanca), ou vazio (moderado) |
| `Characteristics` | `List<string>` | Codigos descritores: `"MI"`, `"BC"`, `"PR"`, `"DR"`, `"BL"`, `"SH"`, `"TS"`, `"FZ"` |
| `Types` | `List<string>` | Tipos de precipitacao/obscurecimento: `"RA"`, `"SN"`, `"DZ"`, `"FG"`, `"BR"`, etc. |

#### Codigos Meteorologicos Comuns

| Codigo | Descricao | Categoria |
|--------|-----------|-----------|
| `RA` | Chuva | Precipitacao |
| `SN` | Neve | Precipitacao |
| `DZ` | Chuvisco | Precipitacao |
| `GR` | Granizo | Precipitacao |
| `GS` | Granizo pequeno / Grao de neve | Precipitacao |
| `PL` | Pelotas de gelo | Precipitacao |
| `FG` | Nevoeiro (visibilidade < 1000m) | Obscurecimento |
| `BR` | Nevoa (visibilidade 1000-5000m) | Obscurecimento |
| `HZ` | Nevoa seca | Obscurecimento |
| `FU` | Fumaca | Obscurecimento |
| `SA` | Areia | Obscurecimento |
| `DU` | Poeira | Obscurecimento |
| `VA` | Cinza vulcanica | Obscurecimento |
| `TS` | Trovoada | Descritor |
| `SH` | Pancadas | Descritor |
| `FZ` | Congelante | Descritor |
| `BL` | Soprado | Descritor |
| `SQ` | Rajada | Outro |
| `FC` | Nuvem funil / Tornado | Outro |

---

### Classe Value

Valor numerico com unidade associada. Suporta conversao de unidades.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `ActualValue` | `double` | Valor numerico na unidade original |
| `ActualUnit` | `Value.Unit` | Unidade de medida |

#### Metodos

| Metodo | Retorno | Descricao |
|--------|---------|-----------|
| `GetConvertedValue(Value.Unit unitTo)` | `float` | Converte o valor para a unidade especificada |
| `ToString()` | `string` | Retorna `"{ActualValue} {ActualUnit}"` |
| `ToInt(string value)` | `int?` | **Estatico**. Converte string codificada METAR para inteiro |

---

## Namespace Taf.Decoder

### Classe TafDecoder

Classe principal para decodificacao de strings TAF.

#### Metodos

| Metodo | Retorno | Descricao |
|--------|---------|-----------|
| `Parse(string rawTaf)` | `DecodedTaf` | Decodifica usando configuracao global |
| `ParseStrict(string rawTaf)` | `DecodedTaf` | Decodifica no modo estrito |
| `ParseNotStrict(string rawTaf)` | `DecodedTaf` | Decodifica no modo nao estrito |
| `ParseWithMode(string rawTaf, bool isStrict = false)` | `DecodedTaf` | **Estatico**. Decodifica com modo especificado |
| `SetStrictParsing(bool isStrict)` | `void` | Define modo de analise global |

---

### Classe DecodedTaf

Representa uma previsao TAF completamente decodificada.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `RawTaf` | `string` | String TAF bruta original |
| `Type` | `TafType` | Tipo: `TAF`, `TAFAMD`, `TAFCOR`, `RTD`, `NULL` |
| `Icao` | `string` | Identificador ICAO do aeroporto |
| `Day` | `int?` | Dia de origem |
| `Time` | `string` | Hora de origem |
| `OriginDateTime` | `DateTime?` | Data/hora de origem |
| `ForecastPeriod` | `ForecastPeriod` | Periodo de previsao valido |
| `SurfaceWind` | `SurfaceWind` | Dados de vento |
| `Visibility` | `Visibility` | Dados de visibilidade |
| `Cavok` | `bool` | Se CAVOK foi reportado |
| `WeatherPhenomenons` | `List<WeatherPhenomenon>` | Fenomenos meteorologicos |
| `Clouds` | `List<CloudLayer>` | Camadas de nuvens |
| `MinimumTemperature` | `Temperature` | Temperatura minima prevista (TNnn/ddhhZ) |
| `MaximumTemperature` | `Temperature` | Temperatura maxima prevista (TXnn/ddhhZ) |
| `Evolutions` | `List<Evolution>` | Mudancas de previsao (BECMG, TEMPO, PROB) |
| `DecodingExceptions` | `ReadOnlyCollection<TafChunkDecoderException>` | Erros de analise |

---

### Classe ForecastPeriod

Periodo de validade da previsao TAF.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `FromDay` | `int?` | Dia de inicio |
| `FromHour` | `int?` | Hora de inicio (UTC) |
| `ToDay` | `int?` | Dia de termino |
| `ToHour` | `int?` | Hora de termino (UTC) |

---

### Classe Temperature

Temperatura prevista com horario associado.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `TemperatureValue` | `Value` | Valor da temperatura (unidade: `DegreeCelsius`) |
| `Day` | `int?` | Dia da temperatura prevista |
| `Hour` | `int?` | Hora (UTC) da temperatura prevista |

---

### Classe Evolution

Mudanca de previsao (evolucao) dentro de um TAF.

| Propriedade | Tipo | Descricao |
|-------------|------|-----------|
| `Type` | `string` | Tipo: `"BECMG"`, `"TEMPO"` |
| `FromDay` | `int?` | Dia de inicio da evolucao |
| `FromHour` | `int?` | Hora de inicio da evolucao |
| `ToDay` | `int?` | Dia de termino da evolucao |
| `ToHour` | `int?` | Hora de termino da evolucao |
| `Probability` | `int?` | Porcentagem de probabilidade (30 ou 40) |
| `SurfaceWind` | `SurfaceWind` | Mudancas de vento |
| `Visibility` | `Visibility` | Mudancas de visibilidade |
| `Cavok` | `bool` | Se CAVOK se aplica |
| `WeatherPhenomenons` | `List<WeatherPhenomenon>` | Mudancas de tempo |
| `Clouds` | `List<CloudLayer>` | Mudancas de nuvens |

---

## Enumeracoes

### MetarType

| Valor | Descricao |
|-------|-----------|
| `NULL` | Nao determinado |
| `METAR` | Relatorio METAR padrao |
| `METAR_COR` | Relatorio METAR corrigido |
| `SPECI` | Relatorio METAR especial |
| `SPECI_COR` | Relatorio METAR especial corrigido |

### TafType

| Valor | Descricao |
|-------|-----------|
| `NULL` | Nao determinado |
| `TAF` | Previsao TAF padrao |
| `TAFAMD` | Previsao TAF amendada |
| `TAFCOR` | Previsao TAF corrigida |
| `RTD` | Relatorio Atrasado |

### Value.Unit

| Valor | Descricao | Abreviacao |
|-------|-----------|------------|
| `None` | Sem unidade | |
| `DegreeCelsius` | Temperatura em Celsius | deg C |
| `Degree` | Direcao em graus | deg |
| `Knot` | Velocidade em nos | kt |
| `MeterPerSecond` | Velocidade em m/s | m/s |
| `KilometerPerHour` | Velocidade em km/h | km/h |
| `Meter` | Distancia em metros | m |
| `Feet` | Distancia em pes | ft |
| `StatuteMile` | Distancia em milhas terrestres | SM |
| `HectoPascal` | Pressao em hectopascais | hPa |
| `MercuryInch` | Pressao em polegadas de mercurio | inHg |
| `UnknownUnit` | Unidade desconhecida | N/A |

---

## Repositorio e Issues

- **Repositorio**: [https://github.com/afonsoft/metar-decoder](https://github.com/afonsoft/metar-decoder)
- **NuGet (Metar)**: [https://www.nuget.org/packages/Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder)
- **NuGet (TAF)**: [https://www.nuget.org/packages/Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder)
- **Issues**: [https://github.com/afonsoft/metar-decoder/issues](https://github.com/afonsoft/metar-decoder/issues)
- **Licenca**: MIT
