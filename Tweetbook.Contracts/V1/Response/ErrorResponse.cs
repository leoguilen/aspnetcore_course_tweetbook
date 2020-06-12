using System.Collections.Generic;

namespace Tweetbook.Contracts.V1.Response
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
