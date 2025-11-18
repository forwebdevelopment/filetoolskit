using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.PdfFile
{
    public record PdfCompressionQuery(FileForm files):IRequest<FileResponse>;
    
  

}
