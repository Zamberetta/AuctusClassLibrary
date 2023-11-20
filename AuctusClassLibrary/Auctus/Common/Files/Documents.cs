#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S4790 // Using weak hashing algorithms is security-sensitive

namespace Auctus.DataMiner.Library.Common.Files
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Methods that aid with dealing with documents.
    /// </summary>
    public static class Documents
    {
        /// <summary>Retrieves document paths for all files within the target directory.</summary>
        /// <param name="directory">The directory to retrieve document paths from.</param>
        /// <param name="extensions">Specify file extensions that should be retrieved.</param>
        public static List<string> GetDocumentPaths(string directory, string[] extensions = null)
        {
            var files = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).OrderBy(File.GetLastWriteTime).ToList();
            var documents = new List<string>();

            foreach (var file in files)
            {
                if (extensions != null && !Array.Exists(extensions, x => file.EndsWith(x)))
                    continue;

                documents.Add(file.Replace(directory, string.Empty));
            }

            return documents;
        }

        /// <summary>Gets the safe folder name for the target protocol.</summary>
        /// <param name="protocolName">Name of the protocol.</param>
        public static string GetProtocolFolderName(string protocolName)
        {
            protocolName = protocolName.Trim().Trim('.');
            return Regex.Replace(protocolName, @"[\/:*?""<>|°;]|[\\]{2}|[\\]{1}", "_", RegexOptions.None, TimeSpan.FromMilliseconds(500));
        }

        /// <summary>Gets the safe file name, replacing any invalid characters.</summary>
        /// <param name="filename">The filename.</param>
        /// <param name="replacementChar">The replacement character.</param>
        public static string GetSafeFilename(string filename, char replacementChar = '-')
        {
            if (Path.GetInvalidFileNameChars().Contains(replacementChar))
            {
                replacementChar = '-';
            }

            return string.Join(replacementChar.ToString(), filename.Split(Path.GetInvalidFileNameChars()));
        }

        /// <summary>Returns an MD5 fingerprint for the target file.</summary>
        /// <param name="path">The target file path.</param>
        public static string GetMD5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
                }
            }
        }
    }
}