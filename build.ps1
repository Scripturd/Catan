#requires -version 5
[CmdletBinding()]
param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"
Push-Location $PSScriptRoot
try {
    $modes = @("Catan.Modes.Standard", "Catan.Modes.Seafarers", "Catan.Modes.Mini")

    foreach ($mode in $modes) {
        dotnet build "src/$mode/$mode.csproj" -c $Configuration --nologo | Out-Null
        if ($LASTEXITCODE -ne 0) { throw "Build of $mode failed." }
    }
    dotnet build src/Catan/Catan.csproj -c $Configuration --nologo | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Build of the CLI failed." }
    dotnet build src/Catan.Server/Catan.Server.csproj -c $Configuration --nologo | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Build of the server failed." }

    foreach ($consumer in @("src/Catan", "src/Catan.Server")) {
        $plugins = Join-Path $consumer "bin/$Configuration/net9.0/plugins"
        New-Item -ItemType Directory -Force -Path $plugins | Out-Null
        foreach ($mode in $modes) {
            Copy-Item "src/$mode/bin/$Configuration/net9.0/$mode.dll" $plugins -Force
        }
    }

    Write-Host "Built all projects and assembled plugins for the CLI and server ($Configuration)."
}
finally {
    Pop-Location
}
