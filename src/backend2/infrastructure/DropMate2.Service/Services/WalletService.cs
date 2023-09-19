using AutoMapper;
using DropMate2.Application.Common;
using DropMate2.Application.Contracts;
using DropMate2.Application.ServiceContracts;
using DropMate2.Domain.Models;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.Exceptions.Sub;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Service.Services
{
    public class WalletService : IWalletService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateWallet(WalletRequestDto walletDto)
        {
            //_ = await GetWalletWithId(walletDto.Id, false); 
            Wallet wallet = _mapper.Map<Wallet>(walletDto);
            _unitOfWork.WalletRepository.CreateWallet(wallet);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteWallet(string id)
        {
            Wallet wallet = await GetWalletWithId(id, false);
            _unitOfWork.WalletRepository.DeleteWallet(wallet);
            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<(IEnumerable<WalletResponseDto> wallets, MetaData metaData)>> GetAllWallets(WalletRequestParameter requestParameters, bool trackChanges)
        {
            PagedList<Wallet> wallets =await _unitOfWork.WalletRepository.GetAllWalletsAsync(requestParameters, trackChanges);
            IEnumerable<WalletResponseDto> responseDtos = _mapper.Map<IEnumerable<WalletResponseDto>>(wallets);
            return StandardResponse<(IEnumerable<WalletResponseDto> wallets, MetaData metaData)>.Success("Successfully retrieve wallets", (responseDtos, wallets.MetaData));
        }

        public async Task<StandardResponse<WalletResponseDto>> GetWalletById(string id, bool trackChanges)
        {
            Wallet wallet = await GetWalletWithId(id, trackChanges);
            WalletResponseDto responseDto = _mapper.Map<WalletResponseDto>(wallet);
            return StandardResponse<WalletResponseDto>.Success("Successfully retrieve wallet", responseDto);
        }

        public async Task UpdateWallet(WalletRequestDto walletDto)
        {
            _ = await GetWalletWithId(walletDto.Id, false);
            Wallet wallet = _mapper.Map<Wallet>(walletDto);
            _unitOfWork.WalletRepository.UpdateWallet(wallet);
            await _unitOfWork.SaveAsync();
        }
        private async Task<Wallet> GetWalletWithId(string id, bool trackChanges)
        {
            return await _unitOfWork.WalletRepository.GetWalletByIdAsync(id, trackChanges)
                ?? throw new WalletNotFoundException(id);
        }
    }
}
