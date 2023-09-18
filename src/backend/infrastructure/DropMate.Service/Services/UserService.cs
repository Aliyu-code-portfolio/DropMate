using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
using Microsoft.AspNetCore.Http;

namespace DropMate.Service.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<StandardResponse<UserResponseDto>> CreateUser(UserCreateRequestDto requestDto)
        {
            User user = _mapper.Map<User>(requestDto);
            _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            UserResponseDto responseDto = _mapper.Map<UserResponseDto>(user);
            return new StandardResponse<UserResponseDto>(201, true, string.Empty, responseDto);
        }

        public async Task DeleteUser(string id, bool trackChanges)
        {
            User user = await GetUserWithId(id, trackChanges);
            _unitOfWork.UserRepository.DeleteUser(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<(IEnumerable<UserResponseDto> users, MetaData metaData)>> GetAllUsers(UserRequestParameters requestParameter, bool trackChanges)
        {
            if (!requestParameter.IsValidDate)
            {
                throw new MaxJoinDateBadRequest();

            }
            PagedList<User> users = await _unitOfWork.UserRepository.GetAllUsersAsync(requestParameter, trackChanges);
            IEnumerable<UserResponseDto> usersDto = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return new StandardResponse<(IEnumerable<UserResponseDto> users, MetaData metaData)>(200, true, string.Empty, (usersDto, users.MetaData));
        }

        public async Task<StandardResponse<UserResponseDto>> GetUserByEmail(string email, bool trackChanges)
        {
            User user = await _unitOfWork.UserRepository.GetByEmailAsync(email, trackChanges);
            UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);
            return new StandardResponse<UserResponseDto>(200, true, string.Empty, userDto);
        }

        public async Task<StandardResponse<UserResponseDto>> GetUserById(string id, bool trackChanges)
        {
            User user = await GetUserWithId(id, trackChanges);
            UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);
            return new StandardResponse<UserResponseDto>(200, true, string.Empty, userDto);
        }

        public async Task RemoveProfileImg(string id)
        {
            User user = await GetUserWithId(id, false);
            if (user.ProfilePicURL == null)
                throw new ImageNotFoundException();
            bool status = _photoService.RemoveUploadedPhoto(id, "DropMateProfileImages");
            if (!status)
                throw new ImageNotFoundException();
            user.ProfilePicURL = null;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateUser(string id, UserUpdateRequestDto requestDto)
        {
            User user = await GetUserWithId(id, false);
            user.Address = requestDto.Address;
            user.PhoneNumber = requestDto.PhoneNumber;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<string> UploadProfileImg(string id, IFormFile file)
        {
            User user = await GetUserWithId(id, false);
            string url = _photoService.UploadPhoto(file, id, "DropMateProfileImages");
            user.ProfilePicURL = url;
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAsync();
            return url;
        }

        private async Task<User> GetUserWithId(string id, bool trackChanges)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(id, trackChanges)
                ?? throw new UserNotFoundException(id);
            return user;
        }
    }
}
