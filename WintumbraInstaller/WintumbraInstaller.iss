[Setup]
AppName = "WintumbraInstaller"
AppVersion = 0.1.0
DefaultDirName = "Antumbra"

[Files]
Source: "dependencies\dotNetFx40_Full_x86_x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall; Check: FrameworkIsNotInstalled
Source: "driver\wintumbra_betav3.inf"; DestDir: {app}\driver
Source: "dependencies\libantumbra.dll"; DestDir: {app}
Source: "dependencies\libusb-1.0.dll"; DestDir: {app}
Source: "dependencies\*.dll"; DestDir: {app}\Extensions
Source: "dependencies\antumbra.exe"; DestDir: {app}
Source: "../Licenses\*"; DestDir: {app}\Licences
Source: "../README.md"; DestDir: {app}

[Run]
Filename: "{tmp}\dotNetFx40_Full_setup.exe"; Check: FrameworkIsNotInstalled
Filename: {sys}\rundll32.exe; Parameters: "setupapi, InstallHinfSection DefaultInstall 128 {app}\driver\wintumbra_betav3.inf"; WorkingDir: {app}\driver; Flags: 32bit

[code]
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;