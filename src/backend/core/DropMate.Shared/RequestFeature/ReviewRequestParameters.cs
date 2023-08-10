using DropMate.Shared.RequestFeature.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Shared.RequestFeature
{
    public class ReviewRequestParameters:RequestParameters
    {
        public uint MinRating { get; set; } = 1;
        public uint MaxRating { get; set; } = 5;
        public bool IsValidRating => MaxRating >= MinRating;

    }
}
