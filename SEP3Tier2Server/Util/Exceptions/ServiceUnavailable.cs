using System;

namespace SEP3Tier2Server.Exceptions
{
    public class ServiceUnavailable : Exception
    {
        public ServiceUnavailable(string message) : base(message)
        {
            
        }
    }
}