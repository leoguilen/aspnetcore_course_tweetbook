using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Request;
using Tweetbook.Contracts.V1.Response;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetPostsAsync();
            var postResponse = posts.Select(post => new PostResponse
            {
                Id = post.Id,
                Name = post.Name,
                UserId = post.UserId,
                Tags = post.Tags.Select(t => new TagResponse { Name = t.TagName })
            }).ToList();

            return Ok(postResponse);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound();

            return Ok(new PostResponse
            {
                Id = post.Id,
                Name = post.Name,
                UserId = post.UserId,
                Tags = post.Tags.Select(t => new TagResponse { Name = t.TagName })
            });
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newPostId = Guid.NewGuid();
            var post = new Post
            {
                Id = newPostId,
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
                Tags = postRequest.Tags.Select(x =>
                    new PostTag { PostId = newPostId, TagName = x }).ToList()
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse
            {
                Id = post.Id,
                Name = post.Name,
                UserId = post.UserId,
                Tags = post.Tags.Select(t => new TagResponse { Name = t.TagName })
            };

            return Created(locationUrl, response);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest postRequest)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new
                {
                    error = "You do not own this post"
                });

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = postRequest.Name;

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok(new PostResponse
                {
                    Id = post.Id,
                    Name = post.Name,
                    UserId = post.UserId,
                    Tags = post.Tags.Select(t => new TagResponse { Name = t.TagName })
                });

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new
                {
                    error = "You do not own this post"
                });

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

    }
}