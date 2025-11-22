using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Response;
using MediatR;

namespace Filetoolkits.application.PdfFile.mergepdf
{
    public class MergePdfHandler : IRequestHandler<MergePdfQuery, FileResponse>
    {
       public IPdfServices _pdfServices;
        public MergePdfHandler(IPdfServices pdfServices)
        {
            _pdfServices = pdfServices;
        }

        public Task<FileResponse> Handle(MergePdfQuery request, CancellationToken cancellationToken)
        {
            return  _pdfServices.FilePathForMerge(request.files);
        }
    }
}
