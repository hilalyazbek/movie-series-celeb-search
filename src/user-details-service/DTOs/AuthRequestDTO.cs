using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace user_details_service.DTOs;

public class AuthRequestDTO
{
    [DefaultValue("user@domain.com")]
    [Required(ErrorMessage = "{0} is required")]
    [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "{0} is not a valid GUID format")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(255, MinimumLength = 6)]
    [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-z\\d][A-Za-z\\d!@#$%^&*()_+]{7,19}$", ErrorMessage = "{0} should contain at least 1 letter, 1 digit, and 1 special characters")]
    public string Password { get; set; } = null!;
}

