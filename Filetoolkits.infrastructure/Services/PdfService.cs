using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using Microsoft.AspNetCore.Http;
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

            string tempFile = await TempFileStore(parameter.File);

            var response =  await _pdfFile.PdfCompression(tempFile);


            return new FileResponse
            {
                LockFile = response,
                TempFile = tempFile,
            };

        }


        private async Task<string> TempFileStore(IFormFile filepath)
        {
            string tempFolder = Path.GetTempPath();
            string tempFile = Path.Combine(tempFolder, filepath.FileName);

            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                await filepath.CopyToAsync(stream);
            }

            return tempFile;
        }



        public async Task<FileResponse> FilePathForWaterMark(WaterMark parameter)
        {

            string tempFile = await TempFileStore(parameter.File);

            var response = await _pdfFile.AddWaterMark(tempFile , parameter.watermarktext);


            return new FileResponse
            {
                LockFile = response,
                TempFile = tempFile,
            };

        }




        public async Task<FileResponse> splitFilePath(SplitFileParam parameter)
        {

            string tempFile = await TempFileStore(parameter.File);

            var response = "";
            if (parameter.operation == "splitByPageSize")
            {
                response = await _pdfFile.SplitAndZipPdfByFixedNumber(tempFile, parameter.PagePerSize);
            } else if (parameter.operation =="splitByRange")
            {
                response = await _pdfFile.ExtractPageRangeAndSave(tempFile , parameter.startRange,parameter.endRange);
            }            


            return new FileResponse
            {
                LockFile = response,
                TempFile = tempFile,
            };

        }


        public async Task<FileResponse> FilePathForMerge(List<IFormFile> parameter)
        {
            if (parameter == null || parameter.Count == 0)
                throw new Exception("No files uploaded");

            // Create a unique temporary folder
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);

            List<string> savedFilePaths = new List<string>();

            // Save each uploaded file into temp folder
            foreach (var file in parameter)
            {
                string tempFilePath = Path.Combine(tempFolder, file.FileName);

                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedFilePaths.Add(tempFilePath);
            }

            // Call merge service
            var mergedFilePath = await _pdfFile.MergeMultiplePDFs(savedFilePaths.ToArray());

            return new FileResponse
            {
                LockFile = mergedFilePath,  // final merged PDF path
                TempFolder = tempFolder     // entire temp folder for cleanup
            };
        }


    }
}
