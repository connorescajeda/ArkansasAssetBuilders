using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArkansasAssetBuilders.Models;

namespace ArkansasAssetBuilders.Data
{
    public class ClientContext : DbContext
    {
        public ClientContext (DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        public DbSet<ArkansasAssetBuilders.Models.Client> Client { get; set; }

        public DbSet<ArkansasAssetBuilders.Models.Demographic> Demographic { get; set; }

        public DbSet<ArkansasAssetBuilders.Models.ReturnData> ReturnData { get; set; }

        public DbSet<ArkansasAssetBuilders.Models.TaxYear> TaxYear { get; set; }
    }
}
