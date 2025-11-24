using Filetoolkits.application.IPersistance;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.infrastructure.Persistance
{
    public class PdfFile : IPdfFile
    {
        public async Task<string> PdfCompression(string inputFilePath , string outputDir = "")
        {


            try
            {
                 outputDir = Path.Combine(
    Path.GetDirectoryName(inputFilePath),
    "compressed_" + Path.GetFileName(inputFilePath)
);

                int imageQuality = 50;

                using (var fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                using (var pdfDocument = new PdfLoadedDocument(fileStream))
                {
                    PdfCompressionOptions Options = new PdfCompressionOptions
                    {
                        CompressImages = true,
                        ImageQuality = Math.Clamp(imageQuality, 1, 100),
                        RemoveMetadata = true,
                        OptimizeFont = true
                    };

                    pdfDocument.Compress(Options);

                    using (var outputStream = new FileStream(outputDir, FileMode.Create, FileAccess.Write))
                    {
                        pdfDocument.Save(outputStream);
                    }

                    pdfDocument.Close(true);
                }

                return outputDir;
            }
            catch (Exception ex) {

               throw new Exception(ex.Message);
            }






        }

        // filePaths = array/list of PDF file paths you want to merge
        public async Task<string>MergeMultiplePDFs(string[] filePaths)
        {
            if (filePaths == null || filePaths.Length == 0)
                throw new ArgumentException("No PDF files provided.");

            //string outputPath = Path.Combine(
            //    Path.GetDirectoryName(filePaths[0]),
            //    "mergedPdf.pdf"
            //);

            try
            {
                string outputPath = Path.Combine(Path.GetTempPath(), "quickfilemerge" + ".pdf");

                using (PdfDocument finalDocument = new PdfDocument())
                {
                    foreach (var file in filePaths)
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                        {
                            PdfLoadedDocument loaded = new PdfLoadedDocument(fs);

                            finalDocument.ImportPageRange(loaded, 0, loaded.Pages.Count - 1);

                            loaded.Close(true); // ✅ Close loaded doc BEFORE fs closes
                        }
                        // ← Now fs is allowed to close
                    }

                    // Output must be open during save
                    using (FileStream outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        finalDocument.Save(outStream);  // ✔ No more ObjectDisposedException
                    }
                }

                return outputPath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error merging PDFs: " + ex.Message, ex);
            }
        }
    }
}