using System;
using System.Collections;
using System.Collections.Generic;

namespace MainServerAPI.Data
{
    public class Match
    {
        public Match()
        {
            usernames = new List<string>();
            percentage = 0;
        }
        public IList<string> usernames { get; set; }
        public double percentage { get; set; }
    }
}