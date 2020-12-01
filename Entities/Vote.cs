using System;
using System.Collections.Generic;
using System.Text;
using Voting.Model;
using Voting.Nyt;

namespace Voting.Entities
{
    public class Vote : IVoteSeries
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public short PrecinctsPercent { get; set; }
        public DateTime VoteTimestamp { get; set; }

        public int TotalVotes { get; set; }
        public int PreviousTotalVotes { get; set; }

        public decimal TrumpPercentOfTotal { get; set; }
        public decimal BidenPercentOfTotal { get; set; }
        public decimal ThirdPartyPercentOfTotal { get; set; }


        public decimal PreviousTrumpPercentOfTotal { get; set; }
        public decimal PreviousBidenPercentOfTotal { get; set; }
        public decimal PreviousThirdPartyPercentOfTotal { get; set; }

        public int TrumpVotes { get; set; }
        public int BidenVotes { get; set; }
        public int ThirdPartyVotes { get; set; }

        public int PreviousTrumpVotes { get; set; }
        public int PreviousBidenVotes { get; set; }
        public int PreviousThirdPartyVotes { get; set; }

        public decimal TrumpPercentOfVoteBatch { get; set; }
        public decimal BidenPercentOfVoteBatch { get; set; }
        public decimal ThirdPartyPercentOfVoteBatch { get; set; }

        // calced
        public int TotalVoteChange { get; set; }
        public decimal TrumpPercentChange { get; set; }
        public decimal BidenPercentChange { get; set; }
        public decimal ThirdPartyPercentChange { get; set; }
        public int TrumpVoteChange { get; set; }
        public int BidenVoteChange { get; set; }
        public int ThirdPartyVoteChange { get; set; }

        public short MinVoteSensitivity { get; set; }

        public static Vote Create(VoteTimeSeries ts)
        {
            return new Vote
            {
                StateName = ts.StateName,
                PrecinctsPercent = (short)ts.PrecinctsPercent,
                VoteTimestamp = ts.VoteTimestamp,
                TotalVotes = ts.TotalVotes,
                PreviousTotalVotes = ts.PreviousTotalVotes,
                TrumpPercentOfTotal = ts.TrumpPercentOfTotal,
                BidenPercentOfTotal = ts.BidenPercentOfTotal,
                ThirdPartyPercentOfTotal = ts.ThirdPartyPercentOfTotal,
                PreviousTrumpPercentOfTotal = ts.PreviousTrumpPercentOfTotal,
                PreviousBidenPercentOfTotal = ts.PreviousBidenPercentOfTotal,
                PreviousThirdPartyPercentOfTotal = ts.PreviousThirdPartyPercentOfTotal,

                TrumpVotes = ts.TrumpVotes,
                BidenVotes = ts.BidenVotes,
                ThirdPartyVotes = ts.ThirdPartyVotes,

                PreviousTrumpVotes = ts.PreviousTrumpVotes,
                PreviousBidenVotes = ts.PreviousBidenVotes,
                PreviousThirdPartyVotes = ts.PreviousThirdPartyVotes,

                TrumpPercentOfVoteBatch = ts.TrumpPercentOfVoteBatch,
                BidenPercentOfVoteBatch = ts.BidenPercentOfVoteBatch,
                ThirdPartyPercentOfVoteBatch = ts.ThirdPartyPercentOfVoteBatch,
                MinVoteSensitivity = ts.MinVoteSensitivity
            };
        }
    }
}
