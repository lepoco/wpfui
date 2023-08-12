# Copyright 2022 Leszek Pomianowski and WPF UI Contributors

[CmdletBinding(PositionalBinding = $false)]
Param(
    [string][Alias('c')]$configuration = "Release",
    [string][Alias('v')]$verbosity = "minimal",
    [string][Alias('p')]$platform = "AnyCPU",
    [string][Alias('s')]$solution = "",
    [switch][Alias('r')]$restore,
    [switch][Alias('b')]$build,
    [switch] $nologo,
    [switch] $help,
    [Parameter(ValueFromRemainingArguments = $true)][String[]]$properties
)

$Script:BuildPath = ""
$Script:VsPath = ""

function Invoke-Help {
    Write-Host "Common settings:"
    Write-Host "  -configuration <value>  Build configuration: 'Debug' or 'Release' (short: -c)"
    Write-Host "  -platform <value>       Platform configuration: 'x86', 'x64' or any valid Platform value to pass to msbuild"
    Write-Host "  -verbosity <value>      Msbuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic] (short: -v)"
    Write-Host "  -nologo                 Doesn't display the startup banner or the copyright message"
    Write-Host "  -help                   Print help and exit"
    Write-Host ""

    Write-Host "Actions:"
    Write-Host "  -restore                Restore dependencies (short: -r)"
    Write-Host "  -build                  Build solution (short: -b)"
    Write-Host ""
}

function Invoke-Hello {
    if ($nologo) {
        return
    }

    $TextInfo = (Get-Culture).TextInfo

    Write-Host " -------------------" -ForegroundColor Cyan
    Write-Host "| " -NoNewline -ForegroundColor Cyan
    Write-Host "       WPF UI"  -NoNewline -ForegroundColor White
    Write-Host "      | "-ForegroundColor Cyan
    Write-Host "| " -NoNewline -ForegroundColor Cyan
    Write-Host " lepo.co 2021-$(Get-Date -UFormat "%Y")" -NoNewline -ForegroundColor Gray
    Write-Host " | " -ForegroundColor Cyan
    Write-Host " ------------------ - " -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Solution:      " -NoNewline
    Write-Host "$($Script:Solution)" -ForegroundColor Cyan
    Write-Host "Platform:      " -NoNewline
    Write-Host "$($TextInfo.ToTitleCase($platform))" -ForegroundColor Cyan
    Write-Host "Configuration: " -NoNewline
    Write-Host "$($TextInfo.ToTitleCase($configuration))" -ForegroundColor Cyan
    Write-Host "Verbosity:     " -NoNewline
    Write-Host "$($TextInfo.ToTitleCase($verbosity))" -ForegroundColor Cyan
    Write-Host ""
}

function Invoke-ExitWithExitCode([int] $exitCode) {
    if ($ci -and $prepareMachine) {
        Stop-Processes
    }

    exit $exitCode
}

function Initialize-Script {

    if ((Test-Path "$($PSScriptRoot)\..\src\$($solution)") -eq $False) {
        Write-Host "Solution $($PSScriptRoot)\..\src\$($solution) not found" -ForegroundColor Red
        Invoke-ExitWithExitCode 1
    }

    $Script:BuildPath = (Resolve-Path -Path "$($PSScriptRoot)\..\src\$($solution)").ToString()
}

function Initialize-VisualStudio {

    if ((Test-Path "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files\Microsoft Visual Studio\2022\Preview\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files\Microsoft Visual Studio\2022\Preview\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files\Microsoft Visual Studio\2022\Professional\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Msbuild\Current\Bin\";
    }
    elseif ((Test-Path "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Msbuild\Current\Bin") -ne $False) {
        $Script:VsPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Msbuild\Current\Bin\";
    }
    else {
        Write-Host "Visual Studio 2022 or 2019 not found." -ForegroundColor Red
        Invoke-ExitWithExitCode 1
    }
}

function Invoke-Build {
    if (-not $build) {
        return
    }

    $msBuild = "$($Script:VsPath)MSBuild.exe"

    if ($platform.ToLower() -eq "anycpu") {
        $platform = "Any CPU"
    }

    if ($restore) {
        & $msBuild $Script:BuildPath `
            /target:Clean `
            /target:Build `
            /p:Configuration=$configuration `
            /p:Platform="$($platform)" `
            --verbosity:$verbosity `
            --restore
    }
    else {
        & $msBuild $Script:BuildPath `
            /target:Clean `
            /target:Build `
            /p:Configuration=$configuration `
            /p:Platform="$($platform)" `
            --verbosity:$verbosity
    }
}

if ($help) {
    Invoke-Help

    exit 0
}

[timespan]$execTime = Measure-Command {
    Invoke-Hello | Out-Default
    Initialize-Script | Out-Default
    Initialize-VisualStudio | Out-Default
    Invoke-Build | Out-Default
}

Write-Host "Finished in " -NoNewline
Write-Host "$($execTime.Minutes) min $($execTime.Seconds),$($execTime.Milliseconds) s." -ForegroundColor Cyan

Write-Host "Finished at " -NoNewline
Write-Host "$(Get-Date -UFormat "%d.%m.%Y %R")" -ForegroundColor Cyan