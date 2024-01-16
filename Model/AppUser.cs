using Microsoft.AspNetCore.Identity;

namespace PraktikaBeTemplate.Model
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}
