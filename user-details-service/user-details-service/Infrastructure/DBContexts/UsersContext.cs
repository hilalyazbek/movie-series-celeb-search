using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace user_details_service.Infrastructure.DBContexts;

public class UsersContext :IdentityUserContext<IdentityUser>
{
	public UsersContext(DbContextOptions<UsersContext> options)
		:base(options)
	{

	}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=user-details-service-db;Username=postgres;Password=P@ssw0rd");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

