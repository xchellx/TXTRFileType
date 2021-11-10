using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TXTRFileType.Util
{
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

        /// <summary>Specifies a title for a enum to use with <see cref="BindEnumToCombobox{TEnum}(ComboBox, TEnum, bool)" />.</summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class EnumTitleForComboBoxAttribute : Attribute
        {
            /// <summary>
            /// Specifies the default value for the <see cref="EnumTitleForComboBoxAttribute" />,
            /// which is an empty string (""). This static field is read-only.
            /// </summary>
            public static readonly EnumTitleForComboBoxAttribute Default = new EnumTitleForComboBoxAttribute();

            private string title;

            /// <summary>Gets the title stored in this attribute.</summary>
            /// <returns>The title stored in this attribute.</returns>
            public virtual string Title => TitleValue;

            /// <summary>Gets or sets the string stored as the title.</summary>
            /// <returns>The string stored as the title. The default value is an empty string ("").</returns>
            protected string TitleValue { get { return title; } set { title = value; } }

            /// <summary>Initializes a new instance of the <see cref="EnumTitleForComboBoxAttribute" /> class with no parameters.</summary>
            public EnumTitleForComboBoxAttribute() : this(string.Empty) { }

            /// <summary>Initializes a new instance of the <see cref="EnumTitleForComboBoxAttribute" /> class with a title.</summary>
            /// <param name="title">The title text. </param>
            public EnumTitleForComboBoxAttribute(string title) { this.title = title; }

            /// <summary>Returns whether the value of the given object is equal to the current <see cref="EnumTitleForComboBoxAttribute" />.</summary>
            /// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
            /// <param name="obj">The object to test the value equality of. </param>
            public override bool Equals(object obj)
            {
                if (obj == this)
                    return true;
                EnumTitleForComboBoxAttribute enumTitleForComboBoxAttribute = obj as EnumTitleForComboBoxAttribute;
                if (enumTitleForComboBoxAttribute != null)
                    return enumTitleForComboBoxAttribute.Title == Title;
                return false;
            }

            private int? hashCache = null;
            public override int GetHashCode()
            {
                if (!hashCache.HasValue)
                    hashCache = Title.GetHashCode();
                return hashCache.Value;
            }

            /// <summary>Returns a value indicating whether this is the default <see cref="EnumTitleForComboBoxAttribute" /> instance.</summary>
            /// <returns>true, if this is the default <see cref="EnumTitleForComboBoxAttribute" /> instance; otherwise, false.</returns>
            public override bool IsDefaultAttribute() { return Equals(Default); }
        }

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}
