@echo off
cd /d "%~dp0"
set TZ=UTC
ruby ..\lib\generate-html.rb
