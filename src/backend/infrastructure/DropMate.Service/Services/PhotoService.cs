using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DropMate.Application.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Service.Services
{
    public class PhotoService : IPhotoService
    {
        public IConfiguration Configuration { get; }
        private Cloudinary _cloudinary;
        public PhotoService(IConfiguration configuration)
        {
            Configuration = configuration;
            var cloudinarySettings = Configuration.GetSection("CloudinarySettings");

            Account account = new Account(cloudinarySettings["CloudName"]
                , cloudinarySettings["ApiKey"], cloudinarySettings["ApiSecret"]
                );
            _cloudinary = new Cloudinary(account);
        }


        public string UploadPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            string url = uploadResult.Url.ToString();
            string publicId = uploadResult.PublicId;//work with this next time

            return url;
        }
        public bool RemoveUploadedPhoto(string url)
        {
            throw new NotImplementedException();
        }
    }
}
