using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SaatStoreProject.DAL;
using SaatStoreProject.Extensions;
using SaatStoreProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace SaatStoreProject.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> model = _context.Sliders.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Shekil daxil edin");
                return View();
            }
            if (!slider.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Shekilin olcusu maximum 2MB ola biler");
                return View();
            }
            if (!slider.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Image file sec");
                return View();
            }

            slider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/images/index");
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);

            if (existSlider == null) return NotFound();
            if (!ModelState.IsValid) return View(existSlider);

            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please select image file");
                    return View(existSlider);
                }
                if (!slider.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be max 2MB");
                    return View(existSlider);
                }

                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images/index", existSlider.Image);
                existSlider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/images/index");
            }
            existSlider.Title = slider.Title;
            existSlider.SubTitle = slider.SubTitle;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return Json(new { status = 404 });

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
