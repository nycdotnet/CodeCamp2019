# Code Camp 2019 gRPC Demo

Steve Ognibene, 2019.

https://github.com/nycdotnet

https://twitter.com/nycdotnet

## Presentation

I gave a presentation at Code Camp NYC 2019 called gRPC with .NET Core in Production.

The presentation is available [here](https://github.com/nycdotnet/CodeCamp2019/blob/master/Presentation/gRPC%20with%20.NET%20Core%20in%20prod%20-%20CodeCamp.pptx) and is licensed to you by Steve Ognibene under [Creative Commons BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

The Code Camp NYC 2019 schedule is available [here](CodeCamp.Data/sessions.csv) and is licensed to you by Code Camp NYC, also under [Creative Commons BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

The remainder of the code in this repo is licensed to you under the [MIT License](LICENSE.MD).


## Getting started

This is a .NET Core solution.  It has been tested with Visual Studio 2019 (16.3.5) on Windows, but should compile for Mac or Linux with minimal or no modifications as long as you have .NET Core 3 installed.  All but the `CodeCamp.Server.AspNetCoreGrpc` should work fine on .NET Core 2.x or .NET Framework by appropriately changing the project file.  (At Namely we use gRPC with .NET Core 2.2 on Linux, Mac, and Windows and .NET Framework 4.7.2 on Windows Server or Desktop.

To run this, you must first build the protos.  **The solution will not build correctly until you do this!**  The best way is to have Docker Desktop installed and to run this docker-compose command from the repository root:

```bash
docker-compose -f protorepo/docker-compose.yml up protogen-sessions protogen-feedback
```

Note - in order for this to work, you will need to have shared your local drive with Docker and be running in Linux container mode.  This will use the docker-compose.yml file in the protorepo sub folder (at Namely, we reference our shared protorepo via git submodule), and compile the referenced protos to C# code in the gen/servicename folders.  

## Demo for proto serialization

This demo works best if you debug it in Visual Studio so you can track the flow and inspect the variables as you step through it.  Right-click the `CodeCamp.Demo.ProtoSerialization` project, and click Debug... Step into new instance.

Follow the comments in `CodeCamp.Demo.ProtoSerialization\Program.cs` for an explanation of what is happening.

## Common library for compiled protos

At Namely, we have a shared protorepo with definitions for around 90 services that we share as a git submodule to all gRPC-enabled projects.  The tools that do the work to compile the protos for all languages we support are accessed via a docker-compose command using a docker-compose.yml in the protorepo itself.  We compile protos to C# using tooling like the docker-compose commands described above.  Once this is done, we create a stub "Grpc" project which references the generated code plus the relevant Protobuf and gRPC NuGet packages.  Edit the `CodeCamp.Grpc.csproj` file for an example of this.  The other projects in the solution can then reference the "Grpc" project to get the relevant references.  In this solution, the CodeCamp.Client.Console, CodeCamp.Demo.ProtoSerialization, and CodeCamp.Server.GoogleGrpc projects all reference the CodeCamp.Grpc project rather than referencing/compiling the protos directly.

## Demonstrating the .NET gRPC servers

This project has one client (CodeCamp.Client.Console) and two example servers.  The CodeCamp.Server.GoogleGrpc project is a server using the "regular" Grpc NuGet package that has been around for a long time and works with older .NET Core and .NET Framework versions.  The CodeCamp.Server.AspNetCoreGrpc project uses the new shiny Grpc.AspNetCore NuGet package which only works with .NET Core 3.0.

To run a gRPC demo, open two console windows - one in the `CodeCamp.Client.Console` folder, and one in either the `CodeCamp.Server.GoogleGrpc` or the `CodeCamp.Server.AspNetCoreGrpc` folder.  In the console pointed at your chosen server project's folder, run `dotnet run -c Release`.  This will launch the server on port 50051.  (If it crashes immediately, there is a chance that you have something else listening on that port already; you should get a stack trace to the console.)  Then run `dotnet run -c Release` in the client console.  You should start to see the server get requests and the client get data back.  You can quit out of either process by pressing CTRL+C.
