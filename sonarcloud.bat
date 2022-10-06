@echo off
dotnet restore Eaf.sln --ignore-failed-sources --configfile nuget.config
dotnet restore Eaf.sln --ignore-failed-sources
dotnet sonar\SonarScanner.MSBuild.dll begin /o:"afonsoft-github" /k:"EAF" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=22744be1b428aa6d018d32d5a78099e00d17ea3d /d:sonar.scm.provider=git /d:sonar.coverage.exclusions="**Test*.cs" /d:sonar.cs.vstest.reportsPaths=resultTest/*.trx
dotnet build Eaf.sln --configuration Release --verbosity minimal --no-incremental --ignore-failed-sources
echo.
echo START TEST
echo.
dotnet test test/Eaf.AspNetCore.Tests/Eaf.AspNetCore.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.AspNetCore.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.AutoMapper.Tests/Eaf.AutoMapper.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.AutoMapper.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.Castle.Log4Net.Tests/Eaf.Castle.Log4Net.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.Castle.Log4Net.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.EntityFrameworkCore.Dapper.Tests/Eaf.EntityFrameworkCore.Dapper.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.EntityFrameworkCore.Dapper.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.EntityFrameworkCore.Tests/Eaf.EntityFrameworkCore.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.EntityFrameworkCore.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.MailKit.Tests/Eaf.MailKit.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.MailKit.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.MemoryDb.Tests/Eaf.MemoryDb.Testss.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.MemoryDb.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.MiddlewareCore.SampleApp/Eaf.MiddlewareCore.SampleApp.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.MiddlewareCore.SampleApp.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.MiddlewareCore.Tests/Eaf.MiddlewareCore.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.MiddlewareCore.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.RedisCache.Tests/Eaf.RedisCache.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.RedisCache.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.TestBase.Tests/Eaf.TestBase.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.TestBase.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.Tests/Eaf.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
dotnet test test/Eaf.Web.Common.Tests/Eaf.Web.Common.Tests.csproj --collect:"Code Coverage" --logger "trx;LogFileName=Eaf.Web.Common.Tests.trx" --results-directory resultTest/ --no-build --no-restore --configuration release -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
echo .
dotnet sonar\SonarScanner.MSBuild.dll end /d:sonar.login=22744be1b428aa6d018d32d5a78099e00d17ea3d
pause

