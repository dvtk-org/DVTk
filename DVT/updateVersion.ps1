param (
    [string]$gitversion = $(git describe --tags)
)

function UpdateFilesWithVersion($files, $version, $filter) {
    foreach ($file in $files) {
        Write-Host "Updating $($file.FullName)"
        $content = Get-Content -Path $file.FullName

        switch -Wildcard ($filter) {
            "*.vdproj" {
                $newContent = $content -replace ('"ProductVersion" = "8:(.*)"'), ('"ProductVersion" = "8:' + $version + '"')
            }
            default {
                Write-Host "Unsupported filter: $filter"
            }
        }

        $newContent | Set-Content -Path $file.FullName
    }
}

if (-not $gitversion) {
    Write-Host "Failed to get version from git describe --tags."
    exit 1
}
$version = [regex]::Match($gitversion, '\d+\.\d+\.\d+(\.\d+)?').Value
if ($version -eq "") {
    Write-Host "Failed to get version from gitversion: $gitversion"
    exit 1
}
$filterList = @("*.vdproj")
foreach ($filter in $filterList) {
    Write-Host "Updating $filter files with version: $version"
    $fileList = Get-ChildItem -Path $PSScriptRoot/.. -Filter $filter -Recurse
    UpdateFilesWithVersion $fileList $version $filter
}
