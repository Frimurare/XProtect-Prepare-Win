<#
.SYNOPSIS
  Build the XProtect Prepare Win MSI installer end-to-end.

.DESCRIPTION
  Publishes the WPF app as a SINGLE self-contained win-x64 exe (the .NET 9
  runtime is bundled — no prerequisite on the target machine), then builds
  the WiX 5 MSI and drops it in <repo>\installer-package\.

  Run from the repo root or from this directory on the Windows build VM.
  Requires:
    - .NET 9 SDK
    - WiX 5 SDK (auto-restored via dotnet build)

.EXAMPLE
  pwsh ./src/XProtectPrepareWin.Installer/build-msi.ps1
#>
[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$Version = ''
)

$ErrorActionPreference = 'Stop'

$repoRoot      = Resolve-Path (Join-Path $PSScriptRoot '..\..')
$appProj       = Join-Path $repoRoot 'src\XProtectPrepareWin\XProtectPrepareWin.csproj'
$installerProj = Join-Path $repoRoot 'src\XProtectPrepareWin.Installer\XProtectPrepareWin.Installer.wixproj'
$publishDir    = Join-Path $PSScriptRoot 'bin\publish'
$packageDir    = Join-Path $repoRoot 'installer-package'

# To cut a release, bump ONE line: <Version> in the app .csproj. When -Version
# is not passed, read it from there so the MSI version can never drift from a
# stale hard-coded default.
if (-not $Version) {
    $csprojText = Get-Content $appProj -Raw
    if ($csprojText -match '<Version>\s*([0-9]+\.[0-9]+\.[0-9]+)\s*<') {
        $Version = $Matches[1]
        Write-Host ("== Version {0} (from XProtectPrepareWin.csproj) ==" -f $Version)
    }
    else {
        throw "Could not read <Version> from XProtectPrepareWin.csproj - pass -Version explicitly."
    }
}

# ----------------------------------------------------------------------------
# 1. Publish the app: single-file, self-contained. Fresh Milestone/XProtect
#    servers have no .NET 9 runtime — bundling it means zero prerequisites.
# ----------------------------------------------------------------------------
Write-Host '== Publishing XProtectPrepareWin (single-file, self-contained) =='
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

dotnet publish $appProj -c $Configuration -r win-x64 --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableWindowsTargeting=true `
    -p:PublishTrimmed=false `
    -p:Version=$Version -p:AssemblyVersion="$Version.0" -p:FileVersion="$Version.0" `
    -o $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE." }

$exe = Join-Path $publishDir 'XProtectPrepareWin.exe'
if (-not (Test-Path $exe)) { throw "Publish output missing: $exe" }
Write-Host ('   exe: {0} ({1:N1} MB)' -f $exe, ((Get-Item $exe).Length / 1MB))

# ----------------------------------------------------------------------------
# 2. Build the MSI (WiX 5, dotnet build with 'wix build' fallback)
# ----------------------------------------------------------------------------
Write-Host '== Building MSI =='
dotnet build $installerProj -c $Configuration -p:ProductVersion=$Version
if ($LASTEXITCODE -ne 0) {
    Write-Warning 'dotnet build of the wixproj failed - trying the wix CLI fallback.'
    Push-Location $PSScriptRoot
    try {
        wix build Product.wxs `
            -ext WixToolset.UI.wixext `
            -arch x64 `
            -d "ProductVersion=$Version" `
            -o (Join-Path $PSScriptRoot "bin\$Configuration\XProtectPrepareWin-$Version.msi")
        if ($LASTEXITCODE -ne 0) { throw "wix build fallback failed with exit code $LASTEXITCODE." }
    }
    finally { Pop-Location }
}

# ----------------------------------------------------------------------------
# 3. Collect the MSI into installer-package\
# ----------------------------------------------------------------------------
$msi = Get-ChildItem -Path (Join-Path $PSScriptRoot 'bin') -Recurse -Filter '*.msi' |
    Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $msi) { throw 'No MSI produced - check the build output above.' }

if (-not (Test-Path $packageDir)) { New-Item -ItemType Directory -Path $packageDir | Out-Null }
$dest = Join-Path $packageDir ("XProtectPrepareWin-{0}.msi" -f $Version)
Copy-Item $msi.FullName $dest -Force

Write-Host ''
Write-Host ('MSI:  {0}' -f $dest)
Write-Host ('Size: {0:N1} MB' -f ((Get-Item $dest).Length / 1MB))
Write-Host 'Done.'
