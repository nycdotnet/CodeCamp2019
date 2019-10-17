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
            var sw = new Stopwatch();

            while (true)
            {
                for (int i = 'A'; i <= 'Z'; i++) // yep, this will cycle A-Z
                {
                    var startsWith = new string((char)i, 1);
                    sw.Restart();
                    var response = await client.GetSessionsAsync(new GetSessionsRequest { TitleStartsWith = startsWith });
                    sw.Stop();
                    Console.WriteLine($"Found {response.Sessions.Count} sessions starting with {startsWith} in {sw.ElapsedTicks / 10} μs ({sw.ElapsedMilliseconds} ms)");
                    foreach (var session in response.Sessions)
                    {
                        Console.WriteLine($"{session.Title} by {session.SpeakerName}");
                    }
                    Console.WriteLine("");
                    if (response.Sessions.Count > 0)
                    {
                        await Task.Delay(2_000);
                    }
                }
            }
        }
    }
}
