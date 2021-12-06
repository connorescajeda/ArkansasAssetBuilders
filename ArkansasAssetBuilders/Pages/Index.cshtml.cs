﻿using ArkansasAssetBuilders.Data;
using ArkansasAssetBuilders.Models;
using ArkansasAssetBuilders.Pages.Clients;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ArkansasAssetBuilders.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ClientContext _context;
        public IndexModel(ClientContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FileUpload fileUpload { get; set; }

        //create new lists for all class data types to add to database
        public List<Client> clients = new List<Client>();
        public List<Demographic> demographics = new List<Demographic>();
        public List<ReturnData> returnDatas = new List<ReturnData>();

        public void OnGet()
        {
            ViewData["SuccessMessage"] = "Upload necessary files (MAX OF 4 FILES)";
        }
        public ActionResult OnPostUpload(FileUpload fileUpload)
        {
            //counter
            int i = 1;

            //type of Record
            RecordType type = RecordType.None;

            //for each file in the form files
            foreach (var file in fileUpload.FormFiles)
            {
                //display filenames
                ViewData[i.ToString()] = file.FileName;
                i++;

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    IgnoreBlankLines = true,
                    UseNewObjectForNullReferenceMembers = false
                };

                //use streamReader and csvHelper to pull records from file
                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                using var sreader = new StreamReader(file.OpenReadStream());
                using (var csv = new CsvReader(sreader, config))
                {
                    string[] headerRow = csv.HeaderRecord;

                    //WRITE A NEW COLUMN TO THE CSV FILE FOR THE CLIENT ID PRIMARY KEY

                    //add all context mappings to interpret mutliple different header names
                    csv.Context.RegisterClassMap<ClientMap>();
                    csv.Context.RegisterClassMap<DemoMap>();
                    csv.Context.RegisterClassMap<DataMap>();

                    //grab records and add them to class lists
                    while (csv.Read())
                    {
                        clients.Add(csv.GetRecord<Client>());
                        //demographics.Add(csv.GetRecord<Demographic>());
                        //returnDatas.Add(csv.GetRecord<ReturnData>());


                        //Use multiple mappings to get data into different parts of database
                        /*if (csv.GetField(0) == "ID" || csv.GetField(0) == "FirstName"
                            || csv.GetField(0) == "LastName" || csv.GetField(0) == "DoB"
                            || csv.GetField(0) == "Last4SS")
                        {
                            csv.ReadHeader();
                            type = RecordType.ClientType;
                            continue;
                        }

                        if (csv.GetField(0) == "Address" || csv.GetField(0) == "Zip"
                         || csv.GetField(0) == "County" || csv.GetField(0) == "State"
                         || csv.GetField(0) == "ID" || csv.GetField(0) == "TaxYear")
                        {
                            csv.ReadHeader();
                            type = RecordType.DemographicType;
                            continue;
                        }

                        if (csv.GetField(0) == "FederalReturn" || csv.GetField(0) == "TotalRefund"
                         || csv.GetField(0) == "EITC" || csv.GetField(0) == "CTC"
                         || csv.GetField(0) == "Dependents" || csv.GetField(0) == "SurveyScore"
                         || csv.GetField(0) == "ID" || csv.GetField(0) == "TaxYear")
                        {
                            csv.ReadHeader();
                            type = RecordType.ReturnDataType;
                            continue;
                        }*/

                    }
                }
            }

            foreach (var client in clients)
            {
                _context.Client.Add(client);
                _context.SaveChanges();
            }

            foreach (var demographic in demographics)
            {
                _context.Demographic.Add(demographic);
                _context.SaveChanges();
            }

            foreach (var returnData in returnDatas)
            {
                _context.ReturnData.Add(returnData);
                _context.SaveChanges();
            }

            //Save database changes
            _context.SaveChanges();

            //Process uploaded files
            ViewData["SuccessMessage"] = fileUpload.FormFiles.Count.ToString() + " file(s) uploaded!";
            var DropDownAndCheckBoxCount = i;
            return Page();
        }
        public class FileUpload
        {
            [Required]
            [Display(Name = "File")]
            public List<IFormFile> FormFiles { get; set; } // convert to list
            public string SuccessMessage { get; set; }
        }
    }


    //Client data mapping
        public sealed class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            Map(m => m.ID).Name("ID", "Id");
            Map(m => m.FirstName).Name("FirstName", "First Name");
            Map(m => m.LastName).Name("LastName", "Last Name");
            Map(m => m.DoB).Name("DateOfBirth", "DoB", "Date of Birth");
            Map(m => m.Last4SS).Name("Last 4", "XXX-XX-1234", "Last Four", "Last 4 SS");
        }
    }

    //enum to reference the different mappings for the csv
    public enum RecordType
    {
        None = 0,
        ClientType,
        DemographicType,
        ReturnDataType,
        TaxYearType
    }

    //Demographic data mapping
    public sealed class DemoMap : ClassMap<Demographic>
    {
        public DemoMap()
        {
            Map(m => m.ID).Name("ID", "Id");
            Map(m => m.TaxYear).Name("Tax Year", "TaxYear");
            Map(m => m.Address).Name("Street Address", "Address");
            Map(m => m.Zip).Name("ZIP", "Zip", "zip", "Postal Code");
            Map(m => m.County).Name("County", "Location");
            Map(m => m.State).Name("State", "ST");
        }
    }

    //ReturnData data mapping
    public sealed class DataMap : ClassMap<ReturnData>
    {
        public DataMap()
        {
            Map(m => m.ID).Name("ID", "Id");
            Map(m => m.TaxYear).Name("TaxYear", "Tax Year");
            Map(m => m.FederalReturn).Name("FedReturn", "Federal");
            Map(m => m.TotalRefund).Name("Total Refund", "TotalRefund");
            Map(m => m.EITC).Name("EITC");
            Map(m => m.CTC).Name("CTC");
            Map(m => m.Dependents).Name("dependents");
            Map(m => m.SurveyScore).Name("Questions");
        }
    }

}