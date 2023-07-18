using Skyline.DataMiner.Net.Messages;
using System.Collections.Generic;

namespace Auctus.DataMiner.Library.Common.SLNetType
{
    public static class DmsMessageExtensions
    {
        /// <summary>Casts a DMS message array to the specified ResponseMessage type.</summary>
        /// <typeparam name="T">The target ResponseMessage to cast to.</typeparam>
        /// <param name="response">The SLNetMessage response to cast.</param>
        public static IEnumerable<T> CastDMSMessage<T>(DMSMessage[] response) where T : DMSMessage
        {
            foreach (var item in response)
            {
                yield return (T)item;
            }
        }
    }
}