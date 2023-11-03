using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OsloMetAngular.Models;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OsloMetAngular.DAL
{
    public class PostDbContext : ApiAuthorizationDbContext<ApplicationUser> // Inherits DbContext
    {
        public PostDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
        {
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
