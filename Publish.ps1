#Requires -Version 7.0

<#
.SYNOPSIS
 Publishes PanoramicData.NCalcExtensions to NuGet.

.DESCRIPTION
    This script performs the following steps:
    1. Checks git repository status (must be clean)
2. Runs unit tests with code coverage
    3. Verifies code coverage meets minimum threshold (90%)
    4. Builds the project in Release mode
    5. Publishes to NuGet using API key from nuget-key.txt

.PARAMETER SkipTests
    Skip running unit tests before publishing

.PARAMETER SkipCoverage
    Skip code coverage check before publishing

.PARAMETER Force
    Force publish even if git working directory is not clean or coverage is below threshold

.PARAMETER MinCoverage
  Minimum required line coverage percentage (default: 90)

.EXAMPLE
    .\Publish.ps1
    
.EXAMPLE
  .\Publish.ps1 -SkipTests
  
.EXAMPLE
    .\Publish.ps1 -Force

.EXAMPLE
    .\Publish.ps1 -MinCoverage 95
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [switch]$SkipTests,
    
    [Parameter(Mandatory = $false)]
  [switch]$SkipCoverage,
    
    [Parameter(Mandatory = $false)]
    [switch]$Force,
    
    [Parameter(Mandatory = $false)]
    [int]$MinCoverage = 90
)

$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$nugetKeyFile = Join-Path $scriptPath "nuget-key.txt"
$projectPath = Join-Path $scriptPath "PanoramicData.NCalcExtensions\PanoramicData.NCalcExtensions.csproj"
$testResultsPath = Join-Path $scriptPath "TestResults"

# Helper function to write colored output
function Write-Status {
    param(
      [string]$Message,
        [string]$Type = "Info"
    )
    
    $color = switch ($Type) {
   "Success" { "`e[32m" }  # Green
        "Error" { "`e[31m" }    # Red
        "Warning" { "`e[33m" }  # Yellow
        "Info" { "`e[36m" }     # Cyan
     default { "`e[0m" }# Reset
    }
    
    $icon = switch ($Type) {
 "Success" { "?" }
        "Error" { "?" }
    "Warning" { "?" }
        "Info" { "?" }
 default { "" }
    }
    
    $reset = "`e[0m"
    Write-Information "$color$icon $Message$reset"
}

function Test-GitPorcelain {
    Write-Status "Checking git repository status..." -Type Info

    # Check if we're in a git repository
    $gitStatus = git status --porcelain 2>&1
 
    if ($LASTEXITCODE -ne 0) {
      Write-Status "Not in a git repository or git is not available" -Type Error
        return $false
    }
    
if ([string]::IsNullOrWhiteSpace($gitStatus)) {
        Write-Status "Git working directory is clean" -Type Success
        return $true
    }
    else {
        Write-Status "Git working directory has uncommitted changes:" -Type Warning
        Write-Information $gitStatus
 return $false
    }
}

function Test-UnitTests {
    Write-Status "Running unit tests..." -Type Info
    
    try {
        $testResult = dotnet test --configuration Release --no-build --verbosity normal
        
   if ($LASTEXITCODE -eq 0) {
       Write-Status "All unit tests passed" -Type Success
      return $true
        }
    else {
  Write-Status "Unit tests failed" -Type Error
         return $false
        }
    }
    catch {
        Write-Status "Error running unit tests: $_" -Type Error
        return $false
  }
}

function Test-UnitTestsWithCoverage {
    Write-Status "Running unit tests with coverage..." -Type Info
    
    try {
     # Clean previous test results
        if (Test-Path $testResultsPath) {
   Remove-Item -Path $testResultsPath -Recurse -Force
        }
    
     # Run tests with coverage
        dotnet test --configuration Release --no-build --collect:"XPlat Code Coverage" --results-directory:$testResultsPath --verbosity normal
    
        if ($LASTEXITCODE -eq 0) {
   Write-Status "All unit tests passed" -Type Success
            return $true
        }
        else {
            Write-Status "Unit tests failed" -Type Error
  return $false
        }
    }
    catch {
        Write-Status "Error running unit tests: $_" -Type Error
        return $false
    }
}

