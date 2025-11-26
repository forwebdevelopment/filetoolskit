using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Filetoolkits.application.IPersistance
{
    public interface IPdfFile
    {
         Task<string> PdfCompression(string filePath , string outputDir="");
         Task<string> MergeMultiplePDFs(string[] filePaths);
         Task<string> SplitAndZipPdfByFixedNumber(string inputFilePath, int pagesPerFile = 4);
         Task<string> ExtractPageRangeAndSave(string inputFilePath, int startPage, int endPage);

         Task<string> AddWaterMark(string inputFilePath , string waterMarkText);
         Task<string> ConvertPdfToPdfA(string inputFilePath);
    }
}