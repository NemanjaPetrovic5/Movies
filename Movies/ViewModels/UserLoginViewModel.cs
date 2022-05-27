using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("User name")]
        [Required]
        public string username { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required]
        public string password { get; set; }

        public bool remember { get; set; }
    }
}
