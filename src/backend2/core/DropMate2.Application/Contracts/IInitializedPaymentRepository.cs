using DropMate2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Application.Contracts
{
    public interface IInitializedPaymentRepository
    {
        Task<InitializedPayment> GetInitializedPaymentByRef(string referenceNumber, bool trackChanges);
        Task<InitializedPayment> GetInitializedPaymentById(string id, bool trackChanges);
        void DeleteInitializedPayment(InitializedPayment payment);
        void CreateInitializedPayment(InitializedPayment payment);
    }
}
