using Filetoolkits.application.IServices;
using Filetoolkits.domain.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.PdfFile.PdfToPdfA
{
    public class PdfToPDFHandler : IRequestHandler<PdfToPDFAQuery, FileResponse>
    {
       public IPdfServices _pdfServices;

        public PdfToPDFHandler(IPdfServices pdfServices)
        {
           _pdfServices= pdfServices;
        }
        public Task<FileResponse> Handle(PdfToPDFAQuery request, CancellationToken cancellationToken)
        {
           return   _pdfServices.PdfA(request.file);
        }
    }
}
