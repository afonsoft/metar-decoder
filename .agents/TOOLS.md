# TOOLS.md — metar-decoder Ferramentas

## Ferramentas de Build

| Ferramenta | Comando | Categoria |
|-----------|---------|-----------|
| dotnet restore | `dotnet restore MetarDecoder.sln` | Read-only |
| dotnet build | `dotnet build MetarDecoder.sln --configuration Release` | Execute |
| dotnet test | `dotnet test MetarDecoder.sln --collect:"XPlat Code Coverage"` | Execute |
| dotnet pack | `dotnet pack --configuration Release` | Execute |

## Ferramentas de Qualidade

| Ferramenta | Uso | Categoria |
|-----------|-----|-----------|
| SonarCloud | Análise de qualidade | Execute |
| coverlet | Cobertura de código | Execute |

## Ferramentas de CI/CD

| Workflow | Trigger | Descrição |
|---------|---------|-----------|
| `ci-build-test.yml` | push feature/*, PR main | Build + testes + cobertura |
| `publish-all.yml` | tag release | Publicação NuGet |
| `code-quality.yml` | PR | SonarCloud + Qodana |
| `security-scan.yml` | schedule/PR | Scan de segurança |
| `openhands-resolver.yml` | issue label | Auto-fix via OpenHands |
| `auto-pr-from-main.yml` | push main | Sync automático |

## CI Legado (não usar para novos desenvolvimentos)

| Ferramenta | Arquivo | Status |
|-----------|---------|--------|
| Travis CI | `.travis.yml` | Legado |
| AppVeyor | `appveyor.yml` | Legado |

## APIs Externas

| API | Uso | Headers |
|-----|-----|---------|
| NuGet.org | Publicação de pacotes | API Key via CI secrets |
| SonarCloud | Qualidade | Token via CI secrets |
| GitHub API | CI/CD | GITHUB_TOKEN |
