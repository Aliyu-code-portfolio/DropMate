using AutoMapper;
using DropMate2.Application.Common;
using DropMate2.Application.ServiceContracts;
using DropMate2.Domain.Models;
using DropMate2.Shared.Dtos.Request;
using DropMate2.Shared.Dtos.Request.Payment;
using DropMate2.Shared.Dtos.Response;
using DropMate2.Shared.Dtos.Response.Payment;
using DropMate2.Shared.Exceptions.Sub;
using DropMate2.Shared.HelperModels;
using DropMate2.Shared.RequestFeatures;
using DropMate2.Shared.RequestFeatures.Common;
using System.Text;
using System.Text.Json;

namespace DropMate2.Service.Services
{
    public class DepositService : IDepositService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public PayStackHelper PayStackHelper;
        public DepositService(IUnitOfWork unitOfWork, IMapper mapper, PayStackHelper payStackHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            PayStackHelper = payStackHelper;
        }


        public async Task DeleteDeposit(int id)
        {
            Deposit deposit = await GetDepositWithId(id, false);
            _unitOfWork.DepositRepository.DeleteDeposit(deposit);
            await _unitOfWork.SaveAsync();
        }

        public async Task<StandardResponse<(IEnumerable<DepositResponseDto> deposits, MetaData metaData)>> GetAllDeposit(DepositRequestParameter requestParameters, bool trackChanges)
        {
            PagedList<Deposit> deposits =await _unitOfWork.DepositRepository.GetAllDepositsAsync(requestParameters, trackChanges);
            IEnumerable<DepositResponseDto> responseDtos = _mapper.Map <IEnumerable<DepositResponseDto>>(deposits);
            return StandardResponse<(IEnumerable<DepositResponseDto> deposits, MetaData metaData)>
                .Success("Successfully retrieved deposits",(responseDtos,deposits.MetaData));
        }

        public async Task<StandardResponse<(IEnumerable<DepositResponseDto> deposits, MetaData metaData)>> GetAllWalletDeposit(DepositRequestParameter requestParameters, string wallerId, bool trackChanges)
        {
            PagedList<Deposit> deposits = await _unitOfWork.DepositRepository.GetAllWalletDepositsAsync(requestParameters, wallerId, trackChanges);
            IEnumerable<DepositResponseDto> responseDtos = _mapper.Map<IEnumerable<DepositResponseDto>>(deposits);
            return StandardResponse<(IEnumerable<DepositResponseDto> deposits, MetaData metaData)>
                .Success("Successfully retrieved deposits", (responseDtos, deposits.MetaData));
        }

        public async Task<StandardResponse<DepositResponseDto>> GetDepositById(int id, bool trackChanges)
        {
            Deposit deposit = await GetDepositWithId(id, trackChanges);
            DepositResponseDto responseDto = _mapper.Map<DepositResponseDto>(deposit);
            return StandardResponse<DepositResponseDto>.Success("Successfully retrieved deposit", responseDto);
        }
        //PayStack inplementations
        public async Task<StandardResponse<Data>> InitializeDeposit(DepositRequestDto depositRequest)
        {
            _ = await _unitOfWork.WalletRepository.GetWalletByIdAsync(depositRequest.WalletId, false)
                ?? throw new WalletNotFoundException(depositRequest.WalletId);
            string initPayId = Guid.NewGuid().ToString();
            InitializePaymentResponseDto initializePaymentResponse;
            InitializePaymentRequestDto initializePaymentRequest = new()
            {
                email = depositRequest.Email,
                amount = depositRequest.Amount.ToString()+"00",
                callback_url = "https://bit.ly/paystack1123"//use this endpoint to call your endpoint /deposits/confirm/{initPayId}  to verify the payment made
            };
            var serializedDto = JsonSerializer.Serialize(initializePaymentRequest);
            var httpContent = new StringContent(serializedDto, Encoding.UTF8,"application/json");
            using(HttpResponseMessage response =await PayStackHelper.ApiClient
                .PostAsync("transaction/initialize", httpContent))
            {
                if(response.IsSuccessStatusCode)
                {
                    initializePaymentResponse = await response.Content.ReadAsAsync<InitializePaymentResponseDto>();
                    initializePaymentResponse.data.status = "Active";
                    initializePaymentResponse.data.id = initPayId;
                }
                else
                {
                        throw new PaymentFailedException(response.ReasonPhrase);
                }
            }
            InitializedPayment initializedPayment = new()
            {
                Id = initPayId,
                WalletId = depositRequest.WalletId,
                Authorization_url = initializePaymentResponse.data.authorization_url,
                Amount = depositRequest.Amount,
                Access_code = initializePaymentResponse.data.access_code,
                Reference = initializePaymentResponse.data.reference
            };
            _unitOfWork.InitializedPaymentRepository.CreateInitializedPayment(initializedPayment);
            await _unitOfWork.SaveAsync();
            return StandardResponse<Data>
                .Success("Successfully initialized a payment... Continue to check out using url provided", initializePaymentResponse.data);
        }
        public async Task CompleteDeposit(string paymentId)
        {
            InitializedPayment initializedPayment =await _unitOfWork.InitializedPaymentRepository
                .GetInitializedPaymentById(paymentId, false)
                ?? throw new DepositBadRequestException(paymentId);
            InitializePaymentResponseDto initializePaymentResponse;
            using (HttpResponseMessage responseMessage =await PayStackHelper.ApiClient.GetAsync($"transaction/verify/{initializedPayment.Reference}"))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    initializePaymentResponse = await responseMessage.Content.ReadAsAsync<InitializePaymentResponseDto>();
                    if (initializePaymentResponse.data.status != "success")
                    {
                        throw new PaymentNotMadeExceptioin(initializedPayment.Reference);
                    }
                }
                else
                {
                    throw new PaymentNotMadeExceptioin();
                }
            }
            Wallet creditWallet = await _unitOfWork.WalletRepository.GetWalletByIdAsync(initializedPayment.WalletId,false)
                ?? throw new WalletNotFoundException(initializedPayment.WalletId);
            creditWallet.Balance += initializedPayment.Amount;
            Deposit deposit = new()
            {
                WalletId = initializedPayment.WalletId,
                Amount = initializedPayment.Amount,
                Reference = initializedPayment.Reference
            };
            _unitOfWork.DepositRepository.CreateDeposit(deposit);
            _unitOfWork.WalletRepository.UpdateWallet(creditWallet);
            _unitOfWork.InitializedPaymentRepository.DeleteInitializedPayment(initializedPayment);
            await _unitOfWork.SaveAsync();
        }
        private async Task<Deposit> GetDepositWithId(int id, bool trackChanges)
        {
            return await _unitOfWork.DepositRepository.GetDepositeByIdAsync(id, trackChanges)
                ?? throw new DepositNotFoundException(id);
        }
    }
}
