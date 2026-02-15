using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MembershipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