function Build-Project {
    Write-Status "Building project in Release mode..." -Type Info
    
    try {
      dotnet build $projectPath --configuration Release
        
    if ($LASTEXITCODE -eq 0) {
            Write-Status "Build completed successfully" -Type Success
        return $true
        }
    else {
            Write-Status "Build failed" -Type Error
        return $false
    }
    }
    catch {
        Write-Status "Error building project: $_" -Type Error
        return $false
    }
}

function Get-NuGetApiKey {
    Write-Status "Reading NuGet API key..." -Type Info
    
    if (-not (Test-Path $nugetKeyFile)) {
        Write-Status "nuget-key.txt file not found at: $nugetKeyFile" -Type Error
        Write-Status "Please create a nuget-key.txt file with your NuGet API key" -Type Error
     return $null
    }
    
    try {
  $apiKey = Get-Content $nugetKeyFile -Raw
    $apiKey = $apiKey.Trim()
        
if ([string]::IsNullOrWhiteSpace($apiKey)) {
        Write-Status "nuget-key.txt is empty" -Type Error
      return $null
        }
        
      Write-Status "NuGet API key loaded successfully" -Type Success
        return $apiKey
  }
    catch {
   Write-Status "Error reading nuget-key.txt: $_" -Type Error
        return $null
    }
}

function Publish-ToNuGet {
    param([string]$ApiKey)
    
    Write-Status "Publishing to NuGet..." -Type Info
    
    # Find the generated .nupkg file
    $nupkgPath = Get-ChildItem -Path "$scriptPath\PanoramicData.NCalcExtensions\bin\Release" -Filter "*.nupkg" -Recurse | 
   Where-Object { $_.Name -notlike "*.symbols.nupkg" } | 
   Sort-Object LastWriteTime -Descending | 
        Select-Object -First 1
  
    if (-not $nupkgPath) {
   Write-Status "Could not find .nupkg file in bin\Release" -Type Error
        return $false
    }
    
  Write-Status "Found package: $($nupkgPath.Name)" -Type Info
    
    try {
        dotnet nuget push $nupkgPath.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json
 
        if ($LASTEXITCODE -eq 0) {
      Write-Status "Package published successfully to NuGet!" -Type Success
            return $true
        }
     else {
          Write-Status "Failed to publish package" -Type Error
       return $false
        }
  }
    catch {
        Write-Status "Error publishing to NuGet: $_" -Type Error
        return $false
    }
}

function Get-CodeCoverage {
    Write-Status "Analyzing code coverage..." -Type Info

    try {
    # Find coverage file
        $coverageFile = Get-ChildItem -Path $testResultsPath -Filter "coverage.cobertura.xml" -Recurse | Select-Object -First 1
        
if (-not $coverageFile) {
     Write-Status "Coverage file not found" -Type Error
         return $null
        }
        
        # Parse coverage XML
    [xml]$coverageXml = Get-Content $coverageFile.FullName
        $lineRate = [double]$coverageXml.coverage.'line-rate' * 100
  $branchRate = [double]$coverageXml.coverage.'branch-rate' * 100
 
        return @{
     LineCoverage = [math]::Round($lineRate, 1)
   BranchCoverage = [math]::Round($branchRate, 1)
       CoverageFile = $coverageFile.FullName
        }
    }
    catch {
        Write-Status "Error analyzing coverage: $_" -Type Error
        return $null
    }
}

