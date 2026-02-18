using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Areas.Members.Controllers
{
    [Area("Members")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
