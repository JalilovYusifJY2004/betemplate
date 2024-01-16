using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PraktikaBeTemplate.Areas.ViewModels;
using PraktikaBeTemplate.Model;

namespace PraktikaBeTemplate.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userIn;
        private readonly SignInManager<AppUser> _signIn;

        public AccountController(UserManager<AppUser> userIn, SignInManager<AppUser> signIn, RoleManager<IdentityRole> roleIn)
        {
            _userIn = userIn;
            _signIn = signIn;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            userVM.Name = userVM.Name.Trim();
            userVM.SurName = userVM.SurName.Trim();
            string name = Char.ToUpper(userVM.Name[0]) + userVM.Name.Substring(1).ToLower();
            string surname = Char.ToUpper(userVM.SurName[0]) + userVM.SurName.Substring(1).ToLower();
            AppUser user = new()
            {
                Name = name,
                SurName = surname,
                Email = userVM.Email,
                UserName = userVM.Name,
            };
            IdentityResult result = await _userIn.CreateAsync(user, userVM.Password);
            if (result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(userVM);
                }
            }
            await _signIn.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");

        }
    }
}
