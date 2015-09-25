using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace MLocati.MediaData
{
    public static class UI
    {

        #region Types

        private class DescribedEnumValue
        {
            public readonly string Name;
            public readonly Enum Value;
            public DescribedEnumValue(string name, Enum value)
            {
                this.Name = name;
                this.Value = value;
            }
            public override string ToString()
            {
                return this.Name;
            }
        }

        #endregion


        #region Static properties

        private static Type _descriptionAttributeType = null;
        private static Type DescriptionAttributeType
        {
            get
            {
                if (UI._descriptionAttributeType == null)
                {
                    UI._descriptionAttributeType = typeof(DescriptionAttribute);
                }
                return UI._descriptionAttributeType;
            }
        }

        #endregion


        #region Static methods

        public static void DescribedEnumToCombobox(Type type, ComboBox cbx)
        {
            UI.DescribedEnumToCombobox(type, cbx, null);
        }
        public static void DescribedEnumToCombobox(Type type, ComboBox cbx, object selectedItem)
        {
            Array enumValues = Enum.GetValues(type);
            List<DescribedEnumValue> wrappedValues = new List<DescribedEnumValue>(enumValues.Length);
            DescribedEnumValue selectedWrappedValue = null;
            foreach (Enum enumValue in enumValues)
            {
                string valueName = enumValue.ToString();
                string valueDescription = valueName;
                DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(type.GetField(valueName), UI.DescriptionAttributeType) as DescriptionAttribute;
                if (descriptionAttribute != null)
                {
                    valueDescription = descriptionAttribute.Description;
                }
                DescribedEnumValue wrappedValue = new DescribedEnumValue(valueDescription, enumValue);
                wrappedValues.Add(wrappedValue);
                if (selectedItem != null && selectedItem.ToString() == valueName)
                {
                    selectedWrappedValue = wrappedValue;
                }
            }
            cbx.DataSource = wrappedValues;
            if (selectedWrappedValue != null)
            {
                cbx.SelectedItem = selectedWrappedValue;
            }
            else if (selectedItem != null && selectedItem.GetType() == typeof(int))
            {
                cbx.SelectedIndex = (int)selectedItem;
            }
        }

        public static object GetDescribedEnumValueOfCombobox(ComboBox cbx)
        {
            DescribedEnumValue v = cbx.SelectedItem as DescribedEnumValue;
            return (v == null) ? null : v.Value;
        }

        #endregion

    }
}
