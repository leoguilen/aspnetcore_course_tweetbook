using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Request.Queries;

namespace Tweetbook.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllPostsUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null) return uri;

            var modifieldUri = QueryHelpers.AddQueryString(_baseUri, new Dictionary<string, string>(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("pageNumber", pagination.PageNumber.ToString()),
                    new KeyValuePair<string, string>("pageSize", pagination.PageSize.ToString())
                }));
                
            return new Uri(modifieldUri);
        }

        public Uri GetPostUri(string postId)
        {
            return new Uri(_baseUri + ApiRoutes.Posts.Get.Replace("{postId}", postId));
        }
    }
}
