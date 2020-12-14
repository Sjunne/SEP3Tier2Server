using System.Dynamic;
using System.Runtime.Serialization;

namespace MainServerAPI.Data
{
    public class ProfileData
    {
        public Details self;
        public Details preferences;
        
        public ProfileData()
        {
            self = new Details();
            preferences = new Details();
            
        }
        
        public string jsonSelf { get; set; }

        public string jsonPref { get; set; }
        
        public string intro { get; set; }
        public string username { get; set; }
        
     
        public string firstName { get; set; }

        public string lastName { get; set; } 

        public string city { get; set; }

        public string education { get; set; }

        public string hobbies { get; set; }

        public int age { get; set; }
        public string instagram { get; set; }
        public string idealdate { get; set; }
        public string interests { get; set; }

        public string spotify { get; set; }
        
        public string toString()
        {
            return "ProfileData{" + "FirstName: " + firstName + " LastName: " + lastName + " Age: " + age + " Education: " + education + " City: " + city + " Hobbies: " +
                   self.hobbies + " HairColor: " + self.hairColor + " EyeColor: " + self.eyeColor + " SkinColor: " + self.skinColor + " Nationality: " +
                   self.nationality + " BodyShape: " + self.bodyShape + " Job: " + self.job + " Gender: " + self.gender + " HaveKids: " + self.kids + " SeeksRelationship: " + self.lookingFor + "}" + "\n"
                   + "Preference{" + "MaximumAge: " + preferences.maximumAge + " MinimumAge: " + preferences.minimumAge + " Education: " + preferences.education + " City: " + preferences.city +
                   " Hobbies: " + preferences.hobbies + " HairColor: " + preferences.hairColor + " EyeColor: " + preferences.eyeColor + " SkinColor: " + preferences.skinColor +
                   " Nationality: " + preferences.nationality + " BodyShape: " + preferences.bodyShape + " Job: " + preferences.job + " Gender: " + preferences.gender + " WantPeopleWhoHaveKids: " + preferences.kids + " WantPeopleWhoWantRelationShip: " + preferences.lookingFor+
                   "}" + "\n";
        }

    }
}