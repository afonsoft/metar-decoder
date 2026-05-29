# AGENTS.md — metar-decoder

## Missão

Biblioteca .NET para decodificação de relatórios meteorológicos METAR e TAF (Terminal Aerodrome Forecast) padronizados pela ICAO. Converte strings brutas em objetos estruturados (`DecodedMetar`, `DecodedTaf`) com suporte a modo estrito e não-estrito. Qualquer agente LLM que trabalhe neste repositório deve seguir as convenções aqui documentadas.

## Stack Tecnológica

| Camada | Tecnologia | Versão |
|--------|-----------|--------|
| Runtime | .NET | Standard 2.0 / 8.0 / 10.0 / FW 4.8 |
| Linguagem | C# | 10.0 |
| Testes | NUnit + coverlet | — |
| CI/CD | GitHub Actions | — |
| Qualidade | SonarCloud | — |
| Licença | MIT | — |

## Estrutura do Projeto

```
src/
├── Metar.Decoder/             # Decodificador METAR
│   ├── ChunkDecoder/          # Parsers regex por segmento
│   │   ├── IcaoChunkDecoder
│   │   ├── DatetimeChunkDecoder
│   │   ├── SurfaceWindChunkDecoder
│   │   ├── VisibilityChunkDecoder
│   │   ├── CloudChunkDecoder
│   │   ├── TemperatureChunkDecoder
│   │   ├── PressureChunkDecoder
│   │   ├── PresentWeatherChunkDecoder
│   │   ├── RunwayVisualRangeChunkDecoder
│   │   ├── WindShearChunkDecoder
│   │   ├── RecentWeatherChunkDecoder
│   │   ├── ReportStatusChunkDecoder
│   │   └── ReportTypeChunkDecoder
│   ├── Entity/                # Objetos de domínio (DecodedMetar, Value, etc.)
│   ├── Exception/             # Exceções tipadas
│   └── Assets/                # Recursos (ícone, readme NuGet)
├── Taf.Decoder/               # Decodificador TAF
│   ├── ChunkDecoder/          # Parsers TAF-específicos
│   ├── Entity/                # DecodedTaf, Evolution, ForecastPeriod
│   ├── Exception/             # Exceções TAF
│   └── Assets/
tests/
├── Metar.Decoder.Tests/       # Testes METAR
│   ├── ChunkDecoder/          # Testes por decoder
│   └── Entity/                # Testes de entidades
└── Taf.Decoder.Tests/         # Testes TAF
    ├── ChunkDecoder/
    └── Entity/
docs/                          # Documentação bilíngue
├── en-US/                     # usage-guide, api-reference
└── pt-BR/                     # guia-de-uso, referencia-api
.agents/                       # Infraestrutura de agentes
```

## Caminhos por Plataforma

| Plataforma | Config Principal | Skills | Rules |
|-----------|-----------------|--------|-------|
| Base (todas) | `AGENTS.md` | `.agents/skills/` | `.agents/RULES.md`, `rules/` |
| Claude Code | `CLAUDE.md` | auto-loaded | `.agents/RULES.md` |
| Devin | `AGENTS.md` | `.agents/skills/` | `.agents/RULES.md` |
| Windsurf | `.windsurfignore` | `.agents/skills/` | `rules/*.instructions.md` |

## Comandos de Build

```bash
# Restaurar dependências
dotnet restore MetarDecoder.sln

# Build completo
dotnet build MetarDecoder.sln --configuration Release

# Testes METAR com cobertura
dotnet test tests/Metar.Decoder.Tests/ --configuration Release \
  --collect:"XPlat Code Coverage" --results-directory TestResults

# Testes TAF com cobertura
dotnet test tests/Taf.Decoder.Tests/ --configuration Release \
  --collect:"XPlat Code Coverage" --results-directory TestResults

# Todos os testes
dotnet test MetarDecoder.sln --configuration Release --collect:"XPlat Code Coverage"
```

## Padrões de Código

### DO (Faça)
- Seguir padrão ChunkDecoder: um decoder por segmento METAR/TAF
- Usar regex para parsing de segmentos
- Documentação XML em todas as APIs públicas
- Testes para cada ChunkDecoder e cenário
- Manter cobertura ~98%
- Documentação bilíngue (pt-BR + en-US)
- Usar Value Objects para valores numéricos com unidades
- Suportar modo estrito e não-estrito

### DON'T (Não Faça)
- Não quebrar compatibilidade com .NET Standard 2.0
- Não reduzir cobertura de testes (98% baseline)
- Não commitar secrets
- Não hardcodar valores METAR/TAF (usar constantes)
- Não ignorar exceções no modo não-estrito (logar em DecodingExceptions)

## Hard Rules

1. **Testes obrigatórios**: CI falha se qualquer teste quebrar
2. **Cobertura ~98%**: não pode diminuir significativamente
3. **Compatibilidade .NET Standard 2.0**: deve compilar
4. **Secrets**: nunca commitar `.env` ou tokens
5. **ICAO compliance**: seguir padrões ICAO para parsing

## Soft Rules

1. Alterar regex de ChunkDecoder → testar com dados reais
2. Adicionar novo ChunkDecoder → seguir padrão existente
3. Modificar Value Object → verificar conversões de unidade
4. Alterar CI workflows → requer revisão

## Agent Loop

> Padrão: **ReAct** (Observe → Think → Act → Verify)

```
1. Receber tarefa
2. Carregar AGENTS.md + RULES.md
3. Analisar ChunkDecoders relevantes
4. Implementar alteração
5. Executar testes: dotnet test
6. Verificar cobertura
7. Criar PR
```

## Response Style

- Idioma: Português (pt-BR) para docs; inglês para código
- Conciso e direto
- Commits: `feat:`, `fix:`, `test:`, `docs:`, `refactor:`

## Conceitos-Chave

| Conceito | Descrição |
|----------|-----------|
| **METAR** | Meteorological Aerodrome Report (observações atuais) |
| **TAF** | Terminal Aerodrome Forecast (previsões) |
| **Chunk** | Segmento delimitado por espaço de um relatório |
| **ChunkDecoder** | Parser regex especializado por segmento |
| **Strict Mode** | Lança exceção em erros de formato |
| **Non-Strict Mode** | Pula chunks malformados, loga exceções |
| **Value** | Container para valores numéricos com conversão de unidades |
| **Evolution** | Mudanças TAF (BECMG, TEMPO, PROB) |
| **CAVOK** | Ceiling and Visibility OK |
| **ICAO** | Código de aeroporto de 4 letras (ex: KJFK) |
| **RVR** | Runway Visual Range |
| **Rollover** | Correção de mês/ano em relatórios sem data completa |

## Referências

- `.agents/CONTEXT.md` — Estratégias de contexto
- `.agents/RULES.md` — Guardrails
- `.agents/TOOLS.md` — Ferramentas
- `.agents/WORKFLOWS.md` — Automação
- `.agents/skills/` — Skills on-demand
- `rules/` — Rules por domínio
- `docs/` — Documentação bilíngue
