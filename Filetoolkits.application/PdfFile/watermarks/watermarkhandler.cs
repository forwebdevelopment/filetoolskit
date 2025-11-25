using Filetoolkits.application.IServices;
using Filetoolkits.domain.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Filetoolkits.application.PdfFile.watermarks
{
    public class watermarkhandler : IRequestHandler<watermarkQuery, FileResponse>
    {
        IPdfServices _pdfServices;

        public watermarkhandler(IPdfServices pdfServices)
        {
            _pdfServices = pdfServices;
        }
        public Task<FileResponse> Handle(watermarkQuery request, CancellationToken cancellationToken)
        {
            return _pdfServices.FilePathForWaterMark(request.waterMark);
        }
    }
}
