using AutoMapper;
using Tweetbook.Contracts.V1.Request.Queries;
using Tweetbook.Domain;

namespace Tweetbook.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}
