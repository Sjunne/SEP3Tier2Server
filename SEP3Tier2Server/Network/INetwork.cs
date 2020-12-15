﻿using System;
 using System.Collections.Generic;
 using MainServerAPI.Data;
 using WebApplication.Data;

 namespace MainServerAPI.Network
{
    public interface INetwork
    {
        RequestOperationEnum updateProfile(ProfileData profile);
        ProfileData GetProfile(string username);

        Warning GetWarning(String username);
        RequestOperationEnum RemoveWarning(String username);

        string GetCover(string username);
        string GetPictures(string username);
        RequestOperationEnum UploadImage(Request request);
        RequestOperationEnum UpdateCover(Request request);
        string GetProfilePicture(string username);
        RequestOperationEnum UpdateProfilePic(Request request);
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