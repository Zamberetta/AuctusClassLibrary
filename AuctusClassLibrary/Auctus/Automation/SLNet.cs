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
    public static class SLNet
    {
        /// <summary>Triggers a file sync on the DMS for the target file, note that the target file must be on the same DMA to which this call is being triggered from.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="filePath">The target file to sync (local path on the DMA).</param>
        /// <returns>
        ///   SetDataMinerInfoResponseMessage
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
        ///   SetDataMinerInfoResponseMessage
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

                return DmsMessageExtensions.CastDMSMessage<T>(response);
            }
            catch (Exception ex)
            {
                engine.Logger(ex);
                return new List<T>();
            }
        }

        /// <summary>Adds or updates a file in the DataMiner Documents folder.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="documentPath">The DataMiner document path that file will be added/updated to.</param>
        /// <param name="documentName">The name of the document to add/update.</param>
        /// <param name="document">The byte[] of the document to be added/updated.</param>
        /// <returns>
        ///   AddDocumentResponseMessage
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

                if (!document.Any())
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