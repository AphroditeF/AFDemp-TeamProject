using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AspNetExtendingIdentityRoles.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Add an instance IDbSet using the 'new' keyword:
        new public virtual IDbSet<ApplicationRole> Roles { get; set; }
        public virtual IDbSet<Message> Messages { get; set; }

        //public System.Data.Entity.DbSet<Manager.Models.ApplicationUser> IdentityUsers { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            modelBuilder.Entity<Message>()
                .HasRequired(i => i.Receiver)
                .WithMany(i => i.RececivedMessages)
                .HasForeignKey(i => i.ReceiverID)
                .WillCascadeOnDelete(false);

          modelBuilder.Entity<Message>()
            .HasRequired(i => i.Sender)
            .WithMany(i => i.SentMessages)
            .HasForeignKey(i => i.SenderID)
            .WillCascadeOnDelete(false);

            // Keep this:
            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

            // Change TUser to ApplicationUser everywhere else - IdentityUser and ApplicationUser essentially 'share' the AspNetUsers Table in the database:
            EntityTypeConfiguration<ApplicationUser> table = 
                modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            table.Property((ApplicationUser u) => u.UserName).IsRequired();

            // EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
            modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) => 
                new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            // Leave this alone:
            EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration = 
                modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) => 
                    new { UserId = l.UserId, LoginProvider = l.LoginProvider, ProviderKey = l.ProviderKey }).ToTable("AspNetUserLogins");

            entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);
            EntityTypeConfiguration<IdentityUserClaim> table1 = modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");
            table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.User);

            // Add this, so that IdentityRole can share a table with ApplicationRole:
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            // Change these from IdentityRole to ApplicationRole:
            EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 = modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();
        }

        


        // public System.Data.Entity.DbSet<AspNetExtendingIdentityRoles.Models.ApplicationUser> IdentityUsers { get; set; }
    }
}