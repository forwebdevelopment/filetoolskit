using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Filetoolkits.domain.Entity
{

    public class FileForm
    {
        public IFormFile File { get; set; }
        public List<IFormFile> Files { get; set; }
    }
    public class LockFileParameter: FileForm
    {
      
        public string Password { get; set; }
        public string FileType { get; set; }
        public string Operation { get; set; }
    }


    public class FileConversionParameter: FileForm
    {
   
        public string conversionType { get; set; }
    }


   
}
