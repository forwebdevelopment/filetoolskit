using Filetoolkits.application.IPersistance;
using iText.Kernel.Pdf;

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Presentation;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.infrastructure.Persistance
{
    public class UnlockFile: IUnlockFiles
    {
        public Task<string> UnLockArchiveFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> UnLockDatabaseFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> UnLockDesignFile(string filePath, string password = null)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UnLockExcelFile(string filePath, string password)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;

                    // Open the encrypted workbook with password
                    IWorkbook workbook;
                    using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // ✅ Correct overload: Stream + Password
                        workbook = application.Workbooks.Open(inputStream, password);
                    }

                    // ✅ Remove the password (unencrypt the file)
                    workbook.PasswordToOpen = null;

                    // Define output path
                    string outputPath = Path.Combine(
                        Path.GetDirectoryName(filePath),
                        "unlocked_" + Path.GetFileName(filePath)
                    );

                    // ✅ Save unlocked copy
                    using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.SaveAs(outputStream);
                    }

                    workbook.Close();
                    return outputPath;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Task<string> UnLockOfficeFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UnLockPdfFile(string filePath, string password)
        {
            try
            {
                // Output file path (unlocked version)
                string outputFile = Path.Combine(Path.GetDirectoryName(filePath), "unlocked_" + Path.GetFileName(filePath));

                // Reader with password
                ReaderProperties readerProperties = new ReaderProperties()
                    .SetPassword(Encoding.UTF8.GetBytes(password));

                using (PdfReader reader = new PdfReader(filePath, readerProperties))
                using (PdfWriter writer = new PdfWriter(outputFile)) // no encryption
                using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                {
                    // just open and save without encryption
                }

                return outputFile;
            }
            catch (Exception ex)
            {

                throw new KeyNotFoundException(ex.Message);
            }
        }

        public async Task<string> UnLockPresentationFile(string filePath, string password)
        {
            try
            {
                // Load the encrypted PowerPoint presentation
                IPresentation presentation = Syncfusion.Presentation.Presentation.Open(filePath, password);

                // Remove encryption (Syncfusion does not keep encryption metadata once opened)
                // Just save the file again without encryption.

                string outputPath = Path.Combine(
                    Path.GetDirectoryName(filePath),
                    "unlocked_" + Path.GetFileName(filePath)
                );

                presentation.RemoveEncryption();
               
                // Save the unlocked file
                presentation.Save(outputPath);
                presentation.Close();

                return outputPath;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Task<string> UnLockProjectFile(string filePath, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> UnLockSourceFile(string filePath, string user)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UnLockWordFile(string filePath, string password)
        {
            try
            {

                string outputPath = Path.Combine(
                   Path.GetDirectoryName(filePath), "unlocked_" + Path.GetFileName(filePath));
                // Open input file as stream
                using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read ))
                {
                    // Load the Word document from the stream
                    using (WordDocument documents = new WordDocument(inputStream, Syncfusion.DocIO.FormatType.Docx , password))
                    {
                        // Apply encryption (lock)
                        //documents.en;

                        // Save to a new locked file
                        using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            documents.Save(outputStream, Syncfusion.DocIO.FormatType.Docx);
                        }

                        documents.Close();
                    }
                }

             
                return outputPath;

            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
        }
    }
}
