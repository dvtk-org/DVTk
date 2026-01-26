$env:GITHUB_WORKSPACE="."
$env:SOLUTION="Dicom.sln"
$env:BUILD_CONFIGURATION="Release"
$env:BUILD_PLATFORM="x86"
$env:DEV_CMD="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.com"

$projects = @( "DVT Setup")#, "DICOM Editor Setup", "DICOM Compare Setup", "DICOM Network Analyzer Setup", "Query Retrieve SCP Emulator Setup", "Query Retrieve SCU Emulator Setup", "RIS Emulator Setup", "Storage SCP Emulator Setup", "Storage SCU Emulator Setup" )
if (Test-Path build-devenv.log) { Remove-Item build-devenv.log -Force }
if (-not (Test-Path "$env:DEV_CMD")) { Write-Host "ERROR: DEV_CMD (devenv) niet gevonden: $env:DEV_CMD" throw "DEV_CMD (devenv) niet gevonden" }
$solPath = Join-Path "$env:GITHUB_WORKSPACE" "$env:SOLUTION"
if (-not (Test-Path $solPath)) { if (Test-Path $env:SOLUTION) { $solPath = $env:SOLUTION } else { Write-Host "ERROR: Solution niet gevonden op $solPath of $env:SOLUTION" throw "Solution niet gevonden" } }
$overallExit = 0

foreach ($p in $projects) {
  Write-Host "=== Building setup project: $p ==="
  $buildParams = @(
  $solPath,
  '/Build', "$($env:BUILD_CONFIGURATION)|$($env:BUILD_PLATFORM)",
  '/Project', $p
  )

  # Log the exact command to the log for debugging
  Add-Content -Path build-devenv.log -Value ("`n=== CMD for project: " + $p + " ===") -Encoding UTF8
  Add-Content -Path build-devenv.log -Value ("CMD: " + $env:DEV_CMD + " " + ($buildParams -join ' ')) -Encoding UTF8

  # Execute devenv with argument array to avoid PowerShell parsing issues
  Write-Host "Command:"
  Write-Host "$env:DEV_CMD $buildParams"
  # Voer devenv uit en stuur de output naar Out-File met de juiste encoding
  #  & "$env:DEV_CMD" @buildParams | Out-File -FilePath build-devenv.log -Append -Encoding UTF8
  & "$env:DEV_CMD" @buildParams | ForEach-Object {
    Write-Host $_                   # Toon in de commandline
    $_ | Out-File -FilePath build-devenv.log -Append -Encoding UTF8 # Sla op in log
  }
  $exit = $LASTEXITCODE
  
  # Add-Content -Path build-devenv.log -Value ("ExitCode: " + $exit)
  Write-Host "devenv exitcode for '$p': $exit"
  
  if ($exit -ne 0) {
    Write-Host "WARNING: build failed for project '$p' (exit $exit). See build-devenv.log for details."
    # record failure so we exit with non-zero at end (step will be marked failed but job continues because continue-on-error: true)
    $overallExit = $exit
  }
}