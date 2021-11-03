using System;
namespace ArkansasAssetBuilders.Models
{
    public class Client
    { 
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }
        public int Last4SS { get; set; }
    }
}
