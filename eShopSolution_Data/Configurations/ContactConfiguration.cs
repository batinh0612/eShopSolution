using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);

            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11);

            builder.Property(x => x.Message).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Status).HasDefaultValue(Status.Active);
        }
    }
}
