using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace user_details_service.DTOs;

public class CreateUserDTO
{
    [DefaultValue("12312312312")]
    [DataType(DataType.PhoneNumber)]
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(11, ErrorMessage = "QID should be 11 digits", MinimumLength = 11)]
    public string? QidNumber { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    public string? LastName { get; set; }

    [DefaultValue("user@domain.com")]
    [Required(ErrorMessage = "{0} is required")]
    [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "{0} is not a valid email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    public string? Username { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(255, MinimumLength = 6)]
    [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-z\\d][A-Za-z\\d!@#$%^&*()_+]{7,19}$", ErrorMessage = "{0} length should be atleast 6, and should contain at least 1 letter, 1 digit, and 1 special characters")]
    public string? Password { get; set; }
}

