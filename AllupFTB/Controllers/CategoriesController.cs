using Microsoft.AspNetCore.Mvc;

namespace AllupFTB.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
