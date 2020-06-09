using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Request;
using Tweetbook.Contracts.V1.Response;
using Xunit;

namespace Tweetbook.IntegrationTests
{
    public class IntegrationTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly HttpClient TestClient;

        public IntegrationTests(CustomWebApplicationFactory appFactory)
        {
            TestClient = appFactory.CreateClient();
        }

        protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
            return await response.Content.ReadAsAsync<PostResponse>();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.Identity.Register,
                new UserRegistrationRequest
                {
                    Email = "integration@test.com",
                    Password = "Test123!"
                });

            var registrationResponse = await response.Content
                .ReadAsAsync<AuthSuccessResponse>();

            return registrationResponse.Token;
        }
    }
}
