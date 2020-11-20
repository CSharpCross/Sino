@echo off

echo ��������Ҫ����İ���

echo 0. �˳�
echo 1. OrvilleX  
echo 2. OrvilleX.EventBus
echo 3. OrvilleX.Dapper
echo 4. OrvilleX.AutoIndex
echo 5. OrvilleX.Cache
echo 6. 

set /p packageNum=�������ţ�

if %packageNum% == 1 (
	echo ��ʼ��OrvilleX����
	dotnet pack .\src\OrvilleX -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 2 (
	echo ��ʼ��OrvilleX.EventBus����
	dotnet pack .\src\OrvilleX.EventBus -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json   
) else if %packageNum% == 3 (
	echo ��ʼ��OrvilleX.Dapper����
	dotnet pack .\src\OrvilleX.Dapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json  
) else if %packageNum% == 4 (
    echo ��ʼ��OrvilleX.AutoIndex����
    dotnet pack .\src\OrvilleX.AutoIndex -c Release -o .\artifacts
    dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 5 (
	echo ��ʼ��OrvilleX.Cache����
	dotnet pack .\src\OrvilleX.Cache -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json
)
rd /s/q .\artifacts

pause