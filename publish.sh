#!/bin/bash

rm -r ./nupkg/*
dotnet pack -o ./nupkg -c Release ./src/InstallSdkGlobalTool/InstallSdkGlobalTool.csproj
