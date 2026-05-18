# Gemini CLI Configuration — metar-decoder

> Referência principal: [AGENTS.md](AGENTS.md)

## Delta Gemini

### Contexto
Gemini CLI carrega este arquivo automaticamente. O AGENTS.md contém todas as convenções.

### Build
```bash
dotnet restore MetarDecoder.sln
dotnet build MetarDecoder.sln --configuration Release
dotnet test MetarDecoder.sln --configuration Release --collect:"XPlat Code Coverage"
```

### Skills
Disponíveis em `.agents/skills/` — carregar conforme contexto.

### Referências
- `.agents/RULES.md` — guardrails
- `.agents/TOOLS.md` — ferramentas
