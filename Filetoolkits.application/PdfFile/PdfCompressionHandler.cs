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
    public class PdfCompressionHandler : IRequestHandler<PdfCompressionQuery, FileResponse>
    {
        IPdfServices _pdfServices;

        public PdfCompressionHandler(IPdfServices pdfServices)
        {
            _pdfServices = pdfServices;
        }
        public Task<FileResponse> Handle(PdfCompressionQuery request, CancellationToken cancellationToken)
        {
            return  _pdfServices.FilePath(request.files);
        }
    }
}
