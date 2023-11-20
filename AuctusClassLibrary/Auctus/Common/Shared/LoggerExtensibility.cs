using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.DataMiner.Library.Auctus.Common.Shared
{
    internal static class LoggerExtensibility
    {
        internal static string BufferToString(char[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer), "Buffer cannot be null.");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be less than 0.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be less than 0.");
            }
            if (buffer.Length - index < count)
            {
                throw new ArgumentException("Count cannot be bigger than the offsetted buffer.");
            }

            var stringbuilder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                stringbuilder.Append(buffer[index + i]);
            }

            return stringbuilder.ToString();
        }
    }
}