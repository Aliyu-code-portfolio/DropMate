using AutoMapper;
using DropMate2.Domain.Models;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;

namespace DropMate2.WebAPI
{
    public class ProfileMapping:Profile
    {
        public ProfileMapping()
        {
            CreateMap<Transaction, TransactionResponseDto>();
            CreateMap<TransactionRequestDto, Transaction>();

        }
    }
}
