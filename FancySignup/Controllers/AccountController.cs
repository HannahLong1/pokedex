using Microsoft.AspNetCore.Mvc;
using FancySignup.Data;
using FancySignup.Models;
using FancySignup.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FancySignup.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Initial page
        public IActionResult Index()
        {
            return View();
        }

        // Signup GET
        public IActionResult Signup()
        {
            ViewBag.Countries = _db.Countries.ToList();
            return View();
        }

        // Signup POST
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
                Password = model.Password
            };

            _db.People.Add(person);
            _db.SaveChanges();

            return RedirectToAction("SignupSuccess");
        }

        public IActionResult SignupSuccess()
        {
            return View();
        }

        // Login GET
        public IActionResult Login()
        {
            return View();
        }

        // Login POST
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _db.People.FirstOrDefault(p => p.Email == email && p.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid login.";
                return View();
            }

            return RedirectToAction("LoginSuccess", new { email = user.Email });
        }

        public IActionResult LoginSuccess(string email)
        {
            var user = _db.People.First(p => p.Email == email);
            ViewBag.Name = $"{user.FirstName} {user.LastName}";
            return View();
        }
    }
}
