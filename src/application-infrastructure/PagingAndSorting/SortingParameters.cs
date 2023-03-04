using System;
using Microsoft.AspNetCore.Identity;

namespace application_infrastructure.PagingAndSorting;

public class SortingParameters
{
    public string SortBy { get; set; } = "";
}