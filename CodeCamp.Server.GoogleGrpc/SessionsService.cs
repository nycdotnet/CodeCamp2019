using Codecamp.Sessions;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Codecamp.Sessions.Sessions;

namespace CodeCamp.Services
{
    public class SessionsService : SessionsBase
    {
        private static readonly Lazy<IEnumerable<Session>> sessions = new Lazy<IEnumerable<Session>>(() => {
            var s = new List<Session>();
            var rawData = File.ReadAllText("..\\sessions\\sessions.txt");
            var elements = rawData.Split('\t');
            var index = 0;
            while (index < elements.Length)
            {
                s.Add(new Session
                {
                    Room = elements[index + 0],
                    TimeSlot = elements[index + 1],
                    SpeakerName = elements[index + 2],
                    Title = elements[index + 3],
                    Abstract = elements[index + 4],
                    Level = elements[index + 5]
                });
                index += 6;
            }
            return s;
        });

        public override async Task<GetSessionsResponse> GetSessions(GetSessionsRequest request, ServerCallContext context)
        {
            var result = new GetSessionsResponse { };
            result.Sessions.Add(string.IsNullOrEmpty(request.TitleStartsWith)
                ? sessions.Value
                : sessions.Value.Where(s => s.Title.StartsWith(request.TitleStartsWith)));
            return await Task.FromResult(result);
        }
    }
}
