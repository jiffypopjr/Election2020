using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Nyt
{
    public class VoteTimeSeries
    {
        [JsonProperty("vote_shares")]
        public VoteShare VoteShares { get; set; }
        [JsonProperty("votes")]
        public int TotalVotes { get; set; }
        [JsonProperty("eevp")]
        public int EstimatedPercentReported { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        public int BidenVotes => (int)Math.Round(TotalVotes * VoteShares.BidenPercent, 0);
        public int TrumpVotes => (int)Math.Round(TotalVotes * VoteShares.TrumpPercent, 0);
        public int ThirdPartyVotes => (int)TotalVotes - BidenVotes - TrumpVotes;
        public VoteTimeSeries Previous { get; private set; }

        public int TrumpVoteChange => TrumpVotes - (Previous?.TrumpVotes ?? 0);
        public int BidenVoteChange => BidenVotes - (Previous?.BidenVotes ?? 0);
        public int ThirdPartyVoteChange => ThirdPartyVotes - (Previous?.ThirdPartyVotes ?? 0);

        public decimal TrumpPercentChange => VoteShares.TrumpPercent - (Previous?.VoteShares?.TrumpPercent ?? 0);
        public decimal BidenPercentChange => VoteShares.BidenPercent - (Previous?.VoteShares?.BidenPercent ?? 0);
        public decimal ThirdPartyPercentChange => VoteShares.ThirdPartyPercent - (Previous?.VoteShares?.ThirdPartyPercent ?? 0);

        public decimal TrumpPercentOfVoteDump => TotalVoteChange != 0 ? Math.Round((decimal)TrumpVoteChange / TotalVoteChange, 3) : 0;
        public decimal BidenPercentOfVoteDump => TotalVoteChange != 0 ? Math.Round((decimal)BidenVoteChange / TotalVoteChange, 3) : 0;
        public decimal ThirdPartyPercentOfVoteDump => TotalVoteChange != 0 ? Math.Round((decimal)ThirdPartyVoteChange / TotalVoteChange, 3) : 0;

        public VoteTimeSeries SetPrevious(VoteTimeSeries previous)
        {
            Previous = previous;
            VoteShares.SetPrevious(previous.VoteShares);
            return this;
        }

        public int TotalVoteChange => TotalVotes - (Previous?.TotalVotes ?? 0);
        public VoteShare VoteShareChange => VoteShares.Diff();
    }
}
