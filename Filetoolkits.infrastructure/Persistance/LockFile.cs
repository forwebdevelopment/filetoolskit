using Filetoolkits.application.IPersistance;
using iText.Kernel.Pdf;

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Presentation;
using Syncfusion.XlsIO;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;

using System.Threading.Tasks;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Filetoolkits.infrastructure.Persistance
{
    public class LockFile: ILockFiles
    {
        public Task<string> LockArchiveFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> LockDatabaseFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> LockDesignFile(string filePath, string password = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> LockOfficeFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<string> LockWordFile(string filePath, string password)
        {
            try
            {

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File not found.", filePath);

                string outputPath = Path.Combine(
                    Path.GetDirectoryName(filePath),
                    "locked_" + Path.GetFileName(filePath)
                );

                // Open input file as stream
                using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Load the Word document from the stream
                    using (WordDocument document = new WordDocument(inputStream, Syncfusion.DocIO.FormatType.Docx))
                    {
                        // Apply encryption (lock)
                        document.EncryptDocument(password);

                        // Save to a new locked file
                        using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            document.Save(outputStream, Syncfusion.DocIO.FormatType.Docx);
                        }

                        document.Close();
                    }
                }

                return outputPath;

              
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }

        }

        public async Task<string> LockExcelFile(string filePath, string password)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;

                    // Open file as stream
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        Syncfusion.XlsIO.IWorkbook workbook = application.Workbooks.Open(stream);

                        // Set password to open
                        workbook.PasswordToOpen = password;

                        // Define output file path
                        string outputPath = Path.Combine(
                            Path.GetDirectoryName(filePath),
                            "protected_" + Path.GetFileName(filePath)
                        );

                        // Save the protected file
                        using (FileStream outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            workbook.SaveAs(outStream);
                        }

                        workbook.Close();
                        return outputPath;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        public async Task<string> LockPresentationFile(string filePath, string password)
        {
            try
            {

                // Load the PowerPoint presentation
                IPresentation presentation = Syncfusion.Presentation.Presentation.Open(filePath);

                // Apply encryption with password
                presentation.Encrypt(password);

                // Define the output file path
                string outputPath = Path.Combine(
                    Path.GetDirectoryName(filePath),
                    "protected_" + Path.GetFileName(filePath)
                );

                // Save the encrypted PowerPoint file
                presentation.Save(outputPath);
                presentation.Close();

                return outputPath;
             

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> LockPdfFile(string filePath, string password)
        {
            try
            {

                string outputFile = Path.Combine(Path.GetDirectoryName(filePath), "protected_" + Path.GetFileName(filePath));

                WriterProperties writerProperties = new WriterProperties()
           .SetStandardEncryption(
               Encoding.UTF8.GetBytes(password),
               Encoding.UTF8.GetBytes(password),
               EncryptionConstants.ALLOW_PRINTING,   // allowed operations for user
               EncryptionConstants.ENCRYPTION_AES_128
           );
                using (PdfReader reader = new PdfReader(filePath))
                using (PdfWriter writer = new PdfWriter(outputFile, writerProperties))
                using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                    return outputFile;
            }
            catch (Exception ex)
            {

                throw new KeyNotFoundException(ex.Message);
            }


        }

        public Task<string> LockProjectFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> LockSourceFile(string filePath, string user)
        {
            throw new NotImplementedException();
        }
    }
}
