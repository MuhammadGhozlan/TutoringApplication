using Castle.Core.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutoringWebsite.API.Controllers;
using TutoringWebsite.API.Interfaces.IServices;
using Microsoft.Extensions.Logging;
using TutoringWebsite.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace TutoringUnitTesting
{
    public class InstructorAPITests
    {
        private readonly Mock<IInstructorService> _mockService;
        private readonly InstructorController _instructorController;
        private readonly Mock<ILogger<InstructorController>> _logger;

        public InstructorAPITests()
        {
            _mockService = new Mock<IInstructorService>();
            _logger = new Mock<ILogger<InstructorController>>();
            _instructorController = new InstructorController(_mockService.Object);
        }
        [Fact]
        public async Task AddInstructor_AddedSuccessfly()
        {
            //Arrange
            var request = new InstructorRequestDto
            {
                Name = "Mdhat",
                DoB = new DateTime(1990, 10, 27),
                Email = "Mdhat@gmail.com"
            };
            var response = new InstructorResponseDto
            {
                Name = "Mdhat",
                DoB = new DateTime(1990, 10, 27),
                Email = "Mdhat@gmail.com"
            };
            _mockService.Setup(i => i.AddInstructor(request)).ReturnsAsync(response);
            var result = await _instructorController.AddInstructor(request);
            //Act
            var okResponse = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<InstructorResponseDto>(okResponse.Value);
            //Assert
            Assert.Equal("Mdhat", returnType.Name);
            Assert.Equal(new DateTime(1990, 10, 27), returnType.DoB);
            Assert.Equal("Mdhat@gmail.com", returnType.Email);
        }
        [Fact]
        public async Task RemoveInstructor_RemovedSuccessfully()
        {
            //Arrange
            var request = 1;
            var response = "removed successfully" ;
            _mockService.Setup(i => i.RemoveInstructor(request)).ReturnsAsync(response);
            //Act
            var result = await _instructorController.RemoveInstructor(request);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<string>(okResult.Value);
            //Assert
            Assert.Equal("removed successfully", returnType);
        }
        [Fact]
        public async Task GetInstructor_RetrievedSuccessfully()
        {
            //Arrange
            var request = new InstructorRequestDto()
            {
                Name = "John Doe",
                DoB = new DateTime(1980 - 04 - 13),
                Email = "JohnDoe@something.com"
            };
            var response = new InstructorResponseDto()
            {
                Name = "John Doe",
                DoB = new DateTime(1980 - 04 - 13),
                Email = "JohnDoe@something.com"
            };
            _mockService.Setup(i => i.GetInstructor(request)).ReturnsAsync(response);
            //Act
            var result = await _instructorController.GetInstructor(request);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<InstructorResponseDto>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType.Name);
            Assert.Equal(new DateTime(1980 - 04 - 13), returnType.DoB);
            Assert.Equal("JohnDoe@something.com", returnType.Email);
        }
        [Fact]
        public async Task GetInstructors_RetrievedSuccessfully()
        {
            //Arrange             
            var response = new List<InstructorResponseDto>()
            {
                new InstructorResponseDto
                {
                   Name = "John Doe",
                   DoB = new DateTime(1980 - 04 - 13),
                   Email = "JohnDoe@something.com"
                }
            };
            _mockService.Setup(i => i.GetInstructors()).ReturnsAsync(response);
            //Act
            var result = await _instructorController.GetInstructors();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<List<InstructorResponseDto>>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType[0].Name);
            Assert.Equal(new DateTime(1980 - 04 - 13), returnType[0].DoB);
            Assert.Equal("JohnDoe@something.com", returnType[0].Email);
        }
        [Fact]
        public async Task GetFilteredInstructors_RetrievedSuccessfully()
        {
            //Arrange
            var request = new InstructorFilterRequestDto
            {
                CourseName = "mathematics"
            };
            var response = new List<InstructorResponseDto>()
            {
                new InstructorResponseDto
                {
                   Name = "John Doe",
                   DoB = new DateTime(1980 - 04 - 13),
                   Email = "JohnDoe@something.com"
                }
            };
            _mockService.Setup(i => i.GetFilteredInstructors(request)).ReturnsAsync(response);
            //Act
            var result = await _instructorController.FilterInstructors(request);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnType = Assert.IsType<List<InstructorResponseDto>>(okResult.Value);
            //Assert
            Assert.Equal("John Doe", returnType[0].Name);
            Assert.Equal(new DateTime(1980 - 04 - 13), returnType[0].DoB);
            Assert.Equal("JohnDoe@something.com", returnType[0].Email);
        }
    }
}
