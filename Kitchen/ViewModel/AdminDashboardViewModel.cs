using Kitchen.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Kitchen.ViewModel
{
    public class AdminDashboardViewModel
    {
        public List<Recipe> Recipes { get; set; }
        public List<SectionContent> SectionContents { get; set; }
        public List<Offer> Offers { get; set; }


        public Dictionary<string, string> TypeTranslations { get; } = new Dictionary<string, string>
        {
            { "Agricultural", "زراعية" },
            { "Chemical", "كيميائية" },
            { "Nutritional", "غذائية" },
            { "Pharmacy" , "صيدلي" },
            { "cosmetics","مستحضرات تجميل" },
            { "Feasibility Studies", "دراسات جدوى" },
            { "Industrial Models", "نماذج صناعية" }
        };

       
        public AdminDashboardViewModel()
        {
            Recipes = new List<Recipe>();
            SectionContents = new List<SectionContent>();
            Offers = new List<Offer>();
        }
    }
}
