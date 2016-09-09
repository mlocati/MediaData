@echo off
setlocal

rem Read the options
set SIGNOPTIONS=%~dp0sign.options.cmd
if not exist "%SIGNOPTIONS%" (
	call :optionsErr "Unable to find the following file: %SIGNOPTIONS%"
	endlocal
	exit /b 1
)
call "%SIGNOPTIONS%"
if errorlevel 1 (
	call :optionsErr "Error loading the options file"
	endlocal
	exit /b 1
)
if "%SIGNTOOL%" equ "" (
	call :optionsErr "The options file does not defines SIGNTOOL"
	endlocal
	exit /b 1
)
if not exist "%SIGNTOOL%" (
	call :optionsErr "Unable to fine the file %SIGNTOOL% specified in the options file"
	endlocal
	exit /b 1
)
if "%PFXFILE%" equ "" (
	call :optionsErr "The options file does not defines PFXFILE"
	endlocal
	exit /b 1
)
if not exist "%PFXFILE%" (
	call :optionsErr "Unable to fine the file %PFXFILE% specified in the options file"
	endlocal
	exit /b 1
)
if "%PFXPASSWORD%" equ "" (
	call :optionsErr "The options file does not defines PFXPASSWORD"
	endlocal
	exit /b 1
)

rem Start "real" script
set SIGNCMD="%SIGNTOOL%" sign
rem Select the best signing cert automatically
set SIGNCMD=%SIGNCMD% /a
rem /v  Print verbose success and status messages
set SIGNCMD=%SIGNCMD% /v
rem The signing cert in a file
set SIGNCMD=%SIGNCMD% /f "%PFXFILE%"
rem The PFX password
set SIGNCMD=%SIGNCMD% /p "%PFXPASSWORD%"
rem The timestamp server's URL
set SIGNCMD=%SIGNCMD% /t http://timestamp.verisign.com/scripts/timstamp.dll
rem Description of the signed content
set SIGNCMD=%SIGNCMD% /d "MediaData - Manage metadata of your media files"
rem URL with more information about the signed content
set SIGNCMD=%SIGNCMD% /du "http://mlocati.github.io/MediaData/"

rem SHA-1 signing
%SIGNCMD% /fd sha1 "%~1"
if errorlevel 1 goto signErr
rem SHA-256 signing
%SIGNCMD% /fd sha256 "%~1"
if errorlevel 1 goto signErr
echo.
echo Success!
echo.
endlocal
exit /b 0

:optionsErr
echo %~1>&2
echo.>&2
echo See the file README_SIGN.md to get help>&2
echo.>&2
exit /b

:signErr
echo.>&2
echo Signing error!>&2
echo.>&2
endlocal
exit /b 1
