@echo off

echo ��������Ҫ����İ���

echo 0. �˳�
echo 1. Sino.Web  
echo 2. Sino.Extensions.EventBus
echo 3. Sino.Extensions.Dapper
echo 4. Sino.Extensions.AutoIndex
echo 5. Sino.Extensions.Aliyun

set /p packageNum=�������ţ�

if %packageNum% == 1 (
	echo ��ʼ��Sino.Web����
	dotnet pack .\src\Sino.Web -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json
) else if %packageNum% == 2 (
	echo ��ʼ��Sino.Extensions.EventBus����
	dotnet pack .\src\Sino.Extensions.EventBus -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json   
) else if %packageNum% == 3 (
	echo ��ʼ��Sino.Extensions.Dapper����
	dotnet pack .\src\Sino.Extensions.Dapper -c Release -o .\artifacts
	dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json  
) else if %packageNum% == 4 (
    echo ��ʼ��Sino.Extensions.AutoIndex����
    dotnet pack .\src\Sino.Extensions.AutoIndex -c Release -o .\artifacts
    dotnet nuget push .\artifacts\*.nupkg -k [��Կ] -s https://api.nuget.org/v3/index.json
)
rd /s/q .\artifacts

pause