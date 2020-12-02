using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Voting.Entities;
using Voting.Nyt;

namespace Voting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var state = "michigan";
            if (args.Length > 0)
                state = args[0];

            var options = new Model.DataLoaderOptions() { From = Model.LoadFrom.Db, StateFilter = state, Show = Anomaly.Dump };
            var result = await DataLoader.LoadDataAsync(options);

            Console.WriteLine($"Data for {state.ToUpper()} with {result.Length} items");
            foreach (var item in result)
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

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
