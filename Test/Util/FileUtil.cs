using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace P4Cmdlets.Test.Util
{
    public class FileUtil
    {
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static DirectoryInfo CreateTempDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            DirectoryInfo dir = Directory.CreateDirectory(path);
            if (dir == null || !dir.Exists) 
                throw new Exception("Unable to create directory at " + path);
            return dir;
        }
    }
}
