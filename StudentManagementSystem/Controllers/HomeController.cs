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

     

        public IActionResult Index()
        {
            return View();
        }

        
        // CONTACT FORM SUBMIT
        

        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {
            

            contact.Status = "Pending";

            

            contact.CreatedDate = DateTime.Now;

            // ye data save karta hai

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Message Sent Successfully!";

            return RedirectToAction("Index", "Student");
        }

      

        public IActionResult ContactList()
        {
            var contacts =
                _context.Contacts.ToList();

            return View(contacts);
        }
     

        public IActionResult Dashboard(
    string search,
    int? page,
    DateTime? startDate,
    DateTime? endDate,
    string rangeType,
    string status)
        {
            

            var adminSession =
                HttpContext.Session.GetString("Admin");

            if (adminSession == null)
            {
                return RedirectToAction("Login", "Admin");
            }

           

            ViewBag.Search = search;
            ViewBag.RangeType = rangeType;
            ViewBag.Status = status;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

          

            var contacts =
                _context.Contacts.AsQueryable();

           

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

          

            if (!string.IsNullOrEmpty(search))
            {
                contacts = contacts.Where(x =>
                    x.FullName.Contains(search) ||
                    x.Email.Contains(search));
            }

         

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



            ViewBag.TotalContacts =
                contacts.Count();

            
            ViewBag.PendingContacts =
                contacts.Count(x =>
                    x.Status == "Pending");

          
            ViewBag.ApprovedContacts =
                contacts.Count(x =>
                    x.Status == "Approved");

           
            // PAGINATION
            

            int pageSize = 10;

            int pageNumber = page ?? 1;

            var result = contacts
                .OrderByDescending(x => x.Id)
                .ToPagedList(pageNumber, pageSize);



            return View(result);
        }


       

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

    

        [HttpPost]
        public IActionResult EditContact(Contact contact)
        {
            _context.Contacts.Update(contact);

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}