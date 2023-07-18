#pragma warning disable S107 // Methods should not have too many parameters

namespace Auctus.DataMiner.Library.Automation
{
    using Skyline.DataMiner.Automation;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class EngineLogger
    {
        /// <summary>Logs the specified string value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        public static void Logger(this Engine engine, string message, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            engine.Log($"ID{engine.InstanceId}|{memberName}|{message}", logType, logLevel);
        }

        /// <summary>Logs the specified exception message and StackTrace.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, Exception exception, LogType logType = LogType.Error, int logLevel = 1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, $"Exception => {exception.GetType()} {exception.Message}{Environment.NewLine}{exception.StackTrace}", logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified object using the specified format information.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">An object to write using format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, string format, object arg0, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, string.Format(format, arg0), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified objects using the specified format information.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to write using the format parameter.</param>
        /// <param name="arg1">The second object to write using the format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, string format, object arg0, object arg1, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, string.Format(format, arg0, arg1), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified objects using the specified format information.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to write using the format parameter.</param>
        /// <param name="arg1">The second object to write using the format parameter.</param>
        /// <param name="arg2">The third object to write using the format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, string format, object arg0, object arg1, object arg2, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, string.Format(format, arg0, arg1, arg2), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified array of objects using the specified format information.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using the format parameter.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        public static void Logger(this Engine engine, string format, object[] arg, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            if (arg == null)
            {
                Logger(engine, string.Format(format, null, null), logType, logLevel, memberName);
            }
            else
            {
                Logger(engine, string.Format(format, arg), logType, logLevel, memberName);
            }
        }

        /// <summary>Logs the text representation of the specified Boolean value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, bool value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value ? "True" : "False", logType, logLevel, memberName);
        }

        /// <summary>Logs the specified Unicode character value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, char value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the specified array of Unicode characters.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="buffer">A Unicode character array.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, char[] buffer, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, buffer, 0, buffer.Length, logType, logLevel, memberName);
        }

        /// <summary>Logs the specified sub-array of Unicode characters, followed by the current line terminator.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
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
        public static void Logger(this Engine engine, char[] buffer, int index, int count, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
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

            Logger(engine, stringbuilder.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified double-precision floating-point value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, double value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified Decimal value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, decimal value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified single-precision floating-point value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, float value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 32-bit signed integer value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, int value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 32-bit unsigned integer value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, uint value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 64-bit signed integer value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, long value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified 64-bit unsigned integer value.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        [MethodImpl]
        public static void Logger(this Engine engine, ulong value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            Logger(engine, value.ToString(), logType, logLevel, memberName);
        }

        /// <summary>Logs the text representation of the specified object.</summary>
        /// <param name="engine">Interfaces with the DataMiner System from an Automation script.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="logType">Specifies the log type.</param>
        /// <param name="logLevel">Specifies the log level.</param>
        /// <param name="memberName">The method or property name that invoked this method.</param>
        public static void Logger(this Engine engine, object value, LogType logType = LogType.Information, int logLevel = -1, [CallerMemberName] string memberName = "")
        {
            if (value != null)
            {
                if (value is IFormattable formattable)
                {
                    Logger(engine, formattable.ToString(), logType, logLevel, memberName);
                }
                else
                {
                    Logger(engine, value.ToString(), logType, logLevel, memberName);
                }

                return;
            }

            Logger(engine, "NULL", logType, logLevel, memberName);
        }
    }
}