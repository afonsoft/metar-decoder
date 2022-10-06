@echo off
dotnet restore MetarDecoder.sln --ignore-failed-sources
dotnet sonar\SonarScanner.MSBuild.dll begin /o:"afonsoft-github" /k:"EAF" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=22744be1b428aa6d018d32d5a78099e00d17ea3d /d:sonar.scm.provider=git /d:sonar.coverage.exclusions="**Test*.cs" /d:sonar.cs.vstest.reportsPaths=resultTest/*.trx
dotnet build MetarDecoder.sln --configuration Release --verbosity minimal --no-incremental --ignore-failed-sources
echo.
echo START TEST
echo.
dotnet test tests/Metar.Decoder.Tests/Metar.Decoder.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Metar.Decoder.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
echo .
dotnet sonar\SonarScanner.MSBuild.dll end /d:sonar.login=22744be1b428aa6d018d32d5a78099e00d17ea3d
pause

