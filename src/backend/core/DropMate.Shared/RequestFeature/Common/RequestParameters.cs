using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.RequestFeature.Common
{
    public abstract class RequestParameters
    {
        const int MaxPageSize = 20;
        private int pageSize=2;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageNumber { get; set; } = 1;
        public string? OrderBy { get; set; }
    }
}
