using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Models;

namespace ShoppingAPI.Models
{
    public class DBContext :DbContext
    {

        public DBContext(DbContextOptions options) : base(options) { }
        public DbSet<ShoppingAPI.Models.CartItem> CartItem { get; set; } = default!;
        public DbSet<ShoppingAPI.Models.InventoryItem> InventoryItem { get; set; } = default!;
        public DbSet<ShoppingAPI.Models.ShoppingCart> ShoppingCart { get; set; } = default!;
        public DbSet<ShoppingAPI.Models.StoreFront> StoreFront { get; set; } = default!;

    }
}
