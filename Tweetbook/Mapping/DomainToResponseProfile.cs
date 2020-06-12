using AutoMapper;
using System.Linq;
using Tweetbook.Contracts.V1.Response;
using Tweetbook.Domain;

namespace Tweetbook.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opts => 
                    opts.MapFrom(src => src.Tags.Select(t => 
                        new TagResponse { Name = t.TagName }))
                    );

            CreateMap<Tag, TagResponse>();
        }
    }
}
