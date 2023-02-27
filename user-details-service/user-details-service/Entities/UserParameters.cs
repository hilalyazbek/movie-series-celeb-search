using System;
using Microsoft.AspNetCore.Identity;

namespace user_details_service.Entities;

public class UserParameters
{
    const int maxPageSize = 10;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 2;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}

