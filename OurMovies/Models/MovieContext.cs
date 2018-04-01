using Microsoft.EntityFrameworkCore;

namespace OurMovies.Models
{
    public class MovieContext:DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {

        }

        public DbSet<Movie> Movie { get; set; }
    }
}
