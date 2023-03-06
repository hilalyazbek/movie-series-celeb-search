using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using application_infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace application_infrastructure.DBContexts;

public class ApplicationDbContext : IdentityUserContext<IdentityUser>
{
    public DbSet<User> Users { get; set; }
    public DbSet<WatchLater> WatchLaters { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<SearchHistory> SearchHistory{ get; set; }

    public ApplicationDbContext() : base()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        SeedData(builder);

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder is null)
        {
            return;
        }

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
            "Database=user-details-db; Host=dbcreditsuisse.postgres.database.azure.com; User Id=postgres@dbcreditsuisse; Password=P@ssw0rd");
        }
    }

    public void SeedData(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            return;
        }
        //modelBuilder.Entity<User>().HasNoKey();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "4146a38c-9f4e-4cf9-acff-175d745f0e54",
                QidNumber = "432432432",
                FirstName = "Bruce",
                LastName = "Wayne",
                Email = "batman@gotham.com",
                UserName = "batman",
                PasswordHash = "0b58fb7e73e1402d43e6263a58e0d3db7f237935"
            });

        modelBuilder.Entity<WatchLater>().HasData(
            new WatchLater
            {
                Id = Guid.NewGuid(),
                ProgramId = 1000,
                ProgramName = "Avatar",
                UserId = "4146a38c-9f4e-4cf9-acff-175d745f0e54",
            });
    }
}

