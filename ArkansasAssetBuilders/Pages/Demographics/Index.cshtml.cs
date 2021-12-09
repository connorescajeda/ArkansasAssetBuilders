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

namespace ArkansasAssetBuilders.Pages.Demographics
{
    public class IndexModel : PageModel
    {
        private readonly ClientContext _context;

        public IndexModel(ClientContext context)
        {
            _context = context;
        }
        //public string IDSort { get; set; }
        public string TaxYearIDSort { get; set; }
        public string AddressSort { get; set; }
        public string ZipSort { get; set; }
        public string CountySort { get; set; }
        public string StateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IList<Demographic> Demographics { get;set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {      // using System;
            //IDSort = sortOrder == "ID" ? "ID_desc" : "ID";
            TaxYearIDSort = sortOrder == "year" ? "year_desc" : "year";
            ZipSort = sortOrder == "zip" ? "zip_desc" : "zip";
            CountySort = sortOrder == "county" ? "county_desc" : "county";
            StateSort = sortOrder == "state" ? "state_desc" : "state";

            CurrentFilter = searchString;
            IQueryable<Demographic> demographicsIQ = from s in _context.Demographic
                                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                demographicsIQ = demographicsIQ.Where(s => s.TaxYearID.Contains(searchString) 
                || s.County.Contains(searchString) || s.State.Contains(searchString)|| s.Zip.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "zip_desc":
                    demographicsIQ = demographicsIQ.OrderByDescending(s => s.Zip);
                    break;
                case "zip":
                    demographicsIQ = demographicsIQ.OrderBy(s => s.Zip);
                    break;
                case "county_desc":
                    demographicsIQ = demographicsIQ.OrderByDescending(s => s.County);
                    break;
                case "county":
                    demographicsIQ = demographicsIQ.OrderBy(s => s.County);
                    break;
                case "state_desc":
                    demographicsIQ = demographicsIQ.OrderByDescending(s => s.State);
                    break;
                case "state":
                    demographicsIQ = demographicsIQ.OrderBy(s => s.State);
                    break;
                case "year_desc":
                    demographicsIQ = demographicsIQ.OrderByDescending(s => s.TaxYearID);
                    break;
                case "year":
                    demographicsIQ = demographicsIQ.OrderBy(s => s.TaxYearID);
                    break;
                default:
                    demographicsIQ = demographicsIQ.OrderBy(s => s.TaxYearID);
                    break;
            }
            Demographics = await demographicsIQ.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {

            var myCs = await _context.Demographic.ToListAsync();
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

            string excelName = $"ClientDemographics-{DateTime.Now:yyyyMMdd}.xlsx";
            // define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

    }

}
