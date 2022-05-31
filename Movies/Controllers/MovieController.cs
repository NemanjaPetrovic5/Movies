using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        public IActionResult Movies(string search, int? page)
        {
            ViewBag.search = search;

            List<Movie> listMovies;

            if (page == null)
            {
                page = 1;
            }
            int pageSize = 12;
            ViewBag.page = page;

            int countMovie;
            (listMovies, countMovie) = _MovieServices.GetMovies(search, (int)page, pageSize);


            double maxPages = (double)countMovie / pageSize;
            ViewBag.maxPages = (int)Math.Ceiling(maxPages);
            ViewBag.total = countMovie;

            var movies = new List<MoviesViewModel>();
            foreach (var item in listMovies)
            {
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
