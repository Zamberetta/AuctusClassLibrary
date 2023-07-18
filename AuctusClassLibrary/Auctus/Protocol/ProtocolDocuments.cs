using Auctus.DataMiner.Library.Common.Files;
using Skyline.DataMiner.Scripting;

namespace Auctus.DataMiner.Library.Protocol
{
    public static class ProtocolDocuments
    {
        /// <summary>Returns the DataMiner documents path for the specified protocol.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="subDirectory">The sub-directory to append to the path.</param>
        /// <param name="protocolName">The name of the target protocol.</param>
        public static string GetDirectory(SLProtocol protocol, string subDirectory = "", string protocolName = null)
        {
            protocolName = Documents.GetProtocolFolderName(protocolName ?? protocol.ProtocolName);

            subDirectory = subDirectory.Trim('\\');

            var subDirectoryDefined = !string.IsNullOrWhiteSpace(subDirectory);

            return $@"C:\Skyline DataMiner\Documents\{protocolName}\{subDirectory}{(subDirectoryDefined ? @"\" : string.Empty)}";
        }
    }
}