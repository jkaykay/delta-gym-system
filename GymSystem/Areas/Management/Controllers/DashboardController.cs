using GymSystem.Data;
using GymSystem.Models;
using GymSystem.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Areas.Management.Controllers
{
    [Area("Management")]
    public class DashboardController : ManagementBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalMembers = await _userManager.GetUsersInRoleAsync("Member").ContinueWith(t => t.Result.Count),
                TotalStaff = await _userManager.GetUsersInRoleAsync("Staff").ContinueWith(t => t.Result.Count),
                ActiveUsers = await _context.Users.CountAsync(u => u.Active),
                RecentSignups = await _context.Users
                    .OrderByDescending(u => u.JoinDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
