using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.IPersistance
{
    public interface ILockFiles
    {
        // 🔹 Office Files (Word, Excel, PowerPoint)
        Task<string> LockOfficeFile(string filePath, string password);
        Task<string> LockPresentationFile(string filePath, string password);

        Task<string> LockWordFile(string filePath, string password);

        Task<string> LockExcelFile(string filePath, string password);
        // 🔹 PDF Files
        Task<string> LockPdfFile(string filePath, string password);

        // 🔹 Archives (ZIP, RAR, 7z)
        Task<string> LockArchiveFile(string filePath, string password);

        // 🔹 Database Files (MS Access, etc.)
        Task<string> LockDatabaseFile(string filePath, string password);

        // 🔹 Design Files (Photoshop, Illustrator, AutoCAD)
        Task<string> LockDesignFile(string filePath, string password = null);

        // 🔹 Project Management Files (MS Project, etc.)
        Task<string> LockProjectFile(string filePath, string password);

        // 🔹 Source Control Files (Git/SVN style locks)
        Task<string> LockSourceFile(string filePath, string user);
    }
}
