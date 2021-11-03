using System;
namespace ArkansasAssetBuilders.Models
{
    public class ReturnData
    {
        public int ID { get; set; }
        public int TaxYearID { get; set; }
        public bool FederalReturn { get; set; }
        public int TotalRefund { get; set; }
        public int EITC { get; set; }
        public int CTC { get; set; }
        public int Dependents { get; set; }
        public int SurveyScore { get; set; }
    }
}
