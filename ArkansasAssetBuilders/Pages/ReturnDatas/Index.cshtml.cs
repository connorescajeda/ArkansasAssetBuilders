using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;
using System.IO;
using OfficeOpenXml;

namespace ArkansasAssetBuilders.Pages.ReturnDatas
{
    public class IndexModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public IndexModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }
        
        public string TaxYearIDSort { get; set; }
        public string FederalReturnSort { get; set; } //bool
        public string TotalRefundSort { get; set; }
        public string EITCSort { get; set; }
        public string CTCSort { get; set; }
        public string DependentsSort { get; set; }
        public string SurveyScoreSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IList<ReturnData> ReturnDatas { get;set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            // using System;
            TaxYearIDSort = String.IsNullOrEmpty(sortOrder) ? "year_desc" : "";
            FederalReturnSort = String.IsNullOrEmpty(sortOrder) ? "fed_desc" : "";
            EITCSort = sortOrder == "eitc" ? "eitc_desc" : "eitc";
            CTCSort = String.IsNullOrEmpty(sortOrder) ? "ctc_desc" : "";
            DependentsSort = sortOrder == "dependents" ? "dependents_desc" : "dependents";
            SurveyScoreSort = sortOrder == "survey" ? "survey_desc" : "survey";

            CurrentFilter = searchString;

            IQueryable<ReturnData> returndatasIQ = from s in _context.ReturnData
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                returndatasIQ = returndatasIQ.Where(s => s.TaxYearID.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "year_desc":
                    returndatasIQ = returndatasIQ.OrderByDescending(s => s.TaxYearID);
                    break;
                case "fed_desc":
                    returndatasIQ = returndatasIQ.OrderByDescending(s => s.FederalReturn);
                    break;
                case "eitc_desc":
                    returndatasIQ = returndatasIQ.OrderByDescending(s => s.EITC);
                    break;
                case "eitc":
                    returndatasIQ = returndatasIQ.OrderBy(s => s.EITC);
                    break;
                case "ctc_desc":
                    returndatasIQ = returndatasIQ.OrderByDescending(s => s.CTC);
                    break;
                case "dependents_desc":
                    returndatasIQ = returndatasIQ.OrderByDescending(s => s.Dependents);
                    break;
                case "dependents":
                    returndatasIQ = returndatasIQ.OrderBy(s => s.Dependents);
                    break;
                case "survey_desc":
                    returndatasIQ = returndatasIQ.OrderByDescending(s => s.SurveyScore);
                    break;
                case "survey":
                    returndatasIQ = returndatasIQ.OrderBy(s => s.SurveyScore);
                    break;
                default:
                    returndatasIQ = returndatasIQ.OrderBy(s => s.TaxYearID);
                    break;
            }
            ReturnDatas = await returndatasIQ.AsNoTracking().ToListAsync();

        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {

            var myCs = await _context.ReturnData.ToListAsync();
            // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
            var stream = new MemoryStream();

            //NonCommercial needed for non business use when no lisence was purchased
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(myCs, true);
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"ClientReturnData-{DateTime.Now:yyyyMMdd}.xlsx";
            // define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

    }
}
