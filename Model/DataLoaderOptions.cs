using System;
using System.Collections.Generic;
using System.Text;
using Voting.Nyt;

namespace Voting.Model
{
    public enum LoadFrom
    {
        Files,
        Db
    }
    public class DataLoaderOptions
    {
        public bool IsDb => From == LoadFrom.Db;

        public LoadFrom From { get; set; } = LoadFrom.Files;
        public bool RecreateDb { get; set; } = false;
        public bool RedownloadFiles { get; set; } = false;
        public string StateFilter { get; set; } = "pennsylvania";
        public Anomaly Show { get; set; } = Anomaly.Both;
    }
}
