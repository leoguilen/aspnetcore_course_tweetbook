using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.V1.Request;

namespace Tweetbook.SwaggerExamples.Requests
{
    public class UserRegistrationRequestExample : IExamplesProvider<UserRegistrationRequest>
    {
        public UserRegistrationRequest GetExamples()
        {
            return new UserRegistrationRequest
            {
                Email = "example.email@domain.com",
                Password = "Example123!"
            };
        }
    }
}
