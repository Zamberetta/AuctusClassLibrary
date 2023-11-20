#pragma warning disable S107 // Methods should not have too many parameters

namespace Auctus.DataMiner.Library.Protocol
{
    using global::Auctus.DataMiner.Library.Auctus.Common.Shared;
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///   Logging extension methods for SLProtocol.
    /// </summary>
    public static class ProtocolLogger
    {
        /// <summary>Logs the specified string value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        public static void Logger(this SLProtocol protocol, string message, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            protocol.Log((int)logType, (int)logLevel, $"QA{protocol.QActionID}|TR{protocol.GetTriggerParameter()}|{memberName}|{message}");
        }

        /// <summary>Logs the specified exception message and StackTrace.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, Exception exception, LogType logType = LogType.Error, LogLevel logLevel = LogLevel.NoLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, $"Exception => {exception.GetType()} {exception.Message}{Environment.NewLine}{exception.StackTrace}", logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified object using the specified format information.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">An object to write using format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, string format, object arg0, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, string.Format(format, arg0), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified objects using the specified format information.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to write using the format parameter.</param>
        /// <param name="arg1">The second object to write using the format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, string format, object arg0, object arg1, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, string.Format(format, arg0, arg1), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified objects using the specified format information.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to write using the format parameter.</param>
        /// <param name="arg1">The second object to write using the format parameter.</param>
        /// <param name="arg2">The third object to write using the format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, string format, object arg0, object arg1, object arg2, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, string.Format(format, arg0, arg1, arg2), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified array of objects using the specified format information.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using the format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        public static void Logger(this SLProtocol protocol, string format, object[] arg, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            if (arg == null)
            {
                Logger(protocol, string.Format(format, null, null), logType, logLevel, memberName);
            }
            else
            {
                Logger(protocol, string.Format(format, arg), logType, logLevel, memberName);
            }
        }

        /// <summary>Logs the text representation of the specified Boolean value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, bool value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value ? "True" : "False", logType, logLevel, memberName);
        }

        /// <summary>Logs the specified Unicode character value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, char value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the specified array of Unicode characters.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="buffer">A Unicode character array.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, char[] buffer, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, buffer, 0, buffer.Length, logType, logLevel, memberName);
        }

        /// <summary>Logs the specified sub-array of Unicode characters, followed by the current line terminator.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="buffer">A Unicode character array.</param>
        /// <param name="index">The starting position in buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        /// <exception cref="System.ArgumentNullException">buffer - Buffer cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index - Index cannot be less than 0.
        /// or
        /// count - Count cannot be less than 0.</exception>
        /// <exception cref="System.ArgumentException">Count cannot be bigger than the offsetted buffer.</exception>
        public static void Logger(this SLProtocol protocol, char[] buffer, int index, int count, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            var message = LoggerExtensibility.BufferToString(buffer, index, count);

            Logger(protocol, message, logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified double-precision floating-point value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, double value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified Decimal value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, decimal value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified single-precision floating-point value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, float value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 32-bit signed integer value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, int value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 32-bit unsigned integer value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, uint value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 64-bit signed integer value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, long value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 64-bit unsigned integer value.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this SLProtocol protocol, ulong value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            Logger(protocol, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified object.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        public static void Logger(this SLProtocol protocol, object value, LogType logType = LogType.Allways, LogLevel logLevel = LogLevel.DevelopmentLogging, [CallerMemberName] string memberName = "")
        {
            if (value != null)
            {
                if (value is IFormattable formattable)
                {
                    Logger(protocol, formattable.ToString(), logType, logLevel, memberName);
                }
                else
                {
                    Logger(protocol, value.ToString(), logType, logLevel, memberName);
                }

                return;
            }

            Logger(protocol, "NULL", logType, logLevel, memberName);
        }
    }
}