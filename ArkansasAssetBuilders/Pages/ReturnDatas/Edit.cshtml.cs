using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;

namespace ArkansasAssetBuilders.Pages.ReturnDatas
{
    public class EditModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public EditModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ReturnData ReturnData { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReturnData = await _context.ReturnData.FirstOrDefaultAsync(m => m.ID == id);

            if (ReturnData == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ReturnData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReturnDataExists(ReturnData.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReturnDataExists(int id)
        {
            return _context.ReturnData.Any(e => e.ID == id);
        }
    }
}
