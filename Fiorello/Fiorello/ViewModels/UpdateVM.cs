﻿using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels
{
    public class UpdateVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        
    }
}
