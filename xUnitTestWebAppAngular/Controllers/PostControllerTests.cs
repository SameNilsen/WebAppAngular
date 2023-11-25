using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using OsloMetAngular.Controllers;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;
using Xunit.Abstractions;

namespace xUnitTestWebAppAngular.Controllers
{
    // TESTS FOR PostController
    public class PostControllerTests
    {

        private readonly ITestOutputHelper output;
        public PostControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // POSITIVE TEST (READ)
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

            // Mocking the post repository
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.GetAll()).ReturnsAsync(postList);
            
            // Mocking the other parameters in the post controller
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockUser = new Mock<IUserRepository>();
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // Setting up the our new post controller with mocked objects
            var postController = new PostController(
                mockPostRepository.Object, 
                mockUser.Object, 
                mockLogger.Object,
                userManagerMock.Object, 
                signInManagerMock.Object, 
                mockUpvoteRepo.Object
                );

            // Act
            var result = await postController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from getAll() is of correct type 
            var postListOkValue = Assert.IsAssignableFrom<List<Post>>(okResult.Value);  //  Test to see that the list inside return is correct type.
            Assert.Equal(2, postListOkValue.Count);

            /*  
                We cannot compare the objects from the lists directly because the controller creates a new instance
                of the post, so the object references are not the same before and after. But the values should be
                the same, so if we instead serialize a object, then we can compare the strings. We have done that for
                two of the objects. We also print them out in the Test Explorer window, for visualization.
            */
            string postListObject = JsonConvert.SerializeObject(postList[0], Formatting.Indented);
            output.WriteLine(postListObject);

            string postListOkValueObject = JsonConvert.SerializeObject(postListOkValue[0], Formatting.Indented);
            output.WriteLine(postListOkValueObject);

