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

namespace ArkansasAssetBuilders.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ClientContext _context;

        public IndexModel(ClientContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string AgeSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public IList<Client> Client { get;set; }

        public async Task OnGetAsync(string sortOrder)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            AgeSort = sortOrder == "Date" ? "date_desc" : "Date";

            IQueryable<Client> studentsIQ = from s in _context.Client
                                             select s;

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Age":
                    studentsIQ = studentsIQ.OrderBy(s => s.DoB);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.DoB);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }
            Client = await _context.Client.ToListAsync();
        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {

            var myCs = await _context.Client.ToListAsync();
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

            string excelName = $"Clients-{DateTime.Now:yyyyMMdd}.xlsx";
            // define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

    }
}
