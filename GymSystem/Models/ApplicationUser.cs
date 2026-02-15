using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [PersonalData]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; } = DateTime.Now;

        public bool Active { get; set; } = true;
    }
}