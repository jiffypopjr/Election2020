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
            var result = await DataLoader.LoadDataAsync(false);

            /*
             // attempt to find the 'gaps' in counting - results are weird
             select *
from
(
	select a.StateName, LAG(a.Timestamp,1) over (order by a.StateName, a.Timestamp) as 'Prev', a.Timestamp
	from Votes a
		inner join States b on a.StateName = b.StateName
	where b.IsSwing = 1
	  and a.Timestamp < '11-05-2020 23:00:00'
  ) aa
where DATEDIFF(hh,  aa.Prev, aa.Timestamp) > 3
             */


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
