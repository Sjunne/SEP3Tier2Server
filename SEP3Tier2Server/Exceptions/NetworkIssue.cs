using System;
using Microsoft.Extensions.Hosting;

namespace SEP3Tier2Server.Exceptions
{
    [Serializable()]
    public class NetworkIssue : System.Exception
    {
        public NetworkIssue(string message) : base(message)
        {
            
        }
    }
}