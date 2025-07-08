using Kitchen.Data;
using Kitchen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Kitchen.Controllers
{
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject ApplicationDbContext
        public OffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index Action: Displays all offers
        public ActionResult Index()
        {
            // Get all offers from the database
            var offers = _context.Offers.ToList();
            return View(offers);
        }

        // Create Action: Displays the offer creation form
        public ActionResult Create()
        {
            return View();
        }

        // Post method for creating a new offer
        [HttpPost]
        public ActionResult Create(Offer offer, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                offer.ImageUrl = "/uploads/" + fileName;
            }

            _context.Offers.Add(offer); // Add the offer to the database
            _context.SaveChanges(); // Save changes to the database

            return RedirectToAction("Index");
        }

        // Edit Action: Displays the offer edit form
        public ActionResult Edit(int id)
        {
            var offer = _context.Offers.FirstOrDefault(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // Post method for editing an existing offer
        [HttpPost]
        public ActionResult Edit(Offer updatedOffer, IFormFile ImageFile)
        {
            var offer = _context.Offers.FirstOrDefault(o => o.Id == updatedOffer.Id);
            if (offer != null)
            {
                offer.TitleAr = updatedOffer.TitleAr;
                offer.TitleEn = updatedOffer.TitleEn;
                offer.DescriptionAr = updatedOffer.DescriptionAr;
                offer.DescriptionEn = updatedOffer.DescriptionEn;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    offer.ImageUrl = "/uploads/" + fileName;
                }

                _context.SaveChanges(); 
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var offer = _context.Offers.FirstOrDefault(o => o.Id == id);
            if (offer != null)
            {
                _context.Offers.Remove(offer); 
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}