using Refit;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1.Request;

namespace Tweetbook.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var tweetbookApi = RestService.For<ITweetbookApi>("https://localhost:5001", new RefitSettings 
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdk.sample@email.com",
                Password = "Sample123!"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdk.sample@email.com",
                Password = "Sample123!"
            });

            cachedToken = loginResponse.Content.Token;

            var allPosts = await tweetbookApi.GetAllAsync();

            var createdPost = await tweetbookApi.CreateAsync(new CreatePostRequest
            {
                Name = "Novo post criado atraves do Refit",
                Tags = new[] {"Refit", "Toppp" }
            });

            var getPost = await tweetbookApi.GetAsync(createdPost.Content.Id);

            var updatedPost = await tweetbookApi.UpdateAsync(createdPost.Content.Id, new UpdatePostRequest 
            {
                Name = "Post atualizado com refit"
            });

            var deletedPost = await tweetbookApi.DeleteAsync(createdPost.Content.Id);

        }
    }
}
