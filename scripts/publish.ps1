foreach($project in Get-Content .\projects.properties) {
    Write-Output($project)
}