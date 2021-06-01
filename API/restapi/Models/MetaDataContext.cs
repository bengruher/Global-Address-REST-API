using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using restapi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace restapi.Models 
{
    public class MetaDataContext : DbContext
    {
        public MetaDataContext(DbContextOptions<MetaDataContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Fields>().HasKey(table => new {
                table.CountryName, table.FieldName
            });
            // Database.SetInitializer(new MyDbContextInitializer());
        }

        public DbSet<Countries> Countries { get; set; }
        public DbSet<Fields> Fields { get; set; }
    }

    /*
    public class MyDbContextInitializer : DropCreateDatabaseAlways<MetaDataContext>
    {
        protected override void Seed(MetaDataContext dbContext)
        {
            // seed data
            base.Seed(dbContext);
        }
    }
    */
}