# 包基础目录
$pkgDir = "publish\nupkgs"
$pkgItem = Get-ChildItem -Path $pkgDir -Filter *.nupkg | Sort-Object -Property Name | Select-Object -Last 1
if($pkgItem){
    Write-Output "pkg file:$($pkgItem.FullName)"
    dotnet.exe nuget push -k yx1234 -s http://tfs:8088/nuget $pkgItem.FullName
}else {
    Write-Output "pkg file is empty!"
}
