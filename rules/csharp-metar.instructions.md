---
name: 'C# metar-decoder Standards'
description: 'Padrões de código C# para a biblioteca metar-decoder, incluindo ChunkDecoder pattern, regex parsing e Value Objects'
applyTo: '**/*.cs'
---

# C# metar-decoder Standards

## Arquitetura

- Padrão ChunkDecoder: um parser por segmento METAR/TAF
- Pipeline de decoders processam a string sequencialmente
- Cada decoder retorna resultado + remaining string
- `DecodedMetar` / `DecodedTaf` como objetos raiz

## ChunkDecoder

- Implementar `IChunkDecoder`
- Regex pattern como constante (`const string`)
- Nome do parâmetro como constante
- Tratar modo estrito (throw) e não-estrito (catch + log)
- Retornar `null` para segmentos não reconhecidos

## Regex

- Usar `RegexOptions.Compiled` quando possível
- Anchor com `^` para match no início
- Testar com dados METAR/TAF reais (não inventados)
- Documentar o padrão regex com comentários

## Value Objects

- Usar `Value` para valores numéricos com unidades
- Implementar conversões de unidade conforme necessário
- Unidades padrão: knots, meters, hectopascals, Celsius

## Nomenclatura

- PascalCase: classes, métodos, propriedades
- camelCase: variáveis locais, parâmetros
- _camelCase: campos privados
- Sufixo `ChunkDecoder` para decoders
- Sufixo `Exception` para exceções

## Testes

- Framework: NUnit
- Dados reais de estações meteorológicas
- Cobrir modo estrito e não-estrito
- Testar edge cases: CAVOK, NIL, RTD, valores negativos, rollover

## Compatibilidade

- .NET Standard 2.0 como target mínimo
- Não usar APIs exclusivas de versões mais recentes sem #if
- Testar em múltiplos TFMs
