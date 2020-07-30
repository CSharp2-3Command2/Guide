﻿

using System.IO;
using Microsoft.AspNetCore.Http;

namespace Guide.Services
{
    public class UploadService
    {
        public async void Upload(string path, string fileName, IFormFile file)
        {
            using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}