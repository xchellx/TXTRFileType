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
using System.Linq;
using System.Windows.Forms;
using TXTRFileType.Models;

namespace TXTRFileType.Extensions
{
    internal static class ComboBoxExtensions
    {
        public static int GetMaxDropDownWidth(this ComboBox? @this)
        {
            if (@this != null)
            {
                object[] items = new object[@this.Items.Count];
                @this.Items.CopyTo(items, 0);
                return items.Select(obj => TextRenderer.MeasureText(@this.GetItemText(obj), @this.Font).Width).Max();
            }
            else
                throw new NullReferenceException();
        }

        public static void BindEnumToCombobox<TEnum>(this ComboBox? @this, TEnum defaultSelection, bool sort = false)
            where TEnum : struct, Enum
        {
            if (@this != null)
            {
                List<ComboBoxEnum<TEnum>> list = Enum.GetValues(typeof(TEnum))
                    .Cast<TEnum>()
                    .Select(value => new ComboBoxEnum<TEnum>(value))
                    .OrderBy(item => sort ? item.Value.ToString() : string.Empty)
                    .ToList();

                @this.DataSource = list;
                @this.DisplayMember = "Title";
                @this.ValueMember = "Value";

                foreach (ComboBoxEnum<TEnum> item in list)
                    if (item.Value.ToString() == defaultSelection.ToString())
                        @this.SelectedItem = item;
            }
            else
                throw new NullReferenceException();
        }
    }
}
