using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.ServiceContracts
{
    public interface IPackageService
    {
        Task<StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>> GetAllPackagesAsync(PackageRequestParameter requestParameter, bool trackChanges);
        Task<StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>> GetAllUserPackagesAsync(PackageRequestParameter requestParameter, string userId, bool trackChanges);
        Task<StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>> GetAllTravelPlanPackagesAsync(PackageRequestParameter requestParameter, int travelPlanId, bool trackChanges);
        Task<StandardResponse<PackageResponseDto>> GetPackageByIdAsync(int id, bool trackChanges);
        Task<StandardResponse<(PackageResponseDto, IEnumerable<TravelPlanResponse>)>> UpdatePackage(int id, PackageRequestDto requestDto);
        Task<StandardResponse<string>> UploadPackageImg(int id, IFormFile file);

        Task UpdateStatusRecieved(int packageId, int code);
        Task UpdateStatusDelivered(int packageId, int code);
        Task DeletePackage(int id);
        Task<StandardResponse<(PackageResponseDto package, IEnumerable<TravelPlanResponse> availableTravelPlans)>> CreatePackage(PackageRequestDto requestDto);
    }
}
