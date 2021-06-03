using AutoMapper;
using Microsoft.SyndicationFeed;
using TimeChimp.Backend.Assessment.DTO;

namespace TimeChimp.Backend.Assessment.Mappings
{
    public class MappingFeed : Profile
    {
        public MappingFeed()
        {
            CreateMap<SyndicationItem, Item>()
                .ReverseMap();
            CreateMap<SyndicationCategory, Category>()
                .ReverseMap();
            CreateMap<SyndicationPerson, Person>()
                .ReverseMap();
            CreateMap<SyndicationLink, Link>()
                .ReverseMap();
        }
    }
}
