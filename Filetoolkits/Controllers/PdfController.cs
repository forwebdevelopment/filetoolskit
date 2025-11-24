using Filetoolkits.application.IServices;
using Filetoolkits.application.PdfFile;
using Filetoolkits.application.PdfFile.mergepdf;
using Filetoolkits.domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Filetoolkits.Controllers
{
    [Route("api/pdf")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        public readonly IMediator _mediator;
        public PdfController(IMediator mediator)
        {
            _mediator = mediator;
               
        
        }  



        [HttpPost("compression")]
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


        [HttpPost("merge")]
        public async Task<IActionResult> MergePdf(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded");

            var response = await _mediator.Send(new MergePdfQuery(files));

            // Read merged PDF file
            var mergedBytes = await System.IO.File.ReadAllBytesAsync(response.LockFile);

            // Cleanup temp files after response completes
            Response.OnCompleted(() =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.TempFolder) && Directory.Exists(response.TempFolder))
                    {
                        Directory.Delete(response.TempFolder, true);
                    }

                    if (System.IO.File.Exists(response.LockFile))
                        System.IO.File.Delete(response.LockFile);
                }
                catch
                {
                    // Ignore cleanup errors
                }

                return Task.CompletedTask;
            });

            return File(
                mergedBytes,
                "application/pdf",
                Path.GetFileName(response.LockFile)
            );
        }

        [HttpPost("SplitPdf")]
        [Produces("application/zip")]
        public async Task<IActionResult> SplitAndZipPdf(SplitFileParam param)
        {
            if (param.File == null || param.File.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }

           

            try
            {
               

                // Call the service method to split the temp file and generate the zip archive
                // We set the pagesPerFile to 4 as a default example.
                var zipFilePath = await _mediator.Send(new PdfSplitQuery(new SplitFileParam
                {
                    PagePerSize = param.PagePerSize,
                    File = param.File,
                    operation = param.operation,
                    startRange = param.startRange,
                    endRange = param.endRange
                    
                }));

                // Read the generated ZIP file into memory
                var zipBytes = await System.IO.File.ReadAllBytesAsync(zipFilePath.LockFile);
                var zipFileName = Path.GetFileName(zipFilePath.LockFile);

                // --- Cleanup Logic ---
                // Ensure temporary input and output files are deleted after the response is sent to the client.
                Response.OnCompleted(() =>
                {
                    try
                    {
                        if (System.IO.File.Exists(zipFilePath.LockFile))
                            System.IO.File.Delete(zipFilePath.LockFile);

                        if (System.IO.File.Exists(zipFilePath.TempFile))
                            System.IO.File.Delete(zipFilePath.TempFile);
                    }
                    catch
                    {
                        // Log cleanup errors if necessary, but don't stop the main task.
                    }
                    return Task.CompletedTask;
                });
                // --- End Cleanup Logic ---

                // Return the file response as a ZIP file

                if(param.operation == "splitByPageSize")
                {
                    return File(
                    zipBytes,
                    "application/zip", // The correct MIME type for a ZIP file
                    zipFileName
                );

                }
                else
                {
                    return File(
               zipBytes,
               "application/pdf",
               Path.GetFileName(zipFilePath.LockFile)
           );
                }

               
            }
            catch (Exception ex)
            {
                // Basic error handling
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
           
        }
    }

}

