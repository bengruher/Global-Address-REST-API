# REST API for Countries and Country Address Fields

Make sure you have the [latest .NET core](https://dotnet.microsoft.com/download) and an [editor of choice](https://code.visualstudio.com/Download).

## To build and run the application locally

```bash
dotnet restore
dotnet build

ASPNETCORE_ENVIRONMENT=Development dotnet run
```

As of now, the following methods are supported;
  - GET ~/ returns a list of country names
  - GET ~/{country} returns a dictionary (key: field name, value: field type)
