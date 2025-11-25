using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.IServices
{
    public interface IPdfServices
    {
        Task<FileResponse> FilePath(FileForm parameter);
        Task<FileResponse> FilePathForMerge(List<IFormFile> parameter);
        Task<FileResponse> splitFilePath(SplitFileParam parameter);
        Task<FileResponse> FilePathForWaterMark(WaterMark parameter);
    }
}
