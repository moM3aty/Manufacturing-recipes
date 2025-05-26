namespace Kitchen.Controllers
{
    using Kitchen.Data;
    using Kitchen.Models;
    using Kitchen.ViewModel;
    using Microsoft.AspNetCore.Components.Sections;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

   
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewData["Direction"] = "rtl";
            ViewData["Language"] = "ar";
            ViewData["Title"] = "صفحة الرئيسية";
            return View();
        }

        public IActionResult Login()
        {
           

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.PasswordHash == password);

            if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminId", admin.Id);
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        public IActionResult Dashboard()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login");


            var model = new AdminDashboardViewModel
            {
                Recipes = _context.Recipes.ToList(),
                SectionContents = _context.SectionContents.ToList()
            };

            return View(model);
        }

        public IActionResult AddRecipe()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [RequestSizeLimit(104857600)] // 100 MB
        public async Task<IActionResult> AddRecipe(IFormFile recipeFile, IFormFile imageFile,
     string titleAr, string titleEn, string descriptionAr, string descriptionEn,
     decimal price, string sloganAr, string sloganEn, string recipeType)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login");

            if (recipeFile == null || recipeFile.Length == 0 || Path.GetExtension(recipeFile.FileName).ToLower() != ".zip")
            {
                ModelState.AddModelError("", "Please upload a valid .zip file for the recipe.");
                return View();
            }

            if (imageFile == null || imageFile.Length == 0 || !IsImage(imageFile))
            {
                ModelState.AddModelError("", "Please upload a valid image (jpg, jpeg, png).");
                return View();
            }

            if (string.IsNullOrWhiteSpace(titleAr) || string.IsNullOrWhiteSpace(titleEn)
                || string.IsNullOrWhiteSpace(descriptionAr) || string.IsNullOrWhiteSpace(descriptionEn)
                || string.IsNullOrWhiteSpace(recipeType))
            {
                ModelState.AddModelError("", "All fields are required including recipe type.");
                return View();
            }

            // Save files
            var zipFolder = Path.Combine(_env.WebRootPath, "uploads", "zips");
            var imageFolder = Path.Combine(_env.WebRootPath, "uploads", "images");
            Directory.CreateDirectory(zipFolder);
            Directory.CreateDirectory(imageFolder);

            var zipFileName = $"{Guid.NewGuid()}{Path.GetExtension(recipeFile.FileName)}";
            var zipFullPath = Path.Combine(zipFolder, zipFileName);
            using (var stream = new FileStream(zipFullPath, FileMode.Create))
            {
                await recipeFile.CopyToAsync(stream);
            }

            var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var imageFullPath = Path.Combine(imageFolder, imageFileName);
            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var recipe = new Recipe
            {
                TitleAr = titleAr,
                TitleEn = titleEn,
                DescriptionAr = descriptionAr,
                DescriptionEn = descriptionEn,
                Price = price,
                SloganAr = sloganAr,
                SloganEn = sloganEn,
                RecipeType = recipeType, // NEW

                FilePath = $"uploads/zips/{zipFileName}",
                ImagePath = $"uploads/images/{imageFileName}"
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

       
        private bool IsImage(IFormFile file)
        {
            var extensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            return extensions.Contains(ext);
        }

        public IActionResult EditRecipe(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();
            ViewBag.RecipeTypes = new List<SelectListItem>
{
        new SelectListItem { Text = "زراعي", Value = "Agricultural" },
        new SelectListItem { Text = "كيميائي", Value = "Chemical" },
        new SelectListItem { Text = "غذائي", Value = "Nutritional" },
        new SelectListItem { Text = "صيدلي", Value = "Pharmacy" },
        new SelectListItem { Text = "مستحضرات تجميل", Value = "cosmetics" }

};

            return View(recipe);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(int id, Recipe updatedRecipe, IFormFile? ImageFile, IFormFile? ZipFile)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            recipe.TitleAr = updatedRecipe.TitleAr;
            recipe.TitleEn = updatedRecipe.TitleEn;
            recipe.DescriptionAr = updatedRecipe.DescriptionAr;
            recipe.DescriptionEn = updatedRecipe.DescriptionEn;
            recipe.Price = updatedRecipe.Price;
            recipe.SloganAr = updatedRecipe.SloganAr;
            recipe.SloganEn = updatedRecipe.SloganEn;
            recipe.RecipeType = updatedRecipe.RecipeType; // NEW

            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(recipe.ImagePath))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, recipe.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var imageFolder = Path.Combine(_env.WebRootPath, "uploads", "images");
                Directory.CreateDirectory(imageFolder);

                var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                var imageFullPath = Path.Combine(imageFolder, imageFileName);

                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                recipe.ImagePath = $"uploads/images/{imageFileName}";
            }

            if (ZipFile != null && ZipFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(recipe.FilePath))
                {
                    var oldZipPath = Path.Combine(_env.WebRootPath, recipe.FilePath);
                    if (System.IO.File.Exists(oldZipPath))
                        System.IO.File.Delete(oldZipPath);
                }

                var zipFolder = Path.Combine(_env.WebRootPath, "uploads", "zips");
                Directory.CreateDirectory(zipFolder);

                var zipFileName = $"{Guid.NewGuid()}{Path.GetExtension(ZipFile.FileName)}";
                var zipFullPath = Path.Combine(zipFolder, zipFileName);

                using (var stream = new FileStream(zipFullPath, FileMode.Create))
                {
                    await ZipFile.CopyToAsync(stream);
                }

                recipe.FilePath = $"uploads/zips/{zipFileName}";
            }

            _context.Update(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            // حذف الصورة
            if (!string.IsNullOrEmpty(recipe.ImagePath))
            {
                var imagePath = Path.Combine(_env.WebRootPath, recipe.ImagePath);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            // حذف الملف المضغوط
            if (!string.IsNullOrEmpty(recipe.FilePath))
            {
                var zipPath = Path.Combine(_env.WebRootPath, recipe.FilePath);
                if (System.IO.File.Exists(zipPath))
                    System.IO.File.Delete(zipPath);
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult AddCode(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            return View(recipe);
        }

      
        [HttpPost]
        public IActionResult AddCode(string code, int recipeId)
        {
            // تحقق أن الوصفة موجودة
            var recipeExists = _context.Recipes.Any(r => r.Id == recipeId);
            if (!recipeExists)
                return NotFound("الوصفة غير موجودة");

            var recipeCode = new RecipeCode
            {
                Code = code,
                RecipeId = recipeId
            };

            _context.RecipeCodes.Add(recipeCode);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        public IActionResult EditSection(int id)
        {
            var sectionContent = _context.SectionContents.Find(id);
            return View(sectionContent);
        }



        [HttpPost]
        public IActionResult EditSection(Models.SectionContent model)
        {
            if (ModelState.IsValid)
            {
                _context.SectionContents.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }
    }


}
   