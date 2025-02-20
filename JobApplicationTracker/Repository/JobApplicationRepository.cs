using JobApplicationTracker.Data;
using JobApplicationTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly JobApplicationDbContext _context;
        private readonly ILogger<JobApplicationRepository> _logger;
        public JobApplicationRepository(JobApplicationDbContext context, ILogger<JobApplicationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<JobApplication>> GetApplicationsAsync()
        {
            try
            { 
                return await _context.Applications.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving job applications.");
                throw;
            }
            
        }

        public async Task<JobApplication?> GetApplicationByIdAsync(int id)
        {
            try
            {
                return await _context.Applications.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving job application with ID {JobApplicationId}", id);
                throw;
            }            
        }

        public async Task AddApplicationAsync(JobApplication application)
        {
            try
            {
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding job application with ID {JobApplicationId}", application.Id);
                throw;
            }
        }

        public async Task UpdateApplicationAsync(JobApplication application)
        {
            try
            {
                _context.Applications.Update(application);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job application with ID {JobApplicationId}", application.Id);
                throw;
            }
        }
    }
}
