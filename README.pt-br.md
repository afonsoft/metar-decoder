# Decodificador METAR e TAF em C#

[![Licenca](https://img.shields.io/github/license/afonsoft/metar-decoder)](https://github.com/afonsoft/metar-decoder/blob/main/LICENSE)
[![Build](https://github.com/afonsoft/metar-decoder/actions/workflows/ci-build-test.yml/badge.svg?branch=main)](https://github.com/afonsoft/metar-decoder/actions/workflows/ci-build-test.yml)
[![Code Quality](https://github.com/afonsoft/metar-decoder/actions/workflows/code-quality.yml/badge.svg?branch=main)](https://github.com/afonsoft/metar-decoder/actions/workflows/code-quality.yml)
[![Security Scan](https://github.com/afonsoft/metar-decoder/actions/workflows/security-scan.yml/badge.svg?branch=main)](https://github.com/afonsoft/metar-decoder/actions/workflows/security-scan.yml)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=alert_status)](https://sonarcloud.io/project/overview?id=afonsoft_metar-decoder)
[![Cobertura](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=coverage)](https://sonarcloud.io/project/overview?id=afonsoft_metar-decoder)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=code_smells)](https://sonarcloud.io/project/overview?id=afonsoft_metar-decoder)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=bugs)](https://sonarcloud.io/project/overview?id=afonsoft_metar-decoder)
[![Manutenibilidade](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=sqale_rating)](https://sonarcloud.io/project/overview?id=afonsoft_metar-decoder)
[![Metar.Decoder NuGet](https://img.shields.io/nuget/v/Metar.Decoder.svg)](https://www.nuget.org/packages/Metar.Decoder/)
[![Taf.Decoder NuGet](https://img.shields.io/nuget/v/Taf.Decoder.svg)](https://www.nuget.org/packages/Taf.Decoder/)
[![GitHub issues](https://img.shields.io/github/issues/afonsoft/metar-decoder)](https://github.com/afonsoft/metar-decoder/issues)
![GitHub top language](https://img.shields.io/github/languages/top/afonsoft/metar-decoder)

> **[Read in English (en-US)](README.md)**

## Documentacao

- **[Guia de Uso (Portugues)](docs/pt-BR/guia-de-uso.md)** - Guia completo com exemplos
- **[Referencia da API (Portugues)](docs/pt-BR/referencia-api.md)** - Todas as classes, propriedades e metodos
- **[Usage Guide (English)](docs/en-US/usage-guide.md)** - Complete guide with examples
- **[API Reference (English)](docs/en-US/api-reference.md)** - All classes, properties and methods

## Descricao do Projeto

Este repositorio contem uma biblioteca .NET para decodificar strings METAR (Meteorological Aerodrome Report) e TAF (Terminal Aerodrome Forecast). Ambas as bibliotecas sao compativeis com .NET Standard 2.0, .NET 8.0, .NET 10.0 e .NET Framework 4.8, permitindo sua utilizacao em projetos .NET Core e .NET Framework.

### Decodificador METAR

O decodificador METAR e uma ferramenta robusta para analisar e interpretar relatorios meteorologicos METAR brutos. METAR e um formato padronizado pela Organizacao da Aviacao Civil Internacional (ICAO) para relatar informacoes meteorologicas, amplamente utilizado por pilotos e meteorologistas para previsao do tempo. A biblioteca processa a string METAR e a transforma em um objeto `DecodedMetar` estruturado, que contem todas as propriedades meteorologicas decodificadas, como vento de superficie, visibilidade, alcance visual da pista, tempo presente, camadas de nuvens, temperatura do ar, temperatura do ponto de orvalho e pressao.

### Decodificador TAF

O decodificador TAF e uma biblioteca .NET para decodificar mensagens TAF (Terminal Aerodrome Forecast), que sao previsoes meteorologicas para aerodromos. Assim como o METAR, o formato TAF e altamente padronizado pela ICAO e e crucial para o planejamento de voos. A biblioteca analisa a string TAF e a converte em um objeto `DecodedTaf`, que inclui informacoes como tipo de relatorio, codigo ICAO, data e hora de origem, periodo de previsao, vento de superficie, visibilidade, fenomenos meteorologicos, camadas de nuvens e temperaturas minima e maxima. O decodificador TAF tambem e capaz de interpretar "evolucoes" (mudancas na previsao ao longo do tempo), como `BECMG` (tornando-se) e `TEMPO` (temporario).

Ambos os decodificadores oferecem modos de analise "estrito" e "nao estrito". No modo nao estrito, a analise continua mesmo que sejam encontrados erros de formato, e as excecoes sao registradas na propriedade `DecodingExceptions` do objeto decodificado. Valores numericos com unidades (como velocidade, distancia e pressao) sao encapsulados em objetos `Value`, permitindo conversoes de unidade flexiveis.

Este projeto e amplamente baseado nas implementacoes de [SafranCassiopee/csharp-metar-decoder](https://github.com/SafranCassiopee/csharp-metar-decoder) e [SafranCassiopee/csharp-taf-decoder](https://github.com/SafranCassiopee/csharp-taf-decoder).

## Status do Projeto

**Ativo e em Desenvolvimento** - Com pipelines modernos de CI/CD

### Novidades Recentes

- **Cobertura de Testes ~98%** - 421 testes unitarios com cobertura abrangente
- **Fix DatetimeChunkDecoder** - Correcao de bug de rollover de dia/mes invalido
- **Suporte RTD** - Suporte completo para relatorios TAF com "Report Delayed"
- **.NET 10.0** - Compatibilidade com a versao mais recente do .NET
- **Workflows Modernos** - CI/CD automatizado com GitHub Actions

## CI/CD e Workflows

Este projeto utiliza pipelines modernos de GitHub Actions para garantir qualidade e automacao:

### Workflows Disponiveis

- **CI Build & Test** (`ci-build-test.yml`) - Pipeline completo de integracao continua
  - Build automatizado para .NET 8.0
  - Testes unitarios com coverage
  - Security scans e performance tests
  - Criacao automatica de PRs

- **Code Quality** (`code-quality.yml`) - Analise de qualidade de codigo
  - Qodana analysis
  - SonarQube integration
  - Snyk security scanning
  - Metricas de qualidade

- **Security Scan** (`security-scan.yml`) - Scans de seguranca
  - CodeQL analysis
  - Vulnerability scanning
  - Scans semanais automaticos

- **Publish NuGet** (`publish-all.yml`) - Publicacao automatizada
  - Publicacao para GitHub Packages
  - Publicacao para NuGet.org
  - Criacao automatica de releases

- **Auto Dependency Update** (`auto-pr-from-main.yml`) - Atualizacao automatica
  - Verificacao de dependencias desatualizadas
  - Updates de seguranca automaticos
  - PRs automaticos para updates

### Badges de Qualidade

[![CI/CD Pipeline](https://github.com/afonsoft/metar-decoder/actions/workflows/ci-build-test.yml/badge.svg)](https://github.com/afonsoft/metar-decoder/actions/workflows/ci-build-test.yml)
[![Code Quality](https://github.com/afonsoft/metar-decoder/actions/workflows/code-quality.yml/badge.svg)](https://github.com/afonsoft/metar-decoder/actions/workflows/code-quality.yml)
[![Security Scan](https://github.com/afonsoft/metar-decoder/actions/workflows/security-scan.yml/badge.svg)](https://github.com/afonsoft/metar-decoder/actions/workflows/security-scan.yml)

## Pacotes NuGet

Os pacotes NuGet oficiais estao disponiveis para facil integracao em seus projetos:

| Pacote | Versao | NuGet |
| ------ | ------ | ----- |
| [Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder/) | 1.0.9 | [![NuGet version](https://badge.fury.io/nu/Metar.Decoder.svg)](https://badge.fury.io/nu/Metar.Decoder) |
| [Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder/) | 1.0.7 | [![NuGet version](https://badge.fury.io/nu/Taf.Decoder.svg)](https://badge.fury.io/nu/Taf.Decoder) |

## Pre-requisitos

Esta biblioteca e compativel com multiplas versoes do .NET:

- **.NET Standard 2.0** - Compatibilidade maxima
- **.NET 8.0** - LTS recomendado
- **.NET 10.0** - Versao mais recente
- **.NET Framework 4.8** - Suporte legado

## Como Instalar

### NuGet Package Manager (recomendado)

No Console do Gerenciador de Pacotes no Visual Studio:

```shell
Install-Package Metar.Decoder
Install-Package Taf.Decoder
```

### .NET CLI

```shell
dotnet add package Metar.Decoder
dotnet add package Taf.Decoder
```

### PackageReference

```xml
<PackageReference Include="Metar.Decoder" Version="1.0.9" />
<PackageReference Include="Taf.Decoder" Version="1.0.7" />
```

Adicione uma referencia a biblioteca e, em seguida, adicione as seguintes diretivas `using`:

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;
```

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;
```

### Manualmente

Baixe a versao mais recente em [GitHub Releases](https://github.com/afonsoft/metar-decoder/releases).

## Inicio Rapido

### Decodificador METAR

Instancie o decodificador e execute-o em uma string METAR. O objeto retornado e um objeto `DecodedMetar` do qual voce pode recuperar todas as propriedades meteorologicas decodificadas.

Todos os valores que possuem uma unidade sao baseados no objeto `Value`, que fornece as propriedades `ActualValue` e `ActualUnit`.

Consulte a classe [`DecodedMetar`](src/Metar.Decoder/Entity/DecodedMetar.cs) para a estrutura do objeto resultante.

```csharp
var d = MetarDecoder.ParseWithMode("METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03");

// Informacoes de contexto
Console.WriteLine($"Valido: {d.IsValid}"); // true
Console.WriteLine($"METAR Bruto: {d.RawMetar}");
Console.WriteLine($"Tipo: {d.Type}"); // MetarType.METAR
Console.WriteLine($"ICAO: {d.ICAO}"); // "LFPO"
Console.WriteLine($"Dia: {d.Day}"); // 23
Console.WriteLine($"Hora: {d.Time}"); // "10:27 UTC"
Console.WriteLine($"Status: {d.Status}"); // "AUTO"

// Vento de superficie
var sw = d.SurfaceWind;
Console.WriteLine($"Vento - Direcao Media: {sw.MeanDirection.ActualValue}"); // 240
Console.WriteLine($"Vento - Velocidade Media: {sw.MeanSpeed.ActualValue} {sw.MeanSpeed.ActualUnit}"); // 4 MeterPerSecond
Console.WriteLine($"Vento - Variacoes de Velocidade: {sw.SpeedVariations.ActualValue}"); // 9

// Visibilidade
var v = d.Visibility;
Console.WriteLine($"Visibilidade Prevalecente: {v.PrevailingVisibility.ActualValue} {v.PrevailingVisibility.ActualUnit}"); // 2500 Meter
Console.WriteLine($"Visibilidade Minima: {v.MinimumVisibility.ActualValue}"); // 1000
Console.WriteLine($"Direcao da Visibilidade Minima: {v.MinimumVisibilityDirection}"); // "NW"

// Alcance Visual da Pista (RVR)
var rvr = d.RunwaysVisualRange;
Console.WriteLine($"RVR Pista 32: {rvr[0].VisualRange.ActualValue}"); // 400
Console.WriteLine($"RVR Pista 08C: {rvr[1].VisualRange.ActualValue}"); // 4

// Tempo Presente
var pw = d.PresentWeather;
Console.WriteLine($"Tempo Presente 1: {pw[0].IntensityProximity}{string.Join("", pw[0].Characteristics)}{string.Join("", pw[0].Types)}"); // "+FZRA"

// Nuvens
var cld = d.Clouds;
Console.WriteLine($"Nuvens - Quantidade: {cld[0].Amount}"); // FEW
Console.WriteLine($"Nuvens - Altura da Base: {cld[0].BaseHeight.ActualValue} {cld[0].BaseHeight.ActualUnit}"); // 1500 Feet

// Temperatura
Console.WriteLine($"Temperatura do Ar: {d.AirTemperature.ActualValue} {d.AirTemperature.ActualUnit}"); // 17 DegreeCelsius
Console.WriteLine($"Temperatura do Ponto de Orvalho: {d.DewPointTemperature.ActualValue}"); // 10

// Pressao
Console.WriteLine($"Pressao: {d.Pressure.ActualValue} {d.Pressure.ActualUnit}"); // 1009 HectoPascal

// Tempo Recente
var rw = d.RecentWeather;
Console.WriteLine($"Tempo Recente - Caracteristicas: {rw.Characteristics}"); // "FZ"
Console.WriteLine($"Tempo Recente - Tipos: {string.Join(", ", rw.Types)}"); // "RA"

// Tesouras de Vento (Windshear)
Console.WriteLine($"Windshear em Todas as Pistas: {d.WindshearAllRunways}");
Console.WriteLine($"Windshear em Pistas Especificas: {string.Join(", ", d.WindshearRunways)}"); // "03"
```

### Decodificador TAF

Instancie o decodificador e execute-o em uma string TAF. O objeto retornado e um objeto `DecodedTaf` do qual voce pode recuperar todas as propriedades de previsao decodificadas.

Consulte a classe [`DecodedTaf`](src/Taf.Decoder/Entity/DecodedTaf.cs) para a estrutura do objeto resultante.

#### Suporte a RTD (Report Delayed)

O decodificador agora suporta relatorios TAF marcados como "RTD" (Report Delayed), que indicam relatorios atrasados:

```csharp
string rtdTaf = "RTD EKEB 190416Z 1905/1912 13006KT 0200 FZFG BKN001 TEMPO 1905/1907 2000 BR BKN003 BECMG 1907/1909 9000 NSW FEW002 PROB40 1909/1911 0400 FZFG BKN002=";
var decoder = new TafDecoder();
var result = decoder.Parse(rtdTaf);

Console.WriteLine($"Tipo: {result.Type}"); // Saida: RTD
Console.WriteLine($"ICAO: {result.Icao}"); // Saida: EKEB
Console.WriteLine($"Valido: {result.IsValid}"); // Saida: True
```

#### Exemplo Completo de Uso TAF

```csharp
var d = TafDecoder.ParseWithMode("TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025 TX12/0816Z TN04/0807Z");

// Informacoes de contexto
Console.WriteLine($"Valido: {d.IsValid}");
Console.WriteLine($"TAF Bruto: {d.RawTaf}");
Console.WriteLine($"Tipo: {d.Type}"); // Pode ser: TAF, TAFAMD, TAFCOR, RTD
Console.WriteLine($"ICAO: {d.Icao}");
Console.WriteLine($"Dia: {d.Day}");
Console.WriteLine($"Hora: {d.Time}");

// Periodo de Previsao
var fp = d.ForecastPeriod;
Console.WriteLine($"Periodo de Previsao - Do Dia: {fp.FromDay}");
Console.WriteLine($"Periodo de Previsao - Da Hora: {fp.FromHour}");
Console.WriteLine($"Periodo de Previsao - Ao Dia: {fp.ToDay}");
Console.WriteLine($"Periodo de Previsao - A Hora: {fp.ToHour}");

// Vento de superficie
var swTaf = d.SurfaceWind;
Console.WriteLine($"TAF Vento - Direcao Media: {swTaf.MeanDirection.ActualValue}");
Console.WriteLine($"TAF Vento - Velocidade Media: {swTaf.MeanSpeed.ActualValue} {swTaf.MeanSpeed.ActualUnit}");

// Visibilidade
var vTaf = d.Visibility;
Console.WriteLine($"TAF Visibilidade Prevalecente: {vTaf.ActualVisibility.ActualValue} {vTaf.ActualVisibility.ActualUnit}");
Console.WriteLine($"TAF CAVOK: {d.Cavok}");

// Nuvens
var cldTaf = d.Clouds;
if (cldTaf.Count > 0)
{
    Console.WriteLine($"TAF Nuvens - Quantidade: {cldTaf[0].Amount}");
    Console.WriteLine($"TAF Nuvens - Altura da Base: {cldTaf[0].BaseHeight.ActualValue} {cldTaf[0].BaseHeight.ActualUnit}");
}

// Temperaturas (Minima e Maxima)
var minTemp = d.MinimumTemperature;
if (minTemp != null)
{
    Console.WriteLine($"Temperatura Minima: {minTemp.TemperatureValue.ActualValue} {minTemp.TemperatureValue.ActualUnit} no dia {minTemp.Day} as {minTemp.Hour}Z");
}
var maxTemp = d.MaximumTemperature;
if (maxTemp != null)
{
    Console.WriteLine($"Temperatura Maxima: {maxTemp.TemperatureValue.ActualValue} {maxTemp.TemperatureValue.ActualUnit} no dia {maxTemp.Day} as {maxTemp.Hour}Z");
}

// Fenomenos Meteorologicos
var wpTaf = d.WeatherPhenomenons;
if (wpTaf.Count > 0)
{
    Console.WriteLine($"TAF Fenomeno Meteorologico: {wpTaf[0].IntensityProximity}{string.Join("", wpTaf[0].Characteristics)}{string.Join("", wpTaf[0].Types)}");
}

// Evolucoes (BECMG, TEMPO, etc.)
foreach (var evolution in d.Evolutions)
{
    Console.WriteLine($"Evolucao: {evolution.Type} de {evolution.FromDay}{evolution.FromHour}Z a {evolution.ToDay}{evolution.ToHour}Z");
}
```

#### Tipos de Relatorio TAF Suportados

| Tipo | Descricao | Exemplo |
|------|-----------|---------|
| `TAF` | Relatorio TAF padrao | `TAF LEMD 080500Z...` |
| `TAFAMD` | Relatorio TAF amendado | `TAF AMD LEMD 080500Z...` |
| `TAFCOR` | Relatorio TAF corrigido | `TAF COR LEMD 080500Z...` |
| `RTD` | Relatorio TAF atrasado | `RTD EKEB 190416Z...` |

### Sobre Objetos de Valor (`Value`)

No exemplo acima, assume-se que todos os parametros solicitados estao disponiveis. No mundo real, alguns campos nao sao obrigatorios, portanto, e importante verificar se o objeto `Value` (contendo tanto o valor quanto sua unidade) nao e nulo antes de usa-lo.

```csharp
var dew_point = d.DewPointTemperature;
if (dew_point == null)
{
    dew_point = new Value(999, Value.Unit.DegreeCelsius);
}

Console.WriteLine(dew_point.ActualValue);
Console.WriteLine(dew_point.ActualUnit);
```

Os objetos `Value` tambem contem sua unidade, que pode ser acessada com a propriedade `ActualUnit`. Ao acessar a propriedade `ActualValue`, voce obtera o valor nesta unidade.

Se voce deseja obter o valor diretamente em outra unidade, pode chamar `GetConvertedValue(unit)`. Os valores suportados sao velocidade, distancia e pressao.

Unidades disponiveis para conversao:

```csharp
// Unidades de velocidade:
// Value.Unit.MeterPerSecond
// Value.Unit.KilometerPerHour
// Value.Unit.Knot

// Unidades de distancia:
// Value.Unit.Meter
// Value.Unit.Feet
// Value.Unit.StatuteMile

// Unidades de pressao:
// Value.Unit.HectoPascal
// Value.Unit.MercuryInch

// Usando conversao em tempo real
var distance_in_sm = visibility.GetConvertedValue(Value.Unit.StatuteMile);
var speed_kph = speed.GetConvertedValue(Value.Unit.KilometerPerHour);
```

### Sobre Erros de Analise

Quando um formato inesperado e encontrado, o erro de analise e registrado no proprio objeto `DecodedMetar` ou `DecodedTaf`. Todos os erros podem ser acessados atraves da propriedade `DecodingExceptions`.

Por padrao, a analise continuara quando um formato incorreto for encontrado. O analisador tambem oferece um modo "estrito" onde a analise para assim que uma nao conformidade e detectada:

```csharp
var decoder = new MetarDecoder();

// Altera o modo de analise global para "estrito"
decoder.SetStrictParsing(true);

// Esta analise sera feita no modo estrito
decoder.Parse("...");

// Esta ignorara o modo global e sera feita no modo nao estrito
MetarDecoder.ParseNotStrict("...");

// Altera o modo de analise global para "nao estrito"
decoder.SetStrictParsing(false);

// Esta analise sera feita no modo nao estrito
decoder.Parse("...");

// Esta ignorara o modo global e sera feita no modo estrito
MetarDecoder.ParseStrict("...");
```

## Como Contribuir

1. **Crie uma branch** a partir da `main`:
   ```bash
   git checkout -b feature/sua-feature
   ```

2. **Faca suas alteracoes** seguindo as boas praticas

3. **Os workflows automaticos** serao executados:
   - **CI Build & Test** - Valida seu codigo
   - **Code Quality** - Analisa qualidade
   - **Security Scan** - Verifica seguranca

4. **Pull Request Automatico**: Se estiver em branches `feature/*`, `bug/*` ou `hotfix/*`, um PR sera criado automaticamente para `main`

5. **Review e Merge**: Apos aprovacao, seu codigo sera mergeado

## Historico de Estrelas

[![Star History Chart](https://api.star-history.com/svg?repos=afonsoft/metar-decoder&type=Date)](https://star-history.com/#afonsoft/metar-decoder&Date)

## StarMapper

[![Mapa StarMapper](https://starmapper.bruniaux.com/afonsoft/metar-decoder/opengraph-image)](https://starmapper.bruniaux.com/afonsoft/metar-decoder)

## Licenca

Este projeto esta licenciado sob a Licenca MIT. Consulte o arquivo [LICENSE](LICENSE) para mais detalhes.