function Test-CoverageThreshold {
    param(
        [hashtable]$Coverage,
        [int]$MinThreshold
    )
    
    Write-Information ""
    Write-Information "`e[36m========================================`e[0m"
    Write-Information "`e[36m  Code Coverage Report`e[0m"
    Write-Information "`e[36m========================================`e[0m"
    Write-Information ""
    
    $lineColor = if ($Coverage.LineCoverage -ge $MinThreshold) { "`e[32m" } else { "`e[31m" }
    Write-Information "  Line Coverage:   $lineColor$($Coverage.LineCoverage)%`e[0m"
    Write-Information "`e[36m  Branch Coverage: $($Coverage.BranchCoverage)%`e[0m"
    Write-Information "`e[33m  Minimum Required: $MinThreshold%`e[0m"
    Write-Information ""
    
    if ($Coverage.LineCoverage -lt $MinThreshold) {
        Write-Status "Coverage is below minimum threshold!" -Type Error
   Write-Status "Current: $($Coverage.LineCoverage)% | Required: $MinThreshold%" -Type Error
   Write-Information ""
Write-Information "`e[33mCoverage report available at: $($Coverage.CoverageFile)`e[0m"
        Write-Information ""
    Write-Information "`e[33mTo generate HTML report, run:`e[0m"
  Write-Information "`e[36m  reportgenerator -reports:`"$($Coverage.CoverageFile)`" -targetdir:`"CoverageReport`"`e[0m"
        Write-Information ""
        return $false
    }
    else {
        Write-Status "Coverage meets minimum threshold ?" -Type Success
        return $true
    }
}

# Main script execution
Write-Information ""
Write-Information "`e[36m========================================`e[0m"
Write-Information "`e[36m  PanoramicData.NCalcExtensions`e[0m"
Write-Information "`e[36m  NuGet Publishing Script`e[0m"
Write-Information "`e[36m========================================`e[0m"
Write-Information ""

try {
    # Step 1: Check git status
    $gitClean = Test-GitPorcelain

    if (-not $gitClean -and -not $Force) {
        Write-Information ""
        $response = Read-Host "Git working directory is not clean. Continue anyway? (y/N)"
        if ($response -ne 'y' -and $response -ne 'Y') {
            Write-Status "Publish cancelled by user" -Type Warning
         exit 1
        }
    }
    
    # Step 2: Build the project
    Write-Information ""
    if (-not (Build-Project)) {
        Write-Status "Build failed. Cannot continue." -Type Error
      exit 1
    }
 
    # Step 3: Run unit tests with coverage
 Write-Information ""
    if (-not $SkipTests) {
     $testsPass = Test-UnitTestsWithCoverage
        
        if (-not $testsPass) {
            Write-Information ""
          $response = Read-Host "Unit tests failed. Continue with publish anyway? (y/N)"
            if ($response -ne 'y' -and $response -ne 'Y') {
      Write-Status "Publish cancelled due to test failures" -Type Warning
       exit 1
    }
 }
        
        # Step 4: Check code coverage
        if (-not $SkipCoverage) {
  Write-Information ""
            $coverage = Get-CodeCoverage
    
   if ($null -eq $coverage) {
if (-not $Force) {
      Write-Status "Could not determine code coverage. Use -Force to skip this check." -Type Error
        exit 1
      }
     else {
    Write-Status "Skipping coverage check (Force parameter specified)" -Type Warning
    }
            }
    else {
       $coverageMet = Test-CoverageThreshold -Coverage $coverage -MinThreshold $MinCoverage
      
  if (-not $coverageMet -and -not $Force) {
     Write-Information ""
       $response = Read-Host "Code coverage is below $MinCoverage%. Continue with publish anyway? (y/N)"
if ($response -ne 'y' -and $response -ne 'Y') {
          Write-Status "Publish cancelled due to insufficient coverage" -Type Warning
        Write-Information ""
         Write-Information "`e[33mOptions:`e[0m"
          Write-Information "`e[36m  1. Add more unit tests to improve coverage`e[0m"
   Write-Information "`e[36m  2. Use -Force to bypass coverage check`e[0m"
         Write-Information "`e[36m  3. Use -MinCoverage <value> to set a different threshold`e[0m"
  Write-Information ""
         exit 1
}
     }
           elseif (-not $coverageMet) {
 Write-Status "Bypassing coverage check (Force parameter specified)" -Type Warning
                }
          }
   }
    }
    else {
        Write-Status "Skipping unit tests and coverage (SkipTests parameter specified)" -Type Warning
    }
    
    # Step 5: Get NuGet API key
    Write-Information ""
    $apiKey = Get-NuGetApiKey
    
    if ($null -eq $apiKey) {
        Write-Status "Cannot proceed without NuGet API key" -Type Error
  exit 1
    }
    
    # Step 6: Publish to NuGet
    Write-Information ""
    if (-not (Publish-ToNuGet -ApiKey $apiKey)) {
        Write-Status "Publish failed" -Type Error
        exit 1
    }
    
    # Success!
    Write-Information ""
    Write-Information "`e[32m========================================`e[0m"
    Write-Status "Publish completed successfully!" -Type Success
    Write-Information "`e[32m========================================`e[0m"
    Write-Information ""
    
    exit 0
}
catch {
    Write-Information ""
    Write-Status "An unexpected error occurred: $_" -Type Error
    Write-Information $_.ScriptStackTrace
    exit 1
}
