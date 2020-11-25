﻿using System.Text.Json.Serialization;

namespace MainServerAPI.Network
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum RequestOperationEnum
    {
        EDITINTRODUCTION,
        PROFILE, 
        COVER,
        PICTURES,
        UPLOADPICTURE,
        EDITABOUT,
        REVIEWS,
        UPDATECOVER,
        PROFILEPIC,
        UPDATEPROFILEPIC, 
        SUCCESS,
        ERROR,
        CREATEPROFILE,
        CREATEPREFERENCE,
        EDITPREFERENCE,
        EDITPROFILE,
        DELETEPROFILE,
        DELETEPHOTO,
        GETPREFERENCE,
        GETCONNECTIONS
        MATCHES,
    }
}