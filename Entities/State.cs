using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Entities
{
    public class State
    {
        public string StateName { get; set; }
        public bool IsSwing { get; set; }
        public DateTime AsOfDate { get; set; }

        public static State Create(string stateName, bool isSwing, DateTime asOf) => new State { StateName = stateName, IsSwing = isSwing, AsOfDate = asOf };
    }
}
