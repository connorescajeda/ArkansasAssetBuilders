using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;

namespace ArkansasAssetBuilders.Pages.TaxYears
{
    public class DeleteModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public DeleteModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaxYear TaxYear { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaxYear = await _context.TaxYear.FirstOrDefaultAsync(m => m.ID == id);

            if (TaxYear == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaxYear = await _context.TaxYear.FindAsync(id);

            if (TaxYear != null)
            {
                _context.TaxYear.Remove(TaxYear);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
