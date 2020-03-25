Get-ChildItem . -recurse .\bin -force | Remove-Item -force
Get-ChildItem . -recurse .\obj -force | Remove-Item -force
Get-ChildItem . -recurse *.nupkg -force | Remove-Item -force