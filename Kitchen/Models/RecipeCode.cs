namespace Kitchen.Models
{
    public class RecipeCode
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; }

        public Recipe Recipe { get; set; }
    }
}
