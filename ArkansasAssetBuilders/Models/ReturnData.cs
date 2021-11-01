using System;
namespace ArkansasAssetBuilders.Models
{
    public class ReturnData
    {
        public int ClientID { get; set; }
        public int TaxYear { get; set; }
        public bool FederalReturn { get; set; }
        public int TotalRefund { get; set; }
        public int EITC { get; set; }
        public int CTC { get; set; }
        public int Dependents { get; set; }
        public int SurveyScore { get; set; }
    }
}
