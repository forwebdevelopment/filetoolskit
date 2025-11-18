using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.IServices
{
    public interface ILockUnlockService
    {
        Task<FileResponse> LockUnlockFiles(LockFileParameter lockFile);
        //Task<FileResponse> UnLockFiles(LockFileParameter lockFile);
    }
}
