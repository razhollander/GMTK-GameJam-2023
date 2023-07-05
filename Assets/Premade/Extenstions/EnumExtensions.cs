using System;

namespace CoreDomain.Scripts.Extensions
{
    public static class EnumExtensions
    {
        public static bool TryToEnum<T>(this string str, out T result, bool ignoreCase = false) where T : Enum
        {
            if (!Enum.TryParse(typeof(T), str, ignoreCase, out var enumResult))
            {
                result = default;
                return false;
            }

            result = (T) enumResult;
            return true;

        }
        
        public static T ToEnum<T>(this int number)
        {
            return (T) Enum.ToObject(typeof(T), number);
        }
        
        public static string[] GetEnumNames(this Enum thisEnum)
        {
            return Enum.GetNames(thisEnum.GetType());
        }
    }
}