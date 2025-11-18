using Filetoolkits.application.PdfFile;
using Filetoolkits.domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Filetoolkits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        public readonly IMediator _mediator;
        public PdfController(IMediator mediator)
        {
            _mediator = mediator;
               
        
        }  



        [HttpPost]
        public async Task<IActionResult> PdfCompression(IFormFile file)
        {
            if(file==null || file.Length == 0) { return BadRequest("file not uploaded"); }

            var response = await  _mediator.Send(new PdfCompressionQuery(new FileForm
            {
                File = file
            }));


            var protectedBytes = await System.IO.File.ReadAllBytesAsync(response.LockFile);
            Response.OnCompleted(() =>
            {
                try
                {
                    if (System.IO.File.Exists(response.TempFile))
                        System.IO.File.Delete(response.TempFile);
                    if (System.IO.File.Exists(response.LockFile))
                        System.IO.File.Delete(response.LockFile);
                }
                catch { /* ignore errors */ }

                return Task.CompletedTask;
            });
            return File(protectedBytes, "application/pdf", Path.GetFileName(response.LockFile));
        }
    }
}
