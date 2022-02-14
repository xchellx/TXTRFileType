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
using System.Collections.Generic;
using System.ComponentModel;

namespace TXTRFileTypeCLI
{
    public sealed class HRESULT : IEquatable<HRESULT>
    {
        public HRESULT(uint code, string message) : this(unchecked((int)code), message)
        {
        }

        public HRESULT(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; }

#pragma warning disable IDE1006 // C style
        ///<summary>
        /// Success code
        ///</summary>
        [Description("Success code")]
        public static HRESULT S_OK { get => new(0x00000000, "Success code"); }

        ///<summary>
        /// Success code false
        ///</summary>
        [Description("Success code false")]
        public static HRESULT S_FALSE { get => new(0x00000001, "Success code false"); }

        ///<summary>
        /// One or more arguments are invalid
        ///</summary>
        [Description("One or more arguments are invalid")]
        public static HRESULT E_INVALIDARG { get => new(0x80070057, "One or more arguments are invalid"); }

        ///<summary>
        /// Operation aborted
        ///</summary>
        [Description("Operation aborted")]
        public static HRESULT E_ABORT { get => new(0x80004004, "Operation aborted"); }

        ///<summary>
        /// Unspecified error
        ///</summary>
        [Description("Unspecified error")]
        public static HRESULT E_FAIL { get => new(0x80004005, "Unspecified error"); }
#pragma warning restore IDE1006 // C style

        public static implicit operator int(HRESULT h) => h.Code;

        public static implicit operator uint(HRESULT h) => unchecked((uint)h.Code);

        public static implicit operator string(HRESULT h) => h.Message;

        public static bool operator ==(HRESULT? left, HRESULT? right)
            => EqualityComparer<HRESULT>.Default.Equals(left, right);

        public static bool operator !=(HRESULT? left, HRESULT? right) => !(left == right);

        public override string ToString() => $"{nameof(HRESULT)}{{Code={Code},Message={Message}}}#{GetHashCode()}";

        public override int GetHashCode() => HashCode.Combine(Code, Message);

        public override bool Equals(object? obj) => Equals(obj as HRESULT);

        public bool Equals(HRESULT? other)
            => other != null &&
            Code == other.Code &&
            Message == other.Message;
    }
}
