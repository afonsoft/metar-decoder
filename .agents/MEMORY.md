# metar-decoder Agent Memory

Este arquivo é mantido automaticamente pelo agente AI para persistir aprendizados sobre o codebase.

## Decisões Técnicas

| Data | Decisão | Motivo | Alternativas Descartadas |
|------|---------|--------|--------------------------|
| 2026-05 | ChunkDecoder pattern com regex | Cada segmento METAR/TAF é independente e parseável por regex | Parser monolítico |
| 2026-05 | Value Objects com conversão de unidades | Flexibilidade para diferentes sistemas de medida | Valores primitivos |
| 2026-05 | Modo estrito vs não-estrito | Tolerância a relatórios malformados | Apenas estrito |
| 2026-05 | Multi-TFM (.NET Standard 2.0+) | Máxima compatibilidade | Apenas .NET 8+ |

## Débitos Técnicos

| Item | Impacto | Prioridade |
|------|---------|-----------|
| Travis CI e AppVeyor legados | Manutenção desnecessária | Baixa |
| Cobertura TAF inferior à METAR | Risco de regressão | Média |

## Lições Aprendidas

| Contexto | Erro | Como Evitar |
|----------|------|-------------|
| DatetimeChunkDecoder | Bug de rollover dia/mês | Testar com datas no final do mês |
| Build net48 em Linux | Falha sem Windows Desktop | Usar `-f net10.0` localmente |
| Relatórios RTD (Report Delayed) | Não eram tratados | Adicionar decoder para RTD |

## Políticas de Limpeza

- Fatos desatualizados devem ser removidos
- Memórias de branches deletadas devem ser descartadas
