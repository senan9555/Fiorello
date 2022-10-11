using Fiorello.DAL;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Categories= await _db.Categories.Where(x=>!x.IsDeactive).ToListAsync(),
                Products= await _db.Products.Where(x => !x.IsDeactive).ToListAsync()
            };
            return View(homeVM);
        }
        public IActionResult MySearch()
        {
            return View();
        }
        public async Task<IActionResult> MySearch(string key)
        {
            List<Product> products = await _db.Products.Where(x => x.Name.Contains(key)).ToListAsync();
            return PartialView("_ProductSearchPartial",products);
        }

        #region Subscribe

        public async Task<IActionResult> Subscribe(string email)
        {
            if(!User.Identity.IsAuthenticated)
            {
                if (!Helper.IsValidEmail(email))
                {
                    return Content("Bos ola bilmez");
                }
               
                await _db.Subscribes.AddAsync(new Subscribe { Email = email });
                await _db.SaveChangesAsync();
            }
            else
            {
                
                AppUser appuser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                bool isExist = await _db.Subscribes.AnyAsync(x => x.Email == appuser.Email);
                if (isExist)
                {
                    return Content("bu Email artiq istifade olunub");
                }
                await _db.Subscribes.AddAsync(new Subscribe { Email = appuser.Email });
                await _db.SaveChangesAsync();
            }
           
            return Content("Abune olundu");
        }
       
        #endregion
    }
}
