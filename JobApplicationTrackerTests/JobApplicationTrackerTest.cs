using Moq;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.Model;
using Microsoft.AspNetCore.Mvc;


namespace JobApplicationTrackerTests{

    public class JobApplicationTrackerTest
    {
        private readonly Mock<IJobApplicationRepository> _mockRepo;
        private readonly JobApplicationsController _controller;

        public JobApplicationTrackerTest()
        {
            _mockRepo = new Mock<IJobApplicationRepository>();
            _controller = new JobApplicationsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetApplications_ReturnsOkResult_WithListOfApplications()
        {
            // Arrange
            var applications = new List<JobApplication>
        {
            new JobApplication { Id = 1, CompanyName = "Company A", Position = "Developer", Status = "Applied", DateApplied = DateTime.Now },
            new JobApplication { Id = 2, CompanyName = "Company B", Position = "Manager", Status = "Interview", DateApplied = DateTime.Now }
        };
            _mockRepo.Setup(repo => repo.GetApplicationsAsync()).ReturnsAsync(applications);

            // Act
            var result = await _controller.GetApplications();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<JobApplication>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetApplication_ReturnsOkResult_WhenApplicationExists()
        {
            // Arrange
            var application = new JobApplication { Id = 1, CompanyName = "Company A", Position = "Developer", Status = "Applied", DateApplied = DateTime.Now };
            _mockRepo.Setup(repo => repo.GetApplicationByIdAsync(1)).ReturnsAsync(application);

            // Act
            var result = await _controller.GetApplication(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<JobApplication>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetApplication_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            var application = new JobApplication { Id = 1, CompanyName = "Company A", Position = "Developer", Status = "Applied", DateApplied = DateTime.Now };
            _mockRepo.Setup(repo => repo.GetApplicationByIdAsync(1)).ReturnsAsync(application);

            // Act
            var result = await _controller.GetApplication(2);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }      

        [Fact]
        public async Task AddApplication_ReturnsCreatedAtAction_WhenValidApplicationIsAdded()
        {
            //Arrange
            var application = new JobApplication { Id = 1, CompanyName = "Company B", Position = "Senior Dev", Status = "Applied", DateApplied = DateTime.Now };
            _mockRepo.Setup(repo => repo.AddApplicationAsync(application)).Returns(Task.CompletedTask);

            //Act
            var result = await _controller.AddApplication(application);

            //Assert
            var createAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetApplication), createAtActionResult.ActionName);

        }

        [Fact]
        public async Task AddApplication_ReturnsBadRequest_WhenInValidApplicationIsAdded()
        {
            //Arrange
            _controller.ModelState.AddModelError("CompanyName", "Required");
            var application = new JobApplication { Id = 1, CompanyName = "", Position = "Tester", Status = "Applied", DateApplied = DateTime.Now };
            _mockRepo.Setup(repo => repo.AddApplicationAsync(application)).Returns(Task.CompletedTask);

            //Act
            var result = await _controller.AddApplication(application);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateApplication_ReturnsBadRequest_WhenInValidApplicationIdIsPassed() 
        {
            //Arrange
            var application = new JobApplication { Id = 1, CompanyName = "Company C", Position = "Developer", Status = "Offer", DateApplied = DateTime.Now };
            
            //Act
            var result = await _controller.UpdateApplication(2,application);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateApplication_ReturnsNotFound_WhenApplicationIsNull()
        {
            //Arrange
            var application = new JobApplication { Id = 3, CompanyName = "Company D", Position = "Dev Lead", Status = "Offer", DateApplied = DateTime.Now };
            _mockRepo.Setup(repo => repo.GetApplicationByIdAsync(1)).ReturnsAsync((JobApplication)null);

            //Act
            var result = await _controller.UpdateApplication(3, application);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateApplication_ReturnsNoContent_WhenValidApplicationIsUpdated()
        {
            //Arrange
            var existingApplication = new JobApplication { Id = 1, CompanyName = "Comapny A", Position = "Full Stack Developer", Status = "Interview", DateApplied = DateTime.Now };
            var updatedApplication = new JobApplication { Id = 1, CompanyName = "Comapny A", Position = "Full Stack Developer", Status = "Offer", DateApplied = DateTime.Now };

            _mockRepo.Setup(repo => repo.GetApplicationByIdAsync(1)).ReturnsAsync(existingApplication);
            _mockRepo.Setup(repo => repo.UpdateApplicationAsync(updatedApplication));

            //Act
            var result = await _controller.UpdateApplication(1, updatedApplication);

            //Assert
            Assert.IsType<NoContentResult>(result);

        }
    }
}
