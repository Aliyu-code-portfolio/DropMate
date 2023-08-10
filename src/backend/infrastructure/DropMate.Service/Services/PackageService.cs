using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using DropMate.Shared.Utilities;

namespace DropMate.Service.Services
{
    internal sealed class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<StandardResponse<PackageResponseDto>> CreatePackage(PackageRequestDto requestDto)
        {
            Package package = _mapper.Map<Package>(requestDto);
            package.Status = Status.Pending;
            package.RecieveCode = Utility.GeneratePackageCode();
            package.DeliverCode = Utility.GeneratePackageCode();
            _unitOfWork.PackageRepository.CreatePackage(package);
            await _unitOfWork.SaveAsync();
            //check database for people going towards that route and push notification to them.
            PackageResponseDto packageDto = _mapper.Map<PackageResponseDto>(package);
            return new StandardResponse<PackageResponseDto>(201, true, string.Empty, packageDto);
        }

        public async Task DeletePackage(int id)
        {
            Package package = await GetPackageWithId(id, false);
            if(package.Status==Status.Booked|| package.Status == Status.Transit || package.Status == Status.Delivered)
            {
                throw new PackageNotAlterableException(id);
            }
            _unitOfWork.PackageRepository.DeletePackage(package);
            await _unitOfWork.SaveAsync();  
        }

        public async Task<StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>> GetAllPackagesAsync(PackageRequestParameter requestParameter, bool trackChanges)
        {
            PagedList<Package> packages = await _unitOfWork.PackageRepository.GetAllPackagesAsync(requestParameter,trackChanges);
            IEnumerable<PackageResponseDto> packageResponseDtos = _mapper.Map<IEnumerable<PackageResponseDto>>(packages);
            return new StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>(200, true, string.Empty, (packageResponseDtos,packages.MetaData));
        }

        public async Task<StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>> GetAllTravelPlanPackagesAsync(PackageRequestParameter requestParameter, int travelPlanId, bool trackChanges)
        {
            PagedList<Package> packages = await _unitOfWork.PackageRepository.GetAllTravelPlanPackagesAsync(requestParameter, travelPlanId, trackChanges);
            IEnumerable<PackageResponseDto> packageResponseDtos = _mapper.Map<IEnumerable<PackageResponseDto>>(packages);
            return new StandardResponse<(IEnumerable<PackageResponseDto>, MetaData)>(200, true, string.Empty, (packageResponseDtos, packages.MetaData));
        }

        public async Task<StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>> GetAllUserPackagesAsync(PackageRequestParameter requestParameter, string userId, bool trackChanges)
        {
            PagedList<Package> packages = await _unitOfWork.PackageRepository.GetAllUserPackagesAsync(requestParameter, userId, trackChanges);
            IEnumerable<PackageResponseDto> packageResponseDtos = _mapper.Map<IEnumerable<PackageResponseDto>>(packages);
            return new StandardResponse<(IEnumerable<PackageResponseDto>,MetaData)>(200, true, string.Empty, (packageResponseDtos,packages.MetaData));
        }

        public async Task<StandardResponse<PackageResponseDto>> GetPackageByIdAsync(int id, bool trackChanges)
        {
            Package package = await GetPackageWithId(id,trackChanges);
            PackageResponseDto packageResponseDto = _mapper.Map<PackageResponseDto>(package);
            return new StandardResponse<PackageResponseDto>(200, true, string.Empty, packageResponseDto);
        }

        public async Task UpdatePackage(int id, PackageRequestDto requestDto)
        {
            Package package = await GetPackageWithId(id, false);
            if (package.Status == Status.Booked || package.Status == Status.Transit || package.Status == Status.Delivered)
            {
                throw new PackageNotAlterableException(id);
            }
            Package packageToUpdate = _mapper.Map<Package>(requestDto);
            _unitOfWork.PackageRepository.UpdatePackage(packageToUpdate);
            await _unitOfWork.SaveAsync();
            //check if there are pairable travel plans
            
        }

        public async Task UpdateStatusRecieved(int packageId,int code, Status status)
        {
            Package package = await GetPackageWithId(packageId, false);
            if(!package.RecieveCode.Equals(code) || !status.Equals(Status.Transit))
            {
                throw new PackageInvalidCodeException(packageId);
            }
            package.Status = status;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateStatusDelivered(int packageId,int code, Status status)
        {
            Package package = await GetPackageWithId(packageId, false);
            if (!package.Status.Equals(Status.Transit) || !package.DeliverCode.Equals(code) || !status.Equals(Status.Delivered))
            {
                throw new PackageInvalidCodeException(packageId);
            }
            package.Status = status;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Package> GetPackageWithId(int id, bool trackChanges)
        {
            return await _unitOfWork.PackageRepository.GetPackageByIdAsync(id, trackChanges)
                ?? throw new PackageNotFoundException(id);
        }
    }
}
