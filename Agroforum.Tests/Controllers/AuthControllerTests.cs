using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Services.Auth;
using Agroforum.Application.ViewModels.Auth;
using Agroforum.Domain;
using Agroforum.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Agroforum.Tests.Controllers
{
    public class AuthControllerTests : ControllerTests
    {
        public AuthControllerTests() : base()
        {
        }

        [Fact]
        public async Task Register_ValidData_ReturnsOkResult()
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                // Arrange
                var authService = new AuthService(DbContext, Configuration);
                var controller = new AuthController(authService);

                //Act
                var registerDto = new RegisterDto
                {
                    Name = Guid.NewGuid().ToString(),
                    Surname = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString() + "@example.com",
                    Password = "password123",
                    ConfirmPassword = "password123",
                    IsFarmer = true
                };

                var result = await controller.Register(registerDto) as NoContentResult;

                //Assert
                Assert.NotNull(result);
                Assert.Equal(204, result.StatusCode);
                var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Name == registerDto.Name
                                                                        && a.Surname == registerDto.Surname
                                                                        && a.Password == registerDto.Password);
                Assert.NotNull(account);
            }
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                // Arrange
                var authService = new AuthService(DbContext, Configuration);
                var controller = new AuthController(authService);
                var user = new Account
                {
                    Name = Guid.NewGuid().ToString(),
                    Surname = Guid.NewGuid().ToString(),
                    Email = $"{Guid.NewGuid()}@example.com",
                    Password = "password123",
                    Roles = new List<Role>()
                };

                DbContext.Accounts.Add(user);
                await DbContext.SaveChangesAsync();

                // Act
                var loginDto = new LoginDto
                {
                    Email = user.Email,
                    Password = "password123"
                };

                var result = await controller.Login(loginDto) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);

                var jwtVm = result.Value as JwtVm;
                Assert.NotNull(jwtVm);
                Assert.NotNull(jwtVm.Token);

            }
            
        }
    }

}
