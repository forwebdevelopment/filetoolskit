using Filetoolkits.application.IPersistance;
using SkiaSharp;
using Syncfusion.DocIO.DLS;
using Syncfusion.EJ2.PdfViewer;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.PdfToImageConverter;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Filetoolkits.infrastructure.Persistance
{
    public class FileConversion : IFileConversion
    {
        public Task<string> ConvertCsvToExcel(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertExcelToCsv(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertExcelToPdf(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertExcelToXml(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertHtmlToPdf(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertImageFormat(string inputFilePath, string format)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertImageToPdf(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertPdfToExcel(string inputFilePath)
        {
            throw new NotImplementedException();
        }


        //working
        public async Task<string> ConvertPdfToImage(string inputFilePath)
        {


            try
            {
                string outputDir = Path.GetDirectoryName(inputFilePath);
                string outputFile = Path.Combine(outputDir,
                    Path.GetFileNameWithoutExtension(inputFilePath) + "_page1.png");

                // Create converter
                using (PdfToImageConverter converter = new PdfToImageConverter())
                {
                    // Load PDF file
                    using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                    {
                        converter.Load(inputStream);
                    }

                    // Convert first page of PDF to image stream
                    using (var imageStream = converter.Convert(0, false, false))
                    {
                        // Save the image stream to a file
                        using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                        {
                            imageStream.CopyTo(fs);
                        }
                    }
                }

                return outputFile;


            }
            catch (Exception) {
                throw new NotImplementedException();

            }
            
        }

        public Task<string> ConvertPdfToPpt(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertPdfToText(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ConvertPdfToWord(string path)
        {
            try
            {
                string fileName = Path.GetFileName(path).Split(".")[0];
                string wordPath = Path.Combine(Path.GetDirectoryName(path), fileName + ".docx");
                //Create a new Word document.
                WordDocument m_wordDocument = new WordDocument();

                //Add a new section to the document.
                IWSection section = m_wordDocument.AddSection();

                //Set the page margins to zero.
                section.PageSetup.Margins.All = 0;

                //Add a new paragraph to the section.
                IWParagraph firstParagraph = section.AddParagraph();

                SizeF defaultPageSize = new SizeF(m_wordDocument.LastSection.PageSetup.PageSize.Width, m_wordDocument.LastSection.PageSetup.PageSize.Height);

                //string path = "Barcode.pdf";
                Stream docStream = new FileStream(path, FileMode.Open);

                //Load the PDF document from the given file path.
                using (PdfLoadedDocument m_loadedDocument = new PdfLoadedDocument(docStream))
                {
                    //Use the Syncfusion.EJ2.PdfViewer assembly.
                    PdfRenderer pdfExportImage = new PdfRenderer();

                    //Load the PDF document.
                    pdfExportImage.Load(docStream);

                    for (int i = 0; i < m_loadedDocument.Pages.Count; i++)
                    {
                        //Export the PDF document pages into images.
                        SKBitmap bitmap = pdfExportImage.ExportAsImage(i);

                        SKImage image = SKImage.FromBitmap(bitmap);
                        SKData encodedData = image.Encode(SKEncodedImageFormat.Jpeg, 100);

                        MemoryStream imageStream = new MemoryStream();
                        encodedData.SaveTo(imageStream);

                        //Add an image to the paragraph.
                        IWPicture picture = firstParagraph.AppendPicture(imageStream);

                        //Set width and height for the image.
                        picture.Width = defaultPageSize.Width;
                        picture.Height = defaultPageSize.Height;

                        imageStream.Dispose();
                    }
                    //Save the PDF to the MemoryStream.
                    MemoryStream stream = new MemoryStream();
                    m_wordDocument.Save(stream, Syncfusion.DocIO.FormatType.Docx);

                    using (FileStream outputStream = new FileStream(wordPath, FileMode.Create))
                    {
                        m_wordDocument.Save(outputStream, Syncfusion.DocIO.FormatType.Docx);
                    }

                    ////Set the position as '0'.
                    //stream.Position = 0;

                    ////Download the PDF document in the browser.
                    //FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/msword");
                    //fileStreamResult.FileDownloadName = "Result.docx";
                  
                    return wordPath;





                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }

        public Task<string> ConvertPptToImage(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertPptToPdf(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertTextToPdf(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertWordToHtml(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertWordToPdf(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConvertWordToText(string inputFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
