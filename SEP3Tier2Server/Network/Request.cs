﻿using System;
using System.Text.Json.Serialization;

namespace MainServerAPI.Network
{
    public class Request
    {
        
        public Object o { get; set; }
        public RequestOperationEnum requestOperation { get; set; }

        public string Username { get; set; }
        
    
    }
    
}