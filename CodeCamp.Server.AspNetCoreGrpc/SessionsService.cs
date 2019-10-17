using Codecamp.Sessions;
using CodeCamp.Data;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Codecamp.Sessions.Sessions;

namespace CodeCamp.Server.AspNetCoreGrpc
{
    public class SessionsService : SessionsBase
    {
        private readonly SessionsRepository sessionsRepo;

        public SessionsService(SessionsRepository sessionsRepo)
        {
            this.sessionsRepo = sessionsRepo;
        }

        public override async Task<GetSessionsResponse> GetSessions(GetSessionsRequest request, ServerCallContext context)
        {
            var result = new GetSessionsResponse { };
            result.Sessions.Add(string.IsNullOrEmpty(request.TitleStartsWith)
                ? GetSessions()
                : GetSessions().Where(s => s.Title.StartsWith(request.TitleStartsWith, StringComparison.OrdinalIgnoreCase)));
            return await Task.FromResult(result);
        }

        private IEnumerable<Session> GetSessions()
        {
            // Note: In a real project, your data repository could link to your proto and serialize/deserialize it
            // directly as a model object.  In this demo project, we have both the ASP.NET Core WebAPI proto gen and the
            // shared CodeCamp.Grpc projects which both compile the proto, so rather than conflict them, we're just
            // returning the strings of the session data and letting each project convert to its own version of `Session`.
            foreach (var row in sessionsRepo.GetSessions())
            {
                yield return new Session
                {
                    Room = row[0],
                    TimeSlot = row[1],
                    SpeakerName = row[2],
                    Title = row[3],
                    Abstract = row[4],
                    Level = row[5]
                };
            }
        }
    }
}
