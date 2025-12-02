using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FancySignup.Data;
using FancySignup.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FancySignup.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        private bool CurrentUserIsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // GET: /Admin
        public IActionResult Index()
        {
            if (!CurrentUserIsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var people = _db.People
                .Include(p => p.Country)
                .ToList();

            var model = new AdminDashboardViewModel
            {
                TotalUsers = people.Count,
                AdminUsers = people.Count(p => p.IsAdmin),
                InactiveUsers = people.Count(p => !p.IsActive),
                Users = people.Select(p => new AdminUserViewModel
                {
                    Email = p.Email,
                    FullName = p.FirstName + " " + p.LastName,
                    CountryName = p.Country?.Name ?? "",
                    IsAdmin = p.IsAdmin,
                    IsActive = p.IsActive
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MakeAdmin(string email)
        {
            if (!CurrentUserIsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = _db.People.FirstOrDefault(p => p.Email == email);
            if (user == null) return NotFound();

            user.IsAdmin = true;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveAdmin(string email)
        {
            if (!CurrentUserIsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = _db.People.FirstOrDefault(p => p.Email == email);
            if (user == null) return NotFound();

            user.IsAdmin = false;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(string email)
        {
            if (!CurrentUserIsAdmin())
                return RedirectToAction("AccessDenied", "Account");

            var user = _db.People.FirstOrDefault(p => p.Email == email);
            if (user == null) return NotFound();

            user.IsActive = !user.IsActive;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
