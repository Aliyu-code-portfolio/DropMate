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
using Google.Maps;
using Google.Maps.DistanceMatrix;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Location = Google.Maps.Location;

namespace DropMate.Service.Services
{
    internal sealed class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IConfiguration _configuration;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper,IPhotoService photoService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
            _configuration = configuration;
        }
        public async Task<StandardResponse<(PackageResponseDto, IEnumerable<TravelPlanResponse>)>> CreatePackage(PackageRequestDto requestDto)
        {
            Package package = _mapper.Map<Package>(requestDto);
            package.Status = Status.Pending;
            package.RecieveCode = Utility.GeneratePackageCode();
            package.DeliverCode = Utility.GeneratePackageCode();

            var priceAndDuration = CalculateCostOfDelivery(package.DepartureLocation, package.ArrivalLocation, package.PackageWeight);
            package.Price = priceAndDuration.Item1;
            package.EstimatedDuration = priceAndDuration.Item2;

            _unitOfWork.PackageRepository.CreatePackage(package);
            await _unitOfWork.SaveAsync();

            //check database for people going towards that route and push notification to them.

            //Add distance from traveler to package should be a factor
            IEnumerable<TravelPlan> plans = await _unitOfWork.TravelPlanRepository
                .GetTravelPlanByDestinationAsync(package.ArrivalLocation, false);
            IEnumerable<TravelPlanResponse> responseDtos = _mapper.Map<IEnumerable<TravelPlanResponse>>(plans);

            PackageResponseDto packageDto = _mapper.Map<PackageResponseDto>(package);
            return StandardResponse<(PackageResponseDto,IEnumerable<TravelPlanResponse>)>
                .Success("Successfully created", (packageDto,responseDtos),201);

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

        public async Task<StandardResponse<(PackageResponseDto, IEnumerable<TravelPlanResponse>)>> UpdatePackage(int id, PackageRequestDto requestDto)
        {
            Package packageFromDb = await GetPackageWithId(id, false);
            if (packageFromDb.Status == Status.Booked || packageFromDb.Status == Status.Transit || packageFromDb.Status == Status.Delivered)
            {
                throw new PackageNotAlterableException(id);
            }
            Package package = _mapper.Map<Package>(requestDto);
            //recalculate prices
            var priceAndDuration = CalculateCostOfDelivery(package.DepartureLocation, package.ArrivalLocation, package.PackageWeight);
            package.Price = priceAndDuration.Item1;
            package.EstimatedDuration = priceAndDuration.Item2;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
            //check database for people going towards that route and push send to package owner.
            IEnumerable<TravelPlan> plans = await _unitOfWork.TravelPlanRepository
                .GetTravelPlanByDestinationAsync(package.ArrivalLocation, false);
            IEnumerable<TravelPlanResponse> responseDtos = _mapper.Map<IEnumerable<TravelPlanResponse>>(plans);

            PackageResponseDto packageDto = _mapper.Map<PackageResponseDto>(package);
            return StandardResponse<(PackageResponseDto, IEnumerable<TravelPlanResponse>)>
                .Success("Successfully created", (packageDto, responseDtos), 201);

        }

        public async Task UpdateStatusRecieved(int packageId,int code)
        {
            Package package = await GetPackageWithId(packageId, false);
            if(!package.RecieveCode.Equals(code))
            {
                throw new PackageInvalidCodeException(packageId);
            }
            package.Status = Status.Transit;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateStatusDelivered(int packageId,int code)
        {
            //Call the transaction service and trigger payment
            Package package = await GetPackageWithId(packageId, false);
            if (!package.Status.Equals(Status.Transit) || !package.DeliverCode.Equals(code))
            {
                throw new PackageInvalidCodeException(packageId);
            }
            package.Status = Status.Delivered;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Package> GetPackageWithId(int id, bool trackChanges)
        {
            return await _unitOfWork.PackageRepository.GetPackageByIdAsync(id, trackChanges)
                ?? throw new PackageNotFoundException(id);
        }
        private (decimal,string) CalculateCostOfDelivery(LagosLocation origin, LagosLocation destination, PackageWeight weight)
        {
            //Use a break point to get the http link if you want to use httpclient instead of this SDK for your map api
            string apiKey = _configuration.GetSection("GoogleApi").Value;
            var distanceMatrixService = new DistanceMatrixService(new GoogleSigned(apiKey));

            // Perform the distance matrix request
            var distanceRequest = new DistanceMatrixRequest();
            distanceRequest.AddOrigin(new Location(SplitCamelCaseWords(origin.ToString())));
            distanceRequest.AddDestination(new Location(SplitCamelCaseWords(destination.ToString())));

            var distanceResponse = distanceMatrixService.GetResponse(distanceRequest);

            var distance = distanceResponse.Rows[0].Elements[0].distance.Text;
            var estimatedDuration = distanceResponse.Rows[0].Elements[0].duration.Text;
            decimal price = GetPrice(distance, weight);
            return (price, estimatedDuration);
        }

        private decimal GetPrice(string distance, PackageWeight weight)
        {
            decimal distanceInDigits = decimal.Parse(distance.Substring(0,distance.Length-2));
            decimal price = 250 * distanceInDigits *(int)weight;
            return price;
        }

        private static string SplitCamelCaseWords(string input)
        {
            string pattern = @"(?<!^)(?=[A-Z])"; // Look for positions before uppercase letters (except at the beginning)
            string[] words = Regex.Split(input, pattern);
            string result=string.Empty;
            foreach (string word in words)
            {
                result=result +" "+ word;
            }
            return result;
        }

        public async Task<StandardResponse<string>> UploadPackageImg(int id, IFormFile file)
        {
            Package package = await GetPackageWithId(id, false);
            string url = _photoService.UploadPhoto(file, id.ToString(), "DropMateProfileImages");
            package.PackageImageUrl = url;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
            return new StandardResponse<string>(200, true, string.Empty, url);
        }
    }
}
