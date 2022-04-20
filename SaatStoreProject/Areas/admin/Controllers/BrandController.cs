using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaatStoreProject.DAL;
using SaatStoreProject.Extensions;
using SaatStoreProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace SaatStoreProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Brand> brands = _context.Brands.Include(b=>b.Watch).ToList();
            return View(brands);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            if (brand.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Shekil daxil edin");
                return View();
            }
            if (!brand.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Shekilin olcusu maximum 2MB ola biler");
                return View();
            }
            if (!brand.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Image file sec");
                return View();
            }
            brand.Image = brand.ImageFile.SaveImg(_env.WebRootPath, "assets/images/brand");
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null) return NotFound();

            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Brand brand)
        {
            Brand existedBrand = _context.Brands.FirstOrDefault(b => b.Id == brand.Id);

            if (!ModelState.IsValid) return View(existedBrand);

            if (existedBrand == null) return NotFound();

            if (existedBrand.ImageFile != null)
            {
                if (!brand.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFiles", "Please select the image file");
                    return View(existedBrand);
                }
                if (!brand.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFiles", "You can choose file which size is max 2MB");
                    return View(existedBrand);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images/brand", existedBrand.Image);
                existedBrand.Image = brand.ImageFile.SaveImg(_env.WebRootPath, "assets/images/brand");
            }

            existedBrand.Name = brand.Name;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null) return Json(new { status = 404 });

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
