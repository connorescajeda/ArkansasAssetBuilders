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

        public DbSet<Client> Client { get; set; }

        public DbSet<Demographic> Demographic { get; set; }

        public DbSet<ReturnData> ReturnData { get; set; }

        public DbSet<TaxYear> TaxYear { get; set; }
    }
}
