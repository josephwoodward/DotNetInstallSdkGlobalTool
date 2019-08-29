#!/bin/bash

DIR="./package"
if [ -d "$DIR" ]; then
rm -r "$DIR"/*
fi

dotnet pack -o $DIR -c Release ./src/DotNetInstallSdk/DotNetInstallSdk.csproj 