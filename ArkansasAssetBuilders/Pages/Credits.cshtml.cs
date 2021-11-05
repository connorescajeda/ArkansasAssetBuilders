using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ArkansasAssetBuilders.Pages
{
    public class CreditsModel : PageModel
    {
        private readonly ILogger<CreditsModel> _logger;

        public CreditsModel(ILogger<CreditsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
