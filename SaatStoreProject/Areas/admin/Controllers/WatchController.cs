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
    public class WatchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public WatchController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Watch> watches = _context.Watches.Include(w => w.WatchImages).Include(w=>w.Brand).Include(w => w.WatchCategory).ToList();
            return View(watches);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Include(c => c.WatchCategory).ThenInclude(wc => wc.Watch).ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Watch watch)
        {
            ViewBag.Categories = _context.Categories.Include(c => c.WatchCategory).ThenInclude(wc => wc.Watch).ToList();
            ViewBag.Brands = _context.Brands.ToList();
            if (!ModelState.IsValid) return View();
            if (watch.BrandId == 0)
            {
                watch.BrandId = 1;
            }
            //watch = _context.Watches.Include(w=>w.WatchImages).Include(w => w.WatchCategory).ThenInclude(wc=>wc.Category).FirstOrDefault();
            watch.WatchCategory = new List<WatchCategory>();
            watch.WatchImages = new List<WatchImage>();
            
            foreach (var id in watch.CategoryIds)
            {
                WatchCategory wCategory = new WatchCategory
                {
                    Watch = watch,
                    CategoryId = id
                };
                watch.WatchCategory.Add(wCategory);
            }

            if (watch.ImageFiles.Count > 3)
            {
                ModelState.AddModelError("ImageFiles", "You can choose only 3 images");
                return View();
            }
            foreach (var image in watch.ImageFiles)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("ImageFiles", "Please choose image file");
                    return View();
                }
                if (!image.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFiles", "Image size must be max 2MB");
                    return View();
                }

            }
            foreach (var image in watch.ImageFiles)
            {
                WatchImage watchImage = new WatchImage
                {
                    Image = image.SaveImg(_env.WebRootPath, "assets/images/watch"),
                    isMain = watch.WatchImages.Count < 1 ? true : false,
                    Watch = watch
                };
                watch.WatchImages.Add(watchImage);
            }
            
            _context.Watches.Add(watch);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Edit(int id)
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();

            Watch watch = _context.Watches.Include(w => w.WatchCategory).Include(w => w.WatchImages).FirstOrDefault(f => f.Id == id);
            if (watch == null) return NotFound();
            return View(watch);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Watch watch)
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Watch existedWatch = _context.Watches.Include(w => w.WatchImages).Include(w => w.WatchCategory).Include(w => w.Brand).FirstOrDefault(w => w.Id == watch.Id);


            if (!ModelState.IsValid) return View(existedWatch);

            if (existedWatch == null) return NotFound();

            if (watch.ImageFiles != null)
            {
                foreach (var image in watch.ImageFiles)
                {
                    if (watch.ImageFiles.Count > 3)
                    {
                        ModelState.AddModelError("ImageFiles", "You can choose only 3 images");
                        return View();
                    }
                    if (!image.IsImage())
                    {
                        ModelState.AddModelError("ImageFiles", "Please select the image file");
                        return View(existedWatch);
                    }
                    if (!image.IsSizeOkay(2))
                    {
                        ModelState.AddModelError("ImageFiles", "You can choose file which size is max 2MB");
                        return View(existedWatch);
                    }
                }

                List<WatchImage> removableImagess = existedWatch.WatchImages.Where(wi => wi.isMain == false && !watch.ImageIds.Contains(wi.Id)).ToList();

                existedWatch.WatchImages.RemoveAll(wi => removableImagess.Any(ri => ri.Id == wi.Id));

                foreach (var item in removableImagess)
                {
                    Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images/watch", item.Image);
                    
                }

                foreach (var image in watch.ImageFiles)
                {
                    WatchImage watchImage = new WatchImage
                    {
                        Image = image.SaveImg(_env.WebRootPath, "assets/images/watch"),
                        isMain = false,
                        WatchId = existedWatch.Id
                    };
                    existedWatch.WatchImages.Add(watchImage);
                }                
            }


            List<WatchImage> removableImages = existedWatch.WatchImages.Where(wi => wi.isMain == false && !watch.ImageIds.Contains(wi.Id)).ToList();

            existedWatch.WatchImages.RemoveAll(wi => removableImages.Any(ri => ri.Id == wi.Id));

            foreach (var item in removableImages)
            {
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images/watch", item.Image);
            }
           

            List<WatchCategory> removableCategories = existedWatch.WatchCategory.Where(wc => !watch.CategoryIds.Contains(wc.Id)).ToList();

            existedWatch.WatchCategory.RemoveAll(fc => removableCategories.Any(rc => fc.Id == rc.Id));
            foreach (var categoryId in watch.CategoryIds)
            {
                WatchCategory watchCategory = existedWatch.WatchCategory.FirstOrDefault(wc => wc.CategoryId == categoryId);
                if (watchCategory == null)
                {
                    WatchCategory wCategory = new WatchCategory
                    {
                        CategoryId = categoryId,
                        WatchId = existedWatch.Id
                    };
                    existedWatch.WatchCategory.Add(wCategory);
                }
            }
            existedWatch.Price = watch.Price;
            existedWatch.CaseThickness = watch.CaseThickness;
            existedWatch.WaterProtection = watch.WaterProtection;
            existedWatch.Glass = watch.Glass;
            existedWatch.Mechanism = watch.Mechanism;
            existedWatch.WatchModel = watch.WatchModel;
            existedWatch.InStock = watch.InStock;
            if (watch.BrandId == 0)
            {
                watch.BrandId = 1;
            }
            existedWatch.BrandId = watch.BrandId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Watch watch = _context.Watches.FirstOrDefault(w => w.Id == id);
            if (watch == null) return Json(new { status = 404 });

            _context.Watches.Remove(watch);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
