using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.domain.Response
{
    public class FileResponse
    {
        public string TempFile { get; set; }
        public string? LockFile { get; set; }

        public string TempFolder { get; set; }
    }
}
