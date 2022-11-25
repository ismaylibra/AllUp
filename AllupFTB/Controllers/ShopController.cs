using AllupFTB.DAL;
using AllupFTB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupFTB.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var mainCategories = await _dbContext.Categories
                .Where(x => !x.IsDeleted && x.IsMain)
                .Include(x => x.Children
                .Where(x => !x.IsDeleted))
                .ToListAsync();

            var selectedCategory = mainCategories.FirstOrDefault();

            if(categoryId is not null)
            {
                selectedCategory = mainCategories.FirstOrDefault(x=>x.Id==categoryId);
                selectedCategory??= mainCategories.SelectMany(x=>x.Children).FirstOrDefault(x=>x.Id == categoryId); 

                if(selectedCategory is null)
                {
                    return NotFound();
                }
            }

            var model = new ShopViewModel
            {
                SelectedCategory = selectedCategory,
                Categories = mainCategories
            };
            return View(model);
        }
    }
}
