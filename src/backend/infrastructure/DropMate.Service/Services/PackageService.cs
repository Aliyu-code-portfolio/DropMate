﻿using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Enums;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using DropMate.Shared.HelperModels;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper,IPhotoService photoService,IEmailService emailService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<StandardResponse<(PackageResponseDto, IEnumerable<TravelPlanResponse>)>> CreatePackage(string userId, PackageRequestDto requestDto)
        {
            Package package = _mapper.Map<Package>(requestDto);
            package.PackageOwnerId = userId;
            package.Status = Status.Pending;
            package.RecieveCode = Utility.GeneratePackageCode();
            package.DeliverCode = Utility.GeneratePackageCode();

            var distanceAndDuration = CalculateDistanceOfLocations(package.DepartureLocation, package.ArrivalLocation);
            package.Price = GetPrice(distanceAndDuration.Item1, package.PackageWeight);
            package.EstimatedDuration = distanceAndDuration.Item2;

            _unitOfWork.PackageRepository.CreatePackage(package);
            await _unitOfWork.SaveAsync();

            //check database for people going towards that route and push send to package owner.
            IEnumerable<TravelPlan> plans = await _unitOfWork.TravelPlanRepository
                .GetTravelPlanByDestinationAsync(package.ArrivalLocation, false);
            IEnumerable<TravelPlanResponse> responseDtos = _mapper.Map<IEnumerable<TravelPlanResponse>>(plans);
            foreach(TravelPlanResponse response in responseDtos)
            {
                var result = CalculateDistanceOfLocations((LagosLocation)Enum.Parse(typeof(LagosLocation),response.DepartureLocation,true),
                    package.DepartureLocation);
                response.DistanceFromPickUp= result.Item1;
                response.EstimatedPickUpTime= result.Item2;
            }

            PackageResponseDto packageDto = _mapper.Map<PackageResponseDto>(package);
            return StandardResponse<(PackageResponseDto,IEnumerable<TravelPlanResponse>)>
                .Success("Successfully created", (packageDto,responseDtos),201);

        }

        public async Task DeletePackage(int id, string token)
        {
            Package package = await GetPackageWithId(id, false);
            if (package.Status == Status.Booked || package.Status == Status.Transit)
            {
                await InitiateRefund(package.Id, token);
            }
            if (package.Status == Status.Delivered)
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
            var distanceAndDuration = CalculateDistanceOfLocations(package.DepartureLocation, package.ArrivalLocation);
            package.Price = GetPrice(distanceAndDuration.Item1, package.PackageWeight); 
            package.EstimatedDuration = distanceAndDuration.Item2;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            await _unitOfWork.SaveAsync();
            //check database for people going towards that route and push send to package owner.
            IEnumerable<TravelPlan> plans = await _unitOfWork.TravelPlanRepository
                .GetTravelPlanByDestinationAsync(package.ArrivalLocation, false);
            IEnumerable<TravelPlanResponse> responseDtos = _mapper.Map<IEnumerable<TravelPlanResponse>>(plans);
            foreach (TravelPlanResponse response in responseDtos)
            {
                var result = CalculateDistanceOfLocations((LagosLocation)Enum.Parse(typeof(LagosLocation), response.DepartureLocation, true),
                    package.DepartureLocation);
                response.DistanceFromPickUp = result.Item1;
                response.EstimatedPickUpTime = result.Item2;
            }

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
        public async Task UpdateStatusDelivered(int packageId,int code,string token)
        {
            Package package = await GetPackageWithId(packageId, false);
            User packageOwner = await _unitOfWork.UserRepository.GetByIdAsync(package.PackageOwnerId, false)
                ?? throw new UserNotFoundException(package.PackageOwnerId);
            if (!package.Status.Equals(Status.Transit) || !package.DeliverCode.Equals(code))
            {
                throw new PackageInvalidCodeException(packageId);
            }
            await InitiateServicePayment(packageId, token);
            package.Status = Status.Delivered;
            _unitOfWork.PackageRepository.UpdatePackage(package);
            sendPackageDeliveredEmail(packageOwner.Email, package.ProductName);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Package> GetPackageWithId(int id, bool trackChanges)
        {
            return await _unitOfWork.PackageRepository.GetPackageByIdAsync(id, trackChanges)
                ?? throw new PackageNotFoundException(id);
        }
        private (string,string) CalculateDistanceOfLocations(LagosLocation origin, LagosLocation destination)
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
            return (distance, estimatedDuration);
        }

        private decimal GetPrice(string distance, PackageWeight weight)
        {
            decimal distanceInDigits = decimal.Parse(distance.Substring(0,distance.Length-2));
            decimal price = 50 * distanceInDigits *(int)weight;
            return price;
        }
        private void sendPackageDeliveredEmail(string email, string packageName)
        {
            string logoUrl = "https://res.cloudinary.com/djbkvjfxi/image/upload/v1694601350/uf4xfoda2c4z0exly8nx.png";
            string title = "Your DropMate Package Delivered";
            string body = $"<html><body><br/><br/>We are pleased to inform you that your package {packageName} have been delivered to it's destination. Please don't hesitate to reach out to us at care@dropmate.com for any conplaints or enquires<p/><br/> With real-time tracking, secure payments, and a seamless user interface, DropMate ensures that your deliveries are not only efficient but also stress-free. It's time to embrace a smarter way to send and receive goods – it's time for DropMate.<p/><br/><br/>With Love from the DropMate Team<p/>Thank you for choosing DropMate.<p/><img src={logoUrl}></body></html>";
            _emailService.SendEmail(email, title, body);
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
        private async Task InitiateRefund(int id, string token)
        {
            PaymentHelper paymentHelper = new(token);

            using(HttpResponseMessage response = await paymentHelper.ApiHelper.PostAsync($"transactions/refund/{id}", new StringContent(null)))
            {
                if(response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                }
                else
                {
                    var reason = response.ReasonPhrase;
                    throw new PackageNotAlterableException($"Failed to initiate refund. {reason}");
                }
            }
        }
        private async Task InitiateServicePayment(int id, string token)
        {
            PaymentHelper paymentHelper = new(token);

            using (HttpResponseMessage response = await paymentHelper.ApiHelper.PostAsync($"confirm/{id}/{false}", new StringContent(null)))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                }
                else
                {
                    var reason = response.ReasonPhrase;
                    throw new PackageNotAlterableException($"Failed to initiate payment. {reason}");
                }
            }
        }
    }
}
