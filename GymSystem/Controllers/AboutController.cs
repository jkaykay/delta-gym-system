using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
