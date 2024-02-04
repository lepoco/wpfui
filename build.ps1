$wingetVersion = & winget --version 2>$null

if ($wingetVersion -eq $null) {
    Write-Output "winget is not installed. Starting installation..."

    Invoke-WebRequest -Uri "https://github.com/microsoft/winget-cli/releases/download/v1.6.3482/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle" -OutFile "$env:TEMP\Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"

    Add-AppxPackage -Path "$env:TEMP\Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"

    Write-Output "winget has been installed."
}

$dotnetVersion = & dotnet --version 2>$null

if ($dotnetVersion -eq $null) {
    Write-Output ".NET SDK is not installed."

    winget install Microsoft.DotNet.SDK.8
} else {
    $majorVersion = $dotnetVersion.Split('.')[0]

    if ($majorVersion -ge 8) {
        Write-Output ".NET SDK version is $dotnetVersion, which is 8.0.0 or newer."
    } else {
        Write-Output ".NET SDK version is $dotnetVersion, which is older than 8.0.0."

        winget install Microsoft.DotNet.SDK.8
    }
}

if ($env:PROCESSOR_ARCHITECTURE -eq "AMD64") {
    dotnet restore Wpf.Ui.sln /tl
    dotnet build src\Wpf.Ui.Gallery\Wpf.Ui.Gallery.csproj --configuration Release --no-restore --verbosity quiet /tl
} else {
    Write-Host "Not in the x64 desktop environment."
}
