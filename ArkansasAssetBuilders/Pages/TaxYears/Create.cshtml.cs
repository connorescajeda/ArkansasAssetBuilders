using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;

namespace ArkansasAssetBuilders.Pages.TaxYears
{
    public class CreateModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public CreateModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TaxYear TaxYear { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TaxYear.Add(TaxYear);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
