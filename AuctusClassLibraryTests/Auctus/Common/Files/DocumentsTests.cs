using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Auctus.DataMiner.Library.Common.Files.Tests
{
    [TestClass]
    public class DocumentsTests
    {
        private readonly string testDirectory = $@"{Directory.GetCurrentDirectory()}\TestFiles";
        private readonly List<string> documents = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            foreach (var filePath in Directory.EnumerateFiles(testDirectory, "*", SearchOption.AllDirectories).OrderBy(File.GetLastWriteTime))
            {
                var fileInfo = new FileInfo(filePath);
                var subDirectory = fileInfo.DirectoryName.Replace(testDirectory, string.Empty).Replace(fileInfo.Name, string.Empty).Trim('\\');

                documents.Add($@"{subDirectory}\{fileInfo.Name}".TrimStart('\\'));
            }
        }

        [TestMethod]
        public void GetDocumentPaths_Test()
        {
            Documents.GetDocumentPaths($@"{testDirectory}\").Should().BeEquivalentTo(documents);

            var xmlDocuments = documents.Where(x => x.EndsWith(".xml")).ToList();

            Documents.GetDocumentPaths($@"{testDirectory}\", new[] { ".xml" }).Should().HaveCount(xmlDocuments.Count);
        }

        [TestMethod]
        [DataRow("DemoProtocol", "DemoProtocol")]
        [DataRow(" DemoProtocol.", "DemoProtocol")]
        [DataRow(" Demo\\\\Protocol.", "Demo_Protocol")]
        [DataRow(" Demo?|\\Protocol.", "Demo___Protocol")]
        public void GetProtocolFolderName_Test(string protocolName, string expected)
        {
            Documents.GetProtocolFolderName(protocolName).Should().Be(expected);
        }

        [TestMethod]
        [DataRow("Unsafe*File\"Name", '-', "Unsafe-File-Name")]
        [DataRow("Unsafe:File?Name", '_', "Unsafe_File_Name")]
        [DataRow("Unsafe\\File/Name", ' ', "Unsafe File Name")]
        [DataRow("Unsafe*File\"Name", '*', "Unsafe-File-Name")]
        public void GetSafeFilename_Test(string fileName, char replacementChar, string expected)
        {
            Documents.GetSafeFilename(fileName, replacementChar).Should().Be(expected);
        }
    }
}