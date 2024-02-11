@echo off
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0build.ps1""""
@REM powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0scripts\build_extension.ps1""""
exit /b %ErrorLevel%
