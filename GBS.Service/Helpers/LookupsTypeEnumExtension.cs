using GBS.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Service.Helpers
{
    public static class LookupsTypeEnumExtension
    {

        public static string GetStringValue(this LookupsTypeEnum enumValue)
        {
            var type = enumValue.GetType();
            var fieldInfo = type.GetField(enumValue.ToString());
            var descriptionAttribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? enumValue.ToString();
        }

        public static LookupsTypeEnum GetEnumFromString(string value)
        {
            foreach (LookupsTypeEnum enumValue in Enum.GetValues<LookupsTypeEnum>())
            {
                if (enumValue.GetStringValue().Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return enumValue;
                }
            }
            throw new ArgumentException($"No enum value found for string: {value}");
        }
    }
}
