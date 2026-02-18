using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Policy = "ManagementAccess")]
    public class ManagementBaseController : Controller
    {
    }
}
