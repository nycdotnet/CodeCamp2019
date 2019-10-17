using Codecamp.Sessions;
using CodeCamp.Data;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace CodeCamp.Server.GoogleGrpc
{
    class Program
    {
        static async Task Main(string[] _)
        {
            const int port = 50051;
            var server = new Grpc.Core.Server
            {
                Services = { Sessions.BindService(new SessionsService(new SessionsRepository())) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();
            
            Console.WriteLine($"server is listening on port {port}!");
            
            await server.ShutdownTask;
        }
    }
}
