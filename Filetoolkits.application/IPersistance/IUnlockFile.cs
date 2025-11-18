using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.IPersistance
{
    public interface IUnlockFiles
    {
        Task<string> UnLockOfficeFile(string filePath, string password);
        Task<string> UnLockPresentationFile(string filePath, string password);

        Task<string> UnLockWordFile(string filePath, string password);

        Task<string> UnLockExcelFile(string filePath, string password);
        // 🔹 PDF Files
        Task<string> UnLockPdfFile(string filePath, string password);

        // 🔹 Archives (ZIP, RAR, 7z)
        Task<string> UnLockArchiveFile(string filePath, string password);

        // 🔹 Database Files (MS Access, etc.)
        Task<string> UnLockDatabaseFile(string filePath, string password);

        // 🔹 Design Files (Photoshop, Illustrator, AutoCAD)
        Task<string> UnLockDesignFile(string filePath, string password = null);

        // 🔹 Project Management Files (MS Project, etc.)
        Task<string> UnLockProjectFile(string filePath, string password);

        // 🔹 Source Control Files (Git/SVN style locks)
        Task<string> UnLockSourceFile(string filePath, string user);
    }
}
