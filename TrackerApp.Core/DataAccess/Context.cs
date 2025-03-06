using Microsoft.EntityFrameworkCore;
using TrackerApp.Core.Models;

namespace TrackerApp.Core.DataAccess
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Location> Location { get; set; }

        public DbSet<User> User { get; set; }

    }
}
