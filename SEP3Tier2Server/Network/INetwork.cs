﻿using System;
 using System.Collections.Generic;
 using MainServerAPI.Data;

namespace MainServerAPI.Network
{
    public interface INetwork
    {
        void updateProfile(ProfileData profile);
        ProfileData GetProfile(string username);

        Byte[] GetCover(string username);
        List<byte[]> GetPictures(string username);
    }
}