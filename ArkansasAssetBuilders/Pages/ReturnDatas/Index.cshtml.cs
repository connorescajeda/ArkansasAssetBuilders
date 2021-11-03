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
    public class IndexModel : PageModel
    {
        private readonly ArkansasAssetBuilders.Data.ClientContext _context;

        public IndexModel(ArkansasAssetBuilders.Data.ClientContext context)
        {
            _context = context;
        }

        public IList<ReturnData> ReturnData { get;set; }

        public async Task OnGetAsync()
        {
            ReturnData = await _context.ReturnData.ToListAsync();
        }
    }
}
