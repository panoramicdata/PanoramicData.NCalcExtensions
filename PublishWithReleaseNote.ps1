param(
	[string]$ReleaseNotesPath = (Join-Path $PSScriptRoot 'RELEASE_NOTES.md'),
	[string]$CommitMessagePrefix = 'docs: finalize release notes for'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-CurrentPackageVersion {
	$versionJson = nbgv get-version -f json | ConvertFrom-Json
	return [string]$versionJson.NuGetPackageVersion
}

function Get-NextPackageVersion([string]$currentVersion) {
	if ($currentVersion -notmatch '^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)$') {
		throw "Unsupported NuGet package version format: $currentVersion"
	}

	return '{0}.{1}.{2}' -f $Matches.major, $Matches.minor, ([int]$Matches.patch + 1)
}

function Get-RepositoryRelativePath([string]$fullPath) {
	$rootPath = (Resolve-Path $PSScriptRoot).Path
	$resolvedPath = (Resolve-Path $fullPath).Path
	$uriRoot = [Uri]($rootPath.TrimEnd('\') + '\')
	$uriPath = [Uri]$resolvedPath
	return [Uri]::UnescapeDataString($uriRoot.MakeRelativeUri($uriPath).ToString()).Replace('/', '\')
}

function Assert-OnlyReleaseNotesChanges([string]$allowedRelativePath) {
	$statusLines = @(git status --porcelain --untracked-files=all)
	if (-not $statusLines) {
		return
	}

	foreach ($statusLine in $statusLines) {
		if ([string]::IsNullOrWhiteSpace($statusLine)) {
			continue
		}

		$path = $statusLine.Substring(3)
		if ($path.StartsWith('"') -and $path.EndsWith('"')) {
			$path = $path.Trim('"')
		}

		if ($path -ne $allowedRelativePath) {
			throw "Working tree must be clean except for $allowedRelativePath. Found: $path"
		}
	}
}

function Update-ReleaseNotes([string]$path, [string]$targetVersion) {
	$content = Get-Content -Raw $path
	$unreleasedPattern = '(?m)^## Unreleased\s*$'
	if (-not [regex]::IsMatch($content, $unreleasedPattern)) {
		throw "Release notes file must contain a '## Unreleased' heading."
	}

	$datedHeading = "## $targetVersion - $(Get-Date -Format 'yyyy-MM-dd')"
	return [regex]::Replace(
		$content,
		$unreleasedPattern,
		"## Unreleased`r`n`r`n$datedHeading",
		1)
}

Push-Location $PSScriptRoot
try {
	$branch = git rev-parse --abbrev-ref HEAD
	if ($branch -ne 'main') {
		throw "Not on main branch. Current branch: $branch"
	}

	$resolvedReleaseNotesPath = (Resolve-Path $ReleaseNotesPath).Path
	$releaseNotesRelativePath = Get-RepositoryRelativePath $resolvedReleaseNotesPath

	Assert-OnlyReleaseNotesChanges $releaseNotesRelativePath

	git fetch origin main --quiet
	$behind = git rev-list --count HEAD..origin/main
	if ($behind -gt 0) {
		throw "Local branch is behind origin/main by $behind commit(s)."
	}

	$currentVersion = Get-CurrentPackageVersion
	$targetVersion = Get-NextPackageVersion $currentVersion
	Write-Host "Current version: $currentVersion"
	Write-Host "Target publish version after release notes commit: $targetVersion"

	$updatedReleaseNotes = Update-ReleaseNotes $resolvedReleaseNotesPath $targetVersion
	Set-Content -Path $resolvedReleaseNotesPath -Value $updatedReleaseNotes -NoNewline

	git add -- $releaseNotesRelativePath
	git commit -m "$CommitMessagePrefix $targetVersion"
	git push origin main

	$actualVersion = Get-CurrentPackageVersion
	if ($actualVersion -ne $targetVersion) {
		throw "Expected version $targetVersion after release notes commit, but NBGV reported $actualVersion."
	}

	& (Join-Path $PSScriptRoot 'Publish.ps1')
}
finally {
	Pop-Location
}