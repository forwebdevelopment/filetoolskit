using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.FileConversion
{
    public class FileConversionHandler : IRequestHandler<FileConversionQuery, FileResponse>
    {
        public IfileConversionService _fileConversion;
        public FileConversionHandler(IfileConversionService fileConversion)
        {
            _fileConversion = fileConversion;
        }
        
        public Task<FileResponse> Handle(FileConversionQuery request, CancellationToken cancellationToken)
        {
            return _fileConversion.FileConversion(request.parameter);
        }
    }
}
