using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Shared.RequestFeature
{
    public class UserRequestParameters:RequestParameters
    {
        public DateTime MinJoinDate { get; set; }= DateTime.MinValue;
        public DateTime MaxJoinDate { get; set; }=DateTime.MaxValue;
        public bool IsValidDate => MaxJoinDate >= MinJoinDate;
    }
}
