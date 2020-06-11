using System.Collections.Generic;

namespace Tweetbook.Contracts.V1.Request
{
    public class CreatePostRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
