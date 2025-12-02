using Microsoft.AspNetCore.Mvc;
using FancySignup.Data;
using FancySignup.Models;
using FancySignup.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FancySignup.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Account/Signup
        public IActionResult Signup()
        {
            ViewBag.Countries = _db.Countries.ToList();
            return View();
        }

        // POST: /Account/Signup
        [HttpPost]
        public IActionResult Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _db.Countries.ToList();
                return View(model);
            }

            if (_db.People.Any(p => p.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                ViewBag.Countries = _db.Countries.ToList();
                return View(model);
            }

            var person = new Person
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CountryId = model.CountryId,
                Password = model.Password,
                IsAdmin = false,   // normal user by default
                IsActive = true    // active by default
            };

            _db.People.Add(person);
            _db.SaveChanges();

            return RedirectToAction("SignupSuccess");
        }

        public IActionResult SignupSuccess()
        {
            return View();
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _db.People.FirstOrDefault(p => p.Email == email && p.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid login.";
                return View();
            }

            if (!user.IsActive)
            {
                ViewBag.Error = "Your account has been disabled. Please contact an administrator.";
                return View();
            }

            // store basic info in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");

            if (user.IsAdmin)
            {
                // admin → admin dashboard
                return RedirectToAction("Index", "Admin");
            }

            // normal user
            return RedirectToAction("LoginSuccess", new { email = user.Email });
        }

        public IActionResult LoginSuccess(string email)
        {
            var user = _db.People.FirstOrDefault(p => p.Email == email);
            if (user == null) return RedirectToAction("Login");

            ViewBag.Name = $"{user.FirstName} {user.LastName}";
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
