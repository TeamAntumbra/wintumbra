call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat" || goto :die
MSBuild Wintumbra.sln /t:Clean || goto :die
MSBuild Wintumbra.sln /p:Configuration=Release /p:Platform=x86 /t:Build || goto :die
goto :EOF

:die
echo Something broke.
exit /b 1
