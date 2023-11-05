using Microsoft.AspNetCore.Mvc;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;

namespace OsloMetAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<PostController> _logger;

        public UserController(IUserRepository userRepository, ILogger<PostController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Console.Write("usercontroller1");
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                _logger.LogError("[UserController] User list not found when executing _userRepository.GetAll(),");
                return NotFound("User list not found");
            }
            return Ok(users);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data");
            }
            bool returnOk = await _userRepository.Create(newUser);

            if (returnOk)
            {
                var response = new { success = true, message = "User " + newUser.Name + " created succesfully" };
                return Ok(response);                
            }
            else
            {
                var response = new { success = false, message = "User creation failed" };
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserbyId(int id)
        {
            var user = await _userRepository.GetItemById(id);
            if (user == null)
            {
                _logger.LogError("[UserController] User list not found when executing _userRepository.GetAll(),");
                return NotFound("User list not found");
            }
            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Invalid user data");
            }

            bool returnOk = await _userRepository.Update(newUser);

            if (returnOk)
            {
                var response = new { success = true, message = "User " + newUser.Name + " updated succesfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "User update failed" };
                return Ok(response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool returnOk= await _userRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[UserController] User deletion failed for the UserId {UserId:0000}", id);
                return NotFound("User deletion failed");
            }
            var response = new { success = true, message = "User " + id.ToString()+ " deleted succesfully" };
            return Ok(response);
        }

        //private static int GetNextItemId()
        //{
        //    if (Items.Count == 0)
        //    {
        //        return 1;
        //    }
        //    return Items.Max(item => item.ItemId) + 1;
        //}
    }
}
