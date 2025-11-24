using Filetoolkits.application.IServices;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Filetoolkits.application.PdfFile
{
    public class PdfSplitHandler : IRequestHandler<PdfSplitQuery, FileResponse>
    {
        IPdfServices _pdfServices;

        public PdfSplitHandler(IPdfServices pdfServices)
        {
            _pdfServices = pdfServices;
        }
        public Task<FileResponse> Handle(PdfSplitQuery request, CancellationToken cancellationToken)
        {
            return  _pdfServices.splitFilePath(request.files);
        }
    }
}
