using Kitchen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kitchen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeCode> RecipeCodes { get; set; }
        public DbSet<SectionContent> SectionContents { get; set; }
        public DbSet<Offer>Offers { get; set; }

        public DbSet<ContactInfo> contactInfos { get; set; }
        public DbSet<PaymentMethod> paymentMethods { get; set; }




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        
        }

        internal void saveChanges()
        {
            throw new NotImplementedException();
        }
    }
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }

}
