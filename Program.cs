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
            var state = "pennsylvania";
            if (args.Length > 0)
                state = args[0];
            var result = await DataLoader.LoadDataAsync(new Model.DataLoaderOptions() { From = Model.LoadFrom.Db, StateFilter = state });




            Console.WriteLine($"Data for {state.ToUpper()}");
            foreach (var item in result)
            {
                var a = item.GetAnomaly();
                switch (a)
                {
                    case Anomaly.Move:
                        item.RenderMove();
                        break;
                    case Anomaly.Dump:
                        item.RenderDump();
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
