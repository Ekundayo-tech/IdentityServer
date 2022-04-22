using MabelApi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Data
{ 
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        } 
         


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            var keysProperties = builder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
 
        }
    }
}
