# [Test Coverage ~98% & Bug Fix](https://github.com/afonsoft/metar-decoder)
> 04/05/2026 17:45:00 UTC
##### ``1.0.9``
🧪 **Cobertura de Testes ~98% e Correção de Bug**

### 🐛 Correções:
- **DatetimeChunkDecoder** - Correção de bug de rollover de dia/mês
  - Quando o dia do METAR/TAF excedia os dias do mês anterior (ex: dia 31 em mês com 30 dias)
  - `ArgumentOutOfRangeException` ao criar DateTime com dia inválido
  - Uso de `DateTime.UtcNow` em vez de `DateTime.Now` para consistência

### 🧪 Novos Testes (421 testes, +67):
- **PresentWeatherTest** - Cobertura de 0% → 100% para entidade PresentWeather
- **MetarExceptionExtendedTest** - Cobertura de 53.5% → 82.1% para exceções METAR
- **TafExceptionExtendedTest** - Cobertura de 32.1% → 82.1% para exceções TAF
- **ValueExtendedTest** (Metar/Taf) - Cobertura de 98.4% → 100% para Value
- **ForecastPeriodTest** - Cobertura de 90.9% → 100% para ForecastPeriod
- **DecodedMetarExtendedTest** - Testes para propriedades e estados do DecodedMetar
- **TafChunkDecoderBaseTest** - Cobertura de 90.4% → 100% para TafChunkDecoder

### 📊 Cobertura:
- **Line coverage**: 94.8% → 97.8%
- **Branch coverage**: 92.3% → 94.7%
- **Method coverage**: 93.8% → 98.6%

---

# [RTD Support & .NET 10.0](https://github.com/afonsoft/metar-decoder/compare/1.0.5.2...feature/update-actions)
> 17/02/2026 16:45:00 UTC
##### ``1.0.8 & 1.0.6``
🚀 **Suporte RTD e .NET 10.0 com Workflows Modernos**

### 🆕 Novas Funcionalidades:
- **RTD Support** - Suporte completo para TAF reports com "Report Delayed"
  - Parsing de relatórios TAF marcados como RTD
  - Reconhecimento automático do tipo `TafType.RTD`
  - Integração completa com todos os elementos do TAF
  - Exemplo: `RTD EKEB 190416Z 1905/1912 13006KT...`

### 🔧 Melhorias Técnicas:
- **.NET 10.0 Support** - Compatibilidade com a versão mais recente
  - Target frameworks: `netstandard2.0;net8.0;net10.0;net48`
  - Build e testes para todas as versões
  - Pacotes NuGet compatíveis

### 🚀 CI/CD Modernizado:
- **GitHub Actions Workflows** - Pipeline completo de automação
  - CI Build & Test com security scans
  - Code Quality analysis (Qodana, SonarQube, Snyk)
  - Publicação automatizada para NuGet.org
  - Atualização automática de dependências

### 📦 Versões:
- **Metar.Decoder**: 1.0.8
- **Taf.Decoder**: 1.0.6

### ✅ Validação:
- 252/252 testes passing (100% sucesso)
- RTD parsing funcional e testado
- Compatibilidade mantida com versões anteriores

---

# [Feature Update - GitHub Actions Workflows](https://github.com/afonsoft/metar-decoder/compare/1.0.5.2...feature/update-actions)
> 17/02/2026 16:45:00 UTC
##### ``Feature Update``
🚀 **Modernização completa do CI/CD com GitHub Actions**

### 🔄 Workflows Adicionados:
- **🚀 CI Build & Test** - Pipeline completo de integração contínua
  - Build automatizado para .NET 8.0
  - Testes unitários com coverage reports
  - Security scans e performance tests
  - Criação automática de PRs para branches

- **📊 Code Quality** - Análise avançada de qualidade
  - Qodana analysis integration
  - SonarQube integration para metar-decoder
  - Snyk security scanning
  - Métricas de qualidade automatizadas

- **🔒 Security Scan** - Scans de segurança abrangentes
  - CodeQL analysis para C#
  - Vulnerability scanning automatizado
  - Scans semanais automáticos

- **🚀 Publish NuGet** - Publicação automatizada
  - Publicação para GitHub Packages
  - Publicação para NuGet.org
  - Criação automática de releases

- **🔄 Auto Dependency Update** - Manutenção automatizada
  - Verificação de dependências desatualizadas
  - Updates de segurança automáticos
  - PRs automáticos para atualizações

### 🛠️ Melhorias Técnicas:
- Atualização para .NET 8.0 em todos os workflows
- Configuração otimizada para MetarDecoder.sln
- Remoção de workflows não aplicáveis (API/Angular)
- Integração com SonarCloud para projeto metar-decoder
- Badges de qualidade no README

### 📦 Impacto:
- ✅ Melhoria significativa na qualidade do código
- ✅ Automação completa do processo de deploy
- ✅ Segurança reforçada com scans automatizados
- ✅ Processo de contribuição simplificado

---

# [TAG 1.0.5 &amp; 1.0.2](https://github.com/afonsoft/metar-decoder/releases/tag/1.0.5.2)
> 03/12/2024 19:25:21 UTC
##### ``1.0.5.2``
TAG 1.0.5 &amp; 1.0.2
# [TAG 1.0.5 &amp; 1.0.1](https://github.com/afonsoft/metar-decoder/releases/tag/1.0.5)
> 03/08/2024 19:31:39 UTC
##### ``1.0.5``
TAG 1.0.5 &amp; 1.0.1
# [Taf &amp; Metar](https://github.com/afonsoft/metar-decoder/releases/tag/1.0.4)
> 03/01/2023 16:54:59 UTC
##### ``1.0.4``
Taf &amp; Metar
# [v1.0.3](https://github.com/afonsoft/metar-decoder/releases/tag/1.03)
> 11/09/2022 12:23:59 UTC
##### ``1.03``
v1.0.3
# [v1.0.2](https://github.com/afonsoft/metar-decoder/releases/tag/1.0.2)
> 10/10/2022 11:21:31 UTC
##### ``1.0.2``
V1.0.2
# [v1.0.1](https://github.com/afonsoft/metar-decoder/releases/tag/1.0.1)
> 10/07/2022 18:46:41 UTC
##### ``1.0.1``
v1.0.1

