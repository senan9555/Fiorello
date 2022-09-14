﻿using Fiorello.DAL;
using Fiorello.Helpers;
using Fiorello.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static Fiorello.Helpers.Helper;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products.Include(x=>x.Category).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,int categoryId)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Products.AnyAsync(x => x.Name == product.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Product artiq movcuddur");
                return View();
            }
            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "please select a photo");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "please select a image type");
                return View();
            }
            if (product.Photo.IsOlder1MB())
            {
                ModelState.AddModelError("Photo", "Max 1 MB");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            product.Image = await product.Photo.SaveFileAsync(folder);
            product.CategoryId = categoryId;
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            if(id== null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.Include(x=>x.ProductDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }

            return View(dbProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Update(int? id, Product product, int categoryId)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.Include(x=>x.ProductDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(dbProduct);
            }
            bool isExist = await _db.Products.AnyAsync(x => x.Name == product.Name&& x.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Product artiq movcuddur");
                return View(dbProduct);
            }
            if (product.Photo != null)
            {
                if (!product.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "please select a image type");
                    return View(dbProduct);
                }
                if (product.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Max 1 MB");
                    return View(dbProduct);
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                dbProduct.Image = await product.Photo.SaveFileAsync(folder);
            }
           
            dbProduct.CategoryId = categoryId;
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ProductDetail.Description = product.ProductDetail.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            if (product.IsDeactive)
            {
                product.IsDeactive = false;
            }
            else
            {
                product.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
