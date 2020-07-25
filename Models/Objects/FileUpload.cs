using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Objects
{
    public class FileUpload
    {
        public IFormFile ImageFile { get; set; }
    }
}
