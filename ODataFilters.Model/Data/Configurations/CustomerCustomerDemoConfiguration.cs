// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using ODataFilters.Model.Data;
using ODataFilters.Model.Models;
using System;


namespace ODataFilters.Model.Data.Configurations
{
    public partial class CustomerCustomerDemoConfiguration : IEntityTypeConfiguration<CustomerCustomerDemo>
    {
        public void Configure(EntityTypeBuilder<CustomerCustomerDemo> entity)
        {
            entity.HasKey(e => new { e.CustomerId, e.CustomerTypeId })
                .IsClustered(false);

            entity.ToTable("CustomerCustomerDemo");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .HasColumnName("CustomerID")
                .IsFixedLength(true);

            entity.Property(e => e.CustomerTypeId)
                .HasMaxLength(10)
                .HasColumnName("CustomerTypeID")
                .IsFixedLength(true);

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.CustomerCustomerDemos)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerCustomerDemo_Customers");

            entity.HasOne(d => d.CustomerType)
                .WithMany(p => p.CustomerCustomerDemos)
                .HasForeignKey(d => d.CustomerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerCustomerDemo");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CustomerCustomerDemo> entity);
    }
}
