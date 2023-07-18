namespace Auctus.DataMiner.Library.Common.Files
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    public static class XmlParser
    {
        /// <summary>Parses the target XML file to an XDocument.</summary>
        /// <param name="directory">The directory containing the XML file.</param>
        /// <param name="targetFile">The target XML file including it's extension.</param>
        public static XDocument ParseXml(string directory, string targetFile)
        {
            var path = $"{directory.TrimEnd('\\')}\\{targetFile.TrimStart('\\')}";
            return ParseXml(path);
        }

        /// <summary>Parses the target XML file to an XDocument.</summary>
        /// <param name="path">The full path to the XML file including it's extension.</param>
        public static XDocument ParseXml(string path)
        {
            var extention = Path.GetExtension(path);

            if (extention != ".xml")
            {
                throw new ArgumentException("File not found or format is not supported.");
            }

            var doc = XDocument.Load(path);
            var declaration = doc.Declaration;

            if (declaration != null)
            {
                declaration.Encoding = "UTF-8";
            }

            return doc;
        }
    }
}