using Microsoft.AspNetCore.Http;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.ViewModels.Admin
{
    public class MoviesViewModel
    {
        public Movie movie { get; internal set; }

        [DisplayName("Movie name")]
        [Required]
        public string name { get; set; }

        [DisplayName("About movie")]
        [Required]
        public string content { get; set; }

        [DisplayName("Picture")]
        public IFormFile image { get; set; }
    }
}
