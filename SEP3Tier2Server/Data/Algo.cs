using System;
using System.Collections.Generic;
using System.Linq;
using MainServerAPI.Network;
using Microsoft.AspNetCore.Mvc;

namespace MainServerAPI.Data

{
    public class Algo
    {
        private INetwork network;
        public Algo()
        {
            network = new NetworkSocket();
            
        }
        
        public void FindMatches()
        {
            IList<string> usernames = network.getAllProfiles();
            IList<ProfileData> profiles = new List<ProfileData>();
            foreach (string username in usernames)
            {
                ProfileData profileData = network.GetProfile(username);
                profileData.preferences = network.GetPreference(username).preferences;
                profiles.Add(profileData); 
            }
            while (profiles.Count > 1)
            {
                ProfileData valgtProfil = profiles[0];
                
                foreach (ProfileData profile in profiles)
                {
                    if (profile.username.Equals(valgtProfil.username))
                    {
                        //så skal den ikke gøre noget
                    }
                    else
                    {
                        double percentage = 0;
                        string ValgtProfilStrings;
                        string ProfileStrings;
                        string[] HobbiesValgtProfilStringArray;
                        string[] ProfileStringArray;
                        if ((valgtProfil.preferences.gender.Equals(profile.self.gender) ||
                             valgtProfil.preferences.gender.Equals("N")) &&
                            (profile.preferences.gender.Equals(valgtProfil.self.gender) ||
                             profile.preferences.gender.Equals("N")))
                        {
                            percentage += 0.01;
                            if (profile.age >= valgtProfil.preferences.minimumAge &&
                                profile.age <= valgtProfil.preferences.maximumAge &&
                                valgtProfil.age >= profile.preferences.minimumAge &&
                                valgtProfil.age <= profile.preferences.maximumAge)
                            {
                                ValgtProfilStrings = valgtProfil.preferences.city;
                                ProfileStrings = profile.preferences.city;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.city))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.city))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                ValgtProfilStrings = valgtProfil.preferences.education;
                                ProfileStrings = profile.preferences.education;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.education))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.education))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                if (valgtProfil.preferences.hobbies.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (valgtProfil.preferences.hobbies.Contains(","))
                                    {
                                        HobbiesValgtProfilStringArray = valgtProfil.preferences.hobbies.Split(",");
                                        foreach (string hobby in HobbiesValgtProfilStringArray)
                                        {
                                            if (profile.self.hobbies.Contains(hobby))
                                            {
                                                percentage += 4.545 / ValgtProfilStrings.Length;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ValgtProfilStrings = valgtProfil.preferences.hobbies;
                                        if (profile.self.hobbies.Contains(ValgtProfilStrings))
                                        {
                                            percentage += 4.545;
                                        }
                                    }

                                }

                                if (profile.preferences.hobbies.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (profile.preferences.hobbies.Contains(","))
                                    {
                                        ProfileStringArray = profile.preferences.hobbies.Split(",");
                                        foreach (string hobby in ProfileStringArray)
                                        {
                                            if (valgtProfil.self.hobbies.Contains(hobby))
                                            {
                                                percentage += 4.545 / ProfileStringArray.Length;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ProfileStrings = profile.preferences.hobbies;
                                        if (valgtProfil.self.hobbies.Contains(ProfileStrings))
                                        {
                                            percentage += 4.545;
                                        }
                                    }
                                }
                                

                                ValgtProfilStrings = valgtProfil.preferences.hairColor;
                                ProfileStrings = profile.preferences.hairColor;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.self.hairColor))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.self.hairColor))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                ValgtProfilStrings = valgtProfil.preferences.eyeColor;
                                ProfileStrings = profile.preferences.eyeColor;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.self.eyeColor))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.self.eyeColor))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                ValgtProfilStrings = valgtProfil.preferences.skinColor;
                                ProfileStrings = profile.preferences.skinColor;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.self.skinColor))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.self.skinColor))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                ValgtProfilStrings = valgtProfil.preferences.nationality;
                                ProfileStrings = profile.preferences.nationality;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.self.nationality))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.self.nationality))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                ValgtProfilStrings = valgtProfil.preferences.bodyShape;
                                ProfileStrings = profile.preferences.bodyShape;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.self.bodyShape))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.self.bodyShape))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                ValgtProfilStrings = valgtProfil.preferences.job;
                                ProfileStrings = profile.preferences.job;
                                if (ValgtProfilStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ValgtProfilStrings.Contains(profile.self.job))
                                    {
                                        percentage += 4.545;
                                    }
                                }
                                
                                if (ProfileStrings.Equals(""))
                                {
                                    percentage += 4.545;
                                }
                                else
                                {
                                    if (ProfileStrings.Contains(valgtProfil.self.job))
                                    {
                                        percentage += 4.545;
                                    }

                                }

                                if (profile.preferences.kids == valgtProfil.self.kids)
                                {
                                    percentage += 4.545;
                                }

                                if (profile.preferences.lookingFor == valgtProfil.self.lookingFor)
                                {
                                    percentage += 4.545;
                                }
                                if (profile.self.kids == valgtProfil.preferences.kids)
                                {
                                    percentage += 4.545;
                                }

                                if (profile.self.lookingFor == valgtProfil.preferences.lookingFor)
                                {
                                    percentage += 4.545;
                                }

                            }

                        }

                        if (percentage > 15)
                        { 
                            Match match = new Match();
                            match.usernames.Add(valgtProfil.username);
                            match.usernames.Add(profile.username);
                            match.percentage = percentage;
                            network.CreateMatch(match);
                        }
                    }
                }
                profiles.Remove(valgtProfil); 
            }
        }

    }
}