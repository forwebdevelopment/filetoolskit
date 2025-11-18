using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using Filetoolkits.infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.infrastructure.Services
{
    public class LockUnlockService: ILockUnlockService
    {
        public ILockFiles _lockfiles;
        public IUnlockFiles _unlockFiles;
        public LockUnlockService(ILockFiles lockfiles, IUnlockFiles unlockFiles)
        {
            _lockfiles = lockfiles;
            _unlockFiles = unlockFiles;
        }
        public async Task<FileResponse> LockUnlockFiles(LockFileParameter lockFile)
            {

            // Use a proper temp path with .pdf extension
            string tempFolder = Path.GetTempPath();
            string tempFile = Path.Combine(tempFolder, lockFile.File.FileName);

            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                await lockFile.File.CopyToAsync(stream);
            }
            string response = "";
            if (lockFile.Operation == "UNLOCK")
            {
                 response = await GetUnLockFileResponse(tempFile, lockFile.Password, lockFile.FileType);
            }
            else
            {
                 response = await GetLockFileResponse(tempFile, lockFile.Password, lockFile.FileType);
            }

                

            return new FileResponse
            {
                LockFile = response,
                TempFile = tempFile,
            };
        }

        //public async Task<FileResponse> UnLockFiles(LockFileParameter lockFile)
        //{
        //    // Use a proper temp path with .pdf extension
        //    string tempFolder = Path.GetTempPath();
        //    string tempFile = Path.Combine(tempFolder, lockFile.File.FileName);

        //    using (var stream = new FileStream(tempFile, FileMode.Create))
        //    {
        //        await lockFile.File.CopyToAsync(stream);
        //    }

        //    string response = await GetUnLockFileResponse(tempFile, lockFile.Password, lockFile.FileType);
        //    return new FileResponse
        //    {
        //        LockFile = response,
        //        TempFile = tempFile,
        //    };
        //}
        private async Task<string> GetLockFileResponse(string TempFile, string password, string fileType)
        {
            var response = "";
            switch (fileType)
            {
                case "PDF":
                    response = await _lockfiles.LockPdfFile(TempFile, password);
                    break;
                case "XLS":
                    response = await _lockfiles.LockExcelFile(TempFile, password);
                    break;
                case "WORD":
                    response = await _lockfiles.LockWordFile(TempFile, password);
                    break;
                case "PPT":
                    response = await _lockfiles.LockPresentationFile(TempFile, password);
                    break;

            }

            return response;

        }


        private async Task<string> GetUnLockFileResponse(string TempFile, string password, string fileType)
        {
            var response = "";
            switch (fileType.ToUpper())
            {
                case "PDF":
                    response = await _unlockFiles.UnLockPdfFile(TempFile, password);
                    break;
                case "XLS":
                    response = await _unlockFiles.UnLockExcelFile(TempFile, password);
                    break;
                case "WORD":
                    response = await _unlockFiles.UnLockWordFile(TempFile, password);
                    break;
                case "PPT":
                    response = await _unlockFiles.UnLockPresentationFile(TempFile, password);
                    break;

            }

            return response;

        }
    }
}
