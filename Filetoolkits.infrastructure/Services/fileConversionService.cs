using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.domain.Entity;
using Filetoolkits.domain.Response;
using Filetoolkits.infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.infrastructure.Services
{
    public class fileConversionService : IfileConversionService
    {
        public  IFileConversion  _fileConversion;
        public fileConversionService(IFileConversion fileConversion)
        {
            _fileConversion = fileConversion;

        }
        public async Task<FileResponse> FileConversion(FileConversionParameter parameter)
        {
         
            string tempFolder = Path.GetTempPath();
            string tempFile = Path.Combine(tempFolder, parameter.File.FileName);

            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                await parameter.File.CopyToAsync(stream);
            }

            string response = await FileConversion(tempFile, parameter.conversionType);
            return new FileResponse
            {
                LockFile = response,
                TempFile = tempFile,
            };
        
        }


        private async Task<string> FileConversion(string tempFile, string conversionType)
        {
            string response = string.Empty;

            switch (conversionType.ToLower())
            {
                // ====== 📄 PDF CONVERSIONS ======
                case "pdftoword":
                    response = await _fileConversion.ConvertPdfToWord(tempFile);
                    break;
                case "pdftoimage":
                    response = await _fileConversion.ConvertPdfToImage(tempFile);
                    break;
                case "pdftoexcel":
                    response = await _fileConversion.ConvertPdfToExcel(tempFile);
                    break;
                case "pdftoppt":
                    response = await _fileConversion.ConvertPdfToPpt(tempFile);
                    break;
                case "pdftotext":
                    response = await _fileConversion.ConvertPdfToText(tempFile);
                    break;

                // ====== 🧾 WORD CONVERSIONS ======
                case "wordtopdf":
                    response = await _fileConversion.ConvertWordToPdf(tempFile);
                    break;
                case "wordtotext":
                    response = await _fileConversion.ConvertWordToText(tempFile);
                    break;
                case "wordtohtml":
                    response = await _fileConversion.ConvertWordToHtml(tempFile);
                    break;

                // ====== 📊 EXCEL CONVERSIONS ======
                case "exceltopdf":
                    response = await _fileConversion.ConvertExcelToPdf(tempFile);
                    break;
                case "exceltocsv":
                    response = await _fileConversion.ConvertExcelToCsv(tempFile);
                    break;
                case "exceltoxml":
                    response = await _fileConversion.ConvertExcelToXml(tempFile);
                    break;

                // ====== 📈 POWERPOINT CONVERSIONS ======
                case "ppttopdf":
                    response = await _fileConversion.ConvertPptToPdf(tempFile);
                    break;
                case "ppttoimage":
                    response = await _fileConversion.ConvertPptToImage(tempFile);
                    break;

                // ====== 🖼️ IMAGE CONVERSIONS ======
                case "imagetopdf":
                    response = await _fileConversion.ConvertImageToPdf(tempFile);
                    break;
                case "imageformat":
                    // Example: "imageformat:png" → extract format part after colon
                    var format = conversionType.Contains(":") ? conversionType.Split(':')[1] : "jpg";
                    response = await _fileConversion.ConvertImageFormat(tempFile, format);
                    break;

                // ====== 🧰 TEXT / CSV / HTML CONVERSIONS ======
                case "texttopdf":
                    response = await _fileConversion.ConvertTextToPdf(tempFile);
                    break;
                case "csvtoexcel":
                    response = await _fileConversion.ConvertCsvToExcel(tempFile);
                    break;
                case "htmltopdf":
                    response = await _fileConversion.ConvertHtmlToPdf(tempFile);
                    break;

                // ====== DEFAULT ======
                default:
                    throw new ArgumentException($"Unsupported conversion type: {conversionType}");
            }

            return response;
        }
    }
}
