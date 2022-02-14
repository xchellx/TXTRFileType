/*
TXTRFileType
Copyright (C) 2021 xchellx

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Text;

namespace TXTRFileTypeCLI.Logging
{
    internal sealed class ConsoleLogger : ALogger<ConsoleLoggerFlags>
    {
        public ConsoleLogger(string tag) : base(tag, LoggerSeverity.VERB, ConsoleLoggerFlags.NONE)
        {
            logMsgBuilder = new StringBuilder();
        }

        public ConsoleLogger(string tag, LoggerSeverity severity) : base(tag, severity, ConsoleLoggerFlags.NONE)
        {
            logMsgBuilder = new StringBuilder();
        }

        public ConsoleLogger(string tag, LoggerSeverity severity, ConsoleLoggerFlags flags) : base(tag, severity, flags)
        {
            logMsgBuilder = new StringBuilder();
        }

        private readonly StringBuilder logMsgBuilder;

        protected override void Log(LoggerSeverity severity, bool newLine, uint value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, string format, params object[] args)
            => Log(severity, string.Format(format, args), newLine);

        protected override void Log(LoggerSeverity severity) => Log(severity, string.Empty, true);

        protected override void Log(LoggerSeverity severity, bool newLine, bool value)
            => Log(severity, (value ? "true" : "false"), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, char[] buffer)
            => Log(severity, new string(buffer), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, char[] buffer, int index, int count)
            => Log(severity, new string(buffer, index, count), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, decimal value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, double value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, ulong value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, int value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, object value)
            => Log(severity, value?.ToString() ?? "null", newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, float value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, string value)
            => Log(severity, value, newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, string format, object arg0)
            => Log(severity, string.Format(format, arg0), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, string format, object arg0, object arg1)
            => Log(severity, string.Format(format, arg0, arg1), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, string format, object arg0, object arg1, object arg2)
            => Log(severity, string.Format(format, arg0, arg1, arg2), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, long value)
            => Log(severity, value.ToString(), newLine);

        protected override void Log(LoggerSeverity severity, bool newLine, char value)
            => Log(severity, value.ToString(), newLine);

        private void Log(LoggerSeverity severity, string value, bool newLine)
        {
            bool consoleEnabled = (Flags & ConsoleLoggerFlags.CONSOLE) == ConsoleLoggerFlags.CONSOLE,
                debugEnabled = (Flags & ConsoleLoggerFlags.DEBUG) == ConsoleLoggerFlags.CONSOLE,
                quietEnabled = (Flags & ConsoleLoggerFlags.QUIET) == ConsoleLoggerFlags.QUIET,
                consoleColorsEnabled = (Flags & ConsoleLoggerFlags.COLORS) == ConsoleLoggerFlags.COLORS,
                noTagEnabled = (Flags & ConsoleLoggerFlags.NOTAG) == ConsoleLoggerFlags.NOTAG;

            if ((int)severity >= (int)Severity && ((consoleEnabled || debugEnabled) && !quietEnabled))
            {
                ConsoleColor previousConsoleColor = Console.ForegroundColor;

                if (consoleColorsEnabled && consoleEnabled)
                {
                    switch (severity)
                    {
                        case LoggerSeverity.VERB:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case LoggerSeverity.INFO:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case LoggerSeverity.WARN:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case LoggerSeverity.ERROR:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }
                }

                string severityStr = string.Empty;
                switch (severity)
                {
                    case LoggerSeverity.VERB:
                        severityStr = nameof(LoggerSeverity.VERB);
                        break;
                    case LoggerSeverity.INFO:
                        severityStr = nameof(LoggerSeverity.INFO);
                        break;
                    case LoggerSeverity.WARN:
                        severityStr = nameof(LoggerSeverity.WARN);
                        break;
                    case LoggerSeverity.ERROR:
                        severityStr = nameof(LoggerSeverity.ERROR);
                        break;
                }

                // Format: yyyy-MM-ddThh:mm:ss.SSS-zzz TAG Severity value
                logMsgBuilder.Clear();
                if (!noTagEnabled)
                {
                    logMsgBuilder.Append(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString("o"))
                        .Append(' ')
                        .Append(Tag)
                        .Append(' ')
                        .Append(severityStr)
                        .Append(' ');
                }
                string logMsg = logMsgBuilder.Append(value).ToString();

                if (consoleEnabled)
                {
                    if (severity == LoggerSeverity.WARN || severity == LoggerSeverity.ERROR)
                    {
                        if (newLine)
                            Console.Error.WriteLine(logMsg);
                        else
                            Console.Error.Write(logMsg);
                    }
                    else
                    {
                        if (newLine)
                            Console.Out.WriteLine(logMsg);
                        else
                            Console.Out.Write(logMsg);
                    }
                }
                if (debugEnabled)
                {
                    if (newLine)
                        Debug.WriteLine(logMsg);
                    else
                        Debug.Write(logMsg);
                }

                Console.ForegroundColor = previousConsoleColor;
            }
        }
    }
}
