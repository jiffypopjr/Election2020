using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voting.Entities;

namespace Voting.Nyt
{
    public static class DataLoader
    {
        public static string[] States = new[] { "Alabama","Alaska","Arizona","Arkansas","California","Colorado","Connecticut","Delaware",
            "District-of-columbia","Florida","Georgia","Hawaii","Idaho","Illinois","Indiana","Iowa","new-jersey","Kansas","Kentucky","Louisiana","Maine",
            "Maryland","Massachusetts","new-mexico","Michigan","Minnesota","Mississippi","Missouri","Montana","Nebraska","Nevada","New-hampshire","New-york",
            "North-carolina","North-dakota","Ohio","Oklahoma","Oregon","Pennsylvania","Rhode-island","South-carolina","South-dakota","Tennessee","Texas","Utah",
            "Vermont","Virginia","Washington","West-virginia","Wisconsin","Wyoming" };
        public static string[] Swingers = new[] { "Pennsylvania", "Michigan", "Wisconson", "Arizona", "Nevada", "Georgia" };
        public static async Task<Dictionary<string, VoteTimeSeries[]>> LoadDataAsync(bool resetDb = false, bool reloadFiles = false)
        {
            var result = new Dictionary<string, VoteTimeSeries[]>();
            DownloadFiles(reloadFiles);

            using (var dbContext = new VoteDbContext())
            {
                if (resetDb)
                    await dbContext.Database.EnsureDeletedAsync();

                await dbContext.Database.EnsureCreatedAsync();

                Console.WriteLine("Loading data");
                foreach (var filePath in Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}\\nyt-data"))
                {
                    var state = filePath.Substring(filePath.LastIndexOf('\\') + 1).Replace(".json", string.Empty);
                    state = state[0].ToString().ToUpper() + state.Substring(1);

                    if (!await dbContext.States.AnyAsync(s => s.StateName == state))
                        dbContext.States.Add(State.Create(state, Swingers.Contains(state), DateTime.UtcNow));

                    Console.WriteLine($"  Loading {state}...");
                    var votes = JObject.Parse(await File.ReadAllTextAsync(filePath));
                    var timeseries = votes?["data"]?["races"]?.FirstOrDefault()?["timeseries"];
                    if (timeseries == null)
                    {
                        Console.WriteLine($"WARNING: {state} voting data is null!");
                        continue;
                    }
                    var typedSeries = timeseries.Select(s => (VoteTimeSeries)s.ToObject(typeof(VoteTimeSeries))).OrderBy(s => s.Timestamp).ToArray();
                    Console.WriteLine($"    There are {typedSeries.Length} items for {state}");
                    typedSeries = typedSeries.Select((ts, i) => i > 0 ? ts.SetPrevious(typedSeries[i - 1]) : ts).ToArray();
                    result.Add(state.ToLower(), typedSeries);
                    if (!await dbContext.Votes.AnyAsync(s => s.StateName == state))
                    {
                        foreach (var ts in typedSeries)
                        {
                            dbContext.Votes.Add(Vote.Create(state, ts));
                        }
                    }

                    if (resetDb)
                        await dbContext.SaveChangesAsync();
                }
            }

            return result;
        }

        private static void DownloadFiles(bool resetFiles = false)
        {
            var client = new WebClient();
            var baseDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\nyt-data";
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            foreach (var state in States)
            {
                // example: https://static01.nyt.com/elections-assets/2020/data/api/2020-11-03/race-page/pennsylvania/president.json

                var filePath = $"{ baseDirectory }\\{ state}.json";
                if (resetFiles && File.Exists(filePath))
                    File.Delete(filePath);

                if (!File.Exists(filePath) || resetFiles)
                {
                    var uri = $"https://static01.nyt.com/elections-assets/2020/data/api/2020-11-03/race-page/{state.ToLower()}/president.json";
                    try
                    {
                        Console.WriteLine($"Downloading {state}..");
                        client.DownloadFile(new Uri(uri), filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR for {state}: {ex.Message}");
                    }
                }
            }
        }
    }
}
