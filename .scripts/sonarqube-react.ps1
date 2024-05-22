param (
    [Parameter(Mandatory=$true)]
    [string]$sonarToken,
    [Parameter(Mandatory=$true)]
    [string]$sonarProjectKey,
    [string]$sonarHostUrl = "http://localhost:9000"
)

# Store the parent directory path in a variable
$parentDir = Split-Path -Path $PSScriptRoot -Parent
$baseDir= "$parentDir\src\DomainDrivenVerticalSlices.Template.UI.React"

# Setting environment variables temporarily for the session
$env:SONAR_TOKEN = $sonarToken
$env:SONAR_PROJECT_KEY = $sonarProjectKey
$env:SONAR_HOST_URL = $sonarHostUrl

# Navigate to the base directory where the package.json is located
Push-Location $baseDir

# Run npm commands to test and scan
try {
    Write-Host "Running tests and generating coverage reports..."
    npm run test-sonar
    if ($LASTEXITCODE -ne 0) { throw "Test execution failed with exit code $LASTEXITCODE" }

    Write-Host "Running SonarQube scanner..."
    npm run sonar
    if ($LASTEXITCODE -ne 0) { throw "SonarQube scanning failed with exit code $LASTEXITCODE" }
}
catch {
    Write-Error $_
    exit $LASTEXITCODE
}
finally {
    # Clean up environment variables by setting them to $null
    $env:SONAR_TOKEN = $null
    $env:SONAR_PROJECT_KEY = $null
    $env:SONAR_HOST_URL = $null
    Pop-Location
}

Write-Host "Process completed successfully."
