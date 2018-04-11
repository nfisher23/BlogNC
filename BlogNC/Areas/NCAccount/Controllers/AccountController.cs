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
            throw new NotImplementedException();
        }
    }
}
