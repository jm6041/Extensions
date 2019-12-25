# 输出基础目录
$outDir = "publish\nupkgs"
Remove-Item $outDir -Recurse -Force
dotnet.exe  pack -c Release -o $outDir
