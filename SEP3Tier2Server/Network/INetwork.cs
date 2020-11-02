﻿using System;
 using MainServerAPI.Data;

namespace MainServerAPI.Network
{
    public interface INetwork
    {
        void updateProfile(ProfileData profile);
        ProfileData GetProfile(string username);

        Byte[] GetCover(string username);
    }
}