using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)//Server Side Validation
            {
                var User = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],// The Name Before @
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree
                };
                var Result = await userManager.CreateAsync(User, model.Password);

                if (Result.Succeeded)

                    return RedirectToAction(nameof(Login));
                foreach (var error in Result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(model);
        }

        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)//Server Side Validation
			{
                var User = await userManager.FindByEmailAsync(model.Email);
				
				if (User is not null)
                {
                    var flag = await userManager.CheckPasswordAsync(User,model.Password);
                    if (flag)
                    {
                        var Result = await signInManager.PasswordSignInAsync(User,model.Password,model.RememberMe,false);
                        if (Result.Succeeded) return RedirectToAction("Index","Home");
                    }
					ModelState.AddModelError(string.Empty, "Password Is Invalid");

				}
				ModelState.AddModelError(string.Empty, "Email Is Invalid");

			}
			return View(model);
		}

        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
		#endregion
	}
}
