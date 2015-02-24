[Setup]
AppName = "Antumbra"
AppVersion = 0.1.16
AppId = "Wintumbra"
DefaultDirName = "{pf32}\Antumbra"  7
UsePreviousAppDir = no

[Icons]
Name: "{commonprograms}\Antumbra"; Filename: "{app}\Antumbra.exe"

[Files]
Source: "dependencies\dotNetFx40_Full_x86_x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall; Check: FrameworkIsNotInstalled
Source: "..\DriverInstaller\*"; DestDir: {app}\DriverInstaller\
Source: "dependencies\*.dll"; DestDir: {app}\Extensions
Source: "dependencies\Antumbra.exe"; DestDir: {app}
Source: "libusb-1.0.dll"; DestDir: {app}
Source: "libantumbra.dll"; DestDir: {app}
Source: "../Licenses\*"; DestDir: {app}\Licences
Source: "README.txt"; DestDir: {app}

[Run]
Filename: "{tmp}\dotNetFx40_Full_setup.exe"; Check: FrameworkIsNotInstalled
Filename: "{app}\DriverInstaller\glowdrvinst.exe";

[code]
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;
