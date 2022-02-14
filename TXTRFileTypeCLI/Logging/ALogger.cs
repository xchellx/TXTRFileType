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

namespace TXTRFileTypeCLI.Logging
{
    internal abstract class ALogger<TFlags> where TFlags : struct, Enum
    {
        protected ALogger(string tag, LoggerSeverity severity, TFlags flags)
        {
            if (tag != null)
            {
                Tag = tag;
                Severity = severity;
                Flags = flags;
            }
            else
                throw new ArgumentNullException(nameof(tag));
        }

        public string Tag { get; }

        public LoggerSeverity Severity { get; set; }

        public TFlags Flags { get; set; }

        public void VerbLine(uint value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(string format, params object[] args) => Log(LoggerSeverity.VERB, true, format, args);

        public void VerbLine() => Log(LoggerSeverity.VERB);

        public void VerbLine(bool value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(char[] buffer) => Log(LoggerSeverity.VERB, true, buffer);

        public void VerbLine(char[] buffer, int index, int count) => Log(LoggerSeverity.VERB, true, buffer, index, count);

        public void VerbLine(decimal value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(double value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(ulong value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(int value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(object value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(float value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(string value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(string format, object arg0) => Log(LoggerSeverity.VERB, true, format, arg0);

        public void VerbLine(string format, object arg0, object arg1) => Log(LoggerSeverity.VERB, true, format, arg0, arg1);

        public void VerbLine(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.VERB, true, format, arg0, arg1, arg2);

        public void VerbLine(long value) => Log(LoggerSeverity.VERB, true, value);

        public void VerbLine(char value) => Log(LoggerSeverity.VERB, true, value);

        public void InfoLine(uint value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(string format, params object[] args) => Log(LoggerSeverity.INFO, true, format, args);

        public void InfoLine() => Log(LoggerSeverity.INFO);

        public void InfoLine(bool value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(char[] buffer) => Log(LoggerSeverity.INFO, true, buffer);

        public void InfoLine(char[] buffer, int index, int count) => Log(LoggerSeverity.INFO, true, buffer, index, count);

        public void InfoLine(decimal value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(double value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(ulong value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(int value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(object value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(float value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(string value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(string format, object arg0) => Log(LoggerSeverity.INFO, true, format, arg0);

        public void InfoLine(string format, object arg0, object arg1) => Log(LoggerSeverity.INFO, true, format, arg0, arg1);

        public void InfoLine(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.INFO, true, format, arg0, arg1, arg2);

        public void InfoLine(long value) => Log(LoggerSeverity.INFO, true, value);

        public void InfoLine(char value) => Log(LoggerSeverity.INFO, true, value);

        public void WarnLine(uint value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(string format, params object[] args) => Log(LoggerSeverity.WARN, true, format, args);

        public void WarnLine() => Log(LoggerSeverity.WARN);

        public void WarnLine(bool value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(char[] buffer) => Log(LoggerSeverity.WARN, true, buffer);

        public void WarnLine(char[] buffer, int index, int count) => Log(LoggerSeverity.WARN, true, buffer, index, count);

        public void WarnLine(decimal value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(double value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(ulong value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(int value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(object value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(float value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(string value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(string format, object arg0) => Log(LoggerSeverity.WARN, true, format, arg0);

        public void WarnLine(string format, object arg0, object arg1) => Log(LoggerSeverity.WARN, true, format, arg0, arg1);

        public void WarnLine(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.WARN, true, format, arg0, arg1, arg2);

        public void WarnLine(long value) => Log(LoggerSeverity.WARN, true, value);

        public void WarnLine(char value) => Log(LoggerSeverity.WARN, true, value);

        public void ErrorLine(uint value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(string format, params object[] args) => Log(LoggerSeverity.ERROR, true, format, args);

        public void ErrorLine() => Log(LoggerSeverity.ERROR);

        public void ErrorLine(bool value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(char[] buffer) => Log(LoggerSeverity.ERROR, true, buffer);

        public void ErrorLine(char[] buffer, int index, int count) => Log(LoggerSeverity.ERROR, true, buffer, index, count);

        public void ErrorLine(decimal value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(double value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(ulong value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(int value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(object value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(float value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(string value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(string format, object arg0) => Log(LoggerSeverity.ERROR, true, format, arg0);

        public void ErrorLine(string format, object arg0, object arg1) => Log(LoggerSeverity.ERROR, true, format, arg0, arg1);

        public void ErrorLine(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.ERROR, true, format, arg0, arg1, arg2);

        public void ErrorLine(long value) => Log(LoggerSeverity.ERROR, true, value);

        public void ErrorLine(char value) => Log(LoggerSeverity.ERROR, true, value);

        public void Verb(uint value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(string format, params object[] args) => Log(LoggerSeverity.VERB, false, format, args);

        public void Verb(bool value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(char[] buffer) => Log(LoggerSeverity.VERB, false, buffer);

        public void Verb(char[] buffer, int index, int count) => Log(LoggerSeverity.VERB, false, buffer, index, count);

        public void Verb(decimal value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(double value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(ulong value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(int value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(object value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(float value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(string value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(string format, object arg0) => Log(LoggerSeverity.VERB, false, format, arg0);

        public void Verb(string format, object arg0, object arg1) => Log(LoggerSeverity.VERB, false, format, arg0, arg1);

        public void Verb(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.VERB, false, format, arg0, arg1, arg2);

        public void Verb(long value) => Log(LoggerSeverity.VERB, false, value);

        public void Verb(char value) => Log(LoggerSeverity.VERB, false, value);

        public void Info(uint value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(string format, params object[] args) => Log(LoggerSeverity.INFO, false, format, args);

        public void Info(bool value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(char[] buffer) => Log(LoggerSeverity.INFO, false, buffer);

        public void Info(char[] buffer, int index, int count) => Log(LoggerSeverity.INFO, false, buffer, index, count);

        public void Info(decimal value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(double value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(ulong value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(int value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(object value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(float value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(string value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(string format, object arg0) => Log(LoggerSeverity.INFO, false, format, arg0);

        public void Info(string format, object arg0, object arg1) => Log(LoggerSeverity.INFO, false, format, arg0, arg1);

        public void Info(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.INFO, false, format, arg0, arg1, arg2);

        public void Info(long value) => Log(LoggerSeverity.INFO, false, value);

        public void Info(char value) => Log(LoggerSeverity.INFO, false, value);

        public void Warn(uint value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(string format, params object[] args) => Log(LoggerSeverity.WARN, false, format, args);

        public void Warn(bool value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(char[] buffer) => Log(LoggerSeverity.WARN, false, buffer);

        public void Warn(char[] buffer, int index, int count) => Log(LoggerSeverity.WARN, false, buffer, index, count);

        public void Warn(decimal value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(double value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(ulong value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(int value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(object value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(float value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(string value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(string format, object arg0) => Log(LoggerSeverity.WARN, false, format, arg0);

        public void Warn(string format, object arg0, object arg1) => Log(LoggerSeverity.WARN, false, format, arg0, arg1);

        public void Warn(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.WARN, false, format, arg0, arg1, arg2);

        public void Warn(long value) => Log(LoggerSeverity.WARN, false, value);

        public void Warn(char value) => Log(LoggerSeverity.WARN, false, value);

        public void Error(uint value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(string format, params object[] args) => Log(LoggerSeverity.ERROR, false, format, args);

        public void Error(bool value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(char[] buffer) => Log(LoggerSeverity.ERROR, false, buffer);

        public void Error(char[] buffer, int index, int count) => Log(LoggerSeverity.ERROR, false, buffer, index, count);

        public void Error(decimal value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(double value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(ulong value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(int value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(object value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(float value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(string value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(string format, object arg0) => Log(LoggerSeverity.ERROR, false, format, arg0);

        public void Error(string format, object arg0, object arg1) => Log(LoggerSeverity.ERROR, false, format, arg0, arg1);

        public void Error(string format, object arg0, object arg1, object arg2) => Log(LoggerSeverity.ERROR, false, format, arg0, arg1, arg2);

        public void Error(long value) => Log(LoggerSeverity.ERROR, false, value);

        public void Error(char value) => Log(LoggerSeverity.ERROR, false, value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, uint value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, string format, params object[] args);

        protected abstract void Log(LoggerSeverity severity);

        protected abstract void Log(LoggerSeverity severity, bool newLine, bool value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, char[] buffer);

        protected abstract void Log(LoggerSeverity severity, bool newLine, char[] buffer, int index, int count);

        protected abstract void Log(LoggerSeverity severity, bool newLine, decimal value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, double value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, ulong value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, int value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, object value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, float value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, string value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, string format, object arg0);

        protected abstract void Log(LoggerSeverity severity, bool newLine, string format, object arg0, object arg1);

        protected abstract void Log(LoggerSeverity severity, bool newLine, string format, object arg0, object arg1, object arg2);

        protected abstract void Log(LoggerSeverity severity, bool newLine, long value);

        protected abstract void Log(LoggerSeverity severity, bool newLine, char value);
    }
}
