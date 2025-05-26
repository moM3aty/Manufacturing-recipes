using Kitchen.Models;

namespace Kitchen.ViewModel
{
    public class AdminDashboardViewModel
    {
        public List<Recipe> Recipes { get; set; }
        public List<SectionContent> SectionContents { get; set; }
        public List<Offer> Offers { get; set; }

    }

}
