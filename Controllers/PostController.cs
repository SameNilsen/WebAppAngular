﻿using Microsoft.AspNetCore.Mvc;
using OsloMetAngular.DAL;
using OsloMetAngular.Models;

namespace OsloMetAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        //private static List<Item> Items = new List<Item>()
        //{
        //    new Item
        //    {
        //        ItemId = 1,
        //        Name = "Pizza",
        //        Price = 150,
        //        Description = "Delicouououoosoo",
        //        ImageUrl = "assets/images/pizza.jpg"
        //    },
        //    new Item
        //    {
        //        ItemId = 2,
        //        Name = "Fried Chicka",
        //        Price = 20,
        //        Description = "Crispy duck",
        //        ImageUrl = "assets/images/chickenleg.jpg"
        //    }            
        //};

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postRepository.GetAll();
            if (posts == null)
            {
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetAll(),");
                return NotFound("Post list not found");
            }
            return Ok(posts);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Post newPost)
        {
            if (newPost == null)
            {
                return BadRequest("Invalid post data");
            }
            //newItem.ItemId = GetNextItemId();
            bool returnOk = await _postRepository.Create(newPost);

            if (returnOk)
            {
                var response = new { success = true, message = "Post " + newPost.Title + " created succesfully" };
                return Ok(response);                
            }
            else
            {
                var response = new { success = false, message = "Post creation failed" };
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItembyId(int id)
        {
            var post = await _postRepository.GetItemById(id);
            if (post == null)
            {
                _logger.LogError("[PostController] Post list not found when executing _postRepository.GetAll(),");
                return NotFound("Post list not found");
            }
            return Ok(post);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Post newPost)
        {
            if (newPost == null)
            {
                return BadRequest("Invalid post data");
            }

            bool returnOk = await _postRepository.Update(newPost);

            if (returnOk)
            {
                var response = new { success = true, message = "Post " + newPost.Title + " updated succesfully" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false, message = "Post update failed" };
                return Ok(response);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            bool returnOk= await _postRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[PostController] Post deletion failed for the PostId {PostId:0000}", id);
                return NotFound("Post deletion failed");
            }
            var response = new { success = true, message = "Post " + id.ToString()+ " deleted succesfully" };
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
