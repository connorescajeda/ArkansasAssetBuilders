using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;

namespace ArkansasAssetBuilders.Pages.ReturnDatas
{
    public class DeleteModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public DeleteModel(ArkansasAssetBuilders.Data.ClientContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReturnData = await _context.ReturnData.FindAsync(id);

            if (ReturnData != null)
            {
                _context.ReturnData.Remove(ReturnData);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
