global using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Infrastructure.Configuration;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using ECommerceNet8.Infrastructure.Data.ReturnExchangeModels;
using ECommerceNet8.Infrastructure.Data.ShoppingCartModels;
using ECommerceNet8.Models.OrderModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace ECommerceNet8.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        

         
        }

        public DbSet<RefreshToken> RefreshTokens {  get; set; }
        public DbSet<MainCategorie> mainCategories { get; set; }

        public DbSet<Material> Materials { get; set; }
        public DbSet<BaseProduct> BaseProducts {  get; set; }
        public DbSet<ImageBase> ImageBases {  get; set; }

        public DbSet<ProductVariant> productVariants {  get; set; }
        public DbSet<ProductSize> productSizes {  get; set; }
        public DbSet<ProductColor> productColors {  get; set; }

        //public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        //public DbSet<CartItem> CartItems {  get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set;}
        public DbSet<PdfInfo> PdfInfos { get; set; }
        public DbSet<Address> Addresses { get; set; }


    }
}
