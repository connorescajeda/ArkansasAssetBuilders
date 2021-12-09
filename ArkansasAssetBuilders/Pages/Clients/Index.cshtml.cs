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

        public IList<Client> Clients { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            NameSort = sortOrder == "name" ? "name_desc" : "name";
            AgeSort = sortOrder == "Age" ? "age_desc" : "Age";
            
            CurrentFilter = searchString;


            IQueryable<Client> clientsIQ = from s in _context.Client
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                clientsIQ = clientsIQ.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper()) || s.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    clientsIQ = clientsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "name":
                    clientsIQ = clientsIQ.OrderBy(s => s.LastName);
                    break;
                case "Age":
                    clientsIQ = clientsIQ.OrderBy(s => s.DoB);
                    break;
                case "age_desc":
                    clientsIQ = clientsIQ.OrderByDescending(s => s.DoB);
                    break;
                default:
                    clientsIQ = clientsIQ.OrderBy(s => s.LastName);
                    break;
            }
            Clients = await clientsIQ.AsNoTracking().ToListAsync();
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
