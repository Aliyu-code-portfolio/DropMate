using AutoMapper;
using DropMate.Application.Common;
using DropMate.Application.ServiceContracts;
using DropMate.Domain.Models;
using DropMate.Shared.Dtos.Request;
using DropMate.Shared.Dtos.Response;
using DropMate.Shared.Exceptions.Sub;

namespace DropMate.Service.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StandardResponse<UserResponseDto>> CreateUser(UserCreateRequestDto requestDto)
        {
            User user = _mapper.Map<User>(requestDto);
            _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            UserResponseDto responseDto = _mapper.Map<UserResponseDto>(user);
            return new StandardResponse<UserResponseDto>(201,true,string.Empty,responseDto);
        }

        public async Task DeleteUser(string id, bool trackChanges)
        {
            User user = await GetUserWithId(id,trackChanges);
            _unitOfWork.UserRepository.DeleteUser(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<IEnumerable<UserResponseDto>>> GetAllUsers(bool trackChanges)
        {
            IEnumerable<User> users = await _unitOfWork.UserRepository.GetAllUsersAsync(trackChanges);
            IEnumerable<UserResponseDto> usersDto = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return new StandardResponse<IEnumerable<UserResponseDto>>(200, true, string.Empty,usersDto);
        }

        public async Task<StandardResponse<UserResponseDto>> GetUserByEmail(string email, bool trackChanges)
        {
            User user = await _unitOfWork.UserRepository.GetByEmailAsync(email,trackChanges);
            UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);
            return new StandardResponse<UserResponseDto>(200, true, string.Empty, userDto);
        }

        public async Task<StandardResponse<UserResponseDto>> GetUserById(string id, bool trackChanges)
        {
            User user = await GetUserWithId(id, trackChanges);
            UserResponseDto userDto = _mapper.Map<UserResponseDto>(user);
            return new StandardResponse<UserResponseDto>(200,true,string.Empty,userDto);
        }

        public async Task UpdateUser(string id, UserUpdateRequestDto requestDto)
        {
            _ = await GetUserWithId(id, false);
            User user = _mapper.Map<User>(requestDto);
            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAsync();
        }
        private async Task<User> GetUserWithId(string id, bool trackChanges)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(id, trackChanges)
                ?? throw new UserNotFoundException(id);
            return user;
        }
    }
}
