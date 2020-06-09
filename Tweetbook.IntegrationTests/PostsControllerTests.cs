using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Request;
using Tweetbook.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Tweetbook.IntegrationTests
{
    public class PostsControllerTests : IntegrationTests
    {
        private readonly ITestOutputHelper _output;

        public PostsControllerTests(ITestOutputHelper output, 
            CustomWebApplicationFactory appFactory) 
            : base(appFactory) =>  _output = output;

        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            await AuthenticateAsync();

            var response = await TestClient
                .GetAsync(ApiRoutes.Posts.GetAll);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInTheDatabase()
        {
            await AuthenticateAsync();
            var createdPost = await CreatePostAsync(
                new CreatePostRequest { Name = "Test Post" });

            _output.WriteLine($"Id: {createdPost.Id} | Name: {createdPost.Name}");

            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get
                .Replace("{postId}", createdPost.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadAsAsync<Post>();
            returnedPost.Id.Should().Be(createdPost.Id);
            returnedPost.Name.Should().Be("Test Post");
        }
    }
}
