@echo off

echo ��������Ҫ����İ���

echo 0. �˳�
echo 1. OrvilleX  
echo 2. OrvilleX.EventBus
echo 3. OrvilleX.Dapper
echo 4. OrvilleX.AutoIndex
echo 5. OrvilleX.Cache
echo 6. OrvilleX.MongoDB
echo 7. OrvilleX.AutoMapper

set apikey=oy2lr3xiu3rygnb3v5jq5wjsenvsyc7e5biu44zbrralgu
set /p packageNum=�������ţ�

if %packageNum% == 1 (
	echo ��ʼ��OrvilleX����
	dotnet pack .\src\OrvilleX -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 2 (
	echo ��ʼ��OrvilleX.EventBus����
	dotnet pack .\src\OrvilleX.EventBus -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json   
) else if %packageNum% == 3 (
	echo ��ʼ��OrvilleX.Dapper����
	dotnet pack .\src\OrvilleX.Dapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json  
) else if %packageNum% == 4 (
    echo ��ʼ��OrvilleX.AutoIndex����
    dotnet pack .\src\OrvilleX.AutoIndex -c Release -o .\artifacts
    dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 5 (
	echo ��ʼ��OrvilleX.Cache����
	dotnet pack .\src\OrvilleX.Cache -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 6 (
	echo ��ʼ��OrvilleX.MongoDB����
	dotnet pack .\src\OrvilleX.MongoDB -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 7 (
	echo ��ʼ��OrvilleX.AutoMapper����
	dotnet pack .\src\OrvilleX.AutoMapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k %apikey% -s https://api.nuget.org/v3/index.json
)
rd /s/q .\artifacts

pause