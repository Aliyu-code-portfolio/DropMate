using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Application.ServiceContracts
{
    public interface IPhotoService
    {
        string UploadPhoto(IFormFile file);
        bool RemoveUploadedPhoto(string url);
    }
}
