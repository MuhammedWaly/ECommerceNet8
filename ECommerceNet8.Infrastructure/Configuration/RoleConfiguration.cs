using ECommerceNet8.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Id = "1372a768-421a-440d-ac9b-3297151b4fd7",
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper()
            },
            new IdentityRole
            {
                Id = "a0ee1d85-8b89-4ce2-8736-8e8476747129",
                Name = Roles.Customer,
                NormalizedName = Roles.Customer.ToUpper()
            });
        }
    }
}
