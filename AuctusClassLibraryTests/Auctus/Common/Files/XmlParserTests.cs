using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static FluentAssertions.FluentActions;

namespace Auctus.DataMiner.Library.Common.Files.Tests
{
    [TestClass]
    public class XmlParserTests
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
        public void ParseXml_Test()
        {
            var targetXmlDocument = documents.First(x => x.EndsWith("DemoProtocol.xml"));
            var targetSvgDocument = documents.First(x => x.EndsWith("Testing_Duo.svg"));

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Invoking(() => XmlParser.ParseXml($@"{testDirectory}\{targetSvgDocument}")).Should().Throw<ArgumentException>();

            Console.SetOut(Console.Out);

            var document = XmlParser.ParseXml($@"{testDirectory}", targetXmlDocument);

            document.Should().NotBeNull();
            document.Declaration.Encoding.Should().Be("UTF-8");

            var xmlDocument = XmlParser.ParseXml($@"{testDirectory}\{targetXmlDocument}");

            xmlDocument.Should().NotBeNull();
            xmlDocument.Descendants().First().Name.LocalName.Should().Be("Protocol");
        }
    }
}