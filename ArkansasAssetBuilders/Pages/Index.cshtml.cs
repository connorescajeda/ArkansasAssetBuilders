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
        private string fullPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "UploadImages";
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
            foreach (var aformFile in fileUpload.FormFiles)
            {
                var formFile = aformFile;
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(fullPath, formFile.FileName);

                    char[] delimiterChar = { ',' };

                    var dataFromFile = new StringBuilder();
                    using (var reader = new StreamReader(formFile.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            dataFromFile.AppendLine(reader.ReadLine());
                    }

                    ViewData["Data"] = dataFromFile;

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyToAsync(stream);
                        //maybe here read csv and parse by commas, read each line through for loop and add to db using _context.Table.Add(Object)
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
            ViewData["SuccessMessage"] = fileUpload.FormFiles.Count.ToString() + " files uploaded!";
            return Page();
        }
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