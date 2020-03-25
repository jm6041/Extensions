# 包基础目录
$pkgDir = "..\publish\nupkgs"
# $pkgItem = Get-ChildItem -Path $pkgDir -Filter *.nupkg | Sort-Object -Property Name | Select-Object -Last 1
$pkgs = Get-ChildItem -Path $pkgDir -Filter *.nupkg | Sort-Object -Property Name
foreach($pkg in $pkgs){
    Write-Output "Nupkg file:$($pkg.FullName)"
    dotnet.exe nuget push $pkg.FullName --skip-duplicate -k oy2cv77aceiah6acry6aqblnrl2tq5qudjohelycsym534 -s https://api.nuget.org/v3/index.json
}

