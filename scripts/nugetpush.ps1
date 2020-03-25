# 包基础目录
$pkgDir = "publish\nupkgs"
$pkgItem = Get-ChildItem -Path $pkgDir -Filter *.nupkg | Sort-Object -Property Name | Select-Object -Last 1
if($pkgItem){
    Write-Output "pkg file:$($pkgItem.FullName)"
    dotnet.exe nuget push $pkgItem.FullName -k oy2cv77aceiah6acry6aqblnrl2tq5qudjohelycsym534 -s https://api.nuget.org/v3/index.json
}else {
    Write-Output "pkg file is empty!"
}
