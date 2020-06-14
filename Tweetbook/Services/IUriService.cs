using System;
using Tweetbook.Contracts.V1.Request.Queries;

namespace Tweetbook.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostsUri(PaginationQuery pagination = null);
    }
}
