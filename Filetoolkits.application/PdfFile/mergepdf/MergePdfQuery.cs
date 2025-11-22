using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filetoolkits.domain.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Filetoolkits.application.PdfFile.mergepdf
{
    public record MergePdfQuery(List<IFormFile> files):IRequest<FileResponse>;
   
}
