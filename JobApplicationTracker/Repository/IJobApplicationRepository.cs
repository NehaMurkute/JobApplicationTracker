using JobApplicationTracker.Model;

namespace JobApplicationTracker.Repository
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetApplicationsAsync();
        Task<JobApplication?> GetApplicationByIdAsync(int id);
        Task AddApplicationAsync(JobApplication application);
        Task UpdateApplicationAsync(JobApplication application);
    }
}
