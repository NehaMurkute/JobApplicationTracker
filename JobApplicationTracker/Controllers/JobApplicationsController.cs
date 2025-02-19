using JobApplicationTracker.Model;
using JobApplicationTracker.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers
{
    [ApiController]
    [Route("api/applications")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationRepository _repository;

        public JobApplicationsController(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications() { 
            return Ok(await _repository.GetApplicationsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _repository.GetApplicationByIdAsync(id);
            if (application == null)
                return NotFound();
            return Ok(application);
        }

        [HttpPost]
        public async Task<IActionResult> AddApplication(JobApplication application)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddApplicationAsync(application);
            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplication(int id, JobApplication application)
        {
            if (id != application.Id)
                return BadRequest();

            var existingApplication = await _repository.GetApplicationByIdAsync(id);
            if (existingApplication == null)
                return NotFound();

            existingApplication.Status = application.Status;
            await _repository.UpdateApplicationAsync(existingApplication);

            return NoContent();
        }
    }
}
