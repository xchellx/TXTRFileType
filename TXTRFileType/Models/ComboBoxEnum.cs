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
using TXTRFileTypeLib.Extensions;

namespace TXTRFileType.Models
{
    internal sealed class ComboBoxEnum<TEnum> : IEquatable<ComboBoxEnum<TEnum>?>
        where TEnum : struct, Enum
    {
        public ComboBoxEnum(TEnum value) : this(value.GetDescription() ?? value.ToString(), value)
        {
        }

        private ComboBoxEnum(string title, TEnum value)
        {
            Title = title;
            Value = value;
        }

        public string Title { get; }

        public TEnum Value { get; }

        public override string? ToString()
            => $"{nameof(ComboBoxEnum<TEnum>)}{{{nameof(Title)}={Title}," +
            $"{nameof(Value)}={Value}}}#{GetHashCode()}";

        public override bool Equals(object? obj)
            => Equals(obj as ComboBoxEnum<TEnum>);

        public bool Equals(ComboBoxEnum<TEnum>? other)
            => other != null &&
            Title == other.Title &&
            EqualityComparer<TEnum>.Default.Equals(Value, other.Value);

        public override int GetHashCode() => HashCode.Combine(Title, Value);

        public static bool operator ==(ComboBoxEnum<TEnum>? left, ComboBoxEnum<TEnum>? right)
            => EqualityComparer<ComboBoxEnum<TEnum>>.Default.Equals(left, right);

        public static bool operator !=(ComboBoxEnum<TEnum>? left, ComboBoxEnum<TEnum>? right)
            => !(left == right);
    }
}
