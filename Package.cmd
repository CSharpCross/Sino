@echo off

echo 请输入需要发版的包：

echo 0. 退出
echo 1. Sino.Web  
echo 2. Sino.Extensions.EventBus
echo 3. Sino.Extensions.Dapper
echo 4. Sino.Extensions.AutoIndex
echo 5. Sino.Extensions.Aliyun

set /p packageNum=请输入编号：

if %packageNum% == 1 (
	echo 开始对Sino.Web发版
	dotnet pack .\src\Sino.Web -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [密钥] -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 2 (
	echo 开始对Sino.Extensions.EventBus发版
	dotnet pack .\src\Sino.Extensions.EventBus -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [密钥] -s https://api.nuget.org/v3/index.json   
) else if %packageNum% == 3 (
	echo 开始对Sino.Extensions.Dapper发版
	dotnet pack .\src\Sino.Extensions.Dapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [密钥] -s https://api.nuget.org/v3/index.json  
) else if %packageNum% == 4 (
    echo 开始对Sino.Extensions.AutoIndex发版
    dotnet pack .\src\Sino.Extensions.AutoIndex -c Release -o .\artifacts
    dotnet nuget push .\artifacts\*.nupkg -k [密钥] -s https://api.nuget.org/v3/index.json
)
rd /s/q .\artifacts

pause