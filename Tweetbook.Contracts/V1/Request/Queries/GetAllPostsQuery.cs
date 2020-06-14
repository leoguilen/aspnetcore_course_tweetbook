using Microsoft.AspNetCore.Mvc;

namespace Tweetbook.Contracts.V1.Request.Queries
{
    public class GetAllPostsQuery
    {
        [FromQuery(Name = "userId")]
        public string UserId { get; set; }
    }
}
