using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class FileSystemUtils
    {
        public static readonly string DOWNLOADS_FOLDER_PATH = Path.Combine(Environment.CurrentDirectory, "Downloads");
    }
}
