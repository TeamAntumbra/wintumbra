[Setup]
AppName = "Antumbra"
AppVersion = 0.2.6-beta
AppId = "Wintumbra"
SetupIconFile = Antumbra.ico
AppPublisher = "Antumbra Technologies Inc."
AppPublisherURL = "https://antumbra.io"
AppSupportURL = "https://github.com/TeamAntumbra/wintumbra"
AppUpdatesURL = "https://github.com/TeamAntumbra/wintumbra"
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
Source: "dependencies\flatTabControl.dll"; DestDir: {app}
Source: "dependencies\Antumbra.exe"; DestDir: {app}
Source: "..\Licenses\*"; DestDir: {app}\Licences
Source: "..\README.md"; DestDir: {app}; DestName: "README.txt"
Source: "..\deps\wintumbra\*.dll"; DestDir: {app}
Source: "..\deps\wintumbra\EasyHook32Svc.exe"; DestDir: {app}
Source: "..\deps\wintumbra\EasyHook64Svc.exe"; DestDir: {app}
Source: "dependencies\DirectXHelper.dll"; DestDir: {app}

[InstallDelete]
Type: files; Name: "{app}\*.exe"
Type: files; Name: "{app}\*.dll"
Type: files; Name: "%appdata%\Antumbra\wintumbra.log"
Type: files; Name: "{app}\Extensions\*.dll"

[Run]
Filename: "{tmp}\dotNetFx40_Full_x86_x64.exe"; Check: FrameworkIsNotInstalled
Filename: "{app}\DriverInstaller\glowdrvinst.exe"; Parameters: "batch"

[code]
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;
