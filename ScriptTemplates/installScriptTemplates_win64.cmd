@echo off
robocopy %~dp0 C:\Progra~1\Unity\Editor\Data\Resources\ScriptTemplates\ *.txt /xf *.cmd
pause