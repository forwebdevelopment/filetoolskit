using Filetoolkits.application.LockFiles;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Filetoolkits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockUnlockFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LockUnlockFileController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> LockPdfFile(IFormFile file, [FromForm] string password, [FromForm] string fileType, [FromForm] string operations)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            //    string response = await _services.LockFiles(file,password,"PDF");
            FileResponse response = await _mediator.Send(new LockUnlockFileQuery(new LockFileParameter
            {
                File = file,
                Password = password,
                FileType = fileType.ToUpper(),
                Operation = operations.ToUpper()

            }));
            var protectedBytes = await System.IO.File.ReadAllBytesAsync(response.LockFile); // must read response.Files

            // 4. Schedule deletion after response is sent
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
            return await ReturnFile(fileType, protectedBytes, response.LockFile);


        }



        private async Task<IActionResult> ReturnFile(string fileType, byte[] protectedBytes, string filePath)
        {
            var response = "";
            switch (fileType.ToUpper())
            {
                case "PDF":
                    return File(protectedBytes, "application/pdf", Path.GetFileName(filePath));

                case "WORD":
                    return File(protectedBytes,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                Path.GetFileName(filePath));

                case "XLS":
                    return File(protectedBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(filePath));

                case "PPT":
                    return File(
              protectedBytes,
             "application/vnd.openxmlformats-officedocument.presentationml.presentation", Path.GetFileName(filePath));

                default:
                    throw new NotImplementedException();

            }

        }



    }
}
