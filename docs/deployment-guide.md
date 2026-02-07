# Deployment Guide

The supported deployment target is the F# web server. This guide covers basic publishing and hosting options.

## Build and Publish

From the src/fsharp-server directory:

```bash
dotnet publish -c Release -r win-x64
```

Publish output is under:

```
src/fsharp-server/bin/Release/net8.0/win-x64/publish/
```

## Run in Production

```bash
dotnet EAArchive.dll
```

## Data Location (External Only)

Publish output does not include `data/archimate`. Provide the data folder externally and set `EAArchive:ElementsPath` to an absolute path in `appsettings.Production.json` before running the server.

## Configure the URL

Set the URL with environment variables:

```bash
set ASPNETCORE_URLS=http://localhost:5001
dotnet EAArchive.dll
```

## IIS Hosting (Windows)

1. Publish the app
2. Create an IIS site pointing to the publish folder
3. Ensure the ASP.NET Core Hosting Bundle is installed

## Docker (Optional)

Create a Dockerfile based on .NET 8 and publish output, then run the container exposing port 5000.

## Notes

- The server reads from `EAArchive:ElementsPath` at startup
- Set the path in `appsettings.Production.json` for production deployments
- Use a reverse proxy for TLS termination in production