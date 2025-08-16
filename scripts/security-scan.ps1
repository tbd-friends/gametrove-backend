<#!
.SYNOPSIS
  Runs a consolidated local security sweep (secrets, vulnerabilities, config hygiene).
.DESCRIPTION
  Executes:
    1. Gitleaks secret scan
    2. Dependency vulnerability listing
    3. Optional (future): Code scanning placeholders
.NOTES
  Requires: gitleaks, dotnet SDK 9
!#>

param(
  [switch]$FailOnFindings = $false
)

Write-Host "== GameTrove25 Security Scan ==" -ForegroundColor Cyan

# 1. Secret Scan
if (Get-Command gitleaks -ErrorAction SilentlyContinue) {
  Write-Host "[1/3] Running gitleaks..." -ForegroundColor Yellow
  gitleaks detect --config .gitleaks.toml --report-format json --report-path security-scan-report.json
  $json = Get-Content security-scan-report.json -Raw | ConvertFrom-Json
  $findings = $json.findings
  if ($findings -and $findings.Count -gt 0) {
    Write-Warning ("Secrets detected: {0}" -f $findings.Count)
    if ($FailOnFindings) { exit 3 }
  } else {
    Write-Host "No secrets detected." -ForegroundColor Green
  }
} else {
  Write-Warning "gitleaks not installed. Skipping secret scan. Install: https://github.com/gitleaks/gitleaks"
}

# 2. Dependency Vulnerabilities
Write-Host "[2/3] Checking vulnerable packages (dotnet)..." -ForegroundColor Yellow
$pkgOutput = dotnet list package --vulnerable --include-transitive 2>&1
$pkgFile = "security-scan-vulns.txt"
$pkgOutput | Out-File $pkgFile -Encoding UTF8
if ($pkgOutput -match "No vulnerable packages found") {
  Write-Host "No vulnerable packages reported." -ForegroundColor Green
} else {
  Write-Warning "Potential vulnerable packages detected. Review $pkgFile"
  if ($FailOnFindings) { exit 4 }
}

# 3. Placeholder for future analyzers
Write-Host "[3/3] Static analyzers integration pending (Roslyn/CodeQL)." -ForegroundColor Yellow

Write-Host "Security scan complete." -ForegroundColor Cyan

