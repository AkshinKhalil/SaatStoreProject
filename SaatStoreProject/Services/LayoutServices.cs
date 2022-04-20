using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SaatStoreProject.DAL;
using SaatStoreProject.Models;
using SaatStoreProject.VidewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaatStoreProject.Services
{
    public class LayoutServices
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;

        public LayoutServices(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContext = httpContextAccessor;
            _userManager = userManager;
        }       
        public Setting getSettingDatas()
        {
            Setting data = _context.Settings.FirstOrDefault();
            return data;
        }
        public List<Category>  getCategoryDatas()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }
        public List<Brand> getBrandDatas()
        {
            List<Brand> brands = _context.Brands.ToList();
            return brands;
        }

        public async Task<BasketVM> ShowBasket()
        {
            string basket = _httpContext.HttpContext.Request.Cookies["Basket"];
            BasketVM basketData = new BasketVM
            {
                TotalPrice = 0,
                BasketItems = new List<BasketItemVM>(),
                Count = 0
            };
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                List<BasketItem> basketItems = _context.BasketItems.Include(b => b.AppUser).Include(b=>b.Watch).ThenInclude(w=>w.WatchImages).Include(b => b.Watch).ThenInclude(w => w.Brand).Where(b => b.AppUserId == user.Id).ToList();
                foreach (BasketItem item in basketItems)
                {
                    Watch watch = _context.Watches.FirstOrDefault(w => w.Id == item.WatchId);
                    if (watch != null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM
                        {
                            Watch = watch,
                            Count = item.Count
                        };
                        basketItemVM.Price = watch.Price;
                        basketData.BasketItems.Add(basketItemVM);
                        basketData.Count++;
                        basketData.TotalPrice += basketItemVM.Price * basketItemVM.Count;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(basket))
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                    foreach (BasketCookieItemVM item in basketCookieItems)
                    {
                        Watch watch = _context.Watches.FirstOrDefault(w => w.Id == item.Id);
                        if (watch != null)
                        {
                            BasketItemVM basketItem = new BasketItemVM
                            {
                                Watch = _context.Watches.Include(w => w.WatchImages).Include(w => w.Brand).FirstOrDefault(w => w.Id == item.Id),
                                Count = item.Count

                            };
                            basketItem.Price = watch.Price;
                            basketData.BasketItems.Add(basketItem);
                            basketData.Count++;
                            basketData.TotalPrice += basketItem.Price * basketItem.Count;
                        }
                    }
                }
            }
            return basketData;
        }
    }
}
