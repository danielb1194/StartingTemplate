param(
    [string]$ModName,
    [string]$ShortDescription,
    [string]$DestinationRoot
)

$ErrorActionPreference = "Stop"
$TemplateRoot = if ($PSScriptRoot) { $PSScriptRoot } else { (Get-Location).Path }

if ([string]::IsNullOrWhiteSpace($ModName)) {
    $ModName = Read-Host "Enter mod name (must be a valid C# identifier)"
}
if ([string]::IsNullOrWhiteSpace($ShortDescription)) {
    $ShortDescription = Read-Host "Enter short description"
}
if ([string]::IsNullOrWhiteSpace($DestinationRoot)) {
    $DestinationRoot = Split-Path -Parent $TemplateRoot
}

$ModName = $ModName.Trim()
$ShortDescription = $ShortDescription.Trim()

if ([string]::IsNullOrWhiteSpace($ModName)) {
    throw "Mod name cannot be empty."
}
if ([string]::IsNullOrWhiteSpace($ShortDescription)) {
    throw "Short description cannot be empty."
}
if ($ModName -notmatch '^[A-Za-z_][A-Za-z0-9_]*$') {
    throw "Mod name '$ModName' is not a valid C# identifier. Use letters, numbers, and underscores, and do not start with a number."
}
if (-not (Test-Path $DestinationRoot)) {
    throw "Destination root does not exist: $DestinationRoot"
}

$targetRoot = Join-Path $DestinationRoot $ModName
if (Test-Path $targetRoot) {
    throw "Target folder already exists: $targetRoot"
}

$excludeDirectories = @(
    '.git',
    '.vs',
    'bin',
    'obj',
    'output'
)

$allowedTopLevelDirectories = @(
    'assets',
    '.vscode'
)

$excludeFiles = @(
    [System.IO.Path]::GetFileName($PSCommandPath)
)

$textExtensions = @(
    '.cs',
    '.csproj',
    '.json',
    '.md',
    '.ps1',
    '.txt',
    '.xml',
    '.props',
    '.targets',
    '.editorconfig',
    '.yml',
    '.yaml',
    '.config',
    '.sln'
)

function Get-RelativePath {
    param(
        [string]$RootPath,
        [string]$Path
    )

    $rootUri = [System.Uri]((Resolve-Path $RootPath).Path + [System.IO.Path]::DirectorySeparatorChar)
    $pathUri = [System.Uri](Resolve-Path $Path).Path
    return [System.Uri]::UnescapeDataString($rootUri.MakeRelativeUri($pathUri).ToString()).Replace('/', [System.IO.Path]::DirectorySeparatorChar)
}

function Replace-TemplateTokens {
    param([string]$InputText)

    return $InputText.Replace('_MOD_NAME_', $ModName).Replace('_MOD_SHORT_DESCRIPTION_', $ShortDescription).Replace('_MOD_NAME_', $ModName)
}

Write-Host "Creating new mod template at: $targetRoot"
$allItems = Get-ChildItem -Path $TemplateRoot -Recurse -Force

New-Item -ItemType Directory -Path $targetRoot -Force | Out-Null

foreach ($item in $allItems) {
    if ($item.FullName.StartsWith($targetRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        continue
    }

    $relative = Get-RelativePath -RootPath $TemplateRoot -Path $item.FullName

    # Skip anything under excluded directories.
    $segments = $relative.Split([System.IO.Path]::DirectorySeparatorChar)
    $matchingExcludedSegments = @($segments | Where-Object { $excludeDirectories -contains $_ })
    if ($matchingExcludedSegments.Count -gt 0) {
        continue
    }

    # Only include known template directories from the root (for now: assets).
    $topLevelSegment = $segments[0]
    if ($segments.Count -gt 1 -and -not ($allowedTopLevelDirectories -contains $topLevelSegment)) {
        continue
    }
    if ($item.PSIsContainer -and $segments.Count -eq 1 -and -not ($allowedTopLevelDirectories -contains $topLevelSegment)) {
        continue
    }

    if ($item.PSIsContainer) {
        $newRelative = Replace-TemplateTokens -InputText $relative
        $targetDir = Join-Path $targetRoot $newRelative
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
        continue
    }

    if ($excludeFiles -contains $item.Name) {
        continue
    }

    $newRelativePath = Replace-TemplateTokens -InputText $relative
    $destinationFile = Join-Path $targetRoot $newRelativePath
    $destinationDir = Split-Path -Parent $destinationFile
    if (-not (Test-Path $destinationDir)) {
        New-Item -ItemType Directory -Path $destinationDir -Force | Out-Null
    }

    $extension = [System.IO.Path]::GetExtension($item.Name)
    if ($textExtensions -contains $extension.ToLowerInvariant()) {
        $content = Get-Content -Path $item.FullName -Raw
        $updatedContent = Replace-TemplateTokens -InputText $content
        Set-Content -Path $destinationFile -Value $updatedContent -Encoding UTF8
    }
    else {
        Copy-Item -Path $item.FullName -Destination $destinationFile -Force
    }
}

Write-Host "Template generation complete."
Write-Host "New mod folder: $targetRoot"
