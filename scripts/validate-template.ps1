[CmdletBinding()]
param(
    [string] $OutputDirectory = (Join-Path $PSScriptRoot '../artifacts/validation'),
    [string] $NuGetCommand = '',
    [switch] $SourceOnly
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repo = Split-Path $PSScriptRoot -Parent
$output = [IO.Path]::GetFullPath($OutputDirectory)
$successEvidence = Join-Path $output 'validation.json'
if (Test-Path $successEvidence) {
    throw "Output directory already contains success evidence ($successEvidence). Choose a fresh output directory."
}
if (Test-Path $output) {
    $stalePackages = @(Get-ChildItem $output -Filter '*.nupkg' -File -ErrorAction SilentlyContinue)
    if ($stalePackages.Count) { throw "Output directory already contains package files. Choose a fresh output directory: $output" }
}
$scratch = Join-Path ([IO.Path]::GetTempPath()) ('ddvs-' + [guid]::NewGuid().ToString('N').Substring(0, 8))
New-Item -ItemType Directory -Force -Path $output, $scratch | Out-Null
$hive = Join-Path $scratch 'hive'
$generated = @{}

function Invoke-Checked {
    param([string] $Command, [string[]] $Arguments)
    Write-Host "> $Command $($Arguments -join ' ')"
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Command failed with exit code $LASTEXITCODE" }
}

function Test-Solution {
    param([string] $Directory, [string] $Solution, [string] $Tests, [switch] $Generated)
    $restoreArgs = @('restore', (Join-Path $Directory $Solution))
    # Generated projects deliberately omit maintainer lock files. CI's inherited
    # RestoreLockedMode must not prevent their initial restore.
    if ($Generated) { $restoreArgs += '-p:RestoreLockedMode=false' }
    $restoreArgs += @('-m:2')
    Invoke-Checked dotnet $restoreArgs
    Invoke-Checked dotnet @('build', (Join-Path $Directory $Solution), '-c', 'Release', '--no-restore', '-m:2')
    $projects = @(Get-ChildItem (Join-Path $Directory $Tests) -Recurse -Filter '*.csproj')
    if ($projects.Count -eq 0) { throw "No tests found in $Directory/$Tests" }
    foreach ($project in $projects) {
        Invoke-Checked dotnet @('test', $project.FullName, '-c', 'Release', '--no-build', '--no-restore', '-m:2')
    }
}

function Get-VulnerabilityFindings {
    param([System.Collections.IDictionary] $Report, [string] $Solution)
    if ($null -eq $Report -or $Report['version'] -ne 1) {
        throw "Vulnerability audit returned an unsupported or incomplete report for $Solution."
    }
    $projectsValue = $Report['projects']
    $projects = @($projectsValue | Where-Object { $null -ne $_ })
    if ($projects.Count -eq 0) { throw "Vulnerability audit returned no projects for $Solution." }
    $errorsValue = $Report['errors']
    if ($null -ne $errorsValue) {
        $errors = @($errorsValue | Where-Object { $null -ne $_ -and "$($_)".Length -gt 0 })
        if ($errors.Count -gt 0) { throw "Vulnerability audit reported errors for $Solution`n$($errors -join "`n")" }
    }

    $findings = [Collections.Generic.List[string]]::new()
    foreach ($project in $projects) {
        $frameworksValue = $project['frameworks']
        foreach ($framework in @($frameworksValue | Where-Object { $null -ne $_ })) {
            foreach ($kind in @('topLevelPackages', 'transitivePackages')) {
                $itemsValue = $framework[$kind]
                foreach ($item in @($itemsValue | Where-Object { $null -ne $_ })) {
                    $vulnerabilitiesValue = $item['vulnerabilities']
                    foreach ($vulnerability in @($vulnerabilitiesValue | Where-Object { $null -ne $_ })) {
                        $findings.Add("$($item['id']) $($item['resolvedVersion']): $($vulnerability['severity'])")
                    }
                }
            }
        }
    }
    return $findings
}

function Test-VulnerabilityReportParser {
    $clean = '{"version":1,"projects":[{"path":"clean.csproj"}]}' | ConvertFrom-Json -AsHashtable
    if (@(Get-VulnerabilityFindings $clean 'clean fixture').Count -ne 0) {
        throw 'Clean vulnerability report fixture unexpectedly produced findings.'
    }
    $vulnerable = '{"version":1,"projects":[{"frameworks":[{"transitivePackages":[{"id":"Fixture.Package","resolvedVersion":"1.0.0","vulnerabilities":[{"severity":"High"}]}]}]}]}' | ConvertFrom-Json -AsHashtable
    if (@(Get-VulnerabilityFindings $vulnerable 'vulnerable fixture').Count -ne 1) {
        throw 'Vulnerable package fixture was not detected.'
    }
    $feedError = '{"version":1,"projects":[{"path":"clean.csproj"}],"errors":["feed unavailable"]}' | ConvertFrom-Json -AsHashtable
    $failed = $false
    try { Get-VulnerabilityFindings $feedError 'feed-error fixture' | Out-Null }
    catch { $failed = $true }
    if (!$failed) { throw 'Vulnerability feed-error fixture was not rejected.' }
}

function Test-Vulnerabilities {
    param([string] $Solution, [string] $ReportPath)
    $arguments = @('list', $Solution, 'package', '--vulnerable', '--include-transitive', '--format', 'json', '--no-restore')
    Write-Host "> dotnet $($arguments -join ' ')"
    $outputLines = & dotnet @arguments 2>&1
    $exitCode = $LASTEXITCODE
    if ($exitCode -ne 0) { throw "Vulnerability audit failed ($Solution) with exit code $exitCode`n$($outputLines -join "`n")" }
    $text = $outputLines -join "`n"
    try { $report = $text | ConvertFrom-Json -AsHashtable -ErrorAction Stop }
    catch { throw "Vulnerability audit returned invalid JSON for $Solution. Feed/command errors are not a clean result.`n$text" }
    try { $findings = @(Get-VulnerabilityFindings $report $Solution) }
    catch { throw "$($_.Exception.Message)`n$text" }
    $report | ConvertTo-Json -Depth 50 | Set-Content -Encoding utf8 $ReportPath
    if ($findings.Count) { throw "NuGet vulnerabilities found in $Solution`n$($findings -join "`n")" }
}

function Assert-CleanOutput {
    param([string] $Directory)
    $forbidden = @('CLAUDE.md', 'AGENTS.md', 'CHANGELOG.md', '.claude', '.agents', '.github',
        '.template.config', 'scripts', 'docs/ROADMAP.md', 'docs/RELEASE_PROCESS.md',
        'docs/MAINTAINER_GUIDE.md', 'docs/architecture', 'docs/archive', 'docs/DEPENDENCIES.md', 'docs/MIGRATIONS.md')
    foreach ($name in $forbidden) {
        if (Test-Path (Join-Path $Directory $name)) { throw "Maintainer file leaked: $Directory/$name" }
    }
    foreach ($file in Get-ChildItem $Directory -Recurse -File -Force) {
        if ($file.Name -eq 'packages.lock.json' -or $file.Extension -eq '.code-workspace') {
            throw "Maintainer file leaked: $($file.FullName)"
        }
        if ($file.Extension -in @('.cs', '.csproj', '.slnx', '.props', '.json', '.md', '.jsx', '.js', '.yml', '.yaml')) {
            if ([IO.File]::ReadAllText($file.FullName).Contains('DomainDrivenVerticalSlices.Template')) {
                throw "Unrenamed template name: $($file.FullName)"
            }
        }
    }
}

function Get-Manifest {
    param([string] $Directory)
    @(Get-ChildItem $Directory -Recurse -File -Force | ForEach-Object {
        $relative = [IO.Path]::GetRelativePath($Directory, $_.FullName).Replace('\', '/')
        "$relative $((Get-FileHash $_.FullName -Algorithm SHA256).Hash)"
    } | Sort-Object)
}

Push-Location $repo
try {
    Test-VulnerabilityReportParser
    $a = Get-Content '.template.config/template.json' -Raw | ConvertFrom-Json
    $b = Get-Content 'templates/ddvs-clean/.template.config/template.json' -Raw | ConvertFrom-Json
    foreach ($part in @('symbols', 'sources')) {
        $left = $a.$part | ConvertTo-Json -Depth 40 -Compress
        if ($part -eq 'sources') { $b.sources[0].source = $a.sources[0].source }
        $right = $b.$part | ConvertTo-Json -Depth 40 -Compress
        if ($left -cne $right) { throw "ddvs/ddvs-clean config mismatch: $part" }
    }
    Push-Location (Join-Path $repo 'src/DomainDrivenVerticalSlices.Template.UI.React')
    try {
        Invoke-Checked npm @('audit', '--offline=false', '--include=dev', '--audit-level=low', '--fetch-retries=0', '--fetch-timeout=30000')
    } finally { Pop-Location }
    Test-Solution $repo 'DomainDrivenVerticalSlices.Template.slnx' 'test'
    Test-Solution (Join-Path $repo 'templates/ddvs-modular') 'DomainDrivenVerticalSlices.Template.slnx' 'tests'
    Invoke-Checked python @('scripts/dependency-audit.py', '--output', (Join-Path $output 'licenses.json'), '--fail-on-review')
    Test-Vulnerabilities 'DomainDrivenVerticalSlices.Template.slnx' (Join-Path $output 'vulnerabilities-clean.json')
    Test-Vulnerabilities 'templates/ddvs-modular/DomainDrivenVerticalSlices.Template.slnx' (Join-Path $output 'vulnerabilities-modular.json')
    if ($SourceOnly) { return }

    if (!$NuGetCommand) {
        $NuGetCommand = if ($IsWindows) { Join-Path $repo 'nuget.exe' } else { 'nuget' }
    }
    $packageDirectory = Join-Path $scratch 'package'
    New-Item -ItemType Directory -Path $packageDirectory | Out-Null
    Invoke-Checked $NuGetCommand @('pack', 'DomainDrivenVerticalSlices.Template.nuspec', '-OutputDirectory', $packageDirectory, '-NoDefaultExcludes')
    $packages = @(Get-ChildItem $packageDirectory -Filter '*.nupkg')
    if ($packages.Count -ne 1) { throw 'Expected exactly one newly built package' }
    $package = $packages[0].FullName
    $zip = [IO.Compression.ZipFile]::OpenRead($package)
    try {
        $entries = @($zip.Entries | ForEach-Object { $_.FullName.Replace('\', '/') })
        $bad = @($entries | Where-Object {
            $_ -match '(^|/)(\.github|\.claude|\.agents|scripts|artifacts|node_modules|bin|obj|coverage|dist|TestResults)/' -or
            $_ -match '(^|/)(CLAUDE\.md|AGENTS\.md|CHANGELOG\.md|packages\.lock\.json)$' -or
            $_ -match '\.code-workspace$|content/docs/(ROADMAP|RELEASE_PROCESS|MAINTAINER_GUIDE|DEPENDENCIES|MIGRATIONS)\.md|content/docs/archive/|content/templates/ddvs-modular/docs/'
        })
        if ($bad.Count) { throw "Package leaks: $($bad -join ', ')" }
        foreach ($required in @('content/.editorconfig', 'content/.template.config/template.json',
            'content/templates/ddvs-clean/.template.config/template.json',
            'content/templates/ddvs-modular/.template.config/template.json',
            'content/templates/ddvs-modular/.editorconfig',
            'content/src/DomainDrivenVerticalSlices.Template.UI.React/.gitignore',
            'content/src/DomainDrivenVerticalSlices.Template.UI.React/package-lock.json')) {
            if ($required -notin $entries) { throw "Missing package entry: $required" }
        }
    } finally { $zip.Dispose() }
    Invoke-Checked dotnet @('new', 'install', $package, '--debug:custom-hive', $hive)

    foreach ($variant in @('ddvs', 'ddvs-clean')) {
        foreach ($api in @('Controller', 'MinimalApi')) {
            foreach ($ui in @('None', 'React')) {
                $key = "$variant-$api-$ui"
                $destination = Join-Path $scratch $key
                Invoke-Checked dotnet @('new', $variant, '-n', 'App', '-o', $destination, '--ApiType', $api, '--UiType', $ui, '--debug:custom-hive', $hive)
                Assert-CleanOutput $destination
                $controllerExists = Test-Path "$destination/src/App.WebApi/Controllers"
                $endpointExists = Test-Path "$destination/src/App.WebApi/Endpoints"
                if ($controllerExists -ne ($api -eq 'Controller') -or $endpointExists -ne ($api -eq 'MinimalApi')) {
                    throw "API option not respected: $key"
                }
                if ((Test-Path "$destination/src/App.UI.React") -ne ($ui -eq 'React')) { throw "UI option not respected: $key" }
                $generated[$key] = @{ Path = $destination; Manifest = @(Get-Manifest $destination); Tests = 'test'; React = ($ui -eq 'React') }
            }
        }
    }
    foreach ($api in @('Controller', 'MinimalApi')) {
        foreach ($ui in @('None', 'React')) {
            $diff = Compare-Object $generated["ddvs-$api-$ui"].Manifest $generated["ddvs-clean-$api-$ui"].Manifest
            if ($diff) { throw "Generated alias mismatch: $api/$ui`n$($diff | Out-String)" }
        }
    }
    foreach ($aspire in @('true', 'false')) {
        $key = "modular-$aspire"
        $destination = Join-Path $scratch $key
        Invoke-Checked dotnet @('new', 'ddvs-modular', '-n', 'App', '-o', $destination, '--UseAspire', $aspire, '--debug:custom-hive', $hive)
        Assert-CleanOutput $destination
        foreach ($project in @('AppHost', 'ServiceDefaults')) {
            if ((Test-Path "$destination/src/App.$project") -ne ($aspire -eq 'true')) { throw "Aspire option not respected: $key/$project" }
        }
        $generated[$key] = @{ Path = $destination; Tests = 'tests'; React = $false }
    }
    foreach ($key in ($generated.Keys | Sort-Object)) {
        $item = $generated[$key]
        Test-Solution $item.Path 'App.slnx' $item.Tests -Generated
        if ($item.React) {
            Push-Location (Join-Path $item.Path 'src/App.UI.React')
            try {
                Invoke-Checked npm @('ci')
                Invoke-Checked npm @('run', 'build')
                Invoke-Checked npm @('test', '--', '--run')
            } finally { Pop-Location }
        }
    }
    # Copy only the package that passed every check. Publication consumes this path.
    Copy-Item -LiteralPath $package -Destination $output
    @{ Package = $packages[0].Name; Sha256 = (Get-FileHash $package -Algorithm SHA256).Hash;
        Variants = @($generated.Keys | Sort-Object); Commit = (& git rev-parse HEAD); Scratch = $scratch
    } | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $output 'validation.json')
    Write-Host "Validated all 10 configurations. Package: $output/$($packages[0].Name)"
} finally {
    Pop-Location
    # Preserve diagnostics, including generated projects, on both success and failure.
    Write-Host "Validation workspace: $scratch"
}
