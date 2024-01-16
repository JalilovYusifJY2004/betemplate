using System.ComponentModel.DataAnnotations;

namespace PraktikaBeTemplate.Areas.ViewModels;

public class RegisterVM
{
    [Required]
    [MaxLength (25,ErrorMessage ="max 25" )]
    public string Name { get; set; }
    [Required]
    [MaxLength(25, ErrorMessage = "max 25")]
    public string SurName { get; set; }
    [Required]
    [MaxLength(25, ErrorMessage = "max 25")]
    public string UserName {  get; set; }
    [Required]
    [MaxLength(25, ErrorMessage = "max 25")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}
