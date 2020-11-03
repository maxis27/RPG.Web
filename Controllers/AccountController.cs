using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RPG.Web.Models;
using RPG.Web.ViewModels;

namespace RPG.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webhostEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                IWebHostEnvironment webhostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webhostEnvironment = webhostEnvironment;
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserNameInUse(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Nazwa użytkownika '{userName}' jest już zajęta.");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if(result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else 
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Nieprawidłowa próba logowania.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if(!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangeAvatar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar(ChangeAvatarViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            string uploadsFolder = Path.Combine(_webhostEnvironment.WebRootPath, "images/avatars");

            if(model.Photo != null)
            {
                List<string> allowExt = new List<string>
                {
                    ".png", ".jpg"
                };

                string fileExt = Path.GetExtension(model.Photo.FileName);

                if(!allowExt.Any(x => x.Contains(fileExt)))
                {
                    ModelState.AddModelError(string.Empty, "Niedozwolony format pliku.");
                    return View(model);
                }

                string uniqueFileName = "u_" + user.Id + fileExt;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if(user.Avatar != string.Empty)
                {
                    string existFilePath = Path.Combine(uploadsFolder, user.Avatar);

                    if(filePath != existFilePath)
                    {
                        if(System.IO.File.Exists(existFilePath))
                        {
                            System.IO.File.Delete(existFilePath);
                        }
                    }
                }

                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                
                user.Avatar = uniqueFileName;
                await _userManager.UpdateAsync(user);
            } 
            else 
            {
                string existFilePath = Path.Combine(uploadsFolder, user.Avatar);

                if(System.IO.File.Exists(existFilePath))
                {
                    System.IO.File.Delete(existFilePath);
                }

                user.Avatar = string.Empty;
                await _userManager.UpdateAsync(user);
            }

            return View();
        }
    }
}