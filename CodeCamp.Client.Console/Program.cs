using Codecamp.Sessions;
using Grpc.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static Codecamp.Sessions.Sessions;

namespace CodeCamp.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = new Channel("localhost", 50051, ChannelCredentials.Insecure);
            
            var client = new SessionsClient(channel);

            while (true)
            {
                await Task.Delay(1000);
                var sw = new Stopwatch();
                sw.Restart();
                var response = await client.GetSessionsAsync(new GetSessionsRequest { TitleStartsWith = "H" });
                sw.Stop();
                Console.WriteLine($"Found {response.Sessions.Count} sessions in {sw.ElapsedTicks / 10} us ({sw.ElapsedMilliseconds} ms)");
                foreach(var session in response.Sessions)
                {
                    Console.WriteLine($"{session.Title} by {session.SpeakerName}");
                }
            }
        }
    }
}
