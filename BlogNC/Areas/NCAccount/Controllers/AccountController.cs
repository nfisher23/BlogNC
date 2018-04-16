using BlogNC.Areas.Blog.Models;
using BlogNC.Areas.NCAccount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAccount.Controllers
{
    [Area("NCAccount")]
    [Authorize]
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> usrMgr,
            SignInManager<AppUser> signInMgr)
        {
            _userManager = usrMgr;
            _signInManager = signInMgr;
        }

        [AllowAnonymous]
        public ViewResult Login(string ReturnUrl)
        {
            return View(new AccountLoginModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user,
                        model.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (SeedData.IsFirstSignIn(model.Name, model.Password))
                        {
                            return RedirectToAction(nameof(CreateAccount),
                                new { returnUrl = model.ReturnUrl });
                        }
                        else
                            return Redirect(model.ReturnUrl ?? "/");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Name or Password");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                await _signInManager.SignOutAsync();
                AppUser newUser = new AppUser
                {
                    UserName = model.Name
                };
                var create = await _userManager.CreateAsync(newUser, model.Password);
                
                if (create.Succeeded)
                {
                    var signIn = await _signInManager.PasswordSignInAsync(newUser, model.Password,
                        false, false);

                    if (signIn.Succeeded)
                    {
                        // delete default seeded account
                        AppUser userToDelete = await _userManager.FindByNameAsync(SeedData.DefaultUsername);
                        if (userToDelete != null)
                            await _userManager.DeleteAsync(userToDelete);

                        return Redirect(model.ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You could not be signed in with your new account " +
                            "Please go to our github page and explain the steps that led to this bad outcome and we will " +
                            "work on a solution");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "We could not create your account. " +
                        "Please go to our github page and explain the steps that led to this bad outcome and we will " +
                        "work on a solution");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ViewResult CreateAccount(string returnUrl)
        {
            return View(new CreateAccountModel { ReturnUrl = returnUrl });
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
