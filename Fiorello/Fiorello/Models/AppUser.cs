﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
        public bool IsDeactive  { get; set; }
        public string Image  { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
