using System.ComponentModel.DataAnnotations;

namespace PraktikaBeTemplate.Areas.ViewModels;

public class CreateTeamVM
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public IFormFile Photo { get; set; }
}
