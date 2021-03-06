﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Voting.Entities;
using Voting.Model;

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

        private static string FilesSubdirectory = "nyt-data";

        public static async Task<IVoteSeries[]> LoadDataAsync(DataLoaderOptions options)
        {
            var result = new List<IVoteSeries>();

            if (await NeedFiles(options))
                EnsureFiles(options.RedownloadFiles);
            using (var dbContext = new VoteDbContext())
            {
                if (options.IsDb)
                {
                    if (options.RecreateDb)
                        await dbContext.Database.EnsureDeletedAsync();

                    await dbContext.Database.EnsureCreatedAsync();

                    //if (!options.RecreateDb)
                    //{
                    //    return await dbContext.Votes
                    //        .Where(v => v.StateName == options.StateFilter && v.PrecinctsPercent > 0)
                    //        .AsNoTracking()
                    //        .ToArrayAsync();
                    //}
                }

                Console.WriteLine("Loading data...");
                foreach (var filePath in Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}\\{FilesSubdirectory}"))
                {
                    var state = filePath.Substring(filePath.LastIndexOf('\\') + 1).Replace(".json", string.Empty);
                    state = state[0].ToString().ToUpper() + state.Substring(1);

                    if (options.StateFilter == null || string.Compare(state, options.StateFilter, true) == 0)
                    {
                        Console.WriteLine($"  Loading {state}...");
                        var votes = JObject.Parse(await File.ReadAllTextAsync(filePath));
                        var timeseries = votes?["data"]?["races"]?.FirstOrDefault()?["timeseries"];
                        if (timeseries == null)
                        {
                            Console.WriteLine($"  WARNING: {state} voting data is null!");
                            continue;
                        }
                        var typedSeries = timeseries.Select(s => (VoteTimeSeries)s.ToObject(typeof(VoteTimeSeries))).OrderBy(s => s.VoteTimestamp).ToArray();
                        typedSeries = typedSeries.Select((ts, i) => i > 0 ? ts.SetPrevious(state, typedSeries[i - 1]) : ts.SetPrevious(state, null)).ToArray();

                        if (!await dbContext.States.AnyAsync(s => s.StateName == state))
                            dbContext.States.Add(State.Create(state, Swingers.Contains(state), DateTime.UtcNow));

                        if (!await dbContext.Votes.AnyAsync(s => s.StateName == state))
                        {
                            Console.WriteLine($"    There are {typedSeries.Length} vote time slices for {state}");
                            foreach (var ts in typedSeries)
                            {
                                dbContext.Votes.Add(Vote.Create(ts));
                            }
                        }

                        // counties
                        var counties = votes?["data"]?["races"]?.FirstOrDefault();
                        if (counties != null)
                        {
                            var typedCounties = counties["counties"]?.Select(
                                                c => (VoteCounty)c.ToObject(typeof(VoteCounty))).ToList();
                            Console.WriteLine($"    There are {typedCounties.Count} counties in {state}");
                            if (!await dbContext.Counties.AnyAsync(c => c.StateName == state))
                                typedCounties.ForEach(c =>
                                                    {
                                                        dbContext.Counties.Add(County.Create(state, c));
                                                    });
                        }
                        else
                            Console.WriteLine("WARNING: {state} had no counties");

                        if (options.IsDb && dbContext.ChangeTracker.Entries().Any())
                            await dbContext.SaveChangesAsync();

                        if (options.StateFilter != null)
                            return typedSeries.Where(s => s.PrecinctsPercent > 0).OrderBy(s => s.VoteTimestamp).ToArray();
                    }
                }
            }

            return new IVoteSeries[] { };
        }

        private static async Task<bool> NeedFiles(DataLoaderOptions options)
        {
            if (!options.IsDb)
                return true;
            else
            {
                try
                {
                    using (var ctx = new VoteDbContext())
                        return await ctx.States.CountAsync() == 51;
                }
                catch { return true; }
            }
        }

        private static void EnsureFiles(bool force = false)
        {
            var client = new WebClient();
            var baseDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\{FilesSubdirectory}";
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            foreach (var state in States)
            {
                // example: https://static01.nyt.com/elections-assets/2020/data/api/2020-11-03/race-page/pennsylvania/president.json

                var filePath = $"{ baseDirectory }\\{ state}.json";
                if (force && File.Exists(filePath))
                    File.Delete(filePath);

                if (!File.Exists(filePath) || force)
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
