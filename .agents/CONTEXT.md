# Context Engineering — metar-decoder

## Estratégias de Carregamento

| Tipo | Quando | Arquivos |
|------|--------|----------|
| **Always-on** | Sempre carregado | `AGENTS.md`, `.agents/RULES.md` |
| **Pattern-matched** | Por tipo de arquivo | `rules/*.instructions.md` (via `applyTo`) |
| **On-demand** | Quando solicitado | `.agents/skills/`, `docs/`, `.agents/MEMORY.md` |
| **Progressive disclosure** | Arquivos grandes | Headers → seções relevantes |

## Hierarquia de Prioridade

1. Instruções do usuário (chat/prompt)
2. `AGENTS.md` (SSoT)
3. Arquivo de plataforma (`CLAUDE.md`, `DEVIN.md`, `GEMINI.md`)
4. `.agents/RULES.md`
5. `rules/*.instructions.md`
6. `.agents/skills/`
7. `.agents/MEMORY.md`

## Token Budget

- Reservar 20% do contexto para output
- `AGENTS.md` ≤ 500 linhas
- Skills: carregar apenas as relevantes
- ChunkDecoders: carregar apenas o decoder relevante para a tarefa

## Chunking

- ChunkDecoders: cada decoder é autocontido — carregar individualmente
- Testes: agrupar por ChunkDecoder
- docs/: carregar por idioma conforme necessário

## Mapa de Diretórios

```
Prioridade alta:
  src/Metar.Decoder/ChunkDecoder/
  src/Taf.Decoder/ChunkDecoder/
  src/Metar.Decoder/Entity/
  src/Taf.Decoder/Entity/
  tests/

Prioridade média:
  src/Metar.Decoder/Exception/
  src/Taf.Decoder/Exception/
  docs/en-US/
  docs/pt-BR/

Prioridade baixa:
  docs/ (SHFB generated API docs)
  sonar/
  .vscode/
```
