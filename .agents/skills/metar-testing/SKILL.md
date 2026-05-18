---
name: metar-testing
description: >
  What: Guia para testes dos decodificadores METAR e TAF usando NUnit e coverlet.
  When: Ao escrever testes, aumentar cobertura, debugar falhas, ou validar decoders com dados reais.
  Do NOT: Não usar para desenvolvimento de features (ver metar-development) ou CI/CD.
---

## Contexto

metar-decoder usa NUnit como framework de testes e coverlet para cobertura. 421 testes com ~98% de cobertura. Testes organizados por ChunkDecoder e entidade.

## Atuação

### Estrutura de Testes
```
tests/
├── Metar.Decoder.Tests/
│   ├── ChunkDecoder/     # Um arquivo por decoder
│   └── Entity/           # Testes de entidades
└── Taf.Decoder.Tests/
    ├── ChunkDecoder/
    └── Entity/
```

### Padrões
- NUnit com `[Test]`, `[TestCase]`, `[TestCaseSource]`
- Testar com dados METAR/TAF reais
- Cobrir modo estrito e não-estrito
- Testar edge cases: CAVOK, NIL, RTD, valores negativos
- Testar rollover de data (final do mês)

### Comandos
```bash
# Todos os testes
dotnet test MetarDecoder.sln --configuration Release

# Com cobertura
dotnet test MetarDecoder.sln --collect:"XPlat Code Coverage"

# Apenas METAR
dotnet test tests/Metar.Decoder.Tests/ --configuration Release

# Apenas TAF
dotnet test tests/Taf.Decoder.Tests/ --configuration Release

# Teste específico
dotnet test --filter "FullyQualifiedName~IcaoChunkDecoderTest"
```

## Restrições

- Não modificar testes existentes para forçar passagem
- Cobertura não pode diminuir (~98% baseline)
- Testes net48 não rodam em Linux
- Não commitar TestResults/

## Exemplos

```csharp
[Test]
public void TestParseICAO()
{
    var decoder = new MetarDecoder();
    var metar = decoder.Parse("METAR KJFK 121755Z 31008KT 10SM FEW250 M04/M19 A3036");
    Assert.That(metar.ICAO, Is.EqualTo("KJFK"));
}

[TestCase("METAR KJFK 121755Z 31008KT 10SM FEW250 M04/M19 A3036")]
[TestCase("METAR EGLL 121750Z 27009KT 9999 FEW040 07/01 Q1024")]
public void TestParseMultipleStations(string raw)
{
    var decoder = new MetarDecoder();
    var metar = decoder.Parse(raw);
    Assert.That(metar.ICAO, Is.Not.Null);
    Assert.That(metar.ICAO.Length, Is.EqualTo(4));
}
```
