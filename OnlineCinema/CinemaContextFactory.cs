using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineCinema
{
    public class CinemaContextFactory : IDesignTimeDbContextFactory<CinemaContext>
    {
        public CinemaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CinemaContext>();

            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS02;Initial Catalog=CinemaDb;Integrated Security=True;Trust Server Certificate=True");

            return new CinemaContext(optionsBuilder.Options);
        }
    }
}
