param (
    [string]$testBaseDir = "test",
    [Parameter(Mandatory=$true)]
    [string]$sonarToken,
    [Parameter(Mandatory=$true)]
    [string]$sonarProjectKey,
    [string]$sonarHostUrl = "http://localhost:9000"
)

# Store the parent directory path in a variable
$parentDir = Split-Path -Path $PSScriptRoot -Parent

# Find all test projects
$testProjects = Get-ChildItem -Path "$parentDir\$testBaseDir" -Recurse -Filter "*.Tests.csproj"

# Run dotnet test with coverage on each found test project
foreach ($testProject in $testProjects) {
    dotnet test $testProject.FullName /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
}

# Begin SonarScanner analysis
dotnet sonarscanner begin `
    /k:"$sonarProjectKey" `
    /d:sonar.host.url="$sonarHostUrl" `
    /d:sonar.token="$sonarToken" `
    /d:sonar.cs.opencover.reportsPaths="$parentDir\$testBaseDir\*\coverage.opencover.xml" `
    /d:sonar.coverage.exclusions="**Tests.cs,**Program.cs,**DatabaseInitializer.cs,**LoggerExtensions.cs,**AppExtensions.cs" `
    /d:sonar.exclusions="**/DomainDrivenVerticalSlices.Template.Infrastructure/Migrations/**"

# Build the project
dotnet build $parentDir

# End SonarScanner analysis
dotnet sonarscanner end /d:sonar.login="$sonarToken"
