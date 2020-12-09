﻿using System;
 using System.Collections.Generic;
 using MainServerAPI.Data;

namespace MainServerAPI.Network
{
    public interface INetwork
    {
        RequestOperationEnum updateProfile(ProfileData profile);
        ProfileData GetProfile(string username);

        byte[] GetCover(string username);
        List<byte[]> GetPictures(string username);
        RequestOperationEnum UploadImage(Request request);
        RequestOperationEnum UpdateCover(string pictureName);
        byte[] GetProfilePicture(string username);
        RequestOperationEnum UpdateProfilePic(string pictureName);
        RequestOperationEnum editProfile(Request request);
        IList<Review> GetReviews(string username);

        void CreateProfile(ProfileData profileData);

        void CreatePreference(ProfileData profileData);
        RequestOperationEnum DeletePhoto(string pictureName);
        ProfileData GetPreference(string username);
        void editPreference(ProfileData profileData);
        void bigEditProfile(ProfileData profileData);
        void deleteProfile(string username);
        IList<String> Matches(string username);
        Request ValidateLogin(User user);
        IList<PrivateMessage> getAllPrivateMessages(Request request);
        Request RegisterUser(Request request);

        RequestOperationEnum AcceptMatch(Match match);
        RequestOperationEnum DeclineMatch(Match match);
        Request ChangePasswordOrUsername(Request request);

        IList<string> getAllProfiles();
        RequestOperationEnum CreateMatch(Match match);


        Request AddReview(Request request);
        Request ReportReview(Request request);
    }
}