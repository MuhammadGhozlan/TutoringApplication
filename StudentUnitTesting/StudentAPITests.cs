using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using TutoringWebsite.API.Controllers;
using TutoringWebsite.API.DTOs;
using TutoringWebsite.API.Interfaces.IRepositories;

namespace StudentUnitTesting
{
    public class StudentAPITests
    {
        private readonly StudentController _studentController;
        private readonly Mock<ILogger<StudentController>> _mockLogger;
        private readonly Mock<IStudentService> _mockService;
        public StudentAPITests()
        {
            _mockService = new Mock<IStudentService>();
            _mockLogger = new Mock<ILogger<StudentController>>();
            _studentController = new StudentController(_mockService.Object);
        }
        [Fact]
        public async Task AddStudent_ShouldAddSuccessfully()
        {
            //Arrange
            var request = new StudentRequestDto
            {
                Name = "John Doe",
                Email = "JohnDoe@something.com"
            };
            var response = new StudentResponseDto
            {
                Name = "John Doe",
                Email = "JohnDoe@something.com"
            };
            _mockService.Setup(s => s.AddStudent(request)).ReturnsAsync(response);
            //Act
            var result = await _studentController.AddStudent(request);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<StudentResponseDto>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType.Name);
            Assert.Equal("JohnDoe@something.com", returnType.Email);

            var json = JsonSerializer.Serialize(returnType);
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "s.json");
            File.WriteAllText(file, json);

        }
        [Fact]
        public async Task RemoveStudent_RemovedSuccessfully()
        {
            //Arrange
            int id = 1;
            var response = "the user has been removed successfully";
            _mockService.Setup(s=>s.RemoveStudent(id)).ReturnsAsync(response);
            //Act
            var result= await _studentController.RemoveStudent(id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<string>(okResult.Value);
            //Assert
            Assert.Equal("the user has been removed successfully", returnType);
        }
    }
}