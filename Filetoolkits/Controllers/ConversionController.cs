using Filetoolkits.application.FileConversion;
using Filetoolkits.application.LockFiles;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Filetoolkits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConversionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> LockPdfFile(IFormFile file, [FromForm] string conversionType, string fileReturnType)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            //    string response = await _services.LockFiles(file,password,"PDF");
            FileResponse response = await _mediator.Send(new FileConversionQuery(new FileConversionParameter
            {
                File = file,
                conversionType = conversionType
            }));
            var protectedBytes = await System.IO.File.ReadAllBytesAsync(response.LockFile); // must read response.Files  , here LockFile is just name 

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
            return await ReturnFile(fileReturnType, protectedBytes, response.LockFile);
        }
        private async Task<IActionResult> ReturnFile(string fileType, byte[] protectedBytes, string filePath)
        {
            switch (fileType.ToUpper())
            {
                case "PDF":
                    return File(protectedBytes, "application/pdf", Path.GetFileName(filePath));

                case "WORD":
                    return File(protectedBytes,
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        Path.GetFileName(filePath));

                case "XLS":
                    return File(protectedBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Path.GetFileName(filePath));

                case "PPT":
                    return File(protectedBytes,
                        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                        Path.GetFileName(filePath));

                case "JPG":
                case "JPEG":
                    return File(protectedBytes, "image/jpeg", Path.GetFileName(filePath));

                case "PNG":
                    return File(protectedBytes, "image/png", Path.GetFileName(filePath));

                case "BMP":
                    return File(protectedBytes, "image/bmp", Path.GetFileName(filePath));

                case "GIF":
                    return File(protectedBytes, "image/gif", Path.GetFileName(filePath));

                case "WEBP":
                    return File(protectedBytes, "image/webp", Path.GetFileName(filePath));

                default:
                    throw new NotImplementedException($"Unsupported file type: {fileType}");
            }
        }
    }
}
