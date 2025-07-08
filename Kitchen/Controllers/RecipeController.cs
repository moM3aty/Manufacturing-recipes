namespace Kitchen.Controllers
{
    using Kitchen.Data;
    using Kitchen.Models;
    using Kitchen.ViewModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging; // Added for logging
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<RecipeController> _logger; // Added for logging

        public RecipeController(ApplicationDbContext context, IWebHostEnvironment env, ILogger<RecipeController> logger) // Added logger
        {
            _context = context;
            _env = env;
            _logger = logger; // Added logger
        }

        private void SetLanguageAndDirection()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";
            ViewData["Language"] = language;
            ViewData["Direction"] = direction;
        }

        public IActionResult ToggleLanguage(string returnUrl)
        {
            var currentLanguage = Request.Cookies["Language"] ?? "ar";
            var newLanguage = currentLanguage == "ar" ? "en" : "ar";

            Response.Cookies.Append("Language", newLanguage, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddYears(1),
                IsEssential = true,
                Path = "/"
            });

            return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        }

        public async Task<IActionResult> Agriculture()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Agriculture";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "Agricultural").ToListAsync();
            return View(recipes);
        }

        public async Task<IActionResult> Chemical()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Chemical";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "Chemical").ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> clinic()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "clinic";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "Pharmacy").ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> makeup()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "makeup";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "cosmetics").ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> food()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "food";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "Nutritional").ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> FeasibilityStudies()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "FeasibilityStudies";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "Feasibility Studies").ToListAsync();
            return View(recipes);
        }
        public async Task<IActionResult> industrialModels()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "industrialModels";
            var recipes = await _context.Recipes.Where(x => x.RecipeType == "Industrial Models").ToListAsync();
            return View(recipes);
        }

        public async Task<IActionResult> Index()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Index";

            var contents = await _context.SectionContents.ToListAsync();
            var language = ViewData["Language"]?.ToString();

            string GetLocalizedText(List<SectionContent> allContents, string key)
            {
                var content = allContents.FirstOrDefault(c => c.Key == key);
                return language == "ar" ? content?.TextAr : content?.TextEn;
            }

            ViewData["HeroTitle"] = GetLocalizedText(contents, "Hero_Title");
            ViewData["HeroDesc"] = GetLocalizedText(contents, "Hero_Desc");
            ViewData["HeroButton"] = GetLocalizedText(contents, "Hero_Button");
            ViewData["whoUS"] = GetLocalizedText(contents, "Who_US");
            ViewData["whoDesc"] = GetLocalizedText(contents, "Who_Desc");
            ViewData["whoDesc2"] = GetLocalizedText(contents, "Who_Desc2");

            var model = new AdminDashboardViewModel
            {
                Recipes = await _context.Recipes.Where(x => x.RecipeType == "Nutritional").ToListAsync(),
                Offers = await _context.Offers.ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> Contact()
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Contact";
            var model = new ContactViewModel
            {
                contacts = await _context.contactInfos.ToListAsync(),
                paymentMethods = await _context.paymentMethods.ToListAsync()
            };
            return View(model);
        }

        public IActionResult EnterCode(int id)
        {
            SetLanguageAndDirection();
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "";
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(int id, string code)
        {
            SetLanguageAndDirection();
            var language = ViewData["Language"]?.ToString() ?? "ar";

            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["ErrorMessage"] = language == "ar" ? "الرجاء إدخال الكود." : "Please enter the code.";
                return RedirectToAction("EnterCode", new { id = id });
            }

            var trimmedCode = code.Trim();
            var matchedCode = await _context.RecipeCodes
                .Include(rc => rc.Recipe)
                .FirstOrDefaultAsync(rc => rc.RecipeId == id && rc.Code.ToUpper() == trimmedCode.ToUpper());

            if (matchedCode == null)
            {
                TempData["ErrorMessage"] = language == "ar" ? "الكود الذي أدخلته غير صحيح." : "The code you entered is incorrect.";
                return RedirectToAction("EnterCode", new { id = id });
            }

            if (matchedCode.IsUsed)
            {
                TempData["ErrorMessage"] = language == "ar" ? "هذا الكود تم استخدامه من قبل." : "This code has already been used.";
                return RedirectToAction("EnterCode", new { id = id });
            }

            if (matchedCode.Recipe == null || string.IsNullOrWhiteSpace(matchedCode.Recipe.FilePath))
            {
                TempData["ErrorMessage"] = language == "ar" ? "خطأ: ملف الوصفة غير موجود. يرجى مراجعة الإدارة." : "Error: Recipe file not found. Please contact administration.";
                return RedirectToAction("EnterCode", new { id = id });
            }

            var filePath = Path.Combine(_env.WebRootPath, matchedCode.Recipe.FilePath.Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMessage"] = language == "ar" ? "عذراً، لم يتم العثور على الملف المطلوب. يرجى مراجعة الإدارة." : "Sorry, the requested file was not found. Please contact administration.";
                return RedirectToAction("EnterCode", new { id = id });
            }

            matchedCode.IsUsed = true;
            _context.RecipeCodes.Update(matchedCode);
            await _context.SaveChangesAsync();

            var mime = "application/zip";
            var fileName = Path.GetFileName(filePath);
            return PhysicalFile(filePath, mime, fileName);
        }

    }
   

}
