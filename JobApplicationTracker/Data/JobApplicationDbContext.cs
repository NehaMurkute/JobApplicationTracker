using JobApplicationTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data
{
    public class JobApplicationDbContext : DbContext
    {
        public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : base(options) { }

        public DbSet<JobApplication> Applications { get; set; }
    }
}
