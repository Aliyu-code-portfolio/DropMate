using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class LoginFailedException:UnauthorizedException
    {
        public LoginFailedException():base("Wrong email or password")
        {
            
        }
    }
}
