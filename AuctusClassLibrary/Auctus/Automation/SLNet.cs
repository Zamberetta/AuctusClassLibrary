﻿using Auctus.DataMiner.Library.Auctus.Common.Models;
using Auctus.DataMiner.Library.Auctus.Common.Shared;
using Auctus.DataMiner.Library.Common;
using Auctus.DataMiner.Library.Common.SLNetType;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Net.Messages.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.DataMiner.Library.Automation
{
    /// <summary>
    /// SLNet methods aiding with automation script development.
    /// </summary>
    public static class SLNet
    {
        /// <summary>Triggers a file sync on the DMS for the target file, note that the target file must be on the same DMA to which this call is being triggered from.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="filePath">The target file to sync (local path on the DMA).</param>
        /// <returns>
        /// <see cref="SetDataMinerInfoResponseMessage"/>
        /// </returns>
        /// <exception cref="ArgumentException">File Path cannot be null, empty or white space.</exception>
        public static SetDataMinerInfoResponseMessage SendDmsFileChange(IEngine engine, string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("File Path cannot be null, empty or white space.");
                }

                var serverDetails = engine.GetUserConnection().ServerDetails;

                var message = new SetDataMinerInfoMessage
                {
                    DataMinerID = serverDetails.AgentID,
                    HostingDataMinerID = serverDetails.AgentID,
                    IInfo2 = 32,
                    StrInfo1 = filePath,
                    What = (int)NotifyType.SendDmsFileChange,
                };

                return engine.SendSLNetSingleResponseMessage(message) as SetDataMinerInfoResponseMessage;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return null;
            }
        }

        /// <summary>Sets the communication state for an element or DVE.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="dataMinerId">The target DataMiner agent ID.</param>
        /// <param name="elementId">The target Element ID.</param>
        /// <param name="isResponding">Whether the element should be set to responding [<c>true</c>] or timeout [<c>false</c>].</param>
        /// <param name="dve">Whether the target element is a DVE.</param>
        /// <param name="connectionID">The target connection to set for elements supporting multiple connections [0 for the main connection].</param>
        /// <returns>
        /// <see cref="SetDataMinerInfoResponseMessage"/>
        /// </returns>
        public static SetDataMinerInfoResponseMessage ChangeCommunicationState(IEngine engine, int dataMinerId, int elementId, bool isResponding, bool dve = false, int connectionID = 0)
        {
            try
            {
                if (dve)
                {
                    connectionID = -1;
                }

                var message = new SetDataMinerInfoMessage
                {
                    bInfo1 = 0,
                    bInfo2 = 0,
                    DataMinerID = -1,
                    ElementID = -1,
                    HostingDataMinerID = -1,
                    IInfo1 = connectionID,
                    IInfo2 = 0,
                    Uia1 = new UIA(new[]
                    {
                        dataMinerId,
                        elementId,
                        isResponding ? 1 : 0,
                    }),
                    Uia2 = null,
                    What = (int)NotifyType.NT_CHANGE_COMMUNICATION_STATE,
                };

                return engine.SendSLNetSingleResponseMessage(message) as SetDataMinerInfoResponseMessage;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return null;
            }
        }

        /// <summary>Generic method, supporting multiple GetInfo calls that return a single response.</summary>
        /// <typeparam name="T">The expected ResponseMessage that is returned by the target call.</typeparam>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="type">The target GetInfo call.</param>
        /// <returns>
        ///   The specified ResponseMessage type.
        /// </returns>
        public static T GetInfo<T>(IEngine engine, InfoTypeSingle type) where T : DMSMessage
        {
            try
            {
                var message = new GetInfoMessage
                {
                    DataMinerID = -1,
                    HostingDataMinerID = -1,
                    Type = (InfoType)type
                };

                return engine.SendSLNetSingleResponseMessage(message) as T;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return null;
            }
        }

        /// <summary>Generic method, supporting multiple GetInfo calls that return an array of responses.</summary>
        /// <typeparam name="T">The expected ResponseMessage that is returned by the target call.</typeparam>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="type">The target GetInfo call.</param>
        /// <returns>
        ///   The specified ResponseMessage type.
        /// </returns>
        public static IEnumerable<T> GetInfo<T>(IEngine engine, InfoTypeArray type) where T : DMSMessage
        {
            try
            {
                var message = new GetInfoMessage
                {
                    DataMinerID = -1,
                    HostingDataMinerID = -1,
                    Type = (InfoType)type
                };

                var response = engine.SendSLNetMessage(message);

                return response.CastDMSMessage<T>();
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return new List<T>();
            }
        }

        /// <summary>Retrieves all available alarm templates for a given protocol and version.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="protocolName">Name of the target protocol.</param>
        /// <param name="protocolVersion">The target protocol version.</param>
        /// <param name="includeTemplates">If <c>true</c> templates are included in the response.</param>
        /// <param name="includeGroups">If <c>true</c> template groups are included in the response.</param>
        /// <returns>
        /// <see cref="GetAvailableAlarmTemplatesResponse"/>
        /// </returns>
        /// <exception cref="System.ArgumentException">Protocol Name cannot be null, empty or white space.
        /// or
        /// Protocol Version cannot be null, empty or white space.</exception>
        public static GetAvailableAlarmTemplatesResponse GetAvailableAlarmTemplates(IEngine engine, string protocolName, string protocolVersion, bool includeTemplates = true, bool includeGroups = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(protocolName))
                {
                    throw new ArgumentException("Protocol Name cannot be null, empty or white space.");
                }

                if (string.IsNullOrWhiteSpace(protocolVersion))
                {
                    throw new ArgumentException("Protocol Version cannot be null, empty or white space.");
                }

                var message = new GetAvailableAlarmTemplatesMessage
                {
                    IncludeGroups = includeGroups,
                    IncludeTemplates = includeTemplates,
                    ProtocolName = protocolName,
                    ProtocolVersion = protocolVersion,
                };

                return engine.SendSLNetSingleResponseMessage(message) as GetAvailableAlarmTemplatesResponse;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return null;
            }
        }

        /// <summary>Updates, adds, renames or deletes a given alarm template.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="definition">The alarm template definition.</param>
        /// <param name="originalName">The original alarm template name.</param>
        /// <param name="updateType">The update type to be made.</param>
        public static void UpdateAlarmTemplate(IEngine engine, AlarmTemplateEventMessage definition, string originalName, UpdateAlarmTemplateType updateType)
        {
            if (string.IsNullOrWhiteSpace(originalName))
            {
                throw new ArgumentException("Original Name cannot be null, empty or white space.");
            }

            var message = new UpdateAlarmTemplateMessage
            {
                Definition = definition,
                OriginalName = originalName,
                UpdateType = updateType,
            };

            engine.SendSLNetMessage(message);
        }

        /// <summary>Deletes the target document from the system.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="element">The target element/protocol name the document resides in.</param>
        /// <param name="document">The document name and extension.</param>
        /// <exception cref="System.ArgumentException">
        /// Element cannot be null, empty or white space.
        /// or
        /// Document cannot be null, empty or white space.
        /// </exception>
        public static void DeleteDocument(IEngine engine, string element, string document)
        {
            if (string.IsNullOrWhiteSpace(element))
            {
                throw new ArgumentException("Element cannot be null, empty or white space.");
            }
            if (string.IsNullOrWhiteSpace(document))
            {
                throw new ArgumentException("Document cannot be null, empty or white space.");
            }

            var message = new DeleteDocumentMessage
            {
                Document = document,
                Element = element,
            };

            engine.SendSLNetMessage(message);
        }

        /// <summary>Retrieves all documents within the DMS for a given folder.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="folder">The target documents folder.</param>
        /// <param name="recursive">If <c>true</c> all documents in sub-folders are also retrieved.</param>
        /// <returns>
        ///    List&lt;<see cref="DmaDocument"/>&gt;
        /// </returns>
        /// <exception cref="System.ArgumentException">Folder cannot be null, empty or white space.
        /// or
        /// Failed to get a valid documents response.</exception>
        public static List<DmaDocument> GetAvailableDocuments(IEngine engine, string folder, bool recursive = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folder))
                {
                    throw new ArgumentException("Folder cannot be null, empty or white space.");
                }

                var message = new GetDocumentsMessage
                {
                    Folder = folder,
                    Recursive = recursive,
                };

                var response = engine.SendSLNetSingleResponseMessage(message) as GetDocumentsResponseMessage;

                if (response == null)
                {
                    throw new ArgumentException("Failed to get a valid documents response.");
                }

                return SLNetExtensibility.ConvertToDmaDocument(response);
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return new List<DmaDocument>();
            }
        }

        /// <summary>Retrieves a file in a DataMiner Documents folder.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="documentPath">The DataMiner document path that file resides.</param>
        /// <param name="documentName">The name of the document to retrieve.</param>
        /// <returns>
        ///   <see cref="byte"/>[]
        /// </returns>
        /// <exception cref="System.ArgumentException">Document Path cannot be null, empty or white space.
        /// or
        /// Document Name cannot be null, empty or white space.</exception>
        public static byte[] GetDocument(IEngine engine, string documentPath, string documentName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(documentPath))
                {
                    throw new ArgumentException("Document Path cannot be null, empty or white space.");
                }

                if (string.IsNullOrWhiteSpace(documentName))
                {
                    throw new ArgumentException("Document Name cannot be null, empty or white space.");
                }

                var getBinaryFileResponse = GetBinaryFile(engine, documentPath, documentName);

                var buffer = new byte[getBinaryFileResponse.Size];
                var bufferSize = getBinaryFileResponse.Size > 65536 ? 65536 : getBinaryFileResponse.Size;
                var iterations = Convert.ToInt32(Math.Floor(Convert.ToDouble(getBinaryFileResponse.Size) / bufferSize));
                var remainder = getBinaryFileResponse.Size - (iterations * bufferSize);
                var fileNumber = getBinaryFileResponse.FileNr;
                var offset = 0;

                for (int i = 0; i < iterations; i++)
                {
                    var response = PullDocument(engine, fileNumber, offset, bufferSize);
                    Buffer.BlockCopy(response.Ba.Ba, 0, buffer, offset, bufferSize);
                    offset += bufferSize;
                }

                if (remainder > 0)
                {
                    var response = PullDocument(engine, fileNumber, offset, remainder);
                    Buffer.BlockCopy(response.Ba.Ba, 0, buffer, offset, remainder);
                }

                SetDocumentEof(engine, fileNumber);

                return buffer;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return Array.Empty<byte>();
            }
        }

        /// <summary>Adds or updates a file in the DataMiner Documents folder.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="documentPath">The DataMiner document path that file will be added/updated to.</param>
        /// <param name="documentName">The name of the document to add/update.</param>
        /// <param name="document">The byte[] of the document to be added/updated.</param>
        /// <returns>
        /// <see cref="AddDocumentResponseMessage"/>
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Document Path cannot be null, empty or white space.
        /// or
        /// Document Name cannot be null, empty or white space.
        /// or
        /// Document cannot be empty.
        /// or
        /// Failed to Add Document
        /// </exception>
        public static AddDocumentResponseMessage AddDocument(IEngine engine, string documentPath, string documentName, byte[] document)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(documentPath))
                {
                    throw new ArgumentException("Document Path cannot be null, empty or white space.");
                }

                if (string.IsNullOrWhiteSpace(documentName))
                {
                    throw new ArgumentException("Document Name cannot be null, empty or white space.");
                }

                if (document.Length == 0)
                {
                    throw new ArgumentException("Document cannot be empty.");
                }

                var bufferSize = document.Length > 65536 ? 65536 : document.Length;
                var addDocumentResponse = AddDocument(engine, documentPath, documentName, bufferSize);

                if (addDocumentResponse == null)
                {
                    throw new ArgumentException("Failed to Add Document");
                }

                var fileNumber = addDocumentResponse.Ret;
                var iterations = Convert.ToInt32(Math.Floor(Convert.ToDouble(document.Length) / bufferSize));
                var remainder = document.Length - (iterations * bufferSize);
                var offset = 0;

                for (int i = 0; i < iterations; i++)
                {
                    PushDocument(engine, fileNumber, document.Skip(offset).Take(bufferSize).ToArray());
                    offset += bufferSize;
                }

                if (remainder > 0)
                {
                    UpdateDocumentBufferSize(engine, fileNumber, remainder);
                    PushDocument(engine, fileNumber, document.Skip(offset).Take(remainder).ToArray());
                }

                UpdateDocumentBufferSize(engine, fileNumber, 0);
                SetDocumentEof(engine, fileNumber);

                return addDocumentResponse;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return null;
            }
        }

        private static GetBinaryFileResponseMessage GetBinaryFile(IEngine engine, string documentPath, string documentName)
        {
            var message = new GetBinaryFileMessage
            {
                File = documentName,
                Map = documentPath,
            };

            return engine.SendSLNetSingleResponseMessage(message) as GetBinaryFileResponseMessage;
        }

        private static PullDocumentResponseMessage PullDocument(IEngine engine, int fileNumber, int start, int length)
        {
            var message = new PullDocumentMessage
            {
                FileNr = fileNumber,
                Length = length,
                Start = start,
            };

            return engine.SendSLNetSingleResponseMessage(message) as PullDocumentResponseMessage;
        }

        private static AddDocumentResponseMessage AddDocument(IEngine engine, string documentPath, string documentName, int bufferSize)
        {
            try
            {
                var serverDetails = engine.GetUserConnection().ServerDetails;

                var message = new AddDocumentMessage
                {
                    BufferSize = bufferSize,
                    DataMinerID = serverDetails.AgentID,
                    Element = documentPath,
                    FileSize = 0,
                    HostingDataMinerID = serverDetails.AgentID,
                    Name = documentName,
                };

                return engine.SendSLNetSingleResponseMessage(message) as AddDocumentResponseMessage;
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return null;
            }
        }

        private static void PushDocument(IEngine engine, int fileNumber, byte[] document)
        {
            var serverDetails = engine.GetUserConnection().ServerDetails;

            var message = new PushDocumentMessage
            {
                Ba = new BA(document),
                DataMinerID = serverDetails.AgentID,
                FileNr = fileNumber,
                HostingDataMinerID = serverDetails.AgentID,
                Size = document.Length,
            };

            engine.SendSLNetMessage(message);
        }

        private static void UpdateDocumentBufferSize(IEngine engine, int fileNumber, int bufferSize)
        {
            var serverDetails = engine.GetUserConnection().ServerDetails;

            var message = new UpdateDocumentBufferSizeMessage
            {
                DataMinerID = serverDetails.AgentID,
                FileNr = fileNumber,
                HostingDataMinerID = serverDetails.AgentID,
                Size = bufferSize,
            };

            engine.SendSLNetMessage(message);
        }

        private static void SetDocumentEof(IEngine engine, int fileNumber)
        {
            var serverDetails = engine.GetUserConnection().ServerDetails;

            var message = new SetDocumentEofMessage
            {
                DataMinerID = serverDetails.AgentID,
                FileNr = fileNumber,
                HostingDataMinerID = serverDetails.AgentID,
            };

            engine.SendSLNetMessage(message);
        }
    }
}