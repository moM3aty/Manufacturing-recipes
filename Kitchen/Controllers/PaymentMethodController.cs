using Kitchen.Data;
using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kitchen.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all payment methods
        public IActionResult Index()
        {
            var methods = _context.paymentMethods.ToList();
            return View(methods);
        }

        // Show create form
        public IActionResult Create()
        {
            return View();
        }

        // Handle create form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaymentMethod method)
        {

            _context.paymentMethods.Add(method);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        // Show edit form
        public IActionResult Edit(int id)
        {
            var method = _context.paymentMethods.FirstOrDefault(m => m.Id == id);
            if (method == null) return NotFound();
            return View(method);
        }

        // Handle edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaymentMethod method)
        {
            if (ModelState.IsValid)
            {
                _context.paymentMethods.Update(method);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(method);
        }


        public IActionResult Delete(int id)
        {
            var method = _context.paymentMethods.FirstOrDefault(m => m.Id == id);
            if (method == null) return NotFound();
            _context.paymentMethods.Remove(method);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}