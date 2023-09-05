﻿using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Service.Services
{
    internal sealed class TravelPlanService : ITravelPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TravelPlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<StandardResponse<TravelPlanResponse>> CreateTravelPlan(TravelPlanRequestDto requestDto)
        {
            TravelPlan plan = _mapper.Map<TravelPlan>(requestDto);
            plan.IsCompleted = Status.Pending;
            _unitOfWork.TravelPlanRepository.CreateTravelPlan(plan);
            await _unitOfWork.SaveAsync();
            TravelPlanResponse planDto = _mapper.Map<TravelPlanResponse>(plan);
            return new StandardResponse<TravelPlanResponse>(201,true,string.Empty,planDto);
        }

        public async Task DeleteTravelPlan(int id)
        {
            TravelPlan plan = await GetTravelPlanWithId(id, false);
            if(plan.Packages.Any())
            {
                throw new TravelPlanNotAlterableException(id);
            }
            _unitOfWork.TravelPlanRepository.DeleteTravelPlan(plan);
            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<(IEnumerable<TravelPlanResponse>,MetaData)>> GetAllTravelPlan(TravelPlanRequestParameters requestParameters, bool trackChanges)
        {
            PagedList<TravelPlan> travelPlans =await _unitOfWork.TravelPlanRepository.GetAllTravelPlanAsync(requestParameters,trackChanges);
            IEnumerable<TravelPlanResponse> travelPlansDto = _mapper.Map<IEnumerable<TravelPlanResponse>>(travelPlans);
            return new StandardResponse<(IEnumerable<TravelPlanResponse>,MetaData)>(200,true, string.Empty,(travelPlansDto,travelPlans.MetaData));
        }

        public async Task<StandardResponse<(IEnumerable<TravelPlanResponse>, MetaData)>> GetAllUserTravelPlan(TravelPlanRequestParameters requestParameters, string userId, bool trackChanges)
        {
            PagedList<TravelPlan> travelPlans = await _unitOfWork.TravelPlanRepository.GetAllUserTravelPlanAsync(requestParameters,userId, trackChanges);
            IEnumerable<TravelPlanResponse> travelPlansDto = _mapper.Map<IEnumerable<TravelPlanResponse>>(travelPlans);
            return new StandardResponse<(IEnumerable<TravelPlanResponse>, MetaData)>(200, true, string.Empty, (travelPlansDto,travelPlans.MetaData));
        }

        public async Task<StandardResponse<TravelPlanResponse>> GetTravelPlanById(int id, bool trackChanges)
        {
            TravelPlan travelPlan = await GetTravelPlanWithId(id, trackChanges);
            TravelPlanResponse travelPlanDto = _mapper.Map<TravelPlanResponse>(travelPlan);
            return new StandardResponse<TravelPlanResponse>(200,true, string.Empty,travelPlanDto);
        }

        public async Task UpdateCompleted(int planId, Status status)
        {
            if(status==Status.Pending || status == Status.Canceled || status == Status.Booked)
                throw new TravelPlanNotAlterableException(planId);
            if (status == Status.Delivered)
            {
                await EnsureAllPackagesDelivered(planId);
            }
            TravelPlan plan = await GetTravelPlanWithId(planId, false);
            plan.IsCompleted = status;
            _unitOfWork.TravelPlanRepository.UpdateTravelPlan(plan);
            await _unitOfWork.SaveAsync();
        }


        public async Task UpdateIsActive(int plabId, bool isActive)
        {
            TravelPlan plan = await GetTravelPlanWithId(plabId, false);
            if (plan.Packages.Any(p => p.Status!=Status.Delivered) )
            {
                throw new TravelPlanNotAlterableException("Not all packages have been delivered yet. Complete delivery.");
            }
            plan.IsActive = isActive;
            _unitOfWork.TravelPlanRepository.UpdateTravelPlan(plan);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateTravelPlan(int id, TravelPlanRequestDto requestDto)
        {
            TravelPlan travelPlan =await GetTravelPlanWithId(id, false);
            if(travelPlan.Packages.Any())
            {
                throw new TravelPlanNotAlterableException(id);
            }
            TravelPlan travelPlanForUpdate = _mapper.Map<TravelPlan>(requestDto);
            _unitOfWork.TravelPlanRepository.UpdateTravelPlan(travelPlanForUpdate);
            await _unitOfWork.SaveAsync();
            //Check for unpaired packages and pair if they match
        }

        private async Task<TravelPlan> GetTravelPlanWithId(int id, bool trackChanges)
        {
            TravelPlan plan = await _unitOfWork.TravelPlanRepository.GetTravelPlanByIdAsync(id, trackChanges)
                ?? throw new TravelPlanNotFoundException(id);
            return plan;
        }
        private async Task EnsureAllPackagesDelivered(int plabId)
        {
            IEnumerable<Package> packages = await _unitOfWork.PackageRepository
                    .GetAllTravelPlanPackagesAsync(new PackageRequestParameter(), plabId, false);
            if (packages.Any(package => package.Status != Status.Delivered))
            {
                throw new TravelPlanNotAlterableException("Not all packages have been delivered yet. Complete delivery.");
            }
        }
    }
}
