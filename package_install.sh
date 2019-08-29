#!/bin/bash

dotnet tool uninstall -g dotnetinstallsdk
dotnet pack -o ./package/ -c Release ./src/DotNetInstallSdk/DotNetInstallSdk.csproj 
dotnet tool install -g DotNetInstallSdk --add-source ./package/
