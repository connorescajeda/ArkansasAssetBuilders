using System;
namespace ArkansasAssetBuilders.Models
{
    public class Demographic
    {
        public int ID { get; set; }
        public int TaxYearID { get; set; }
        public string Address { get; set; }
        public int Zip { get; set; }
        public string County { get; set; }
        public string State { get; set; }
    }
}
