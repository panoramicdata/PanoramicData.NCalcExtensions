param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$readmePath = Join-Path $repoRoot 'README.md'
$docRoot = Join-Path $repoRoot 'Documentation'
$mainReadme = Join-Path $docRoot 'README.md'

function Normalize-Text {
    param([string[]]$Lines)

    $text = ($Lines | ForEach-Object { if ($null -eq $_) { '' } else { $_.Trim() } } | Where-Object { $_ -ne '' }) -join ' '
    $text = $text -replace '\s+', ' '
    return $text.Trim()
}

function Get-SectionBlock {
    param(
        [string[]]$Lines,
        [string]$Heading
    )

    $startIndex = [Array]::FindIndex($Lines, [Predicate[string]]{ param($line) $line.Trim() -eq $Heading })
    if ($startIndex -lt 0) { return @() }

    $endIndex = $Lines.Length
    for ($index = $startIndex + 1; $index -lt $Lines.Length; $index++) {
        if ($Lines[$index] -match '^#### ' -or $Lines[$index] -match '^### ' -or $Lines[$index] -match '^---$') {
            $endIndex = $index
            break
        }
    }

    if ($endIndex -le $startIndex + 1) { return @() }
    return $Lines[($startIndex + 1)..($endIndex - 1)]
}

function Get-Bullets {
    param([string[]]$Lines)

    $items = New-Object System.Collections.Generic.List[string]
    $current = $null

    foreach ($line in $Lines) {
        $trimmed = $line.TrimEnd()

        if ($trimmed -match '^\s*\*\s+(.*)$') {
            if ($null -ne $current) {
                $items.Add($current.Trim())
            }

            $current = $Matches[1].Trim()
            continue
        }

        if ($null -ne $current) {
            if ($trimmed -eq '') {
                continue
            }

            if ($trimmed -match '^#### ' -or $trimmed -match '^### ' -or $trimmed -match '^---$') {
                $items.Add($current.Trim())
                $current = $null
                continue
            }

            $current += ' ' + $trimmed.Trim()
        }
    }

    if ($null -ne $current) {
        $items.Add($current.Trim())
    }

    return $items.ToArray()
}

function ConvertTo-SafeName {
    param([string]$Name)

    return (($Name -replace '[^A-Za-z0-9]+', '-').Trim('-')).ToLowerInvariant()
}

function ConvertTo-ExpectedAnnotation {
    param([string]$Expected)

    $value = $Expected.Trim()

    switch -Regex ($value) {
        '^(true|false)$' { return "// theAnswer:System.Boolean:$value" }
        '^-?\d+$' { return "// theAnswer:System.Int32:$value" }
        '^-?\d+\.\d+$' { return "// theAnswer:System.Double:$value" }
        '^null$' { return '// theAnswer:System.Object:null' }
        "^'(.*)'$" { return "// theAnswer:System.String:$($Matches[1])" }
        default { return $null }
    }
}

function Get-ReturnTypeDisplay {
    param([string]$Expected)

    if ([string]::IsNullOrWhiteSpace($Expected)) {
        return ''
    }

    $value = $Expected.Trim()

    switch -Regex ($value) {
        '^(true|false)$' { return 'bool' }
        '^-?\d+$' { return 'int' }
        '^-?\d+\.\d+$' { return 'double' }
        '^null$' { return 'object' }
        "^'.*?'($|\s+\()" { return 'string' }
        '^list\(' { return 'System.Collections.Generic.List<object?>' }
        '^JObject\b|^jObject\b' { return 'Newtonsoft.Json.Linq.JObject' }
        '^JArray\b|^jArray\b' { return 'Newtonsoft.Json.Linq.JArray' }
        '^JsonDocument\b|^JsonDocument array\b' { return 'System.Text.Json.JsonDocument' }
        '^DateTimeOffset\b' { return 'System.DateTimeOffset' }
        '^A date time representing\b|^DateTime\b' { return 'System.DateTime' }
        '^typeof\(' { return 'System.Type' }
        '^Dictionary<|^a dictionary\b' { return 'System.Collections.Generic.Dictionary<string, object?>' }
        default { return '' }
    }
}

