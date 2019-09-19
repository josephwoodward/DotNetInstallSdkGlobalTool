# .NET Install SDK Global Tool

Global tool to make installing .NET Core versions that little bit easier


To install globally:

```bash
$ dotnet tool install -g installsdkglobaltool
```

You can also find InstallSdkGlobalTool [on NuGet here](https://www.nuget.org/packages/InstallSdkGlobalTool/).

## Usage:

You can use this tool to download and install .NET Core SDK versions based on a variety of ways, these are:

### Install based on global.json

1. Navigate to a directory that contains a `global.json` file in your terminal.
2. Run `$ dotnet install-sdk`

The .NET Core Install SDK global tool will then download and launch the installer for the version of the SDK dictated by the global.json file.

### Install latest .NET Core SDK Preview

1. Run `$ dotnet install-sdk --latest-preview` from any directory

The .NET core Install SDK global tool will then download and lauch the installer for the latest preview of the .NET Core SDK.

##Contributions

Big thanks to [@stuartblang](https://twitter.com/stuartblang) who contributed loads to this.

### Options

```bash
$ dotnet install-sdk -h

Usage: DotNetInstallSdk [options]

Options:
  -LP|--latest-preview     Optional. Install the latest preview version of the .NET Core SDK
  -H|--headless <Boolean>  Optional. Install .NET Core SDK in headless mode (default is true
  -?|-h|--help             Show help information
```
