using System;
using Microsoft.AspNetCore.Identity;

namespace user_details_service.Helpers.PagingAndSorting;

public class SortingParameters
{
    public string SortBy { get; set; } = "";
}