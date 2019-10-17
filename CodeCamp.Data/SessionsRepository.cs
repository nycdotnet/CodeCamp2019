using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CodeCamp.Data
{
    public class SessionsRepository
    {
        /// <summary>
        /// Reads the sessions out of the embedded CSV.
        /// </summary>
        private static readonly Lazy<IEnumerable<string[]>> sessions = new Lazy<IEnumerable<string[]>>(() => {
            var result = new List<string[]>();
            // the sessions.csv file is set as an Embedded Resource in the project, so we can read it out of the assembly itself
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CodeCamp.Data.sessions.csv"))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader))
            using (var csvData = new CsvDataReader(csv))
            {
                csv.Configuration.HasHeaderRecord = false;
                do
                {
                    var values = new string[6];
                    csvData.GetValues(values);
                    result.Add(values);
                }
                while (csvData.Read());
            }
            return result;
        });

        /// <summary>
        /// Gets all of the sessions that will be held at Code Camp.
        /// </summary>
        public IEnumerable<string[]> GetSessions() => sessions.Value;
    }
}
