using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Interfaces;
using Movies.Models;
using Movies.Services;
using Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsersServices _UsersServices;

        public UserController(IUsersServices UsersServices)
        {
            _UsersServices = UsersServices;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
        // Login stranica
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // GET: UserController/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View(user);

            User u = _UsersServices.Find(user.username);

            if (u == null || u.password != Security.Hash256(user.password))
            {
                // neusepesna prijava -- poruka
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(user);
            }

            string role = "User";
            if (u.type == 2)
                role = "Admin";

            List<Claim> userClaims = new()
            {
                new Claim("userID", u.userID),
                new Claim("username", u.username),
                new Claim("email", u.email),
                new Claim(ClaimTypes.Role, role),
            };

            var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

            HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties
            {
                IsPersistent = user.remember,
                ExpiresUtc = DateTime.Now.AddHours(1)
            });

            // dodavanje auth cookie
            if(u.type == 1)
            {
                return RedirectToAction(actionName: "Movies", controllerName: "Movie");
            }
            else
            {
                return RedirectToAction(actionName: "Index", controllerName: "Admin");
            }
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        // Profil stranica
        [HttpGet]
        public IActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                string Id = HttpContext.User.Claims.First(c => c.Type == "userID").Value;
                User u = _UsersServices.FindUserById(Id);
                ViewBag.user = u;

                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterViewModel user)
        {
            if (!ModelState.IsValid)
                return View(user);

            // provera da li je korisnik vec registrovan
            User oldUser = _UsersServices.Find(user.username);
            if (oldUser != null)
            {
                ModelState.AddModelError(string.Empty, $"Username \"{user.username}\" is already taken.");
                return View();
            }

            User u = new()
            {
                username = user.username,
                email = user.email,
                password = Security.Hash256(user.password),
                type = 1
            };
            _UsersServices.Insert(u);
            return RedirectToAction("Index");
        }
    }
}
