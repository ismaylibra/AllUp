using AllupFTB.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupFTB.ViewComponents
{
    public class FeaturedCategoryVIewComponent: ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public FeaturedCategoryVIewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted && x.IsMain).ToListAsync();
            return View(categories);  
        }
    }
}
