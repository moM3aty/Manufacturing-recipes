using Kitchen.Data;
using Kitchen.Models;
using Kitchen.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kitchen.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RecipeController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult ToggleLanguage(string returnUrl)
        {
            // Get the current language from the cookies (default is Arabic)
            var currentLanguage = Request.Cookies["Language"] ?? "ar";
            var newLanguage = currentLanguage == "ar" ? "en" : "ar";

            // Set the new language cookie
            Response.Cookies.Append("Language", newLanguage, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddYears(1),
                IsEssential = true
            });

            // Redirect to the previous page
            return LocalRedirect(returnUrl);
        }

        public IActionResult Agriculture()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Agriculture";

            var recipes = _context.Recipes.Where(x => x.RecipeType == "Agricultural").ToList();
            return View(recipes);
        }

        public IActionResult Chemical()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Chemical";

            var recipes = _context.Recipes.Where(x => x.RecipeType == "Chemical").ToList();
            return View(recipes);
        }
        public IActionResult clinic()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "clinic";

            var recipes = _context.Recipes.Where(x => x.RecipeType == "Pharmacy").ToList();
            return View(recipes);
        }
        public IActionResult makeup()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "makeup";

            var recipes = _context.Recipes.Where(x => x.RecipeType == "cosmetics").ToList();
            return View(recipes);
        }
        public IActionResult food()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "food";

            var recipes = _context.Recipes.Where(x => x.RecipeType == "Nutritional").ToList();
            return View(recipes);
        }
        public IActionResult Contact()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Contact";
            var contacts = _context.contactInfos?.ToList() ?? new List<ContactInfo>();
            var payments = _context.paymentMethods?.ToList() ?? new List<PaymentMethod>();

            var model = new ContactViewModel
            {
                contacts = contacts,
                paymentMethods = payments
            };
            return View(model);
        }

        public IActionResult Index()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "Index";
            
            var contents = _context.SectionContents.ToList();

            // Localize the content
            ViewData["HeroTitle"] = GetLocalizedText(contents, "Hero_Title");
            ViewData["HeroDesc"] = GetLocalizedText(contents, "Hero_Desc");
            ViewData["HeroButton"] = GetLocalizedText(contents, "Hero_Button");
            ViewData["whoUS"] = GetLocalizedText(contents, "Who_US");
            ViewData["whoDesc"] = GetLocalizedText(contents, "Who_Desc");
            ViewData["whoDesc2"] = GetLocalizedText(contents, "Who_Desc2");

            var model = new AdminDashboardViewModel
            {
                Recipes = _context.Recipes.Where(x => x.RecipeType == "Nutritional").ToList(),

                Offers = _context.Offers.ToList()
            };

            return View(model);

        }


        public class LanguageMiddleware
        {
            private readonly RequestDelegate _next;

            public LanguageMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var language = context.Request.Cookies["Language"] ?? "ar";
                var direction = language == "ar" ? "rtl" : "ltr";

                if (context.Items["ViewData"] is ViewDataDictionary viewData)
                {
                    viewData["Language"] = language;
                    viewData["Direction"] = direction;
                    viewData["Controller"] = "Recipe";
                    viewData["Action"] = "";
                }

                await _next(context);
            }
        }

        // Show code input form
        public IActionResult EnterCode(int id)
        {
            var language = Request.Cookies["Language"] ?? "ar";
            var direction = language == "ar" ? "rtl" : "ltr";

            ViewData["Direction"] = direction;
            ViewData["Language"] = language;
            ViewData["Controller"] = "Recipe";
            ViewData["Action"] = "";

            return View(id);
        }

            // Handle code verification and file download
            [HttpPost]
        public IActionResult VerifyCode(int id, string code)
        {
            var matchedCode = _context.RecipeCodes
                .Include(rc => rc.Recipe)
                .FirstOrDefault(rc => rc.RecipeId == id && rc.Code == code && !rc.IsUsed);

            if (matchedCode != null)
            {
                matchedCode.IsUsed = true;
                _context.SaveChanges();

                var filePath = Path.Combine(_env.WebRootPath, matchedCode.Recipe.FilePath);

                if (!System.IO.File.Exists(filePath))
                {
                    ModelState.AddModelError("", "عذراً، لم يتم العثور على الملف.");
                    return View("EnterCode", id);
                }

                var mime = "application/zip";
                var fileName = Path.GetFileName(filePath);
                return PhysicalFile(filePath, mime, fileName);
            }

            ModelState.AddModelError("", "الكود غير صحيح أو تم استخدامه بالفعل.");
            return View("EnterCode", id);
        }

        private string GetLocalizedText(List<SectionContent> contents, string key)
        {
            var content = contents.FirstOrDefault(c => c.Key == key);
            var direction = GetDirection();

            return direction == "rtl" ? content?.TextAr : content?.TextEn;
        }

        private string GetDirection()
        {
            var language = Request.Cookies["Language"] ?? "ar";
            return language == "ar" ? "rtl" : "ltr";
        }
    }
}
