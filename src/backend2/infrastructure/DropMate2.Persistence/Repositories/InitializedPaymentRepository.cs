using DropMate2.Application.Contracts;
using DropMate2.Domain.Models;
using DropMate2.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Persistence.Repositories
{
    public class InitializedPaymentRepository :RepositoryBase<InitializedPayment>, IInitializedPaymentRepository
    {
        public InitializedPaymentRepository(RepositoryContext context):base(context)
        {
            
        }
        public void CreateInitializedPayment(InitializedPayment payment)
        {
            Add(payment);
        }

        public async Task<InitializedPayment> GetInitializedPaymentById(int id, bool trackChanges)
        {
            return await FindByCondition(i => i.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }

        public async Task<InitializedPayment> GetInitializedPaymentByRef(string referenceNumber, bool trackChanges)
        {
            return await FindByCondition(i => i.Reference.Equals(referenceNumber), trackChanges).FirstOrDefaultAsync();
        }
    }
}
