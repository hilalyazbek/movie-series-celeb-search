using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using user_details_service.DTOs;
using user_details_service.Entities;

namespace user_details_service.Infrastructure.DBContexts;

public class UsersContext :IdentityUserContext<IdentityUser>
{
    public DbSet<User> Users { get; set; }

    public UsersContext(DbContextOptions<UsersContext> options)
		:base(options)
	{

	}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

