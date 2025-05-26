namespace Kitchen.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public string SloganAr { get; set; }
        public string SloganEn { get; set; }

        public string FilePath { get; set; }
        public string ImagePath { get; set; }
        public string RecipeType { get; set; } 
    }
}
