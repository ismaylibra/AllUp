using Microsoft.AspNetCore.Mvc;

namespace AllupFTB.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
