using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.infrastructure.Services
{
    public   class FileService: IPdfServices
    {
        IPdfFile _pdfFile;
       public FileService(IPdfFile pdfFile) 
        { 
           _pdfFile = pdfFile;
        }

        public  async Task<FileResponse>  FilePath(FileForm parameter)
        {

            string tempFolder = Path.GetTempPath();
            string tempFile = Path.Combine(tempFolder, parameter.File.FileName);

            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                await parameter.File.CopyToAsync(stream);
            }

            var response =  await _pdfFile.PdfCompression(tempFile);


            return new FileResponse
            {
                LockFile = response,
                TempFile = tempFile,
            };

        }


    }
}
