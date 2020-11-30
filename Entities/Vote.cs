using System;
using System.Collections.Generic;
using System.Text;
using Voting.Nyt;

namespace Voting.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public short PrecinctsPercent { get; set; }
        public DateTime Timestamp { get; set; }

        public int TotalVotes { get; set; }
        public int PreviousTotalVotes { get; set; }

        public decimal TrumpPercent { get; set; }
        public decimal BidenPercent { get; set; }
        public decimal ThirdPartyPercent { get; set; }

        public decimal PreviousTrumpPercent { get; set; }
        public decimal PreviousBidenPercent { get; set; }
        public decimal PreviousThirdPartyPercent { get; set; }

        public int TrumpVotes { get; set; }
        public int BidenVotes { get; set; }
        public int ThirdPartyVotes { get; set; }

        public int PreviousTrumpVotes { get; set; }
        public int PreviousBidenVotes { get; set; }
        public int PreviousThirdPartyVotes { get; set; }


        // calced
        public int TotalVoteChange { get; set; }
        public decimal TrumpPercentChange { get; set; }
        public decimal BidenPercentChange { get; set; }
        public decimal ThirdPartyPercentChange { get; set; }
        public int TrumpVoteChange { get; set; }
        public int BidenVoteChange { get; set; }
        public int ThirdPartyVoteChange { get; set; }

        public static Vote Create(string state, VoteTimeSeries ts)
        {
            return new Vote
            {
                StateName = state,
                PrecinctsPercent = (short)ts.EstimatedPercentReported,
                Timestamp = ts.Timestamp,
                TotalVotes = ts.TotalVotes,
                PreviousTotalVotes = (ts.Previous?.TotalVotes ?? 0),
                TrumpPercent = ts.VoteShares.TrumpPercent,
                BidenPercent = ts.VoteShares.BidenPercent,
                ThirdPartyPercent = ts.VoteShares.ThirdPartyPercent,
                PreviousTrumpPercent = ts.Previous?.VoteShares?.TrumpPercent ?? 0,
                PreviousBidenPercent = ts.Previous?.VoteShares?.BidenPercent ?? 0,
                PreviousThirdPartyPercent = ts.Previous?.VoteShares?.ThirdPartyPercent ?? 0,

                TrumpVotes = ts.TrumpVotes,
                BidenVotes = ts.BidenVotes,
                ThirdPartyVotes = ts.ThirdPartyVotes,

                PreviousTrumpVotes = ts.Previous?.TrumpVotes ?? 0,
                PreviousBidenVotes = ts.Previous?.BidenVotes ?? 0,
                PreviousThirdPartyVotes = ts.Previous?.ThirdPartyVotes ?? 0
            };
        }


        // calc columns?
    }
}
