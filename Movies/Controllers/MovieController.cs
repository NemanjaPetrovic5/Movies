using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Interfaces;
using Movies.Models;
using Movies.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class MovieController : Controller
    {
        // instanciranje servisa (svih funkcija koje su navedene u IMoviesServices i implementirane u MovieServices) za kontrolu korisnika
        private readonly IMoviesServices _MovieServices;
        // 
        public MovieController(IMoviesServices MovieServices)
        {
            _MovieServices = MovieServices;
        }
        // GET: MovieController
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet("/movies/{naziv?}")]
        public IActionResult Movies(int? page)
        {
       
            List<Movie> listMovies;

            if (page == null)
            {
                page = 1;
            }
            int pageSize = 9;
            ViewBag.page = page;

            int countMovie;
            (listMovies, countMovie) = _MovieServices.GetMovies((int)page, pageSize);


            double maxPages = (double)countMovie / pageSize;
            ViewBag.maxPages = (int)Math.Ceiling(maxPages);
            ViewBag.total = countMovie;

            var movies = new List<MoviesViewModel>();
            foreach (var item in listMovies)
            {
                // skracivanje opisa kursa
                if (item.content.Length > 50)
                    item.content = item.content.Substring(0, 50) + "...";
                if (item.name.Length > 46)
                    item.name = item.name.Substring(0, 44) + "...";

                movies.Add(new MoviesViewModel()
                {
                    movie = item
                });
            }

            var viewmodel = new AdminMoviesViewModel
            {
                movie = movies
            };
            ViewBag.movies = movies;
            return View(viewmodel);
        }
        [HttpGet]
        public ActionResult<Movie> MovieDetails(string id)
        {
            var findMovie = _MovieServices.Find(id);

            return View(findMovie);
        }
    }
}
