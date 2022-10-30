using System;
using Microsoft.AspNetCore.Identity;

namespace BonusStore.Model
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? Birthday { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

