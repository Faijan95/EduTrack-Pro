using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }


        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // EDIT POST
        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}