if (Test-Path $docRoot) {
    Remove-Item $docRoot -Recurse -Force
}
New-Item -ItemType Directory -Path $docRoot | Out-Null

$lines = Get-Content $readmePath
$functionStart = [Array]::FindIndex($lines, [Predicate[string]]{ param($line) $line.Trim() -eq '## Function documentation' })
if ($functionStart -lt 0) {
    throw 'Could not find the Function documentation section in README.md.'
}

$sections = New-Object System.Collections.Generic.List[object]
$currentName = $null
$currentLines = New-Object System.Collections.Generic.List[string]

for ($index = $functionStart + 1; $index -lt $lines.Length; $index++) {
    $line = $lines[$index]

    if ($line -match '^###\s+(.+?)\(\)\s*$') {
        if ($null -ne $currentName) {
            $sections.Add([pscustomobject]@{
                Name = $currentName
                Lines = $currentLines.ToArray()
            })
        }

        $currentName = $Matches[1].Trim()
        $currentLines = New-Object System.Collections.Generic.List[string]
        continue
    }

    if ($null -ne $currentName) {
        $currentLines.Add($line)
    }
}

if ($null -ne $currentName) {
    $sections.Add([pscustomobject]@{
        Name = $currentName
        Lines = $currentLines.ToArray()
    })
}

$frontIndex = New-Object System.Text.StringBuilder
[void]$frontIndex.AppendLine('<table>')
[void]$frontIndex.AppendLine('  <tr>')
[void]$frontIndex.AppendLine('    <td><h1>Daria</h1></td>')
[void]$frontIndex.AppendLine('    <td align="right"><img src="../PanoramicData.NCalcExtensions/Panoramic%20Data.png" alt="Panoramic Data logo" width="96" /></td>')
[void]$frontIndex.AppendLine('  </tr>')
[void]$frontIndex.AppendLine('</table>')
[void]$frontIndex.AppendLine()
[void]$frontIndex.AppendLine('This index points to the generated documentation folders under `Documentation/<function>/`.')
[void]$frontIndex.AppendLine()
[void]$frontIndex.AppendLine('| Function | Purpose | Examples | Folder |')
[void]$frontIndex.AppendLine('| --- | --- | ---: | --- |')

