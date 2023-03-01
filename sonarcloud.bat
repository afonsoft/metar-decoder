@echo off
REM dotnet tool install --global dotnet-sonarscanner
dotnet restore MetarDecoder.sln --ignore-failed-sources
dotnet sonar\SonarScanner.MSBuild.dll begin /o:"afonsoft-github" /k:"metar-decoder" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=a10c5ba8c51553dcdbe8f87d07806305bf6b8eeb /d:sonar.scm.provider=git /d:sonar.coverage.exclusions="**Test*.cs" /d:sonar.cs.vstest.reportsPaths=resultTest/*.trx
dotnet build MetarDecoder.sln --configuration Release --verbosity minimal --no-incremental --ignore-failed-sources /nr:false
echo.
echo START Metar TEST
echo.
dotnet test tests/Metar.Decoder.Tests/Metar.Decoder.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Metar.Decoder.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
echo.
echo START Taf TEST
echo.
dotnet test tests/Taf.Decoder.Tests/Taf.Decoder.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Taf.Decoder.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
echo.
dotnet sonar\SonarScanner.MSBuild.dll end /d:sonar.login=a10c5ba8c51553dcdbe8f87d07806305bf6b8eeb
echo .
pause
