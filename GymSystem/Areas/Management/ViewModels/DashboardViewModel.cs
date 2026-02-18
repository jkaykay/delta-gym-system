using GymSystem.Models;

namespace GymSystem.Areas.Management.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalMembers { get; set; }
        public int TotalStaff { get; set; }
        public int ActiveUsers { get; set; }
        public List<ApplicationUser> RecentSignups { get; set; } = new();
    }
}
