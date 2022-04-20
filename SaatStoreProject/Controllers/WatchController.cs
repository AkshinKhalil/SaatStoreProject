using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SaatStoreProject.DAL;
using SaatStoreProject.Models;
using SaatStoreProject.VidewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaatStoreProject.Controllers
{
    public class WatchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public WatchController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index(int? sortId, int? categoryId, int? selectBox, int page = 1 )
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Watches.Count() / 4);
            if (sortId > 0 || categoryId > 0)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Where(w => w.BrandId == sortId || w.WatchCategory.Any(wc => wc.CategoryId == categoryId)).Skip((page - 1) * 4).Take(4).ToList(),
                    Categories = _context.Categories.Include(c => c.WatchCategory).ThenInclude(wc => wc.Watch).ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else if (selectBox == 1)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Skip((page - 1) * 4).Take(4).OrderBy(w=>w.Brand.Name).ToList(),
                    Categories = _context.Categories.ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else if (selectBox == 2)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Skip((page - 1) * 4).Take(4).OrderByDescending(w => w.Brand.Name).ToList(),
                    Categories = _context.Categories.ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else if (selectBox == 3)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Skip((page - 1) * 4).Take(4).OrderBy(w => w.Price).ToList(),
                    Categories = _context.Categories.ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else if (selectBox == 4)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Skip((page - 1) * 4).Take(4).OrderByDescending(w => w.Price).ToList(),
                    Categories = _context.Categories.ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Skip((page - 1) * 4).Take(4).ToList(),
                    Categories = _context.Categories.ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string searchwatchName,int min_value,int max_value)
        {
            if(searchwatchName!= null)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Where(w => w.Brand.Name.Contains(searchwatchName)).ToList(),
                    Categories = _context.Categories.Include(c => c.WatchCategory).ThenInclude(wc => wc.Watch).ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else if(min_value>0 && max_value>0 && min_value < max_value)
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).Where(w=>w.Price>min_value && w.Price<max_value).OrderBy(w=>w.Price).ToList(),
                    Categories = _context.Categories.Include(c => c.WatchCategory).ThenInclude(wc => wc.Watch).ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            }
            else 
            {
                WatchVM model = new WatchVM()
                {
                    Watches = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wt => wt.Category).Include(w => w.WatchImages).ToList(),
                    Categories = _context.Categories.ToList(),
                    Brands = _context.Brands.Include(b => b.Watch).ToList()
                };
                return View(model);
            } 
        }

       
        public IActionResult Details(int id)
        {
            Watch watch = _context.Watches.Include(w => w.WatchCategory).ThenInclude(wc => wc.Category).Include(w => w.Brand).Include(w => w.WatchImages).FirstOrDefault(t => t.Id == id);
            return View(watch);
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            Watch watch = _context.Watches.FirstOrDefault(w => w.Id == id);
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.WatchId == watch.Id && b.AppUserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        WatchId = watch.Id,
                        Count = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];

                if (basket == null)
                {
                    List<BasketCookieItemVM> basketCookieItems = new List<BasketCookieItemVM>();

                    basketCookieItems.Add(new BasketCookieItemVM
                    {
                        Id = watch.Id,
                        Count = 1
                    });
                    string basketStr = JsonConvert.SerializeObject(basketCookieItems);
                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                }
                else
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                    BasketCookieItemVM cookieItem = basketCookieItems.FirstOrDefault(c => c.Id == watch.Id);

                    if (cookieItem == null)
                    {
                        cookieItem = new BasketCookieItemVM
                        {
                            Id = watch.Id,
                            Count = 1
                        };
                        basketCookieItems.Add(cookieItem);
                    }
                    else
                    {
                        cookieItem.Count++;
                    }
                    string basketStr = JsonConvert.SerializeObject(basketCookieItems);

                    HttpContext.Response.Cookies.Append("Basket", basketStr);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveFromCart(int Removeid)
        {
            Watch watch = _context.Watches.FirstOrDefault(w => w.Id == Removeid);
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.WatchId == watch.Id && b.AppUserId == user.Id);
                _context.BasketItems.Remove(basketItem);
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["Basket"];
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                BasketCookieItemVM cookieItem = basketCookieItems.FirstOrDefault(c => c.Id == watch.Id);
                basketCookieItems.Remove(cookieItem);
                string basketStr = JsonConvert.SerializeObject(basketCookieItems);
                HttpContext.Response.Cookies.Append("Basket", basketStr);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
