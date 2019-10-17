# Code Camp 2019 gRPC Demo

Steve Ognibene, 2019.

https://github.com/nycdotnet
https://twitter.com/nycdotnet

## Getting started

This is a .NET Core solution.  It has been tested with Visual Studio 2019 (16.3.5) on Windows, but should compile for Mac or Linux with minimal or no modifications as long as you have .NET Core 3 installed.  All but the `CodeCamp.Server.AspNetCoreGrpc` should work fine on .NET Core 2.x or .NET Framework by appropriately changing the project file.  (At Namely we use gRPC with .NET Core 2.2 on Linux, Mac, and Windows and .NET Framework 4.7.2 on Windows Server or Desktop.

To run this, you must first build the protos.  The best way is to have Docker Desktop installed and to run this docker-compose command from the folder with the `docker-compose.yml` file in it:

```bash
docker-compose up protogen-feedback protogen-sessions
```

(Note - in order for this to work, you will need to have shared your local drive with Docker and be running in Linux container mode)

## Demo for proto serialization

This demo works best if you debug it in Visual Studio so you can track the flow and inspect the variables as you step through it.  Right-click the `CodeCamp.Demo.ProtoSerialization` project, and click Debug... Step into new instance.

Follow the comments in `CodeCamp.Demo.ProtoSerialization\Program.cs` for an explanation of what is happening.
