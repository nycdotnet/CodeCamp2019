using Codecamp.Sessions;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace CodeCamp.Services
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new Server
            {
                Services = { Sessions.BindService(new SessionsService()) },
                Ports = { new ServerPort("localhost", 50051, ServerCredentials.Insecure) }
            };
            server.Start();
            
            Console.WriteLine("server is running!");
            
            await server.ShutdownTask;
        }
    }
}
