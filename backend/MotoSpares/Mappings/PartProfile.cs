using AutoMapper;
using MotoSpares.Entities;
using MotoSpares.DTOs.Part;

namespace MotoSpares.Mappings
{
    public class PartProfile : Profile
    {
        public PartProfile()
        {
            CreateMap<PartCreateDto, Part>();
            CreateMap<PartUpdateDto, Part>();
            CreateMap<Part, PartResponseDto>()
                .ForMember(dest => dest.VendorName, 
                    opt => opt.MapFrom(src => src.Vendor != null ? src.Vendor.Name : null));
        }
    }
}
