
using Microsoft.EntityFrameworkCore;

namespace EF_ConcurrencyTest.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<Student> Students { get; set; }
    }
}