            Assert.Equal(postListObject, postListOkValueObject);
        }

        // NEGATIVE TEST (READ)
        [Fact]
        public async Task TestGetAllFail()
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

            // Mocking the post repository
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.GetAll()).Returns(Task.FromResult<IEnumerable<Post>>(null));

            // Mocking the other parameters in the post controller
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockUser = new Mock<IUserRepository>();
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            //  Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // Setting up the our new post controller with mocked objects
            var postController = new PostController(
                mockPostRepository.Object,
                mockUser.Object,
                mockLogger.Object,
                userManagerMock.Object,
                signInManagerMock.Object,
                mockUpvoteRepo.Object
                );

            // Act
            var result = await postController.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);    //  Test to see the return from getAll is of correct type 
            var notFoundValue = Assert.IsAssignableFrom<string>(notFoundResult.Value);  //  Test to see that the value inside return is correct type.
            var expectedErrorMessage = "Post list not found";
            Assert.Equal(expectedErrorMessage, notFoundValue);
        }

        // POSITIVE TEST (CREATE)
        [Fact]
        public async Task TestCreate()
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

            // Mocking the post repository
            var mockPostRepository = new Mock<IPostRepository>();
            
            /*
                Normally, we would put "post" as input in repo.Create(), but because we tweak the Post a bit 
                in the controller (we set user to be the logged in user), the post from test is not the same as 
                the one ultimately used in the Create() method. In those cases that they are not the same, 
                this setup up thing will automatically return false (or null in other cases). So we instead
                put It.IsAny<Post>() because the actual Post argument is not relevant in testing.
            */
            mockPostRepository.Setup(repo => repo.Create(It.IsAny<Post>())).ReturnsAsync(true); //  Return true to indicate this is a positive test.

            // Mocking the logger
            var mockLogger = new Mock<ILogger<PostController>>();

            // Set up User Repository. It is used alot by the Create() method, so we have to controll it.
            var mockUser = new Mock<IUserRepository>();
            var user = new User { Name = "BOB", Credebility = 11, UserId = 1 };   //  Dummy User.
            mockUser.Setup(repo => repo.GetUserByIdentity("identityId")).ReturnsAsync(user);  //  When its asks for a user, return dummy.
            mockUser.Setup(repo => repo.Create(user)).ReturnsAsync(false);  //  We dont want it to accidentally create a user hehe.
            mockUser.Setup(repo => repo.Update(user)).ReturnsAsync(false);  //  When creating a post, the user gets credibility, but not now.

            // Mocking the upvote repository
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>()  ,
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // We use It.IsAny<...> beacause it is irrelavant what the input is. We dont
            // use it in the testing scenario and it just returns dummy values.
            userManagerMock.Setup(repo => repo.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("123abc");
            userManagerMock.Setup(repo => repo.GetUserName(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("Bobby");

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // You are signed in (can be changed)
            signInManagerMock.Setup(repo => repo.IsSignedIn(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);

            // Setting up the our new post controller with mocked objects
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
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from Create is of correct type 
            Assert.True(okResult is OkObjectResult);  //  Another way to test return type.

            /*
                Now we test to see if the Creation actually worked. We know we get an anonymous object from the value of the OkObjectResult
                in return from Create(), which has a "success" key that is either true or false. It is
                not very easy to get values from keys from anonymous objects inbetween functions, but we found a way here: 
                https://stackoverflow.com/questions/874746/how-can-i-get-a-value-of-a-property-from-an-anonymous-type
            */
            bool okResultSuccess = (bool)okResult.Value!.GetType().GetProperty("success")!.GetValue(okResult.Value, null)!;  //  Should be "true".
            Assert.True(okResultSuccess);  //  Check if creation worked.

            //  We can also check to see if we get the correct message string in return. If creation is
            //  successful then we can also check to see if the post Title remains the same.
            string okResultMessage = (string)okResult.Value!.GetType().GetProperty("message")!.GetValue(okResult.Value, null)!;
            var expectedOkresultMessage = "Post " + post.Title + " created succesfully";
            Assert.Equal(expectedOkresultMessage, okResultMessage);  //  Check to see if we get correct message.
        }

        // NEGATIVE TEST (CREATE)
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

            // Mocking the post repository
            var mockPostRepository = new Mock<IPostRepository>();

            /* 
               Normally, we would put "post" as input in repo.Create(), but because we tweak the Post a bit
               in the controller (we set user to be the logged in user), the post from test is not the same as 
               the one ultimately used in the Create() method. In those cases that they are not the same, 
               this setup up thing will automatically return false (or null in other cases). So we instead
               put It.IsAny<Post>() because the actual Post argument is not relevant in testing.
            */
            mockPostRepository.Setup(repo => repo.Create(It.IsAny<Post>())).ReturnsAsync(false); // Return false to indicate this is a negative test.

            // Mocking the logger
            var mockLogger = new Mock<ILogger<PostController>>();

            // Set up User Repository. It is used alot by the Create() method, so we have to control it.
            var mockUser = new Mock<IUserRepository>();
            var user = new User { Name = "BOB", Credebility = 11, UserId = 1 };   // Dummy User.
            mockUser.Setup(repo => repo.GetUserByIdentity("identityId")).ReturnsAsync(user);  // When its asks for a user, return dummy.
            mockUser.Setup(repo => repo.Create(user)).ReturnsAsync(false);  // We dont want it to accidentally create a user hehe.
            mockUser.Setup(repo => repo.Update(user)).ReturnsAsync(false);  // When creating a post, the user gets credibility, but not now.

            // Mocking the upvote repository
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            //  We use It.IsAny<...> beacause it is irrelavant what the input is. We dont
            //  use it in the testing scenario and it just returns dummy values.
            userManagerMock.Setup(repo => repo.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("123abc");
            userManagerMock.Setup(repo => repo.GetUserName(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("Bobby");

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            //  You are signed in (can be changed)
            signInManagerMock.Setup(repo => repo.IsSignedIn(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);

            // Setting up the our new post controller with mocked objects
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
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from Create is of correct type 
            Assert.True(okResult is OkObjectResult);  //  Another way to test return type.

            /*  
                Now we test to see if the Creation actually failed. We know we get an anonymous object
                in return from Create() which has a "success" key that is either true or false. It is
                not very easy to get values from keys from anonymous objects inbetween functions, but we found a way here: 
                https://stackoverflow.com/questions/874746/how-can-i-get-a-value-of-a-property-from-an-anonymous-type
            */
            bool okResultSuccess = (bool)okResult.Value!.GetType().GetProperty("success")!.GetValue(okResult.Value, null)!;
            Assert.False(okResultSuccess);  //  Check if creation failed.
            string okResultMessage = (string)okResult.Value!.GetType().GetProperty("message")!.GetValue(okResult.Value, null)!;
            var expectedOkresultMessage = "Post creation failed";
            Assert.Equal(expectedOkresultMessage, okResultMessage);  //  Check to see if we get correct message.        
        }

        // POSITIVE TEST (UPDATE)
        [Fact]
        public async Task TestUpdate()
        {
            // Arrange
            var post = new Post
            {
                PostID = 1,
                Title = "TestUpdate",
                Text = "Test",
                ImageUrl = "assets/images/test.jpg",
                PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                UpvoteCount = 1,
                SubForum = "Gaming",
                //User = new User { Name = "BOB", Credebility = 11, UserId = 1}
            };

            // Mocking the post repository
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.Update(post)).ReturnsAsync(true);

            // Mocking the logger
            var mockLogger = new Mock<ILogger<PostController>>();

            // Set up User Repository. It is used alot by the Create() method, so we have to control it.
            var mockUser = new Mock<IUserRepository>();
            var user = new User { Name = "ALICE", Credebility = 9, UserId = 2 };   //  Dummy User.
            mockUser.Setup(repo => repo.GetUserByIdentity("identityId")).ReturnsAsync(user);  //  When its asks for a user, return dummy.
            mockUser.Setup(repo => repo.Create(user)).ReturnsAsync(false);  //  We dont want it to accidentally create a user hehe.
            mockUser.Setup(repo => repo.Update(user)).ReturnsAsync(false);  //  When creating a post, the user gets credibility, but not now.
           
            // Mocking the upvote repository
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // We use It.IsAny<...> beacause it is irrelavant what the input is. We dont
            // use it in the testing scenario and it just returns dummy values.
            userManagerMock.Setup(repo => repo.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("123abc");
            userManagerMock.Setup(repo => repo.GetUserName(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("Bobby");

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // You are signed in (can be changed)
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
            var result = await postController.Update(post);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from Update is of correct type 
            Assert.True(okResult is OkObjectResult);  //  Another way to test return type.

            /*  
                Now we test to see if Update actually works. We know we get an anonymous object
                in return from Update() which has a "success" key that is either true or false. It is
                not very easy to get values from keys from anonymous objects inbetween functions, but we found a way here:
                https://stackoverflow.com/questions/874746/how-can-i-get-a-value-of-a-property-from-an-anonymous-type
            */
            bool okResultSuccess = (bool)okResult.Value!.GetType().GetProperty("success")!.GetValue(okResult.Value, null)!;
            Assert.True(okResultSuccess);  //  Check if update works.
        }

        // NEGATIVE TEST (UPDATE)
        [Fact]
        public async Task TestUpdateFail()
        {
            // Arrange
            var post = new Post
            {
                PostID = 1,
                Title = "TestUpdate",
                Text = "Test",
                ImageUrl = "assets/images/test.jpg",
                PostDate = new DateTime(2000, 1, 1, 12, 12, 0).ToString("dd.MM.yyyy HH:mm"),
                UpvoteCount = 1,
                SubForum = "Gaming",
                //User = new User { Name = "BOB", Credebility = 11, UserId = 1}
            };

            // Mocking the post repository
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repo => repo.Update(post)).ReturnsAsync(false);

            // Mocking the logger
            var mockLogger = new Mock<ILogger<PostController>>();

            // Set up User Repository. It is used alot by the Create() method, so we have to controll it.
            var mockUser = new Mock<IUserRepository>();
            var user = new User { Name = "ALICE", Credebility = 9, UserId = 2 };   //  Dummy User.
            mockUser.Setup(repo => repo.GetUserByIdentity("identityId")).ReturnsAsync(user);  //  When its asks for a user, return dummy.
            mockUser.Setup(repo => repo.Create(user)).ReturnsAsync(false);  //  We dont want it to accidentally create a user hehe.
            mockUser.Setup(repo => repo.Update(user)).ReturnsAsync(false);  //  When creating a post, the user gets credibility, but not now.
            
            // Mocking the upvote repository
            var mockUpvoteRepo = new Mock<IUpVoteRepository>();

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            //  We use It.IsAny<...> beacause it is irrelavant what the input is. We dont
            //  use it in the testing scenario and it just returns dummy values.
            userManagerMock.Setup(repo => repo.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("123abc");
            userManagerMock.Setup(repo => repo.GetUserName(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("Bobby");

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            //  You are signed in (can be changed)
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
            var result = await postController.Update(post);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  //  Test to see the return from Update is of correct type 
            Assert.True(okResult is OkObjectResult);  //  Another way to test return type.

            /*  
                Now we test to see if Update actually failed. We know we get an anonymous object
                in return from Update() which has a "success" key that is either true or false. It is
                not very easy to get values from keys from anonymous objects inbetween functions, but
                https://stackoverflow.com/questions/874746/how-can-i-get-a-value-of-a-property-from-an-anonymous-type
            */

            bool okResultSuccess = (bool)okResult.Value!.GetType().GetProperty("success")!.GetValue(okResult.Value, null)!;
            Assert.False(okResultSuccess);  //  Check if update failed.
        }

        // POSITIVE TEST (DELETE)
        [Fact]
        public async Task TestDelete()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>(); // Mocking the post repository
            mockPostRepository.Setup(repo => repo.Delete(1)).ReturnsAsync(true); //  Return true to indicate this is a positive test.

            var mockLogger = new Mock<ILogger<PostController>>(); // Mocking the logger

            var mockUser = new Mock<IUserRepository>(); // Mocking user

            var mockUpvoteRepo = new Mock<IUpVoteRepository>(); // Mocking upvote repository

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // Setting up the our new post controller with mocked objects
            var postController = new PostController(
                mockPostRepository.Object,
                mockUser.Object,
                mockLogger.Object,
                userManagerMock.Object,
                signInManagerMock.Object,
                mockUpvoteRepo.Object
                );

            // Act
            var result = await postController.DeleteItem(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);    //  Test to see the return from Delete method is of correct type. 
            Assert.True(okResult is OkObjectResult);  //  Another way to test return type. If Deletion failed, it would return NotFoundObjectResult.
            
            /*  
                Now we test to see if the Deletion actually worked. We know we get an anonymous object as value of OkObjectResult
                in return from Create(), which has a "success" key that is either true or false. It is
                not very easy to get values from keys from anonymous objects inbetween functions, but we found a way here:
                https://stackoverflow.com/questions/874746/how-can-i-get-a-value-of-a-property-from-an-anonymous-type
            */
            bool okResultSuccess = (bool)okResult.Value!.GetType().GetProperty("success")!.GetValue(okResult.Value, null)!;
            Assert.True(okResultSuccess);  //  Check if deletion worked.

        }

        // NEGATIVE TEST (DELETE)
        [Fact]
        public async Task TestDeleteFail()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>(); // Mocking the post repository
            mockPostRepository.Setup(repo => repo.Delete(1)).ReturnsAsync(false); //  Return false to indicate this is a negative test.

            var mockLogger = new Mock<ILogger<PostController>>(); // Mocking the logger

            var mockUser = new Mock<IUserRepository>(); // Mocking the user

            var mockUpvoteRepo = new Mock<IUpVoteRepository>(); // Mocking the upvote repository

            // Mocking userManager
            // From: https://code-maze.com/aspnetcore-identity-testing-usermanager-rolemanager/
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // Mocking signInManager
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // Setting up the our new post controller with mocked objects
            var postController = new PostController(
                mockPostRepository.Object,
                mockUser.Object,
                mockLogger.Object,
                userManagerMock.Object,
                signInManagerMock.Object,
                mockUpvoteRepo.Object
                );

            // Act
            var result = await postController.DeleteItem(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);    //  Test to see the return from Delete method is of correct type. 
            Assert.True(notFoundResult is NotFoundObjectResult);  //  Another way to test return type. If Deletion failed, it would return NotFoundObjectResult.
            var errorMessage = Assert.IsAssignableFrom<string>(notFoundResult.Value);  //  Test to see that the value inside NotFoundObjectResult is of correct type (string).
            var expectedErrorMessage = "Post deletion failed";
            Assert.Equal(expectedErrorMessage, errorMessage);  //  Test to if the string is correct.
        }
    }
}
