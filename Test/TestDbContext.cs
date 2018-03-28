using Microsoft.EntityFrameworkCore;
using Test.BLL;

namespace Test
{
    public class TestDbContext: DbContext
    {
        public DbSet<Graph> Graphs { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        public DbSet<Test.BLL.Node> Node { get; set; }
    }
}
