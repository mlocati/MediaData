# Signing

## Requirements

In order to sign the executable, the installed and the uninstaller, you need to:
1. Have a digital signature file in `pfx` format
2. The Microsoft `signtool.exe` program

Copy the file called `sign.options.sample.cmd` and save it as `sign.options.cmd`.
Edit this new file and change the values of the variables you find in it.

## Signing the MediaData executable program

Signing the executable should be done automatically when compiling MediaData in Visual Studio - Take a look at the `<PostBuildEvent>` section of the [MediaData.csproj](https://github.com/mlocati/MediaData/blob/master/MediaData.csproj) file.

## Signing the installer/uninstaller

Open the `MediaData.iss` file with the InnoSetup editor.

Choose the `Configure Sign Tools...` in the `Tools` menu and hit the `Add` button:
- `Name of the Sign Tool` must be: `MediaDataSigner`
- `Command of the Sign Tool` must be: `C:\path\to\MediaData\source\folder\sign.cmd $f`
