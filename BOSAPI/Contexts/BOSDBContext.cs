using BOSAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BOSAPI.Contexts
{
    public class BOSDBContext : DbContext
    {
        public BOSDBContext(DbContextOptions<BOSDBContext> options) : base(options) { }

        public virtual DbSet<User> User { get; set; }
    }
}
