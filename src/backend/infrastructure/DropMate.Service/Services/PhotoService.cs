using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DropMate.Application.ServiceContracts;
using DropMate.Shared.Exceptions.Sub;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var cloudinarySettings = Configuration.GetSection("CloudinaryConfig");

            Account account = new Account(cloudinarySettings["CloudName"]
                , cloudinarySettings["ApiKey"], cloudinarySettings["ApiSecret"]
                );
            _cloudinary = new Cloudinary(account);
        }


        public string UploadPhoto(IFormFile file, string id, string folder)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        PublicId = id,
                        Folder = folder
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                    
                }
            }
            string url = uploadResult.Url.ToString();
            string publicId = uploadResult.PublicId;

            //check quality of uploaded image
            CheckQuality(folder, publicId);
            return url;
        }


        public bool RemoveUploadedPhoto(string id, string folder)
        {
            string path = $"{folder}/{id}";
            var deleteParams = new DelResParams()
            {
                PublicIds = new List<string> { path },
                Type = "upload",
                ResourceType = ResourceType.Image
            };
            var result = _cloudinary.DeleteResources(deleteParams);
            if (result.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        }
        private void CheckQuality(string folder, string publicId)
        {
            var getResourceParams = new GetResourceParams(publicId)
            {
                QualityAnalysis = true
            };
            var getResourceResult = _cloudinary.GetResource(getResourceParams);
            var resultJson = getResourceResult.JsonObj;
            var analysis = resultJson["quality_analysis"];
            var analysisInDouble = double.Parse(analysis["focus"].ToString());
            if (!(analysisInDouble >= 0.6))
            {
                RemoveUploadedPhoto(publicId, folder);
                throw new ImageBadQualityException();
            }
        }
    }
}
