using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.Utilities
{
    public static class Utility
    {
        public static int GeneratePackageCode()
        {
            return new Random().Next(1000, 9999);
        }
    }
}
