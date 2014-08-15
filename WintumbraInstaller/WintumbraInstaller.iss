[Setup]
AppName = "WintumbraInstaller"
AppVersion = 0.1.0
DefaultDirName = "Antumbra"

[Files]
Source: "dependencies\dotNetFx40_Full_x86_x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall; Check: FrameworkIsNotInstalled
Source: "driver\wintumbra.inf"; DestDir: {app}\driver;

[Run]
Filename: "{tmp}\dotNetFx40_Full_setup.exe"; Check: FrameworkIsNotInstalled
Filename: {sys}\rundll32.exe; Parameters: "setupapi,InstallHinfSection DefaultInstall 128 {app}\driver\wintumbra.inf"; WorkingDir: {app}\driver; Flags: 32bit;

[code]
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;