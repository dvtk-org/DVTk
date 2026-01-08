# Maak een NIEUW code signing certificaat
$cert = New-SelfSignedCertificate `
    -Type CodeSigningCert `
    -Subject "CN=DVTk" `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -HashAlgorithm SHA256 `
    -Provider "Microsoft Enhanced RSA and AES Cryptographic Provider" `
    -CertStoreLocation "Cert:\CurrentUser\My" `
    -NotAfter (Get-Date).AddYears(10)

# Verify that it has Code Signing
Write-Host "`n📋 Certificate details:"
$cert | Select-Object Subject, Thumbprint, NotAfter, @{Name='EKU';Expression={$_.EnhancedKeyUsageList.FriendlyName}} | Format-List

# Ask for password for PFX export
Write-Host "`n🔐 Enter a password for the PFX file:"
$password = Read-Host "Password" -AsSecureString
$passwordConfirm = Read-Host "Confirm password" -AsSecureString

# Convert to plain text to compare
$pwd1 = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))
$pwd2 = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($passwordConfirm))

if ($pwd1 -ne $pwd2) {
    Write-Error "❌ Passwords do not match!"
    exit 1
}

if ($pwd1.Length -lt 8) {
    Write-Error "❌ Password must be at least 8 characters!"
    exit 1
}

Write-Host "✅ Password accepted"

# Export to PFX
$pfxPath = "C:\Temp\codesigning-cert.pfx"
if (-not (Test-Path "C:\Temp")) {
    New-Item -ItemType Directory -Path "C:\Temp" -Force | Out-Null
}

Export-PfxCertificate -Cert $cert -FilePath $pfxPath -Password $password

Write-Host "`n✅ Certificate created and exported to: $pfxPath"
Write-Host "Thumbprint: $($cert.Thumbprint)"

# Convert to Base64 for GitHub Secret
$bytes = [System.IO.File]::ReadAllBytes($pfxPath)
$base64 = [Convert]::ToBase64String($bytes)
$base64 | Set-Clipboard

Write-Host "Base64 string copied to clipboard!"
Write-Host "Next steps:"
Write-Host "1. Go to GitHub repository -> Settings -> Secrets and variables -> Actions"
Write-Host "2. Create secret: CERTIFICATE_BASE64 (paste the value from clipboard)"
Write-Host "3. Create secret: CERTIFICATE_PASSWORD (use the password you just entered)"
Write-Host "Save the password in a secure location"