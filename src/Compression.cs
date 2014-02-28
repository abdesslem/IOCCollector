using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace main
{
    class Compression
    {
        public static void CreateZipFile(List<string> items, string destination)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                // Loop through all the items
                foreach (string item in items)
                {
                    // If the item is a file
                    if (System.IO.File.Exists(item))
                    {
                        // Add the file in the root folder inside our zip file
                        zip.AddFile(item, "");
                    }
                    // if the item is a folder    
                    else if (System.IO.Directory.Exists(item))
                    {
                        // Add the folder in our zip file with the folder name as its name
                        zip.AddDirectory(item, new System.IO.DirectoryInfo(item).Name);
                    }
                }
                // Finally save the zip file to the destination we want
                zip.Save(destination);
            }
        }

        static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
        {
            //Compress file name
            char[] chars = sRelativePath.ToCharArray();
            zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
            foreach (char c in chars)
                zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

            //Compress file content
            byte[] bytes = File.ReadAllBytes(sRelativePath);
            zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
            zipStream.Write(bytes, 0, bytes.Length);
        }
        /*****************************         compresse directory     **************************************************/
        static void CompressDirectory(string sInDir, string sOutFile)
        {
            string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
                foreach (string sFilePath in sFiles)
                {
                    CompressFile(sInDir, sFilePath, str);
                }
        }

    }
}
