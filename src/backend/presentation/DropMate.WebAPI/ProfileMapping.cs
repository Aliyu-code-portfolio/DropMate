using AutoMapper;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;

namespace DropMate.WebAPI
{
    public class ProfileMapping:Profile
    {
        public ProfileMapping()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserCreateRequestDto, User>();
            CreateMap<UserUpdateRequestDto, User>();
            CreateMap<TravelPlan, TravelPlanResponse>()
                .ForMember(dest => dest.DepartureLocation, opt => opt.MapFrom(src => src.DepartureLocation.ToString()))
                .ForMember(dest => dest.ArrivalLocation, opt => opt.MapFrom(src => src.ArrivalLocation.ToString()))
                .ForMember(dest => dest.MaximumPackageWeight, opt => opt.MapFrom(src => src.MaximumPackageWeight.ToString()));
            CreateMap<TravelPlanRequestDto, TravelPlan>();
            CreateMap<Package, PackageResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DepartureLocation, opt => opt.MapFrom(src => src.DepartureLocation.ToString()))
                .ForMember(dest => dest.ArrivalLocation, opt => opt.MapFrom(src => src.ArrivalLocation.ToString()))
                .ForMember(dest => dest.PackageWeight, opt => opt.MapFrom(src => src.PackageWeight.ToString()));
            CreateMap<PackageRequestDto, Package>();
            CreateMap<Review, ReviewResponseDto>();
            CreateMap<ReviewRequestDto, Review>();
        }
    }
}
