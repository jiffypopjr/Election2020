using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Nyt
{
    public class VoteShare
    {
        [JsonProperty("trumpd")]
        public decimal TrumpPercent { get; set; }
        [JsonProperty("bidenj")]
        public decimal BidenPercent { get; set; }

        public decimal ThirdPartyPercent => 1.0M - TrumpPercent - BidenPercent;

        public VoteShare Previous { get; private set; }
        public VoteShare SetPrevious(VoteShare previous)
        {
            Previous = previous;
            return this;
        }

        public VoteShare Diff()
        {
            if (Previous == null)
                return this;
            else
                return new VoteShare { TrumpPercent = TrumpPercent - Previous.TrumpPercent, BidenPercent = BidenPercent - Previous.BidenPercent };
        }
    }
}
