using System;
namespace ArkansasAssetBuilders.Models
{
    public class Demographic
    {
        public int ID { get; set; }
        public string TaxYearID { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string State { get; set; }
    }

}
