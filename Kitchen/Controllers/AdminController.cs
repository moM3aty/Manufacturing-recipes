namespace Kitchen.Controllers
{
    using Kitchen.Data;
    using Kitchen.Models;
    using Kitchen.ViewModel;
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

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "البريد الإلكتروني وكلمة المرور مطلوبان.");
                return View();
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email && a.PasswordHash == password);

            if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminId", admin.Id);
                return RedirectToAction(nameof(Dashboard));
            }

            ModelState.AddModelError("", "بيانات الاعتماد غير صالحة.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            var model = new AdminDashboardViewModel
            {
                Recipes = await _context.Recipes.ToListAsync(),
                SectionContents = await _context.SectionContents.ToListAsync(),
                Offers = await _context.Offers.ToListAsync()
            };

            return View(model);
        }

        public IActionResult AddRecipe()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            ViewBag.RecipeTypes = GetRecipeTypes();
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


    

        public async Task<IActionResult> EditRecipe(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            ViewBag.RecipeTypes = GetRecipeTypes(recipe.RecipeType);
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(int id, Recipe updatedRecipe, IFormFile? imageFile, IFormFile? zipFile)
        {
            if (id != updatedRecipe.Id) return BadRequest();

            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            var recipe = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                updatedRecipe.ImagePath = recipe.ImagePath;
                updatedRecipe.FilePath = recipe.FilePath;

                if (imageFile != null && imageFile.Length > 0)
                {
                    DeleteFile(recipe.ImagePath);
                    updatedRecipe.ImagePath = await SaveFileAsync(imageFile, "images");
                }

                if (zipFile != null && zipFile.Length > 0)
                {
                    DeleteFile(recipe.FilePath);
                    updatedRecipe.FilePath = await SaveFileAsync(zipFile, "zips");
                }

                _context.Update(updatedRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.RecipeTypes = GetRecipeTypes(updatedRecipe.RecipeType);
            return View(updatedRecipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            DeleteFile(recipe.ImagePath);
            DeleteFile(recipe.FilePath);

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> AddCode(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            return View(recipe);
        }

        [HttpPost]
        public IActionResult AddCode(string code, int recipeId)
        {
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
        public async Task<IActionResult> EditSection(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            var sectionContent = await _context.SectionContents.FindAsync(id);
            if (sectionContent == null) return NotFound();

            return View(sectionContent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSection(SectionContent model)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction(nameof(Login));

            if (ModelState.IsValid)
            {
                _context.SectionContents.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }

            return View(model);
        }

        private bool IsImage(IFormFile file)
        {
            var extensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extensions.Contains(ext);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0) return null;

            var folderPath = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"uploads/{subfolder}/{fileName}";
        }

        private void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, filePath.Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private List<SelectListItem> GetRecipeTypes(string selectedValue = "")
        {
            var types = new List<SelectListItem>
            {
                new SelectListItem { Text = "زراعي", Value = "Agricultural" },
                new SelectListItem { Text = "كيميائي", Value = "Chemical" },
                new SelectListItem { Text = "غذائي", Value = "Nutritional" },
                new SelectListItem { Text = "صيدلي", Value = "Pharmacy" },
                new SelectListItem { Text = "مستحضرات تجميل", Value = "cosmetics" },
                new SelectListItem { Text = "دراسات جدوى", Value = "Feasibility Studies" },
                new SelectListItem { Text = "نماذج صناعية", Value = "Industrial Models" }
            };

            if (!string.IsNullOrEmpty(selectedValue))
            {
                var selectedItem = types.FirstOrDefault(t => t.Value == selectedValue);
                if (selectedItem != null)
                {
                    selectedItem.Selected = true;
                }
            }

            return types;
        }
    }
}
