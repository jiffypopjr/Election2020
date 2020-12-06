using System;
using System.Collections.Generic;
using System.Text;
using Voting.Nyt;

namespace Voting.Entities
{
    public class County
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public string CountyName { get; set; }
        public DateTime CountyTimestamp { get; set; }
        public int TotalPrecincts { get; set; }

        public int TotalVotes { get; set; }
        public int TrumpVotes { get; set; }
        public int BidenVotes { get; set; }
        public int ThirdPartyVotes { get; set; }
        public int AbsenteeVotes { get; set; }

        public string LeadingParty { get; set; }
        public decimal LeadingMarginPercent { get; set; }
        public decimal Margin2020 { get; set; }
        public decimal Margin2016 { get; set; }
        public decimal Margin2012 { get; set; }
        public int Votes2016 { get; set; }
        public int Votes2012 { get; set; }


        public static County Create(string state, VoteCounty county)
        {
            return new County
            {
                StateName = state,
                CountyName = county.Name,
                TotalVotes = county.TotalVotes,
                AbsenteeVotes = county.AbsenteeVotes,
                TotalPrecincts = county.TotalPrecincts,
                TrumpVotes = county.Results.TrumpCount,
                BidenVotes = county.Results.BidenCount,
                ThirdPartyVotes = county.TotalVotes - county.Results.TrumpCount - county.Results.BidenCount,
                LeadingParty = county.LeadingParty,
                LeadingMarginPercent = Math.Round((county.LeadingMargin ?? 0) / 100.0M, 7),
                Margin2020 = Math.Round((county.Margin2020 ?? 0)/ 100.0M, 7),
                Margin2016 = Math.Round(county.Margin2016 / 100.0M, 7),
                Margin2012 = Math.Round(county.Margin2012 / 100.0M, 7),
                Votes2016 = county.Votes2016,
                Votes2012 = county.Votes2012,
                CountyTimestamp = county.Timestamp
            };
        }
    }
}
