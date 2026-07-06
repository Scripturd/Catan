#requires -version 5
[CmdletBinding()]
param(
    [int]$Port = 5200
)

$ErrorActionPreference = "Stop"
$server = $null
Push-Location $PSScriptRoot
try {
    if (-not (Get-Command cloudflared -ErrorAction SilentlyContinue)) {
        Write-Host "cloudflared is not installed." -ForegroundColor Yellow
        Write-Host "Install it once with:  winget install --id Cloudflare.cloudflared"
        exit 1
    }

    Write-Host "Building the server..."
    dotnet build src/Catan.Server/Catan.Server.csproj -c Release --nologo | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Build failed." }

    $exe = Join-Path $PSScriptRoot "src/Catan.Server/bin/Release/net9.0/Catan.Server.exe"
    $workDir = Join-Path $PSScriptRoot "src/Catan.Server"

    Write-Host "Starting Catan server on http://localhost:$Port ..."
    $server = Start-Process -FilePath $exe -ArgumentList "--port", "$Port" -WorkingDirectory $workDir -PassThru

    $deadline = (Get-Date).AddSeconds(60)
    while ($true) {
        try {
            Invoke-WebRequest "http://localhost:$Port/" -UseBasicParsing -TimeoutSec 2 | Out-Null
            break
        } catch {
            if ((Get-Date) -gt $deadline) { throw "Server did not start within 60 seconds." }
            Start-Sleep -Milliseconds 500
        }
    }

    Write-Host ""
    Write-Host "Server is up. Opening a Cloudflare tunnel..." -ForegroundColor Green
    Write-Host "Share the https://<something>.trycloudflare.com URL printed below with your friends."
    Write-Host "Press Ctrl+C to end the session and close the tunnel."
    Write-Host ""

    cloudflared tunnel --url "http://localhost:$Port"
}
finally {
    if ($server -and -not $server.HasExited) {
        Write-Host "Stopping the server..."
        Stop-Process -Id $server.Id -Force
    }
    Pop-Location
}
