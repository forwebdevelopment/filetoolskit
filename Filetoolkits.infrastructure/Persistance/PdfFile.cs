using Filetoolkits.application.IPersistance;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
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


        public async Task<string> SplitAndZipPdfByFixedNumber(string inputFilePath, int pagesPerFile = 4)
        {
            // ... (Argument validation code omitted for brevity) ...
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException($"The file was not found: {inputFilePath}", inputFilePath);
            if (pagesPerFile <= 0)
                throw new ArgumentOutOfRangeException(nameof(pagesPerFile), "Pages per file must be a positive number.");

            List<string> createdFiles = new List<string>();
            string outputZipPath = null;
            string outputDirectory = Path.GetDirectoryName(inputFilePath) ?? Path.GetTempPath();

            try
            {
                // 1. Perform the PDF splitting operation (using manual iteration as discussed previously)
                using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                {
                    PdfLoadedDocument loadedDocument = new PdfLoadedDocument(inputStream);
                    int totalPages = loadedDocument.Pages.Count;
                    string fileNameBase = Path.GetFileNameWithoutExtension(inputFilePath);
                    int startPage = 0;
                    int fileIndex = 1;

                    while (startPage < totalPages)
                    {
                        int endPage = Math.Min(startPage + pagesPerFile, totalPages);

                        using (PdfDocument newDocument = new PdfDocument())
                        {
                            newDocument.ImportPageRange(loadedDocument, startPage, endPage - 1);
                            string outputPath = Path.Combine(outputDirectory, $"{fileNameBase}_Part{fileIndex}.pdf");

                            using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                newDocument.Save(outputStream);
                            }
                            createdFiles.Add(outputPath); // Track created files
                        }

                        startPage = endPage;
                        fileIndex++;
                    }
                    loadedDocument.Close(true);
                }

                // 2. Create the ZIP archive using System.IO.Compression
                outputZipPath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputFilePath) + "_Splits.zip");

                // This creates an empty zip file and allows adding entries to it
                using (var zipArchiveStream = new FileStream(outputZipPath, FileMode.Create))
                using (var archive = new ZipArchive(zipArchiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var filePath in createdFiles)
                    {
                        // Adds the file to the zip archive using its original filename as the entry name
                        archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
                    }
                }

                // 3. Clean up the individual PDF files after zipping (Optional, but usually desired)
                foreach (var file in createdFiles)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }

                // Return the path to the final ZIP file
                return outputZipPath;
            }
            catch (Exception ex)
            {
                // Clean up partial files if an error occurred during split or zipping
                foreach (var file in createdFiles)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                // Clean up the partial zip file if an error occurred during zipping
                if (File.Exists(outputZipPath))
                {
                    File.Delete(outputZipPath);
                }
                throw new Exception($"Error during PDF splitting or zipping process: {ex.Message}", ex);
            }
        }


        public async Task<string> ExtractPageRangeAndSave(string inputFilePath, int startPage, int endPage)
        {
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException($"The file was not found: {inputFilePath}", inputFilePath);

            string outputFilePath = null;

            try
            {
                using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                {
                    PdfLoadedDocument loadedDocument = new PdfLoadedDocument(inputStream);

                    if (startPage < 1 || endPage > loadedDocument.Pages.Count || startPage > endPage)
                    {
                        loadedDocument.Close(true);
                        throw new ArgumentException("Invalid page range specified.");
                    }

                    // Define the output path where the file will be saved.
                    // We'll place it in the same directory as the input file for simplicity.
                    string outputDirectory = Path.GetDirectoryName(inputFilePath) ?? Path.GetTempPath();
                    string outputFileName = $"{Path.GetFileNameWithoutExtension(inputFilePath)}_Pages_{startPage}_to_{endPage}.pdf";
                    outputFilePath = Path.Combine(outputDirectory, outputFileName);

                    // Create the new PDF document
                    using (PdfDocument newDocument = new PdfDocument())
                    {
                        newDocument.ImportPageRange(loadedDocument, startPage - 1, endPage - 1); // Use 0-based index

                        // Save the new document directly to the defined file path
                        using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                        {
                            newDocument.Save(outputStream);
                        }
                    }

                    loadedDocument.Close(true);
                }
            }
            catch (Exception ex)
            {
                // Clean up if the file was partially created during an error
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                throw new Exception($"Error extracting page range: {ex.Message}", ex);
            }

            return outputFilePath;
        }



        public async  Task<string> AddWaterMark(string inputFilePath , string waterMarkText)
        {
            try
            {
                string outputpath = Path.Combine(Path.GetDirectoryName(inputFilePath), "watermark_"+Path.GetFileName(inputFilePath));

                using (var fileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileStream))
                {
                    // Iterate through all pages in the document
                    foreach (PdfPageBase loadedPage in loadedDocument.Pages)
                    {
                        // Create PDF graphics for the current page.
                        PdfGraphics graphics = loadedPage.Graphics;

                        // Set the standard font.
                        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 30);

                        // Add watermark text.
                        // Save the current graphics state before applying transformations specific to the watermark.
                        PdfGraphicsState state = graphics.Save();
                        graphics.SetTransparency(0.25f);
                        graphics.RotateTransform(-40); // Apply a rotation for the diagonal effect

                        // Calculate position dynamically to cover the page center.
                        // The Y coordinate is the vertical center of the page.
                        float x = -150;
                        float y = loadedPage.Size.Height / 2;

                        graphics.DrawString(waterMarkText, font, PdfPens.Red, PdfBrushes.Red, new PointF(x, y));

                        // Restore the graphics state to its original settings (removes transparency/rotation for the next page).
                        graphics.Restore(state);
                    }

                    // Save the modified document.
                    using (FileStream stream = new FileStream(outputpath, FileMode.Create, FileAccess.Write))
                    {
                        loadedDocument.Save(stream);
                    }
                }


                return outputpath;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
