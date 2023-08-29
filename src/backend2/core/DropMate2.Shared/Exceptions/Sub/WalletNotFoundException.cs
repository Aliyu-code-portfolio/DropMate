using DropMate2.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate2.Shared.Exceptions.Sub
{
    public class WalletNotFoundException:NotFoundException
    {
        public WalletNotFoundException(string id):base($"The wallet with id: {id} is not found in the database")
        {
            
        }
        public WalletNotFoundException(string id, string walletOwner) : base($"The wallet with id: {id} for {walletOwner} is not found in the database")
        {

        }
    }
}
