using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Request;
using Tweetbook.Contracts.V1.Response;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
    [Produces("application/json")]
    public class TagsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public TagsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all tags in the system 
        /// </summary>
        /// <response code="200">Returns all tags in the system</response>
        [HttpGet(ApiRoutes.Tags.GetAll)]
        [ProducesResponseType(typeof(List<TagResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _postService.GetAllTagsAsync();

            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        /// <summary>
        /// Returns tag in the system by your name
        /// </summary>
        /// <response code="200">Return tag in the system by your name</response>
        /// <response code="404">Not found any tag with this name</response>
        [HttpGet(ApiRoutes.Tags.Get)]
        [ProducesResponseType(typeof(TagResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] string tagName)
        {
            var tag = await _postService.GetTagByNameAsync(tagName);

            if (tag == null) return NotFound();

            return Ok(_mapper.Map<TagResponse>(tag));
        }

        /// <summary>
        /// Create a tag in the system
        /// </summary>
        /// <response code="200">Create a tag in the system</response>
        /// <response code="400">Unable to create the tag due to validation error</response>
        [HttpPost(ApiRoutes.Tags.Create)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.TagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.Now
            };

            var created = await _postService.CreateTagAsync(newTag);

            if (!created)
                return BadRequest(new ErrorResponse
                {
                    Errors = new List<ErrorModel> {
                        new ErrorModel {
                            Message = "Unable to create the tag"
                        }
                    }
                });

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);

            return Created(locationUri, _mapper.Map<TagResponse>(newTag));
        }

        /// <summary>
        /// Delete a tag in the system
        /// </summary>
        /// <response code="204">Delete a tag in the system</response>
        /// <response code="404">Not found any tag with this name</response>
        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Policy = "DeletePolicy")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var deleted = await _postService.DeleteTagAsync(tagName);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
