
# Decodificador METAR e TAF em C#

[![GitHub license](https://img.shields.io/github/license/afonsoft/metar-decoder)](https://github.com/afonsoft/metar-decoder/blob/main/LICENSE)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=code_smells)](https://sonarcloud.io/dashboard?id=afonsoft_metar-decoder)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=bugs)](https://sonarcloud.io/dashboard?id=afonsoft_metar-decoder)
![GitHub top language](https://img.shields.io/github/languages/top/afonsoft/metar-decoder)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=alert_status)](https://sonarcloud.io/dashboard?id=afonsoft_metar-decoder)
[![GitHub issues](https://img.shields.io/github/issues/afonsoft/metar-decoder)](https://github.com/afonsoft/metar-decoder/issues)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=afonsoft_metar-decoder)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=afonsoft_metar-decoder&metric=coverage)](https://sonarcloud.io/summary/new_code?id=afonsoft_metar-decoder)
[![GitHub all releases](https://img.shields.io/github/downloads/afonsoft/metar-decoder/total)](https://github.com/afonsoft/metar-decoder/releases)

## Descrição do Projeto

Este repositório contém uma biblioteca .NET para decodificar strings METAR (Meteorological Aerodrome Report) e TAF (Terminal Aerodrome Forecast). Ambas as bibliotecas são compatíveis com .NET Standard 2.0, .NET 6.0 e .NET 8.0, permitindo sua utilização em projetos .NET Core e .NET Framework.

### Decodificador METAR

O decodificador METAR é uma ferramenta robusta para analisar e interpretar relatórios meteorológicos METAR brutos. METAR é um formato padronizado pela Organização da Aviação Civil Internacional (ICAO) para relatar informações meteorológicas, amplamente utilizado por pilotos e meteorologistas para previsão do tempo. A biblioteca processa a string METAR e a transforma em um objeto `DecodedMetar` estruturado, que contém todas as propriedades meteorológicas decodificadas, como vento de superfície, visibilidade, alcance visual da pista, tempo presente, camadas de nuvens, temperatura do ar, temperatura do ponto de orvalho e pressão.

### Decodificador TAF

O decodificador TAF é uma biblioteca .NET para decodificar mensagens TAF (Terminal Aerodrome Forecast), que são previsões meteorológicas para aeródromos. Assim como o METAR, o formato TAF é altamente padronizado pela ICAO e é crucial para o planejamento de voos. A biblioteca analisa a string TAF e a converte em um objeto `DecodedTaf`, que inclui informações como tipo de relatório, código ICAO, data e hora de origem, período de previsão, vento de superfície, visibilidade, fenômenos meteorológicos, camadas de nuvens e temperaturas mínima e máxima. O decodificador TAF também é capaz de interpretar "evoluções" (mudanças na previsão ao longo do tempo), como `BECMG` (tornando-se) e `TEMPO` (temporário).

Ambos os decodificadores oferecem modos de análise "estrito" e "não estrito". No modo não estrito, a análise continua mesmo que sejam encontrados erros de formato, e as exceções são registradas na propriedade `DecodingExceptions` do objeto decodificado. Valores numéricos com unidades (como velocidade, distância e pressão) são encapsulados em objetos `Value`, permitindo conversões de unidade flexíveis.

Este projeto é amplamente baseado nas implementações de [SafranCassiopee/csharp-metar-decoder](https://github.com/SafranCassiopee/csharp-metar-decoder) e [SafranCassiopee/csharp-taf-decoder](https://github.com/SafranCassiopee/csharp-taf-decoder).

## Status do Projeto

Concluída

## Pacotes NuGet

Os pacotes NuGet oficiais estão disponíveis para fácil integração em seus projetos:

| Pacote | NuGet |
| ------ | ------ |
| [Metar.Decoder](https://www.nuget.org/packages/Metar.Decoder/) | [![NuGet version](https://badge.fury.io/nu/Metar.Decoder.svg)](https://badge.fury.io/nu/Metar.Decoder) |
| [Taf.Decoder](https://www.nuget.org/packages/Taf.Decoder/) | [![NuGet version](https://badge.fury.io/nu/Taf.Decoder.svg)](https://badge.fury.io/nu/Taf.Decoder) |

## Pré-requisitos

Esta biblioteca é compatível com .NET Standard 2.0 e .NET 8.0.

## Como Instalar

### Com nuget.exe (recomendado)

No Console do Gerenciador de Pacotes no Visual Studio:

```shell
nuget install Metar.Decoder
nuget install Taf.Decoder
```

Adicione uma referência à biblioteca e, em seguida, adicione as seguintes diretivas `using`:

```csharp
using Metar.Decoder;
using Metar.Decoder.Entity;
```

```csharp
using Taf.Decoder;
using Taf.Decoder.Entity;
```

### Manualmente

Baixe a versão mais recente em [GitHub Releases](https://github.com/afonsoft/metar-decoder/releases).

Extraia o conteúdo onde desejar em seu projeto. A biblioteca em si está no diretório `Metar.Decoder/` e `Taf.Decoder/`; os outros diretórios não são obrigatórios para o funcionamento da biblioteca.

Adicione os projetos `Metar.Decoder` e `Taf.Decoder` à sua solução e, em seguida, adicione uma referência a eles em seu próprio projeto. Finalmente, adicione as mesmas diretivas `using` mencionadas acima.

## Como Usar

### Decodificador METAR

Instancie o decodificador e execute-o em uma string METAR. O objeto retornado é um objeto `DecodedMetar` do qual você pode recuperar todas as propriedades meteorológicas decodificadas.

Todos os valores que possuem uma unidade são baseados no objeto `Value`, que fornece as propriedades `ActualValue` e `ActualUnit`.

Consulte a classe [`DecodedMetar`](src/Metar.Decoder/Entity/DecodedMetar.cs) para a estrutura do objeto resultante.

```csharp
var d = MetarDecoder.ParseWithMode("METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03");

// Informações de contexto
Console.WriteLine($"Válido: {d.IsValid}"); // true
Console.WriteLine($"METAR Bruto: {d.RawMetar}"); // "METAR LFPO 231027Z AUTO 24004G09MPS 2500 1000NW R32/0400 R08C/0004D +FZRA VCSN //FEW015 17/10 Q1009 REFZRA WS R03"
Console.WriteLine($"Tipo: {d.Type}"); // MetarType.METAR
Console.WriteLine($"ICAO: {d.ICAO}"); // "LFPO"
Console.WriteLine($"Dia: {d.Day}"); // 23
Console.WriteLine($"Hora: {d.Time}"); // "10:27 UTC"
Console.WriteLine($"Status: {d.Status}"); // "AUTO"

// Vento de superfície
var sw = d.SurfaceWind; // Objeto SurfaceWind
Console.WriteLine($"Vento - Direção Média: {sw.MeanDirection.ActualValue}"); // 240
Console.WriteLine($"Vento - Velocidade Média: {sw.MeanSpeed.ActualValue} {sw.MeanSpeed.ActualUnit}"); // 4 MeterPerSecond
Console.WriteLine($"Vento - Variações de Velocidade: {sw.SpeedVariations.ActualValue}"); // 9

// Visibilidade
var v = d.Visibility; // Objeto Visibility
Console.WriteLine($"Visibilidade Prevalecente: {v.PrevailingVisibility.ActualValue} {v.PrevailingVisibility.ActualUnit}"); // 2500 Meter
Console.WriteLine($"Visibilidade Mínima: {v.MinimumVisibility.ActualValue}"); // 1000
Console.WriteLine($"Direção da Visibilidade Mínima: {v.MinimumVisibilityDirection}"); // "NW"
Console.WriteLine($"NDV: {v.NDV}"); // false

// Alcance Visual da Pista (RVR)
var rvr = d.RunwaysVisualRange; // Array de RunwayVisualRange
Console.WriteLine($"RVR Pista 32: {rvr[0].VisualRange.ActualValue}"); // 400
Console.WriteLine($"RVR Pista 08C: {rvr[1].VisualRange.ActualValue}"); // 4

// Tempo Presente
var pw = d.PresentWeather; // Array de WeatherPhenomenon
Console.WriteLine($"Tempo Presente 1: {pw[0].IntensityProximity}{string.Join("", pw[0].Characteristics)}{string.Join("", pw[0].Types)}"); // "+FZRA"
Console.WriteLine($"Tempo Presente 2: {pw[1].IntensityProximity}{string.Join("", pw[1].Types)}"); // "VCSN"

// Nuvens
var cld = d.Clouds; // Array de CloudLayer
Console.WriteLine($"Nuvens - Quantidade: {cld[0].Amount}"); // FEW
Console.WriteLine($"Nuvens - Altura da Base: {cld[0].BaseHeight.ActualValue} {cld[0].BaseHeight.ActualUnit}"); // 1500 Feet

// Temperatura
Console.WriteLine($"Temperatura do Ar: {d.AirTemperature.ActualValue} {d.AirTemperature.ActualUnit}"); // 17 DegreeCelsius
Console.WriteLine($"Temperatura do Ponto de Orvalho: {d.DewPointTemperature.ActualValue}"); // 10

// Pressão
Console.WriteLine($"Pressão: {d.Pressure.ActualValue} {d.Pressure.ActualUnit}"); // 1009 HectoPascal

// Tempo Recente
var rw = d.RecentWeather;
Console.WriteLine($"Tempo Recente - Características: {rw.Characteristics}"); // "FZ"
Console.WriteLine($"Tempo Recente - Tipos: {string.Join(", ", rw.Types)}"); // "RA"

// Tesouras de Vento (Windshear)
Console.WriteLine($"Windshear em Todas as Pistas: {d.WindshearAllRunways}"); // null (ou true/false se presente)
Console.WriteLine($"Windshear em Pistas Específicas: {string.Join(", ", d.WindshearRunways)}"); // "03"
```

### Decodificador TAF

Instancie o decodificador e execute-o em uma string TAF. O objeto retornado é um objeto `DecodedTaf` do qual você pode recuperar todas as propriedades de previsão decodificadas.

Consulte a classe [`DecodedTaf`](src/Taf.Decoder/Entity/DecodedTaf.cs) para a estrutura do objeto resultante.

```csharp
var d = TafDecoder.ParseWithMode("TAF LEMD 080500Z 0806/0912 23010KT 9999 SCT025 TX12/0816Z TN04/0807Z");

// Informações de contexto
Console.WriteLine($"Válido: {d.IsValid}");
Console.WriteLine($"TAF Bruto: {d.RawTaf}");
Console.WriteLine($"Tipo: {d.Type}");
Console.WriteLine($"ICAO: {d.Icao}");
Console.WriteLine($"Dia: {d.Day}");
Console.WriteLine($"Hora: {d.Time}");

// Período de Previsão
var fp = d.ForecastPeriod;
Console.WriteLine($"Período de Previsão - Do Dia: {fp.FromDay}");
Console.WriteLine($"Período de Previsão - Da Hora: {fp.FromHour}");
Console.WriteLine($"Período de Previsão - Ao Dia: {fp.ToDay}");
Console.WriteLine($"Período de Previsão - À Hora: {fp.ToHour}");

// Vento de superfície
var swTaf = d.SurfaceWind;
Console.WriteLine($"TAF Vento - Direção Média: {swTaf.MeanDirection.ActualValue}");
Console.WriteLine($"TAF Vento - Velocidade Média: {swTaf.MeanSpeed.ActualValue} {swTaf.MeanSpeed.ActualUnit}");

// Visibilidade
var vTaf = d.Visibility;
Console.WriteLine($"TAF Visibilidade Prevalecente: {vTaf.PrevailingVisibility.ActualValue} {vTaf.PrevailingVisibility.ActualUnit}");
Console.WriteLine($"TAF CAVOK: {d.Cavok}");

// Nuvens
var cldTaf = d.Clouds;
if (cldTaf.Count > 0)
{
    Console.WriteLine($"TAF Nuvens - Quantidade: {cldTaf[0].Amount}");
    Console.WriteLine($"TAF Nuvens - Altura da Base: {cldTaf[0].BaseHeight.ActualValue} {cldTaf[0].BaseHeight.ActualUnit}");
}

// Temperaturas (Mínima e Máxima)
var minTemp = d.MinimumTemperature;
if (minTemp != null)
{
    Console.WriteLine($"Temperatura Mínima: {minTemp.TemperatureValue.ActualValue} {minTemp.TemperatureValue.ActualUnit} no dia {minTemp.Day} às {minTemp.Hour}Z");
}
var maxTemp = d.MaximumTemperature;
if (maxTemp != null)
{
    Console.WriteLine($"Temperatura Máxima: {maxTemp.TemperatureValue.ActualValue} {maxTemp.TemperatureValue.ActualUnit} no dia {maxTemp.Day} às {maxTemp.Hour}Z");
}

// Fenômenos Meteorológicos
var wpTaf = d.WeatherPhenomenons;
if (wpTaf.Count > 0)
{
    Console.WriteLine($"TAF Fenômeno Meteorológico: {wpTaf[0].IntensityProximity}{string.Join("", wpTaf[0].Characteristics)}{string.Join("", wpTaf[0].Types)}");
}

// Evoluções (BECMG, TEMPO, etc.)
foreach (var evolution in d.Evolutions)
{
    Console.WriteLine($"Evolução: {evolution.Type} de {evolution.FromDay}{evolution.FromHour}Z a {evolution.ToDay}{evolution.ToHour}Z");
    // Acessar propriedades específicas da evolução, como vento, visibilidade, nuvens, etc.
}
```

### Sobre Objetos de Valor (`Value`)

No exemplo acima, assume-se que todos os parâmetros solicitados estão disponíveis. No mundo real, alguns campos não são obrigatórios, portanto, é importante verificar se o objeto `Value` (contendo tanto o valor quanto sua unidade) não é nulo antes de usá-lo. O que você faz caso seja nulo fica a seu critério.

Aqui está um exemplo:

```csharp
// verifica se o ponto de orvalho não é nulo e atribui um valor padrão se for
var dew_point = d.DewPointTemperature;
if (dew_point == null)
{
    dew_point = new Value(999, Value.Unit.DegreeCelsius);
}

// o objeto dew_point agora pode ser acessado com segurança
Console.WriteLine(dew_point.ActualValue);
Console.WriteLine(dew_point.ActualUnit);
```

Os objetos `Value` também contêm sua unidade, que pode ser acessada com a propriedade `ActualUnit`. Ao acessar a propriedade `ActualValue`, você obterá o valor nesta unidade.

Se você deseja obter o valor diretamente em outra unidade, pode chamar `GetConvertedValue(unit)`. Os valores suportados são velocidade, distância e pressão.

Aqui estão todas as unidades disponíveis para conversão:

```csharp
// unidades de velocidade:
// Value.Unit.MeterPerSecond
// Value.Unit.KilometerPerHour
// Value.Unit.Knot

// unidades de distância:
// Value.Unit.Meter
// Value.Unit.Feet
// Value.Unit.StatuteMile

// unidades de pressão:
// Value.Unit.HectoPascal
// Value.Unit.MercuryInch

// usar conversão em tempo real
var distance_in_sm = visibility.GetConvertedValue(Value.Unit.StatuteMile);
var speed_kph = speed.GetConvertedValue(Value.Unit.KilometerPerHour);
```

### Sobre Erros de Análise

Quando um formato inesperado é encontrado para uma parte do METAR/TAF, o erro de análise é registrado no próprio objeto `DecodedMetar` ou `DecodedTaf`.

Todos os erros de análise para um METAR/TAF podem ser acessados através da propriedade `DecodingExceptions`.

Por padrão, a análise continuará quando um formato incorreto for encontrado. No entanto, o analisador também oferece um modo "estrito" onde a análise para assim que uma não conformidade é detectada. O modo pode ser definido globalmente para um objeto `MetarDecoder`/`TafDecoder`, ou apenas uma vez, como você pode ver neste exemplo:

```csharp
var decoder = new MetarDecoder();
decoder.SetStrictParsing(true);

// altera o modo de análise global para "estrito"
decoder.SetStrictParsing(true);

// esta análise será feita no modo estrito
decoder.Parse("...");

// mas esta ignorará o modo global e será feita no modo não estrito
decoder.ParseNotStrict("...");

// altera o modo de análise global para "não estrito"
decoder.SetStrictParsing(false);

// esta análise será feita no modo não estrito
decoder.Parse("...");

// mas esta ignorará o modo global e será feita no modo estrito
decoder.ParseStrict("...");
```

### Sobre Erros de Análise (Novamente)

No modo não estrito, é possível obter um erro de análise para um determinado decodificador de "chunk", enquanto ainda obtém as informações decodificadas para este "chunk" no final. Como isso é possível?

Isso ocorre porque o modo não estrito não apenas continua a decodificação onde há um erro, mas também tenta a análise novamente no "próximo chunk" (com base no separador de espaço em branco). No entanto, todos os erros na primeira tentativa permanecerão registrados, mesmo que a segunda tentativa tenha sido bem-sucedida.

Por exemplo, se você tiver o "chunk" `AAA 12003KPH ...` fornecido ao decodificador de "chunk" `SurfaceWind`. Este decodificador falhará em `AAA`, tentará decodificar `12003KPH` e terá sucesso. A primeira exceção para o decodificador de vento de superfície será mantida, mas o objeto `SurfaceWind` será preenchido com algumas informações.

Tudo isso não se aplica ao modo estrito, pois a análise é interrompida no primeiro erro de análise neste caso.

## Estrutura do Repositório

```
.
├── CHANGELOG.md                # Histórico de todas as mudanças notáveis no projeto.
├── EAF.ico                     # Ícone do projeto.
├── EAF.png                     # Imagem do projeto.
├── LICENSE                     # Arquivo de licença do projeto.
├── MetarDecoder.sln            # Solução principal do Visual Studio para o projeto.
├── README.md                   # Este arquivo de documentação do projeto.
├── appveyor.yml                # Configuração para integração contínua com AppVeyor.
├── docs/                       # Documentação gerada, incluindo arquivos de ajuda e XML.
│   ├── Working/                # Documentação em andamento ou arquivos temporários de documentação.
│   │   └── Taf.Decoder.xml     # Arquivo XML de documentação para o decodificador TAF.
│   └── media/                  # Imagens e outros recursos de mídia para a documentação.
│       ├── AlertCaution.png
│       ├── AlertLanguage.png
│       ├── AlertNote.png
│       ├── AlertSecurity.png
│       ├── AlertToDo.png
│   └── ... (outros arquivos .md gerados para documentação)
├── metar.docs.shfbproj         # Projeto Sandcastle Help File Builder para gerar a documentação.
├── metar.docs.sln              # Solução do Visual Studio para o projeto de documentação.
├── nuget.config                # Configurações do NuGet para o projeto.
├── sonar/                      # Arquivos relacionados à análise de código com SonarQube/SonarCloud.
│   ├── Google.Protobuf.dll
│   ├── Newtonsoft.Json.dll
│   ├── SonarQube.Analysis.xml
│   ├── SonarScanner.MSBuild.Common.dll
│   ├── SonarScanner.MSBuild.PostProcessor.dll
│   ├── SonarScanner.MSBuild.PreProcessor.dll
│   ├── SonarScanner.MSBuild.Shim.dll
│   ├── SonarScanner.MSBuild.Tasks.dll
│   ├── SonarScanner.MSBuild.dll
│   ├── SonarScanner.MSBuild.runtimeconfig.json
│   └── Targets/                # Arquivos de destino para integração do SonarQube com MSBuild.
│       ├── SonarQube.Integration.ImportBefore.targets
│       └── SonarQube.Integration.targets
│   └── sonar-scanner-4.8.0.2856/ # Diretório do SonarScanner.
│       ├── bin/                # Binários do SonarScanner.
│       ├── conf/               # Arquivos de configuração do SonarScanner.
│       └── lib/                # Bibliotecas do SonarScanner.
├── sonarcloud.bat              # Script em lote para execução da análise do SonarCloud.
├── src/                        # Código-fonte principal do projeto.
│   ├── Metar.Decoder/          # Projeto da biblioteca para decodificação de METAR.
│   │   ├── ChunkDecoder/       # Decodificadores de "chunks" individuais do METAR.
│   │   │   ├── Abstract/       # Classes abstratas e interfaces para decodificadores de "chunks".
│   │   │   │   ├── IMetarChunkDecoder.cs
│   │   │   │   └── MetarChunkDecoder.cs
│   │   │   ├── CloudChunkDecoder.cs        # Decodificador para informações de nuvens.
│   │   │   ├── DatetimeChunkDecoder.cs     # Decodificador para data e hora da observação.
│   │   │   ├── IcaoChunkDecoder.cs         # Decodificador para o código ICAO do aeroporto.
│   │   │   ├── PresentWeatherChunkDecoder.cs # Decodificador para fenômenos meteorológicos presentes.
│   │   │   ├── PressureChunkDecoder.cs     # Decodificador para informações de pressão.
│   │   │   ├── RecentWeatherChunkDecoder.cs # Decodificador para tempo recente.
│   │   │   ├── ReportStatusChunkDecoder.cs # Decodificador para o status do relatório (e.g., AUTO, NIL).
│   │   │   ├── ReportTypeChunkDecoder.cs   # Decodificador para o tipo de relatório (e.g., METAR, SPECI).
│   │   │   ├── RunwayVisualRangeChunkDecoder.cs # Decodificador para alcance visual da pista.
│   │   │   ├── SurfaceWindChunkDecoder.cs  # Decodificador para informações de vento de superfície.
│   │   │   ├── TemperatureChunkDecoder.cs  # Decodificador para informações de temperatura.
│   │   │   ├── VisibilityChunkDecoder.cs   # Decodificador para informações de visibilidade.
│   │   │   └── WindShearChunkDecoder.cs    # Decodificador para informações de tesoura de vento.
│   │   ├── EAF.ico
│   │   ├── EAF.png
│   │   ├── Entity/             # Classes de entidade que representam os dados decodificados do METAR.
│   │   │   ├── CloudLayer.cs           # Representa uma camada de nuvens.
│   │   │   ├── DecodedMetar.cs         # Objeto principal que contém o METAR decodificado.
│   │   │   ├── PresentWeather.cs       # Representa fenômenos meteorológicos presentes.
│   │   │   ├── RunwayVisualRange.cs    # Representa o alcance visual da pista.
│   │   │   ├── SurfaceWind.cs          # Representa o vento de superfície.
│   │   │   ├── Value.cs                # Classe genérica para valores com unidades.
│   │   │   ├── Visibility.cs           # Representa informações de visibilidade.
│   │   │   └── WeatherPhenomenon.cs    # Representa um fenômeno meteorológico.
│   │   ├── Exception/          # Classes de exceção específicas do decodificador METAR.
│   │   │   └── MetarChunkDecoderException.cs
│   │   ├── Metar.Decoder.csproj    # Arquivo de projeto C# para a biblioteca Metar.Decoder.
│   │   └── MetarDecoder.cs         # Lógica principal do decodificador METAR.
│   └── Taf.Decoder/            # Projeto da biblioteca para decodificação de TAF.
│       ├── ChunkDecoder/       # Decodificadores de "chunks" individuais do TAF.
│       │   ├── Abstract/       # Classes abstratas e interfaces para decodificadores de "chunks" TAF.
│       │   │   ├── ITafChunkDecoder.cs
│       │   │   └── TafChunkDecoder.cs
│       │   ├── CloudChunkDecoder.cs
│       │   ├── DatetimeChunkDecoder.cs
│       │   ├── EvolutionChunkDecoder.cs    # Decodificador para evoluções (BECMG, TEMPO).
│       │   ├── ForecastPeriodChunkDecoder.cs # Decodificador para o período de previsão.
│       │   ├── IcaoChunkDecoder.cs
│       │   ├── ReportTypeChunkDecoder.cs
│       │   ├── SurfaceWindChunkDecoder.cs
│       │   ├── TemperatureChunkDecoder.cs
│       │   ├── VisibilityChunkDecoder.cs
│       │   └── WeatherChunkDecoder.cs
│       ├── EAF.ico
│       ├── EAF.png
│       ├── Entity/             # Classes de entidade que representam os dados decodificados do TAF.
│       │   ├── BaseEntity.cs
│       │   ├── CloudLayer.cs
│       │   ├── DecodedTaf.cs           # Objeto principal que contém o TAF decodificado.
│       │   ├── Evolution.cs            # Representa uma evolução na previsão.
│       │   ├── ForecastPeriod.cs       # Representa o período de previsão.
│       │   ├── SurfaceWind.cs
│       │   ├── Temperature.cs
│       │   ├── Value.cs
│       │   ├── Visibility.cs
│       │   └── WeatherPhenomenon.cs
│       ├── Exception/          # Classes de exceção específicas do decodificador TAF.
│       │   └── TafChunkDecoderException.cs
│       ├── README.md           # README específico para o decodificador TAF (será consolidado no README principal).
│       ├── Taf.Decoder.csproj      # Arquivo de projeto C# para a biblioteca Taf.Decoder.
│       └── TafDecoder.cs           # Lógica principal do decodificador TAF.
└── tests/                      # Projetos de teste para as bibliotecas.
    ├── Metar.Decoder.Tests/    # Testes para o decodificador METAR.
    │   ├── BasicTest.cs
    │   ├── ChunkDecoder/       # Testes para os decodificadores de "chunks" do METAR.
    │   ├── Integration.cs
    │   ├── MetarChunkDecoderExceptionTest.cs
    │   ├── MetarDecoderTest.cs
    │   └── ValueTest.cs
    └── Taf.Decoder.Tests/      # Testes para o decodificador TAF.
        ├── BasicTest.cs
        ├── ChunkDecoder/       # Testes para os decodificadores de "chunks" do TAF.
        ├── Taf.Decoder.Tests.csproj
        ├── TafDecoderTest.cs
        ├── ValueTest.cs
        └── packages.config

