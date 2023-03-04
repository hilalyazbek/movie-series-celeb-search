using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.Entities;

public class SearchHistory : BaseEntity
{
    public String? Query { get; set; }

    public List<string>? Results { get; set; }
}