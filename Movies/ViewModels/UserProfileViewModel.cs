﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        [DisplayName("Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }
}
