using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Model
{
    public interface IVoteSeries
    {
        string StateName { get; }
        short PrecinctsPercent { get; }
        DateTime VoteTimestamp { get; }

        int TotalVotes { get; }
        int PreviousTotalVotes { get; }

        decimal TrumpPercentOfTotal { get; }
        decimal BidenPercentOfTotal { get; }
        decimal ThirdPartyPercentOfTotal { get; }

        decimal PreviousTrumpPercentOfTotal { get; }
        decimal PreviousBidenPercentOfTotal { get; }
        decimal PreviousThirdPartyPercentOfTotal { get; }

        int TrumpVotes { get; }
        int BidenVotes { get; }
        int ThirdPartyVotes { get; }

        int PreviousTrumpVotes { get; }
        int PreviousBidenVotes { get; }
        int PreviousThirdPartyVotes { get; }

        decimal TrumpPercentOfVoteBatch { get; }
        decimal BidenPercentOfVoteBatch { get; }
        decimal ThirdPartyPercentOfVoteBatch { get; }

        int TotalVoteChange { get; }
        decimal TrumpPercentChange { get; }
        decimal BidenPercentChange { get; }
        decimal ThirdPartyPercentChange { get; }
        int TrumpVoteChange { get; }
        int BidenVoteChange { get; }
        int ThirdPartyVoteChange { get; }

        // Precision of percent of votes is to 3 decimals, so must calc a sensitivity
        short MinVoteSensitivity { get; }
    }
}
