using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Nyt
{
    public class VoteCounty
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("votes")]
        public int TotalVotes { get; set; }
        [JsonProperty("absentee_votes")]
        public int AbsenteeVotes { get; set; }
        [JsonProperty("precincts")]
        public int TotalPrecincts { get; set; }
        [JsonProperty("results")]
        public VoteCount Results { get; set; }
        [JsonProperty("leader_margin_value")]
        public decimal? LeadingMargin { get; set; }
        [JsonProperty("leader_party_id")]
        public string LeadingParty { get; set; }
        [JsonProperty("margin2020")]
        public decimal? Margin2020 { get; set; }
        [JsonProperty("margin2016")]
        public decimal Margin2016 { get; set; }
        [JsonProperty("votes2016")]
        public int Votes2016 { get; set; }
        [JsonProperty("margin2012")]
        public decimal Margin2012 { get; set; }
        [JsonProperty("votes2012")]
        public int Votes2012 { get; set; }
        [JsonProperty("last_updated")]
        public DateTime Timestamp { get; set; }
    }
}
