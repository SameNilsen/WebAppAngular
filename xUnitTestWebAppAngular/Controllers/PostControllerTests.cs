using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OsloMetAngular.Controllers;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;
using OsloMetAngular.ViewModels;

namespace xUnitTestWebAppAngular.Controllers
{
    // TESTS FOR PostController
    public class PostControllerTests
    {
        // Read (CRUD)
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var postList = new List<Post>()
            {
                new() {
                    Title = "Test",
                    Text = "Test",
                    ImageUrl = "assets/images/test.jpg",
                    UserId = 1,
                    PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                    SubForum = "Gaming"
                },
                new() {
                    Title = "Test2",
                    Text = "Test2",
                    ImageUrl = "assets/images/test2.jpg",
                    UserId = 2,
                    PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                    SubForum = "Gaming"
                }
            };

            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.GetAll()).ReturnsAsync(postList);

            var mockLogger = new Mock<ILogger<PostController>>();
            var mockUser = new Mock<IUserRepository>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Mock.Of<IEnumerable<UserValidator<ApplicationUser>>>(),
                Mock.Of<IEnumerable<IPasswordValidator<ApplicationUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>()
                );
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>();

            var mockPostDbContext = new Mock<PostDbContext>();
            var postController = new PostController(
                mockPostRepository.Object, 
                mockUser.Object, 
                mockLogger.Object, 
                mockUserManager.Object, 
                mockSignInManager.Object, 
                mockPostDbContext.Object
                );

            // Act
            var result = await postController.GetAll();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var postListViewModel = Assert.IsAssignableFrom<PostListViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, postListViewModel.Posts.Count());
            Assert.Equal(postList, postListViewModel.Posts);
        }
    }
}
