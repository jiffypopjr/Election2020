using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Voting.Entities;
using Voting.Model;
using Voting.Nyt;

namespace Voting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string state = "texas";
            if (args.Length > 0)
                state = args[0];

            var options = new Model.DataLoaderOptions()
            {
                From = LoadFrom.Db,
                StateFilter = state,
                Show = Anomaly.Dump,
                //RecreateDb = true
            };
            var results = await DataLoader.LoadDataAsync(options);

            if (state != null)
            {
                Console.WriteLine($"Data for {state.ToUpper()} with {results.Length} items");
                Console.WriteLine("BIDEN lost {0} votes from MOVEs>>", results.Where(r => r.TotalVoteChange > 0 && r.BidenVoteChange < 0).Sum(r => r.BidenVoteChange));
                Console.WriteLine("TRUMP lost {0} votes from MOVEs>>", results.Where(r => r.TotalVoteChange > 0 && r.TrumpVoteChange < 0).Sum(r => r.TrumpVoteChange));

                foreach (var item in results)
                {
                    var a = item.GetAnomaly();
                    switch (a)
                    {
                        case Anomaly.Move:
                            item.RenderMove(options.Show);
                            break;
                        case Anomaly.Dump:
                            item.RenderDump(options.Show);
                            break;
                        default:
                            break;
                    }
                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
