# WORKFLOWS.md — metar-decoder Automação

## Workflow: Novo ChunkDecoder

### Precondições
- Novo segmento METAR/TAF identificado
- Padrão regex definido

### Passos
1. Criar classe em `src/Metar.Decoder/ChunkDecoder/` ou `src/Taf.Decoder/ChunkDecoder/`
2. Implementar interface `IChunkDecoder`
3. Definir regex pattern e nome do parâmetro
4. Registrar no pipeline de decodificação
5. Criar testes em `tests/` cobrindo cenários válidos e inválidos
6. Testar em modo estrito e não-estrito
7. Documentar em `docs/`
8. Criar PR

### Critérios de Sucesso
- Testes passam em todos os TFMs
- Cobertura mantida ~98%
- Dados METAR/TAF reais validados

---

## Workflow: Bug Fix em Decoder

### Passos
1. Identificar o ChunkDecoder afetado
2. Criar teste que reproduz o bug (com string METAR/TAF real)
3. Implementar correção no regex ou lógica
4. Verificar que teste passa
5. Rodar suite completa
6. Criar PR

---

## Workflow: Suporte a Novo Formato

### Precondições
- Formato documentado pela ICAO ou autoridade aeronáutica

### Passos
1. Analisar especificação do formato
2. Identificar ChunkDecoders necessários
3. Criar entidades em `Entity/`
4. Implementar decoders
5. Testes extensivos com dados reais
6. Documentação bilíngue
7. Criar PR

---

## Verification Loop

```
Código → Build → Testes (421) → Cobertura (~98%) → CI → Review
  ↑                                                      |
  └──── Ajustar (máx. 2x) ─────────────────────────────┘
```

### Execução Local
```bash
dotnet restore MetarDecoder.sln
dotnet build MetarDecoder.sln --configuration Release
dotnet test MetarDecoder.sln --configuration Release --collect:"XPlat Code Coverage"
```

---

## Workflow: Release

### Passos
1. Atualizar versão nos `.csproj` (Metar.Decoder e Taf.Decoder)
2. Atualizar `CHANGELOG.md`
3. Criar tag de release
4. CI publica automaticamente no NuGet

---

## Trigger Conditions

| Evento | Workflow |
|--------|---------|
| Push em `feature/*`, `bug/*`, `hotfix/*` | `ci-build-test.yml` |
| PR para `main` | `ci-build-test.yml` + `code-quality.yml` |
| Tag de release | `publish-all.yml` |
| Push em `main` | `auto-pr-from-main.yml` |
| Schedule | `security-scan.yml` |