foreach ($section in $sections) {
    $functionName = $section.Name
    $functionFolderName = ConvertTo-SafeName $functionName
    $functionFolder = Join-Path $docRoot $functionFolderName
    New-Item -ItemType Directory -Path $functionFolder -Force | Out-Null

    $purpose = Normalize-Text (Get-SectionBlock -Lines $section.Lines -Heading '#### Purpose')
    $notes = Normalize-Text (Get-SectionBlock -Lines $section.Lines -Heading '#### Notes')
    $parameters = Normalize-Text (Get-SectionBlock -Lines $section.Lines -Heading '#### Parameters')
    $exampleBullets = Get-Bullets (Get-SectionBlock -Lines $section.Lines -Heading '#### Examples')

    $exampleRows = New-Object System.Collections.Generic.List[object]
    $exampleIndex = 1

    foreach ($bullet in $exampleBullets) {
        $exampleText = $bullet
        $expected = $null

        if ($bullet -match '^(.*?)(?:\s+:\s+|\s+:\s+)(.+)$') {
            $exampleText = $Matches[1].Trim()
            $expected = $Matches[2].Trim()
        }

        $exampleFileName = ('example-{0:00}.ncalc' -f $exampleIndex)
        $exampleFilePath = Join-Path $functionFolder $exampleFileName
        $annotation = $null
        if ($null -ne $expected) {
            $annotation = ConvertTo-ExpectedAnnotation $expected
        }

        $exampleContent = New-Object System.Text.StringBuilder
        [void]$exampleContent.AppendLine('/*')
        [void]$exampleContent.AppendLine("# $functionName()")
        [void]$exampleContent.AppendLine()
        [void]$exampleContent.AppendLine("Generated documentation example $exampleIndex for $functionName().")
        if ($null -ne $expected) {
            [void]$exampleContent.AppendLine()
            [void]$exampleContent.AppendLine('**Expected result**')
            [void]$exampleContent.AppendLine($expected)
        }
        [void]$exampleContent.AppendLine('*/')
        [void]$exampleContent.AppendLine()
        if ($null -ne $annotation) {
            [void]$exampleContent.AppendLine($annotation)
            [void]$exampleContent.AppendLine()
        }
        [void]$exampleContent.AppendLine($exampleText)

        Set-Content -Path $exampleFilePath -Value $exampleContent.ToString() -Encoding UTF8

        $rawUrl = "https://raw.githubusercontent.com/panoramicdata/PanoramicData.NCalcExtensions/main/Documentation/$functionFolderName/$exampleFileName"
        $ncalc101Url = 'https://ncalc101.magicsuite.net/?url=' + [System.Uri]::EscapeDataString($rawUrl)

        $exampleRows.Add([pscustomobject]@{
            Number = $exampleIndex
            Expression = $exampleText
            Expected = $expected
            ReturnType = (Get-ReturnTypeDisplay $expected)
            File = $exampleFileName
            NCalc101 = $ncalc101Url
        })

        $exampleIndex++
    }

    $indexBuilder = New-Object System.Text.StringBuilder
    [void]$indexBuilder.AppendLine("# $functionName()")
    [void]$indexBuilder.AppendLine()
    [void]$indexBuilder.AppendLine('| Field | Value |')
    [void]$indexBuilder.AppendLine('| --- | --- |')
    if ($purpose) { [void]$indexBuilder.AppendLine("| Purpose | $purpose |") }
    if ($parameters) { [void]$indexBuilder.AppendLine("| Parameters | $parameters |") }
    if ($notes) { [void]$indexBuilder.AppendLine("| Notes | $notes |") }
    [void]$indexBuilder.AppendLine("| Examples | $($exampleRows.Count) |")
    [void]$indexBuilder.AppendLine()
    [void]$indexBuilder.AppendLine('## Examples')
    [void]$indexBuilder.AppendLine()
    [void]$indexBuilder.AppendLine('| # | Example | Return type | Expected | .ncalc | NCalc101 |')
    [void]$indexBuilder.AppendLine('| ---: | --- | --- | --- | --- | --- |')

    foreach ($row in $exampleRows) {
        $escapedExpression = $row.Expression.Replace('|', '\|')
        $escapedReturnType = if ($row.ReturnType) { $row.ReturnType.Replace('|', '\|') } else { '' }
        $escapedExpected = if ($row.Expected) { $row.Expected.Replace('|', '\|') } else { '' }
        $ncalc101Link = "[Open example]($($row.NCalc101))"
        [void]$indexBuilder.AppendLine('| ' + $row.Number + ' | ' + $escapedExpression + ' | ' + $escapedReturnType + ' | ' + $escapedExpected + ' | [' + $row.File + '](' + $row.File + ') | ' + $ncalc101Link + ' |')
    }

    Set-Content -Path (Join-Path $functionFolder 'README.md') -Value $indexBuilder.ToString() -Encoding UTF8

    [void]$frontIndex.AppendLine('| [' + $functionName + '()](Documentation/' + $functionFolderName + '/) | ' + $purpose + ' | ' + $exampleRows.Count + ' | [Folder](Documentation/' + $functionFolderName + '/) |')
}

Set-Content -Path $mainReadme -Value $frontIndex.ToString() -Encoding UTF8

Write-Host "Generated documentation for $($sections.Count) functions under $docRoot"
