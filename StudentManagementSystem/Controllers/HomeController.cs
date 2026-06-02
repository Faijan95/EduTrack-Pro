using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // LANDING PAGE
        // =========================

        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // CONTACT FORM SUBMIT
        // =========================

        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {
            // Default Status

            contact.Status = "Pending";

            // Save Current Date

            contact.CreatedDate = DateTime.Now;

            // Save Data

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Message Sent Successfully!";

            return RedirectToAction("Index", "Student");
        }

        // =========================
        // CONTACT LIST
        // =========================

        public IActionResult ContactList()
        {
            var contacts =
                _context.Contacts.ToList();

            return View(contacts);
        }
        // =========================
        // DASHBOARD
        // =========================

        public IActionResult Dashboard(
    string search,
    int? page,
    DateTime? startDate,
    DateTime? endDate,
    string rangeType,
    string status)
        {
            // =========================
            // SESSION CHECK
            // =========================

            var adminSession =
                HttpContext.Session.GetString("Admin");

            if (adminSession == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            // =========================
            // VIEWBAG
            // =========================

            ViewBag.Search = search;
            ViewBag.RangeType = rangeType;
            ViewBag.Status = status;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            // =========================
            // QUERY
            // =========================

            var contacts =
                _context.Contacts.AsQueryable();

            // =========================
            // STATUS FILTER
            // =========================

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Approved")
                {
                    contacts = contacts.Where(x =>
                        x.Status == "Approved");
                }

                else if (status == "Pending")
                {
                    contacts = contacts.Where(x =>
                        x.Status == "Pending");
                }
            }

            // =========================
            // SEARCH FILTER
            // =========================

            if (!string.IsNullOrEmpty(search))
            {
                contacts = contacts.Where(x =>
                    x.FullName.Contains(search) ||
                    x.Email.Contains(search));
            }

            // =========================
            // DATE FILTER
            // =========================

            if (rangeType == "today")
            {
                contacts = contacts.Where(x =>
                    x.CreatedDate.Date == DateTime.Today);
            }

            else if (rangeType == "yesterday")
            {
                contacts = contacts.Where(x =>
                    x.CreatedDate.Date ==
                    DateTime.Today.AddDays(-1));
            }

            else if (rangeType == "7days")
            {
                var last7 =
                    DateTime.Today.AddDays(-7);

                contacts = contacts.Where(x =>
                    x.CreatedDate >= last7);
            }

            else if (rangeType == "30days")
            {
                var last30 =
                    DateTime.Today.AddDays(-30);

                contacts = contacts.Where(x =>
                    x.CreatedDate >= last30);
            }

            else if (rangeType == "custom")
            {
                if (startDate.HasValue &&
                    endDate.HasValue)
                {
                    contacts = contacts.Where(x =>
                        x.CreatedDate.Date >= startDate.Value.Date &&
                        x.CreatedDate.Date <= endDate.Value.Date);
                }
            }

            // =========================
            // COUNTS
            // =========================

            // TOTAL FILTERED
            ViewBag.TotalContacts =
                contacts.Count();

            // PENDING FILTERED
            ViewBag.PendingContacts =
                contacts.Count(x =>
                    x.Status == "Pending");

            // APPROVED FILTERED
            ViewBag.ApprovedContacts =
                contacts.Count(x =>
                    x.Status == "Approved");

            // =========================
            // PAGINATION
            // =========================

            int pageSize = 10;

            int pageNumber = page ?? 1;

            var result = contacts
                .OrderByDescending(x => x.Id)
                .ToPagedList(pageNumber, pageSize);

            // =========================
            // RETURN
            // =========================

            return View(result);
        }


        // =========================
        // APPROVE CONTACT
        // =========================

        public IActionResult Approve(int id)
        {
            var contact =
                _context.Contacts.Find(id);

            if (contact != null)
            {
                contact.Status = "Approved";

                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // =========================
        // DELETE CONTACT
        // =========================

        public IActionResult DeleteContact(int id)
        {
            var contact =
                _context.Contacts.Find(id);

            if (contact != null)
            {
                _context.Contacts.Remove(contact);

                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // =========================
        // EDIT PAGE OPEN
        // =========================

        public IActionResult EditContact(int id)
        {
            var adminSession =
                HttpContext.Session.GetString("Admin");

            if (adminSession == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var contact =
                _context.Contacts.Find(id);

            return View(contact);
        }

        // =========================
        // UPDATE CONTACT
        // =========================

        [HttpPost]
        public IActionResult EditContact(Contact contact)
        {
            _context.Contacts.Update(contact);

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}