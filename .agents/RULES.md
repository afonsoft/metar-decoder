# RULES.md — metar-decoder Guardrails

## Hard Rules (bloqueio imediato)

| # | Regra | Verificação |
|---|-------|-------------|
| H1 | Todos os testes devem passar (421 testes) | CI: `dotnet test` |
| H2 | Cobertura ~98% — não pode diminuir significativamente | CI: coverlet |
| H3 | Compatibilidade .NET Standard 2.0 obrigatória | Build multi-TFM |
| H4 | Nunca commitar secrets | `.gitignore` |
| H5 | Não push direto em `main` | Branch protection |
| H6 | ICAO compliance nos parsers | Testes com dados reais |

## Soft Rules (warning + confirmação)

| # | Regra | Ação |
|---|-------|------|
| S1 | Alterar regex de ChunkDecoder | Testar com dados METAR/TAF reais |
| S2 | Adicionar novo ChunkDecoder | Seguir padrão existente |
| S3 | Modificar Value Objects | Verificar conversões de unidade |
| S4 | Alterar CI workflows | Requer revisão |
| S5 | Modificar modo estrito/não-estrito | Testar ambos os modos |

## Permissões por Ambiente

| Ambiente | Read | Write | Execute |
|----------|------|-------|---------|
| **dev** | Livre | Livre | Sandbox |
| **CI** | Livre | Via PR | Automático |
| **NuGet** | — | — | Via release workflow |

## Tool Permissions

- **Read-only**: busca, navegação, análise
- **Write**: edição via PR
- **Execute**: build, test em sandbox
- **Publish**: apenas via CI/CD

## Arquivos Imutáveis

```
bin/
obj/
docs/ (SHFB generated — exceto en-US/ e pt-BR/)
sonar/
.git/
.vs/
.vscode/
TestResults/
```
