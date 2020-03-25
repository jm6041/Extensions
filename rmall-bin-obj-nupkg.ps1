Get-ChildItem . -recurse .\bin -force | Remove-Item -Recurse -Force
Get-ChildItem . -recurse .\obj -force | Remove-Item -Recurse -Force
Get-ChildItem . -recurse *.nupkg -force | Remove-Item -force