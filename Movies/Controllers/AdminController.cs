using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Interfaces;
using Movies.Models;
using Movies.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMoviesServices _MoviesServices;

        public object MoviesViewModel { get; private set; }

        public AdminController(IMoviesServices MoviesServices)
        {
            _MoviesServices = MoviesServices;
        }

        // GET: AdminController
        public ActionResult adminPanel()
        {
            return View();
        }

        // GET: AdminController/Movies/
        public ActionResult Movies()
        {
            List<Movie> MovieList;

            //READ
            MovieList = _MoviesServices.Read();
            var movies = new List<MoviesViewModel>();

            foreach (var item in MovieList)
            {
                // skracivanje opisa kursa
                if (item.content.Length > 50)
                    item.content = item.content.Substring(0, 50) + "...";

                movies.Add(new MoviesViewModel()
                {
                    movie = item
                });
            }

            var viewmodel = new AdminMoviesViewModel
            {
                movie = movies,
            };
            ViewBag.movies = movies;
            return View(viewmodel);
        }

        // GET: AdminController/Create
        public ActionResult insertMovie()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult insertMovie(MoviesViewModel m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Movie movie = new()
                    {
                        name = m.name,
                        content = m.content,
                        image = "no.image.jpg"

                    };
                    var file = m.image;
                    if (file != null)
                    {
                        movie.image = movie.movieID + "-Original-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
                        var filePath = Directory.GetCurrentDirectory() + "/wwwroot/images/Kursevi";
                        if (Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        var path = Path.Combine(filePath, movie.image);
                        FileStream fs = new FileStream(path, FileMode.Create);
                        file.CopyTo(fs);
                        fs.Close();
                    }

                    _MoviesServices.Insert(movie);
                    return RedirectToAction("Movies");
                };
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
