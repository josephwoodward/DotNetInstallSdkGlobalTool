#!/bin/bash

dotnet tool uninstall -g installsdkglobaltool
dotnet pack -o ./package/ -c Release ./src/DotNetInstallSdk/DotNetInstallSdk.csproj 
dotnet tool install -g installsdkglobaltool --add-source ./package/
