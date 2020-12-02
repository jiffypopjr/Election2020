using System;
using System.Collections.Generic;
using System.Text;
using Voting.Model;
using Voting.Nyt;

namespace Voting
{
    public static class DataExtensions
    {
        public static string ToGainLoss(this int value)
        {
            if (value < 0)
                return "lost";
            else if (value > 0)
                return "gained";
            else
                return "remains at";
        }

        public static Anomaly GetAnomaly(this IVoteSeries v)
        {
            var negSens = v.MinVoteSensitivity * -1;
            // set to 20k for now

            if (v.TotalVoteChange > 20000 && (v.TrumpPercentOfVoteBatch > 0.8M || v.BidenPercentOfVoteBatch > 0.8M || v.ThirdPartyPercentOfVoteBatch > 0.8M))
                return Anomaly.Dump;

            if (v.TrumpVoteChange < negSens || v.BidenVoteChange < negSens || v.ThirdPartyVoteChange < negSens)
                //  || v.TrumpVoteChange > v.MinVoteSensitivity || v.BidenVoteChange > v.MinVoteSensitivity || v.ThirdPartyVoteChange > v.MinVoteSensitivity)
                return Anomaly.Move;
            return Anomaly.None;
        }

        public static void RenderMove(this IVoteSeries v, Anomaly show = Anomaly.Both)
        {
            if (show == Anomaly.Both || show == Anomaly.Move)
                Console.WriteLine($">>MOVE {v.VoteTimestamp} ({v.PrecinctsPercent}%) Of {v.TotalVoteChange}: BIDEN {v.BidenVoteChange.ToGainLoss()} {v.BidenVoteChange} | TRUMP {v.TrumpVoteChange.ToGainLoss()} {v.TrumpVoteChange} | THIRD {v.ThirdPartyVoteChange.ToGainLoss()} {v.ThirdPartyVoteChange}");
        }
        public static void RenderDump(this IVoteSeries v, Anomaly show = Anomaly.Both)
        {
            if (show == Anomaly.Both || show == Anomaly.Dump)
                Console.WriteLine($"^DUMP^ {v.VoteTimestamp} ({v.PrecinctsPercent}%) Of {v.TotalVoteChange}: BIDEN {v.BidenPercentOfVoteBatch} to {v.BidenVoteChange} | TRUMP {v.TrumpPercentOfVoteBatch} to {v.TrumpVoteChange} | THIRD {v.ThirdPartyPercentOfVoteBatch} to {v.ThirdPartyVoteChange}");
        }
    }
}
