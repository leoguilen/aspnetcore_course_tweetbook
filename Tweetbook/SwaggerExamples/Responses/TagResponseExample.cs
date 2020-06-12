using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.V1.Response;

namespace Tweetbook.SwaggerExamples.Responses
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "new tag",
            };
        }
    }
}
