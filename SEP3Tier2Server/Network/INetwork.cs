﻿using System;
 using System.Collections.Generic;
 using MainServerAPI.Data;

namespace MainServerAPI.Network
{
    public interface INetwork
    {
        void updateProfile(ProfileData profile);
        ProfileData GetProfile(string username);

        byte[] GetCover(string username);
        List<byte[]> GetPictures(string username);
        void UploadImage(Request request);
        void editProfile(Request request);
    }
}