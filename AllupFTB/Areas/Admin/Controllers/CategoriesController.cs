using AllupFTB.Areas.Admin.Data;
using AllupFTB.Areas.Admin.Models.ViewModels;
using AllupFTB.DAL;
using AllupFTB.DAL.Entities;
using AllupFTB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace AllupFTB.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoriesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return View(categories);

        }
        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories.Where(x=> !x.IsDeleted && x.IsMain).ToListAsync();


            var categoryListItem = new List<SelectListItem>()
            {
                new SelectListItem("--Select Parent Category--", "0")
            };
             
            categories.ForEach(x=> categoryListItem.Add( new SelectListItem(x.Name, x.Id.ToString()) ) );
            

            var model = new CategoryCreateViewModel()
            {
                ParentCategories = categoryListItem
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            var parentCategories = await _dbContext.Categories.Where(x => !x.IsDeleted && x.IsMain).Include(x=>x.Children).ToListAsync();


            var categoryListItem = new List<SelectListItem>()
            {
                new SelectListItem("--Select Parent Category--", "0")
            };

            parentCategories.ForEach(x => categoryListItem.Add(new SelectListItem(x.Name, x.Id.ToString())));


            var  viewModel = new CategoryCreateViewModel()
            {
                ParentCategories = categoryListItem
            };
            
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var createdCategory = new Category();

            if (model.IsMain)
            {

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Şəkil seçilməlidir..!");
                    return View(viewModel);
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("", "Şəkil ölçüsü 10mb-dan çox ola bilməz..! ");
                    return View(viewModel);
                }

                if (parentCategories.Any(x => x.Name.ToLower().Equals(model.Name.ToLower())))
                {
                    ModelState.AddModelError("", "Bu adda kateqoriya mövcuddur..! ");
                    return View(viewModel);
                }
                var unicalName = await model.Image.GenerateFile(Constants.CategoryPath);
                createdCategory.ImageUrl = unicalName;
            }
            else
            {
                if(model.ParentId == 0)
                {
                    ModelState.AddModelError("", "Parent kateqoriya seçilməlidir..!");
                    return View(viewModel);
                }

                var parentCategory = parentCategories.FirstOrDefault(x => x.Id == model.ParentId);

                if (parentCategory.Children.Any(x => x.Name.ToLower().Equals(model.Name.ToLower())))
                    {
                    ModelState.AddModelError("", "Bu adda alt kateqoriya mövcuddur..!");
                    return View(viewModel);
                }
                createdCategory.ImageUrl = "";
                createdCategory.ParentId = model.ParentId;
            }
            createdCategory.Name = model.Name;
            createdCategory.IsMain = model.IsMain;
            createdCategory.IsDeleted = false;

            await _dbContext.Categories.AddAsync(createdCategory);
            await _dbContext.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));
        }
    }
}
