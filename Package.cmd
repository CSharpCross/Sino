@echo off

echo 请输入需要发版的包：

echo 0. 退出
echo 1. OrvilleX  
echo 2. OrvilleX.EventBus
echo 3. OrvilleX.Dapper
echo 4. OrvilleX.AutoIndex
echo 5. OrvilleX.Cache
echo 6. OrvilleX.MongoDB
echo 7. OrvilleX.AutoMapper

set apikey=oy2lr3xiu3rygnb3v5jq5wjsenvsyc7e5biu44zbrralgu
set /p packageNum=请输入编号：

if %packageNum% == 1 (
	echo 开始对OrvilleX发版
	dotnet pack .\src\OrvilleX -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 2 (
	echo 开始对OrvilleX.EventBus发版
	dotnet pack .\src\OrvilleX.EventBus -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json   
) else if %packageNum% == 3 (
	echo 开始对OrvilleX.Dapper发版
	dotnet pack .\src\OrvilleX.Dapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json  
) else if %packageNum% == 4 (
    echo 开始对OrvilleX.AutoIndex发版
    dotnet pack .\src\OrvilleX.AutoIndex -c Release -o .\artifacts
    dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 5 (
	echo 开始对OrvilleX.Cache发版
	dotnet pack .\src\OrvilleX.Cache -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 6 (
	echo 开始对OrvilleX.MongoDB发版
	dotnet pack .\src\OrvilleX.MongoDB -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 7 (
	echo 开始对OrvilleX.AutoMapper发版
	dotnet pack .\src\OrvilleX.AutoMapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
)
rd /s/q .\artifacts

pause