using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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
        }
        [Fact]
        public async Task RemoveStudent_RemovedSuccessfully()
        {
            //Arrange
            int id = 1;
            var response = "the user has been removed successfully";
            _mockService.Setup(s => s.RemoveStudent(id)).ReturnsAsync(response);
            //Act
            var result = await _studentController.RemoveStudent(id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<string>(okResult.Value);
            //Assert
            Assert.Equal("the user has been removed successfully", returnType);
        }
        [Fact]
        public async Task GetStudent()
        {
            //Arrange
            var request = new StudentRequestDto
            {
                Name = "John Doe",
                Email = "JohnDoe@gmail.com"
            };
            var response = new StudentResponseDto
            {
                Name = "John Doe",
                Email = "JohnDoe@gmail.com"
            };
            _mockService.Setup(s => s.GetStudent(request)).ReturnsAsync(response);
            //Act
            var result = await _studentController.GetStudent(request);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<StudentResponseDto>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType.Name);
            Assert.Equal("JohnDoe@gmail.com", returnType.Email);
        }
        [Fact]
        public async Task UpdateStudent()
        {
            //Arrange
            int id = 1;
            var request = new StudentRequestDto
            {
                Name = "John Doe",
                Email = "JD@something.com"
            };
            var response = new StudentResponseDto
            {
                Name = "John Doe",
                Email = "JD@something.com"
            };
            _mockService.Setup(s => s.UpdateStudent(id, request)).ReturnsAsync(response);
            //Act
            var result = await _studentController.UpdateStudent(request, id);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<StudentResponseDto>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType.Name);
            Assert.Equal("JD@something.com", returnType.Email);
        }
        [Fact]
        public async Task GetStudents()
        {
            //Arrange
            var response = new List<StudentResponseDto>
            {
                new StudentResponseDto
                {
                   Name = "John Doe",
                   Email = "JD@something.com"
                }
            };
            _mockService.Setup(s => s.GetStudents()).ReturnsAsync(response);
            //Act
            var result = await _studentController.GetStudents();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<List<StudentResponseDto>>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType[0].Name);
            Assert.Equal("JD@something.com", returnType[0].Email);
        }
        [Fact]
        public async Task FilterStudents()
        {
            //Arrange
            var request = new StudentFilterRequestDto
            {
                CourseId = 1,
            };
            var response = new List<StudentResponseDto>
            {
                new StudentResponseDto
                {
                   Name = "John Doe",
                   Email = "JD@gmail.com"
                }
            };
            _mockService.Setup(s => s.FilterStudents(request)).ReturnsAsync(response);
            //Act
            var result = await _studentController.FilterStudents(request);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<List<StudentResponseDto>>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType[0].Name);
            Assert.Equal("JD@gmail.com", returnType[0].Email);
        }
    }
}