using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
          CreateMap<Region, RegionDTo>().ReverseMap();
          CreateMap<AddRegionRequestDTo, Region>().ReverseMap();
          CreateMap<UpdateRegionRequestDTo, Region>().ReverseMap();

          // mapping of walks
          CreateMap<AddWalkRequestDTo, Walk>().ReverseMap();
          CreateMap<Walk, WalkDTo>().ReverseMap();
          CreateMap<Difficulty, DifficultyDTo>().ReverseMap();
          CreateMap<UpdateWalkRequestDTo, Walk>().ReverseMap();
        }
    }
}
