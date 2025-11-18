using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.IPersistance
{
    public interface IFileConversion
    {

        // ====== 📄 PDF CONVERSIONS ======
        Task<string> ConvertPdfToWord(string inputFilePath );
        Task<string> ConvertPdfToImage(string inputFilePath );
        Task<string> ConvertPdfToExcel(string inputFilePath );
        Task<string> ConvertPdfToPpt(string inputFilePath );
        Task<string> ConvertPdfToText(string inputFilePath );

        // ====== 🧾 WORD CONVERSIONS ======
        Task<string> ConvertWordToPdf(string inputFilePath );
        Task<string> ConvertWordToText(string inputFilePath );
        Task<string> ConvertWordToHtml(string inputFilePath );

        // ====== 📊 EXCEL CONVERSIONS ======
        Task<string> ConvertExcelToPdf(string inputFilePath );
        Task<string> ConvertExcelToCsv(string inputFilePath );
        Task<string> ConvertExcelToXml(string inputFilePath );

        // ====== 📈 POWERPOINT CONVERSIONS ======
        Task<string> ConvertPptToPdf(string inputFilePath );
        Task<string> ConvertPptToImage(string inputFilePath );

        //// ====== 🧠 REVERSE CONVERSIONS ======
        //string ConvertWordToPdfBack(string inputFilePath ); // PDF → Word
        //string ConvertExcelToPdfBack(string inputFilePath ); // PDF → Excel
        //string ConvertPptToPdfBack(string inputFilePath ); // PDF → PPT

        // ====== 🖼️ IMAGE CONVERSIONS ======
        Task<string> ConvertImageToPdf(string inputFilePath );
        Task<string> ConvertImageFormat(string inputFilePath , string format); // jpg, png, bmp, etc.

        // ====== 🧰 TEXT / CSV / HTML CONVERSIONS ======
        Task<string> ConvertTextToPdf(string inputFilePath );
        Task<string> ConvertCsvToExcel(string inputFilePath );
        Task<string> ConvertHtmlToPdf(string inputFilePath );
    }
}
