using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Nyt
{
    public class VoteCount
    {
        [JsonProperty("trumpd")]
        public int TrumpCount { get; set; }
        [JsonProperty("bidenj")]
        public int BidenCount { get; set; }
    }
}
