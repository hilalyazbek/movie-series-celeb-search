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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

