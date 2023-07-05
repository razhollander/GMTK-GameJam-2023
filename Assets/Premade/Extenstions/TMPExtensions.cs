using System;
using System.Collections.Generic;
using TMPro;

namespace CoreDomain.Scripts.Extensions
{
    public static class TMPExtensions
    {
        public static void PopulateEnum(this TMP_Dropdown dropdown, Enum targetEnum)
        {
            var enumType = targetEnum.GetType();
            var newOptions = new List<TMP_Dropdown.OptionData>();

            foreach (var name in Enum.GetNames(enumType))
            {
                newOptions.Add(new TMP_Dropdown.OptionData(name));
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(newOptions);
        }
    }
}
