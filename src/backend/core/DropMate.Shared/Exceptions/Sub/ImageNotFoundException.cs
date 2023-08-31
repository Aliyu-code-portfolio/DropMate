using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class ImageNotFoundException:NotFoundException
    {
        public ImageNotFoundException():base("The image was not found in the server")
        {
            
        }
    }
}
