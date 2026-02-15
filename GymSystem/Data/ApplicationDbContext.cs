using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GymSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace GymSystem.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser, IdentityRole, string>
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
