using ArkansasAssetBuilders.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace ArkansasAssetBuilders.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string fullPath = "/UploadedCSVs/";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public FileUpload fileUpload { get; set; }
        public void OnGet()
        {
            ViewData["SuccessMessage"] = "";
            ViewData["Data"] = "";
        }
        public IActionResult OnPostUpload(FileUpload fileUpload)
        {
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            foreach (var file in fileUpload.FormFiles)
            {
                using (var sreader = new StreamReader(file.OpenReadStream()))
                {
                    string[] headers = sreader.ReadLine().Split(',');
                    while (!sreader.EndOfStream)
                    {
                        string[] rows = sreader.ReadLine().Split(',');
                    }
                }
            }
            // Process uploaded files
            ViewData["SuccessMessage"] = fileUpload.FormFiles.Count.ToString() + " file(s) uploaded!";
            return Page();
        }
        public class FileUpload
        {
            [Required]
            [Display(Name = "File")]
            public List<IFormFile> FormFiles { get; set; } // convert to list
            public string SuccessMessage { get; set; }
            public string Data { get; set; }
        }
    }
}