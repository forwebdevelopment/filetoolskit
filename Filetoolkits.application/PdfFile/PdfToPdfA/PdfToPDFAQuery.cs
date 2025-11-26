using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.PdfFile.PdfToPdfA
{
    public record PdfToPDFAQuery(FileForm file):IRequest<FileResponse>;
    
}
