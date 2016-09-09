#ifexist "sign.options.cmd"
#define SIGN true
#else
#define SIGN false
#endif

#define DotNetVersion "v4\Client"
#define DotNetServicePackVersion 0

#define MyAppName "MediaData"
#define MyAppVersion "1.3.0.0"
#define MyAppPublisher "Michele Locati"
#define MyAppURL "https://github.com/mlocati/MediaData"
#define MyAppExeName "MediaData.exe"

[Setup]
AppId={{A3591E3E-67BD-4088-9E0F-D1C3EF2D1433}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=LICENSE.txt
OutputDir=temp
OutputBaseFilename=MediaData-setup
Compression=lzma2/max
SolidCompression=yes
#if SIGN
SignTool=MediaDataSigner
SignedUninstaller=yes
#endif

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"

[CustomMessages]
MissingDotNetFramework={#MyAppName} requires Microsoft .NET Framework 4 Client Profile.%n%nPlease install it and then re-run the {#MyAppName} setup program.%n%nDo you want to download the .NET Framework now?
italian.MissingDotNetFramework={#MyAppName} richiede Microsoft .NET Framework 4 Client Profile.%n%nInstallarlo e lanciare nuovamente il programma di installazione di {#MyAppName}.%n%nScaricare il Framework .NET adesso?
DotNetFrameworkURL=http://www.microsoft.com/en-US/download/details.aspx?id=24872
italian.DotNetFrameworkURL=http://www.microsoft.com/it-IT/download/details.aspx?id=24872

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "bin\Release\MediaData.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\GMap.NET.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\GMap.NET.WindowsForms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\LitJson.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "licenses\*"; DestDir: "{app}\licenses"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "LICENSE.txt"; DestDir: "{app}\licenses"; DestName: "MediaData.txt"; Flags: ignoreversion
Source: "bin\Release\tools\*"; DestDir: "{app}\tools"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\it\*"; DestDir: "{app}\it"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]

function GetCommandlineArgument(argument: String): String;
var
	search: String;
	value: String;
	i: Integer;
begin
	search := '/' + argument + '=';
	Result := '';
	for i:= 1 to ParamCount do begin
		if Length(ParamStr(i)) > Length(search) then begin
			if CompareText(Copy(ParamStr(i), 1, Length(search)), search) = 0 then begin
				value := Copy(ParamStr(i), Length(search) + 1, 2147483647);
				Result := RemoveQuotes(value);
				break;
			end;
     end;
   end;
end;

function VerifyADotNet(frameworkVersion: string; servicePackVersion: cardinal): boolean;
var
	key: string;
	install: cardinal;
	keyRead: boolean;
	success: boolean;
	serviceCount: cardinal;
begin
	success := false;
	key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + frameworkVersion;
	if Pos('v3.0', frameworkVersion) = 1 then begin
		keyRead := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
	end else begin
		keyRead := RegQueryDWordValue(HKLM, key, 'Install', install);
	end;
	if keyRead then begin
		if install = 1 then begin
			if servicePackVersion <= 0 then begin
				success := true;
			end else begin
				if Pos('v4', frameworkVersion) = 1 then begin
					keyRead := RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
				end else begin
					keyRead := RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
				end;
				if keyRead then begin
					if serviceCount >= servicePackVersion then begin
						success := true;
					end;
				end;
			end;
		end;
	end;
	result := success;
end;

function InitializeSetup(): Boolean;
var
	ErrorCode: Integer;
begin
	if not VerifyADotNet('{#DotNetVersion}', {#DotNetServicePackVersion}) then begin
		if MsgBox(ExpandConstant('{cm:MissingDotNetFramework}'), mbConfirmation, MB_YESNO)= IDYES then begin
			ShellExec('open', ExpandConstant('{cm:DotNetFrameworkURL}'), '', '', SW_SHOW, ewNoWait, ErrorCode);
		end;
		result := false;
	end else begin
		result := true;
	end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
	ResultCode: Integer;
begin
	Result := True;
	if (CurPageID = wpFinished) then begin
		if ((not WizardForm.YesRadio.Visible) or (not WizardForm.YesRadio.Checked)) then begin
			if Length(GetCommandlineArgument('upgrading')) > 0 then begin
				ExecAsOriginalUser(ExpandConstant('{app}\{#MyAppExeName}'), '', '', SW_SHOWNORMAL, ewNoWait, ResultCode);
			end;
		end;
	end;
end;
