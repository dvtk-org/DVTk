# PowerShell script to generate test certificates for DVTk secure connection tests
# Requires OpenSSL to be installed and available in PATH

param(
    [string]$OutputPath = ".\bin\Debug\Resources\Sessions",
    [int]$ValidityDays = 36500 # 100 years
)

Write-Host "=====================================================================================================" -ForegroundColor Cyan
Write-Host "DVTk Test Certificate Generation Script" -ForegroundColor Cyan
Write-Host "=====================================================================================================" -ForegroundColor Cyan
Write-Host ""

# Check if OpenSSL is available
try
{
    $opensslVersion = & openssl version 2>&1
    Write-Host "OpenSSL found: $opensslVersion" -ForegroundColor Green
}
catch
{
    Write-Error "OpenSSL not found in PATH. Please install OpenSSL first."
    Write-Host "Download from: https://slproweb.com/products/Win32OpenSSL.html" -ForegroundColor Yellow
    exit 1
}

# Create output directory if it doesn't exist
if (-not (Test-Path $OutputPath))
{
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
    Write-Host "Created directory: $OutputPath" -ForegroundColor Green
}

# Change to output directory
Push-Location $OutputPath

Write-Host ""
Write-Host "Generating test certificates for secure DICOM connection..." -ForegroundColor Cyan
Write-Host ""

# Generate test_sys_1 (SCP) certificate and key
Write-Host "1️⃣  Generating test_sys_1 (SCP) certificate and key..." -ForegroundColor Yellow
& openssl req -x509 -newkey rsa:2048 -keyout test_sys_1.key.pem -out test_sys_1.cert.pem -days $ValidityDays -nodes -subj "/CN=test_sys_1/O=DVTk/OU=DICOM SCP/C=NL" 2>&1 | Out-Null

if ($LASTEXITCODE -eq 0)
{
    Write-Host "   test_sys_1.cert.pem created" -ForegroundColor Green
    Write-Host "   test_sys_1.key.pem created" -ForegroundColor Green
}
else
{
    Write-Error "   Failed to generate test_sys_1 certificate"
    Pop-Location
    exit 1
}

# Combine certificate and key for test_sys_1
Write-Host "   Combining certificate and key..." -ForegroundColor Yellow
Get-Content test_sys_1.key.pem, test_sys_1.cert.pem | Set-Content test_sys_1.cert_and_key.pem -Encoding ASCII
Write-Host "   test_sys_1.cert_and_key.pem created" -ForegroundColor Green
Write-Host ""

# Generate test_sys_2 (SCU) certificate and key
Write-Host "Generating test_sys_2 (SCU) certificate and key..." -ForegroundColor Yellow
& openssl req -x509 -newkey rsa:2048 -keyout test_sys_2.key.pem -out test_sys_2.cert.pem -days $ValidityDays -nodes -subj "/CN=test_sys_2/O=DVTk/OU=DICOM SCU/C=NL" 2>&1 | Out-Null

if ($LASTEXITCODE -eq 0)
{
    Write-Host "   test_sys_2.cert.pem created" -ForegroundColor Green
    Write-Host "   test_sys_2.key.pem created" -ForegroundColor Green
}
else
{
    Write-Error "   Failed to generate test_sys_2 certificate"
    Pop-Location
    exit 1
}

# Combine certificate and key for test_sys_2
Write-Host "   Combining certificate and key..." -ForegroundColor Yellow
Get-Content test_sys_2.key.pem, test_sys_2.cert.pem  | Set-Content test_sys_2.cert_and_key.pem -Encoding ASCII
Write-Host "   test_sys_2.cert_and_key.pem created" -ForegroundColor Green
Write-Host ""

# Clean up individual key files (optional, keep only combined files)
Write-Host "Cleaning up temporary key files..." -ForegroundColor Yellow
Remove-Item test_sys_1.key.pem -ErrorAction SilentlyContinue
Remove-Item test_sys_2.key.pem -ErrorAction SilentlyContinue
Write-Host "   Cleanup complete" -ForegroundColor Green
Write-Host ""

# Display generated files
Write-Host "=====================================================================================================" -ForegroundColor Cyan
Write-Host "Certificate generation completed successfully!" -ForegroundColor Green
Write-Host "=====================================================================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Generated files in: $(Resolve-Path .)" -ForegroundColor White
Write-Host ""
Write-Host "   For SCP (test_sys_1):" -ForegroundColor White
Write-Host "   - test_sys_1.cert.pem          (public certificate)" -ForegroundColor Gray
Write-Host "   - test_sys_1.cert_and_key.pem  (certificate + private key)" -ForegroundColor Gray
Write-Host ""
Write-Host "   For SCU (test_sys_2):" -ForegroundColor White
Write-Host "   - test_sys_2.cert.pem          (public certificate)" -ForegroundColor Gray
Write-Host "   - test_sys_2.cert_and_key.pem  (certificate + private key)" -ForegroundColor Gray
Write-Host ""
Write-Host "Session file configuration:" -ForegroundColor White
Write-Host "   SCP uses: CREDENTIALS-FILENAME test_sys_1.cert_and_key.pem" -ForegroundColor Gray
Write-Host "            CERTIFICATE-FILENAME test_sys_2.cert.pem" -ForegroundColor Gray
Write-Host "   SCU uses: CREDENTIALS-FILENAME test_sys_2.cert_and_key.pem" -ForegroundColor Gray
Write-Host "            CERTIFICATE-FILENAME test_sys_1.cert.pem" -ForegroundColor Gray
Write-Host ""
Write-Host "You can now run the secure_connection_1 NUnit test." -ForegroundColor Green
Write-Host ""

Pop-Location