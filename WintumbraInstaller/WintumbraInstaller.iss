[Setup]
AppName = "Antumbra"
AppVersion = 0.1.20
AppId = "Wintumbra"
DefaultDirName = "{pf32}\Antumbra"
UsePreviousAppDir = no

[Icons]
Name: "{commonprograms}\Antumbra"; Filename: "{app}\Antumbra.exe"

[Files]
Source: "..\deps\wintumbra\dotNetFx40_Full_x86_x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall; Check: FrameworkIsNotInstalled
Source: "..\deps\libwdi\*.dll"; DestDir: {app}\DriverInstaller
Source: "..\deps\win32-libantumbra\glowdrvinst.exe"; DestDir: {app}\DriverInstaller\
Source: "dependencies\*.dll"; DestDir: {app}\Extensions
Source: "dependencies\libusb-1.0.dll"; DestDir: {app}
Source: "dependencies\libantumbra.dll"; DestDir: {app}
Source: "dependencies\Antumbra.exe"; DestDir: {app}
Source: "../Licenses\*"; DestDir: {app}\Licences
Source: "..\README.md"; DestDir: {app}; DestName: "README.txt"

[Run]
Filename: "{tmp}\dotNetFx40_Full_setup.exe"; Check: FrameworkIsNotInstalled
Filename: "{app}\DriverInstaller\glowdrvinst.exe"; Parameters: "batch"

[code]
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;
