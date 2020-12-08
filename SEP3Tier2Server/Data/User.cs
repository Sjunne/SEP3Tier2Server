namespace MainServerAPI.Data
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
       
        public string email { get; set; }
        public string newPassword { get; set; }


        public override string ToString()
        {
            return username + " " + password;
        }
    }
}