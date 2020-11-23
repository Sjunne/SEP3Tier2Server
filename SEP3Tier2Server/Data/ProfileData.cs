using System.Runtime.Serialization;

namespace MainServerAPI.Data
{
    public class ProfileData
    {
        public string intro { get; set; }
        public string username { get; set; }
        
        public string instagram { get; set; }
        public string idealdate { get; set; }
        public string interests { get; set; }
        public string spotify { get; set; }
        public int age { get; set; }
    }
}