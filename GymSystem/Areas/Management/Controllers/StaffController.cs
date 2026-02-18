using GymSystem.Areas.Management.ViewModels;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Areas.Management.Controllers
{
    public class StaffController : ManagementBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Both Staff and Admin can view
        public async Task<IActionResult> Index()
        {
            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            return View(staffUsers.Concat(adminUsers).ToList());
        }

        // View staff details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Staff") && !roles.Contains("Admin")) return NotFound();

            return View(user);
        }

        // Only Admin can create Staff accounts
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStaffViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Generate a valid username from EmployeeId or email prefix
            var username = !string.IsNullOrWhiteSpace(model.EmployeeId)
                ? model.EmployeeId.Replace("-", "").ToLowerInvariant()
                : model.Email.Split('@')[0];

            var user = new ApplicationUser
            {
                UserName = username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmployeeId = model.EmployeeId,
                HireDate = DateTime.UtcNow,
                Active = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role); // "Staff" or "Admin"
                TempData["Success"] = "Staff member created successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // Staff can edit some details (not Id, delete or change roles)
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(new EditStaffViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStaffViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Staff member updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // Only Admin can delete
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Prevent deleting yourself
            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "Cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "Staff member deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}