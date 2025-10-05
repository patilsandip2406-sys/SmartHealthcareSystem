using AutoMapper;
using ClaimService.API.DTOs;
using Shared.Library.Models;

namespace ClaimService.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Claim, ClaimDto>();
        }
    }
}
