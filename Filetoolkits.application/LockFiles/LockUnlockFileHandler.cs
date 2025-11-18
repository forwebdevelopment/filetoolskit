
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.LockFiles
{
    public class LockUnlockFileHandler : IRequestHandler<LockUnlockFileQuery, FileResponse>
    {
        public ILockUnlockService _services;
        public LockUnlockFileHandler(ILockUnlockService services)
        {
            _services = services;
        }
        public async Task<FileResponse> Handle(LockUnlockFileQuery request, CancellationToken cancellationToken)
        {
            return  await _services.LockUnlockFiles(request.LockFile);
        }
    }
}
