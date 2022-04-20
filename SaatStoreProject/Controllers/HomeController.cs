using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaatStoreProject.DAL;
using SaatStoreProject.Models;
using SaatStoreProject.VidewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SaatStoreProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM model = new HomeVM()
            {
                Watches = _context.Watches.Include(w => w.Brand).Include(w => w.WatchCategory).Include(w =>w.WatchImages).ToList(),
                Categories = _context.Categories.ToList(),
                Sliders = _context.Sliders.ToList(),
                Blogs=_context.Blogs.ToList(),
                Settings=_context.Settings.ToList()
            };
            return View(model);
        }

    }
}
