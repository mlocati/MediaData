@echo off

cd /d "%~dp0"

rem xsd.exe is part of Microsoft Windows SDK
xsd.exe ffprobe.xsd /classes /language:CS /namespace:MLocati.MediaData.FFprobe /outputdir:.
