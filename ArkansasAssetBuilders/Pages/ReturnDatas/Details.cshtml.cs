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
    public class DetailsModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public DetailsModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }

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
    }
}
