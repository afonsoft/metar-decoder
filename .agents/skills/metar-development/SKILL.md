---
name: metar-development
description: >
  What: Guia para desenvolvimento de ChunkDecoders, entidades e funcionalidades dos decodificadores METAR e TAF.
  When: Ao criar novos decoders, modificar entidades, trabalhar com regex de parsing, ou adicionar suporte a novos formatos.
  Do NOT: Não usar para testes (ver metar-testing), CI/CD, ou documentação isolada.
---

## Contexto

metar-decoder usa o padrão ChunkDecoder: cada segmento de um relatório METAR/TAF é parseado por um decoder especializado usando regex. Os decoders são encadeados em pipeline.

## Atuação

### Novo ChunkDecoder
1. Criar classe em `src/Metar.Decoder/ChunkDecoder/` ou `src/Taf.Decoder/ChunkDecoder/`
2. Implementar interface `IChunkDecoder`
3. Definir regex pattern como constante
4. Implementar método `Parse()` que retorna o resultado e o remaining string
5. Registrar no pipeline de decodificação (array de decoders)
6. Tratar modo estrito (throw) e não-estrito (log + skip)

### Value Objects
- Usar `Value` class para valores numéricos com unidades
- Implementar conversões: `Value.ActualValue`, `Value.Unit`
- Unidades comuns: knots, m/s, km/h, hPa, inHg, feet, meters

### Entidades
- `DecodedMetar` / `DecodedTaf`: objetos raiz
- Propriedades tipadas para cada segmento decodificado
- `DecodingExceptions`: lista de erros no modo não-estrito
- `RawMetar` / `RawTaf`: string original preservada

### Evolutions (TAF)
- `BECMG`: mudança gradual
- `TEMPO`: condição temporária
- `PROB30/PROB40`: probabilidade
- `FM`: a partir de (horário)

## Restrições

- Manter compatibilidade .NET Standard 2.0
- Regex deve ser testada com dados METAR/TAF reais
- Modo estrito deve lançar `MetarChunkDecoderException`
- Modo não-estrito deve logar em `DecodingExceptions`
- ICAO compliance obrigatória

## Exemplos

```csharp
// Decodificar METAR
var decoder = new MetarDecoder();
var metar = decoder.Parse("METAR KJFK 121755Z 31008KT 10SM FEW250 M04/M19 A3036");
Console.WriteLine(metar.ICAO);           // KJFK
Console.WriteLine(metar.SurfaceWind);    // 310° at 8 knots

// Decodificar TAF
var tafDecoder = new TafDecoder();
var taf = tafDecoder.Parse("TAF KJFK 121720Z 1218/1324 31008KT P6SM FEW250");
```
