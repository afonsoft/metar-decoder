# Devin Configuration — metar-decoder

> Referência principal: [AGENTS.md](AGENTS.md)

## Delta Devin

### Ambiente
- .NET 10.0 necessário (multi-TFM: netstandard2.0, net8.0, net10.0, net48)
- NUnit como framework de testes (não xUnit)

### Comandos Rápidos
```bash
dotnet restore MetarDecoder.sln
dotnet build MetarDecoder.sln --configuration Release
dotnet test MetarDecoder.sln --configuration Release --collect:"XPlat Code Coverage"
```

### PRs
- Branch: `devin/<timestamp>-<descricao>`
- Target: `main`
- CI deve passar antes de notificar o usuário

### Notas
- Testes `net48` não rodam em Linux (sem Windows Desktop)
- 421 testes com ~98% de cobertura
- Dois decoders independentes: Metar.Decoder e Taf.Decoder
- Travis CI e AppVeyor são legados — GitHub Actions é o CI principal
