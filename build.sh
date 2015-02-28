#!/bin/bash

set -e
shopt -s extglob

cmd /c buildsub.bat
'/cygdrive/c/Program Files (x86)/Inno Setup 5/ISCC.exe' WintumbraInstaller/WintumbraInstaller.iss
mkdir stage
cp WintumbraInstaller/Output/setup.exe stage
cp README.md stage
mkdir stage/without-installer/{,DriverInstaller,Extensions}
cp WintumbraInstaller/dependencies/{Antumbra.exe,libantumbra.dll,libusb-1.0.dll} stage/without-installer
cp WintumbraInstaller/dependencies/!(libantumbra|libusb-1.0).dll stage/without-installer/Extensions
cp deps/libwdi/*.dll deps/win32-libantumbra/glowdrvinst.exe stage/without-installer/DriverInstaller

find stage -type f -print0 | xargs -0 chmod 0644
find stage -type d -print0 | xargs -0 chmod 0755
find stage -type f \( -name '*.dll' -o -name '*.exe' \) -print0 | xargs -0 chmod 0755

cd stage
bsdtar --format=zip -cf ../BUILD.zip -- *
