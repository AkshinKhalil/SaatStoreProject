using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaatStoreProject.DAL;
using SaatStoreProject.Models;
using SaatStoreProject.VidewModels;
using System.Collections.Generic;
using System.Linq;

namespace SaatStoreProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            BlogVM model = new BlogVM()
            {
                Blogs = _context.Blogs.ToList(),
                Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Include(w=>w.Brand).ToList()               
            };
            return View(model);
        }
        public IActionResult Details(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            List<Watch> watch = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Include(w => w.Brand).ToList();
            return View(blog);
            
        }
    }
}
