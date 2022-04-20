using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaatStoreProject.DAL;
using SaatStoreProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace SaatStoreProject.Controllers
{
    public class InfoController : Controller
    {
        private readonly AppDbContext _context;
        public InfoController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Setting setting = _context.Settings.FirstOrDefault();
            return View(setting);
        }
    }
}
