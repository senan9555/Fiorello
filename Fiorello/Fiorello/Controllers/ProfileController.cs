﻿using Fiorello.Models;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                                                                   RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            else
            {
                AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if(appUser == null)
                {
                    return BadRequest();
                }
                ProfilVM profilVM = new ProfilVM
                {
                    Email=appUser.Email,
                    Fullname=appUser.FullName,
                    Username=appUser.UserName,
                    Image=appUser.Image,

                };
                return View(profilVM);
            }

        }
    }
}
