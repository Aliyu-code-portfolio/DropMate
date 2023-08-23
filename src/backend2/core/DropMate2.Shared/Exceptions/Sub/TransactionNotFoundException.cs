using DropMate2.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class TransactionNotFoundException:NotFoundException
    {
        public TransactionNotFoundException(int id):base($"The transaction with id: {id} is not found in the database")
        {
            
        }
    }
}
