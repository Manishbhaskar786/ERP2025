using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Common.Dto
{
    public class FileDto
    {
        public long Id { get; set; }
        public IFormFile File { get; set; }
    }
}
