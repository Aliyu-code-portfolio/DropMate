﻿using AutoMapper;
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
            CreateMap<TravelPlan, TravelPlanResponse>();
            CreateMap<TravelPlanRequestDto, TravelPlan>();
            CreateMap<Package, PackageResponseDto>();
            CreateMap<PackageRequestDto, Package>();
            CreateMap<Review, ReviewResponseDto>();
            CreateMap<ReviewRequestDto, Review>();
        }
    }
}