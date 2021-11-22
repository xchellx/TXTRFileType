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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;

namespace TXTRFileType.Util
{
    /// <summary>
    /// UI utility class
    /// </summary>
    public static class UIUtil
    {
        public static int GetDropDownWidth(ComboBox combo)
        {
            // Get max dropdown width from dropdown items
            object[] items = new object[combo.Items.Count];
            combo.Items.CopyTo(items, 0);
            return items.Select(obj => TextRenderer.MeasureText(combo.GetItemText(obj), combo.Font).Width).Max();
        }

        public static void BindEnumToCombobox<TEnum>(this ComboBox comboBox, TEnum defaultSelection, bool sort = false)
            where TEnum : Enum
        {
            List<ComboBoxEnumItem<TEnum>> list = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(value => ComboBoxEnumItem<TEnum>.CreateComboBoxEnumItem(value))
                .OrderBy(item => sort ? item.Value.ToString() : string.Empty)
                .ToList();

            comboBox.DataSource = list;
            comboBox.DisplayMember = "Title";
            comboBox.ValueMember = "Value";

            foreach (ComboBoxEnumItem<TEnum> item in list)
                if (item.Value.ToString() == defaultSelection.ToString())
                    comboBox.SelectedItem = item;
        }

        public class ComboBoxEnumItem<TEnum> where TEnum : Enum
        {
            private ComboBoxEnumItem() { }

            public string Title { get; private set; }
            public TEnum Value { get; private set; }

            public static ComboBoxEnumItem<TEnum> CreateComboBoxEnumItem(TEnum value) => new ComboBoxEnumItem<TEnum>()
            {
                Title = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                    typeof(EnumTitleForComboBoxAttribute)) as EnumTitleForComboBoxAttribute)?.Title ?? value.ToString(),
                Value = value
            };
        }

        /// <summary>
        /// Specifies a title for a enum in usage of a <see cref="ComboBoxEnumItem{TEnum}"/>.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class EnumTitleForComboBoxAttribute : Attribute
        {
            /// <summary>
            /// Specifies the default value for the <see cref='EnumTitleForComboBoxAttribute'/>,
            /// which is an empty string (""). This <see langword='static'/> field is read-only.
            /// </summary>
            public static readonly EnumTitleForComboBoxAttribute Default = new EnumTitleForComboBoxAttribute();

            public EnumTitleForComboBoxAttribute() : this(string.Empty)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref='EnumTitleForComboBoxAttribute'/> class.
            /// </summary>
            public EnumTitleForComboBoxAttribute(string title)
            {
                TitleValue = title;
            }

            /// <summary>
            /// Gets the title stored in this attribute.
            /// </summary>
            public virtual string Title => TitleValue;

            /// <summary>
            /// Read/Write property that directly modifies the string stored in the title
            /// attribute. The default implementation of the <see cref="Title"/> property
            /// simply returns this value.
            /// </summary>
            protected string TitleValue { get; set; }

            public override bool Equals([NotNullWhen(true)] object? obj) =>
                obj is EnumTitleForComboBoxAttribute other && other.Title == Title;

            private int? hashCache = null;
            public override int GetHashCode()
            {
                if (!hashCache.HasValue)
                    hashCache = Title?.GetHashCode() ?? 0;
                return hashCache.Value;
            }

            public override bool IsDefaultAttribute() => Equals(Default);
        }

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}
