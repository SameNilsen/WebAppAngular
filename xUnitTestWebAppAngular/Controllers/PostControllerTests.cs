using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using OsloMetAngular.Controllers;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;
using OsloMetAngular.ViewModels;
using static IdentityModel.ClaimComparer;

namespace xUnitTestWebAppAngular.Controllers
{
    // TESTS FOR PostController
    public class PostControllerTests
    {
        //  POSITIVE TEST.
        // Read (CRUD)
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var postList = new List<Post>()
            {
                new() {
                    PostID = 1,
                    Title = "Test",
                    Text = "Test",
                    ImageUrl = "assets/images/test.jpg",
                    PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                    UpvoteCount = 1,
                    SubForum = "Gaming",
                    User = new User { Name = "BOB", Credebility = 11, UserId = 1}
                },
                new() {
                    PostID = 2,
                    Title = "Test2",
                    Text = "Test2",
                    ImageUrl = "assets/images/test2.jpg",
                    PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                    UpvoteCount = 2,
                    SubForum = "Gaming",
                    User = new User { Name = "BOB", Credebility = 11, UserId = 1}
                }
            };

            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.GetAll()).ReturnsAsync(postList);
            
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockUser = new Mock<IUserRepository>();
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            //  Not in use:
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                //Mock.Of<IUserStore<ApplicationUser>>(),
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new Mock<IEnumerable<UserValidator<ApplicationUser>>>().Object,
                new Mock<IEnumerable<IPasswordValidator<ApplicationUser>>>().Object,
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object
                );

            //  From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            //  Almost the same way to create it, but is able to make objects.
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            //  The constructor for UserManager:
            //public UserManager(IUserStore<TUser> store,
            //    IOptions<IdentityOptions> optionsAccessor,
            //    IPasswordHasher<TUser> passwordHasher,
            //    IEnumerable<IUserValidator<TUser>> userValidators,
            //    IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            //    ILookupNormalizer keyNormalizer,
            //    IdentityErrorDescriber errors,
            //    IServiceProvider services,
            //    ILogger<UserManager<TUser>> logger)
            //private readonly UserManager<ApplicationUser>? _userManager;

            //  The constructor for SignInManager:
            //public SignInManager(UserManager<TUser> userManager,
            //    IHttpContextAccessor contextAccessor,
            //    IUserClaimsPrincipalFactory<TUser> claimsFactory,
            //    IOptions<IdentityOptions> optionsAccessor,
            //    ILogger<SignInManager<TUser>> logger,
            //    IAuthenticationSchemeProvider schemes,
            //    IUserConfirmation<TUser> confirmation)
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                //new Mock<UserManager<ApplicationUser>>().Object,
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            //  We cannot create a mock DbContext (with abstracting and messing around alot):
            //public PostDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            //    : base(options, operationalStoreOptions)
            //    {
            //            Database.EnsureCreated();
            //        }
            //var mockPostDbContext = new Mock<PostDbContext>(
            //    new Mock<DbContextOptions>().Object,
            //    new Mock<IOptions<OperationalStoreOptions>>().Object);

            //var operationalStoreOptions = Microsoft.Extensions.Options.Options.Create(new OperationalStoreOptions());
            
            //var connectionString = "DataSource=PostDatabase.db;Cache=Shared";

            //var options = new DbContextOptionsBuilder<PostDbContext>()
            //    .UseSqlite(connectionString)
            // .Options;
            
            //var realPostDbContext = new PostDbContext(options, operationalStoreOptions);

            var postController = new PostController(
                mockPostRepository.Object, 
                mockUser.Object, 
                mockLogger.Object,
                userManagerMock.Object, 
                signInManagerMock.Object, 
                mockUpvoteRepo.Object
                //mockPostDbContext.Object
                );

            // Act
            var result = await postController.GetAll();

            // Assert
            Assert.Equal(2, 2);    //  Test to see that this stuff actually works.
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from getAll is of correct type 
            var postListOkValue = Assert.IsAssignableFrom<List<Post>>(okResult.Value);  //  Test to see that the list inside return is correct type.
            Assert.Equal(2, postListOkValue.Count());

            //  We cant use this because the controller creates a dummy user in getAll(). So the User
            //   object is not the same before and after. We could, i guess, test to see if the rest 
            //   of the values are the same.
            //Assert.Equal(postList, postListOkValue); 
        }

        // NEGATIVE TEST.
        [Fact]
        public async Task TestCreateFail()
        {
            // Arrange
            var post = new Post
            {
                    PostID = 1,
                    Title = "TestCreate",
                    Text = "Test",
                    ImageUrl = "assets/images/test.jpg",
                    PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                    UpvoteCount = 1,
                    SubForum = "Gaming",
                    //User = new User { Name = "BOB", Credebility = 11, UserId = 1}
            };

            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.Create(post)).ReturnsAsync(false); //  Return false to indicate this is a negative test.

            var mockLogger = new Mock<ILogger<PostController>>();

            //  Set up User Repository. It is used alot by the Create() method, so we have to controll it.
            var mockUser = new Mock<IUserRepository>();
            var user = new User { Name = "BOB", Credebility = 11, UserId = 1 };   //  Dummy User.
            //Task<User?> responseTask = Task.FromResult(user);
            mockUser.Setup(repo => repo.GetUserByIdentity("identityId")).ReturnsAsync(user);  //  When its asks for a user, return dummy.
            mockUser.Setup(repo => repo.Create(user)).ReturnsAsync(false);  //  We dont want it to accidentally create a user hehe.
            mockUser.Setup(repo => repo.Update(user)).ReturnsAsync(false);  //  When creating a post, the user gets credibility, but not now.

            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            //  From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            //  We just return some dummy values. We use It.IsAny<...> beacause it is irrelavant what the input is. We dont
            //   use it in the testing scenario and it just returns dummy values.
            userManagerMock.Setup(repo => repo.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("123abc");
            userManagerMock.Setup(repo => repo.GetUserName(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("Bobby");

            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            //  For now it says you are signed in. Can be switched i guess.
            signInManagerMock.Setup(repo => repo.IsSignedIn(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);

            var postController = new PostController(
                mockPostRepository.Object,
                mockUser.Object,
                mockLogger.Object,
                userManagerMock.Object,
                signInManagerMock.Object,
                mockUpvoteRepo.Object
                );

            // Act
            var result = await postController.Create(post);

            // Assert
            Assert.Equal(2, 2);    //  Test to see that this stuff actually works.
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from Create is of correct type 
            Assert.True(okResult is OkObjectResult);  //  Another way to test return type.
            //  Now we test to see if the Creation actually failed. We know we get an anonymous object
            //   in return from Create() which has a "success" key that is either true or false. It is
            //   not very easy to get values from keys from anonymous objects inbetween functions, but 
            //   we found a way here: https://stackoverflow.com/questions/874746/how-can-i-get-a-value-of-a-property-from-an-anonymous-type
            bool okResultSuccess = (bool)okResult.Value!.GetType().GetProperty("success")!.GetValue(okResult.Value, null)!;
            Assert.False(okResultSuccess);  //  Check if creation failed.

        }
    }
}
