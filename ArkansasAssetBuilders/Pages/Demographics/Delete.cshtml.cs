using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;

namespace ArkansasAssetBuilders.Pages.Demographics
{
    public class DeleteModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public DeleteModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Demographic Demographic { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Demographic = await _context.Demographic.FirstOrDefaultAsync(m => m.ID == id);

            if (Demographic == null)
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

            Demographic = await _context.Demographic.FindAsync(id);

            if (Demographic != null)
            {
                _context.Demographic.Remove(Demographic);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
