# Get the current year
$currentYear = (Get-Date).Year

# Define the new copyright text
$newCopyright = "Copyright $([char]0xA9) $currentYear DVTk"

# Get all AssemblyInfo.cs files in the main directory
$fileList = Get-ChildItem -Path $PSScriptRoot/.. -Filter "AssemblyInfo.cs" -Recurse

foreach ($file in $fileList) {
    # Read the content of the file
    $content = Get-Content -Path $file.FullName

    # Replace the AssemblyCopyright line
    $newContent = $content -replace 'AssemblyCopyright\(".*"\)', "AssemblyCopyright(`"$newCopyright`")"
    $newContent = $newContent -replace 'AssemblyCompany\(".*"\)', `
    'AssemblyCompany("DVTk - The Healthcare Validation Toolkit (www.dvtk.org)")'

    # Write the new content back to the file
    Set-Content -Path $file.FullName -Value $newContent

    Write-Host "Updated $($file.FullName)"
}