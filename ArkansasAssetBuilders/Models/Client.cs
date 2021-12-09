using System;
using System.ComponentModel.DataAnnotations;
namespace ArkansasAssetBuilders.Models
{
    public class Client
    {
        public int? ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DoB { get; set; }
        public int? Last4SS { get; set; }

    }

}
