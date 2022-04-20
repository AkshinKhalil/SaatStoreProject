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
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Blog> blogs = _context.Blogs.ToList();
            return View(blogs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid) return View();

            if (blog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Shekil daxil edin");
                return View();
            }
            if (!blog.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Shekilin olcusu maximum 2MB ola biler");
                return View();
            }
            if (!blog.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Image file sec");
                return View();
            }
            blog.Image = blog.ImageFile.SaveImg(_env.WebRootPath, "assets/images/blog");
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();

            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Blog blog)
        {
            Blog existedBlog = _context.Blogs.FirstOrDefault(b => b.Id == blog.Id);

            if (!ModelState.IsValid) return View(existedBlog);

            if (existedBlog == null) return NotFound();

            if (existedBlog.ImageFile != null)
            {
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFiles", "Please select the image file");
                    return View(existedBlog);
                }
                if (!blog.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFiles", "You can choose file which size is max 2MB");
                    return View(existedBlog);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images/blog", existedBlog.Image);
                existedBlog.Image = blog.ImageFile.SaveImg(_env.WebRootPath, "assets/images/blog");
            }
            existedBlog.Name = blog.Name;
            existedBlog.Description = blog.Description;
            existedBlog.StartDate = blog.StartDate;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) return Json(new { status = 404 });

            _context.Blogs.Remove(blog);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
