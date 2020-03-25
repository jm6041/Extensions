$outDir = "..\publish\nupkgs"
if(Test-Path $outDir){
    Write-Output("Delete all old nupkgs."+[System.Environment]::NewLine)
    Get-ChildItem $outDir -Recurse *.nupkg -Force | Remove-Item -Force
}

$projects=Get-Content .\projects.properties
$all=Get-ChildItem ..\src -Recurse -Include $projects  *.csproj
foreach($item in $all){
    $fn=$item.FullName
    Write-Output("Pack FullName: $($fn)")
    dotnet.exe pack -c Release -o $outDir $fn
    Write-Output("Pack End=========="+[System.Environment]::NewLine)
}