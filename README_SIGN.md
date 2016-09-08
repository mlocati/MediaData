# Signing

Let's assume you have in the directory `C:\Signing` the following files:
- your PFX signature file named `signature.pfx`
- the Signing Tool `signtool.exe`

Create a folder named `temp` in the MediaData source directory, and create a file named `sign.cmd` in it.

Here's its content:

```bat
@echo off
setlocal

set SIGNDIR=C:\Signing

SET SIGNCMD="%SIGNDIR%\signtool.exe" sign
REM Select the best signing cert automatically
SET SIGNCMD=%SIGNCMD% /a
REM /v  Print verbose success and status messages
SET SIGNCMD=%SIGNCMD% /v
REM The signing cert in a file
SET SIGNCMD=%SIGNCMD% /f "%SIGNDIR%\signature.pfx"
REM The PFX password
SET SIGNCMD=%SIGNCMD% /p YOUR_PASSWORD_HERE
REM The timestamp server's URL
SET SIGNCMD=%SIGNCMD% /t http://timestamp.verisign.com/scripts/timstamp.dll
REM Description of the signed content
SET SIGNCMD=%SIGNCMD% /d "MediaData - Manage metadata of your media files"
REM URL with more information about the signed content
SET SIGNCMD=%SIGNCMD% /du "http://mlocati.github.io/MediaData/"

REM SHA-1 signing
%SIGNCMD% /fd sha1 "%~1"
REM SHA-256 signing
%SIGNCMD% /fd sha256 "%~1"

endlocal
```