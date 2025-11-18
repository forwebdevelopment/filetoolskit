using Filetoolkits.application.IPersistance;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
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
                using var fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                PdfLoadedDocument pdfDocument = new PdfLoadedDocument(fileStream);  
                PdfCompressionOptions Options = new PdfCompressionOptions();
                Options.CompressImages = true;
                Options.ImageQuality = Math.Clamp(imageQuality, 1, 100);
                Options.RemoveMetadata = true;
                Options.OptimizeFont = true;
                pdfDocument.Compress(Options);
                FileStream outputStream = new FileStream(outputDir, FileMode.Create, FileAccess.Write);
                pdfDocument.Save(outputStream);

                return outputDir;
            }
            catch (Exception ex) {

               throw new Exception(ex.Message);
            }


            //int imageQuality = 50;

            //if (!File.Exists(inputFilePath))
            //    throw new FileNotFoundException("Input PDF file not found.", inputFilePath);

            //if (!Directory.Exists(outputDir))
            //    Directory.CreateDirectory(outputDir);

            //using var fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
            //using var loadedDoc = new PdfLoadedDocument(fileStream);

            //// Create output path
            //string outputFilePath = Path.Combine(outputDir,
            //    Path.GetFileNameWithoutExtension(inputFilePath) + "_page1.png");
            //outputFilePath = Path.ChangeExtension(outputFilePath, ".pdf");

            //// Use PdfSaveOptions for compression
            //var saveOptions = new PdfCompressionOptions
            //{
            //    CompressImages = true,
            //    ImageQuality = Math.Clamp(imageQuality, 1, 100), // 1..100
            //    RemoveMetadata = true,
            //    OptimizeFont = true
            //};

            //using var outStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
            //loadedDoc.Save(outStream, saveOptions);

            //return outputFilePath;




        }
    }
    }