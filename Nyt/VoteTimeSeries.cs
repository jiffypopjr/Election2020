using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Voting.Model;

namespace Voting.Nyt
{
    public enum Anomaly
    {
        None,
        Dump,
        Move,
        Both
    }
    public class VoteTimeSeries : IVoteSeries
    {
        public string StateName { get; set; }
        [JsonProperty("vote_shares")]
        public VoteShare VoteShares { get; set; }
        [JsonProperty("votes")]
        public int TotalVotes { get; set; }
        [JsonProperty("eevp")]
        public short PrecinctsPercent { get; set; }
        [JsonProperty("timestamp")]
        public DateTime VoteTimestamp { get; set; }

        public int PreviousTotalVotes => Previous?.TotalVotes ?? 0;

        public decimal TrumpPercentOfTotal => VoteShares.TrumpPercent;
        public decimal BidenPercentOfTotal => VoteShares.BidenPercent;
        public decimal ThirdPartyPercentOfTotal => VoteShares.ThirdPartyPercent;

        public decimal PreviousTrumpPercentOfTotal => VoteShares.Previous?.TrumpPercent ?? 0;
        public decimal PreviousBidenPercentOfTotal => VoteShares.Previous?.BidenPercent ?? 0;
        public decimal PreviousThirdPartyPercentOfTotal => VoteShares.Previous?.ThirdPartyPercent ?? 0;

        public int BidenVotes => (int)Math.Round(TotalVotes * VoteShares.BidenPercent, 0);
        public int TrumpVotes => (int)Math.Round(TotalVotes * VoteShares.TrumpPercent, 0);
        public int ThirdPartyVotes => (int)TotalVotes - BidenVotes - TrumpVotes;

        public int PreviousBidenVotes => Previous?.BidenVotes ?? 0;
        public int PreviousTrumpVotes => Previous?.TrumpVotes ?? 0;
        public int PreviousThirdPartyVotes => Previous?.ThirdPartyVotes ?? 0;

        private VoteTimeSeries Previous { get; set; }

        public int TrumpVoteChange => TrumpVotes - (Previous?.TrumpVotes ?? 0);
        public int BidenVoteChange => BidenVotes - (Previous?.BidenVotes ?? 0);
        public int ThirdPartyVoteChange => ThirdPartyVotes - (Previous?.ThirdPartyVotes ?? 0);

        public decimal TrumpPercentChange => VoteShares.TrumpPercent - (Previous?.VoteShares?.TrumpPercent ?? 0);
        public decimal BidenPercentChange => VoteShares.BidenPercent - (Previous?.VoteShares?.BidenPercent ?? 0);
        public decimal ThirdPartyPercentChange => VoteShares.ThirdPartyPercent - (Previous?.VoteShares?.ThirdPartyPercent ?? 0);

        public decimal TrumpPercentOfVoteBatch => TotalVoteChange != 0 ? Math.Round((decimal)TrumpVoteChange / TotalVoteChange, 3) : 0;
        public decimal BidenPercentOfVoteBatch => TotalVoteChange != 0 ? Math.Round((decimal)BidenVoteChange / TotalVoteChange, 3) : 0;
        public decimal ThirdPartyPercentOfVoteBatch => TotalVoteChange != 0 ? Math.Round((decimal)ThirdPartyVoteChange / TotalVoteChange, 3) : 0;

        public short MinVoteSensitivity => (short)Math.Ceiling(TotalVotes * 0.0005);

        public VoteTimeSeries SetPrevious(string state, VoteTimeSeries previous)
        {
            StateName = state;
            if (previous != null)
            {
                Previous = previous;
                VoteShares.SetPrevious(previous.VoteShares);
            }
            return this;
        }

        public int TotalVoteChange => TotalVotes - (Previous?.TotalVotes ?? 0);
    }
}
