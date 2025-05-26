using Kitchen.Data;
using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kitchen.Controllers
{
    public class ContactInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactInfo
        public async Task<IActionResult> Index()
        {

            return View(await _context.contactInfos.ToListAsync());
        }

        // GET: ContactInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactInfo);
        }

        // GET: ContactInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contactInfo = await _context.contactInfos.FindAsync(id);
            if (contactInfo == null) return NotFound();

            return View(contactInfo);
        }

        // POST: ContactInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactInfo contactInfo)
        {
            if (id != contactInfo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(contactInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactInfo);
        }

        // GET: ContactInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contactInfo = await _context.contactInfos.FindAsync(id);
            if (contactInfo == null) return NotFound();
            _context.contactInfos.Remove(contactInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           
        }

       
    }

}
