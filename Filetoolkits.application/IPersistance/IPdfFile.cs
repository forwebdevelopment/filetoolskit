using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.IPersistance
{
    public interface IPdfFile
    {
         Task<string> PdfCompression(string filePath , string outputDir="");
         Task<string> MergeMultiplePDFs(string[] filePaths);
    }
}