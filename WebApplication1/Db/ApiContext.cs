using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApi.Db
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)  { }
        public DbSet<Invoice> Invoices { get; set; }
        
    }
}
