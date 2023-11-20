using Auctus.DataMiner.Library.Auctus.Common.Models;
using Skyline.DataMiner.Net.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Auctus.DataMiner.Library.Auctus.Common.Shared
{
    internal static class SLNetExtensibility
    {
        internal static List<DmaDocument> ConvertToDmaDocument(GetDocumentsResponseMessage response)
        {
            var documents = new List<DmaDocument>();

            for (var i = 0; i < response.Sa.Sa.Length; i += 5)
            {
                var segment = new ArraySegment<string>(response.Sa.Sa, i, 5);
                var isHyperLink = segment.Array[segment.Offset + 4].StartsWith("TRUE;");
                var hyperlink = isHyperLink ? segment.Array[segment.Offset + 4].Replace("TRUE;", string.Empty) : string.Empty;

                var document = new DmaDocument(
                    name: segment.Array[segment.Offset + 0],
                    description: segment.Array[segment.Offset + 1],
                    comments: segment.Array[segment.Offset + 2],
                    date: DateTime.Parse(segment.Array[segment.Offset + 3], DateTimeFormatInfo.CurrentInfo),
                    isHyperlink: isHyperLink,
                    hyperlink: hyperlink
                );

                documents.Add(document);
            }

            return documents;
        }
    }
}