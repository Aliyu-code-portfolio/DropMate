using DropMate.Shared.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Exceptions.Sub
{
    public class ImageBadQualityException:BadRequestException
    {
        public ImageBadQualityException():base("The quality of image uploaded did not reach the accepted standard")
        {
            
        }
    }
}
