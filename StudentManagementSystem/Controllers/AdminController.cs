using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;


namespace StudentManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            var data = _context.Admins
                .FirstOrDefault(x =>
                    x.Email == admin.Email &&
                    x.Password == admin.Password);

            if (data != null)
            {
                HttpContext.Session.SetString("Admin", "true");

                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.Message = "Invalid Email or Password";

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}