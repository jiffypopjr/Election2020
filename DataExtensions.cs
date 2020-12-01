using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
