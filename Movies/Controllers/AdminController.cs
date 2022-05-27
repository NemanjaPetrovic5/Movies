using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Interfaces;
using Movies.Models;
using Movies.Services;
using Movies.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMoviesServices _MoviesServices;
        private readonly IUsersServices _UsersServices;

        public object MoviesViewModel { get; private set; }

        public AdminController(IMoviesServices MoviesServices, IUsersServices UsersServices)
        {
            _MoviesServices = MoviesServices;
            _UsersServices = UsersServices;
        }
        public IActionResult Index()
        {
            return RedirectToAction("adminPanel");
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

        // POST: AdminController/insertMovie
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
                        image = "no-image.jpg"

                    };
                    var file = m.image;

                    if (file != null)
                    {
                        movie.image = movie.name + "-Image-" + DateTime.Now.ToString("yyyyMMdd") + ".png";
                        var filePath = Directory.GetCurrentDirectory() + "/wwwroot/images/Movies";
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
        public ActionResult Users()
        {
            List<User> UserList;

            //READ
            UserList = _UsersServices.Read();
            var users = new List<UserViewModel>();

            foreach (var item in UserList)
            {
                users.Add(new UserViewModel()
                {
                    user = item
                });
            }

            var viewmodel = new AdminUsersViewModel
            {
                users = users,
            };
            ViewBag.users = users;
            return View(viewmodel);
        }
        public ActionResult insertUser()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult insertUser(UserViewModel u)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // provera da li je korisnik vec registrovan
                    User oldUser = _UsersServices.Find(u.username);
                    if (oldUser != null)
                    {
                        ModelState.AddModelError(string.Empty, $"Username \"{u.username}\" is already taken.");
                        return View();
                    }
                    User user = new()
                    {
                        username = u.username,
                        email = u.email,
                        password = Security.Hash256(u.password),
                        type = 1

                    };
                    _UsersServices.Insert(user);
                    return RedirectToAction("insertUser");
                };
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
