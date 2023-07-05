using System;
using System.Collections.Generic;
using System.Text;

public static class CollectionExtensions // TODO : Jenya is it in use?>??
{
    public static bool IsNullOrEmpty<T, TV>(this IDictionary<T, TV> dictionary) => dictionary == null || dictionary.Count == 0;
    public static bool DoesntContainKey<T, TV>(this IDictionary<T, TV> dictionary, T key) => !dictionary.ContainsKey(key);

    public static string ToStringDebug<T>(this ICollection<T> list)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("Count: ");
        sb.Append(list.Count);
        sb.AppendLine();

        foreach (var item in list)
        {
            sb.Append(item);
            sb.Append(", ");
        }

        return sb.ToString();
    }

    public static bool TryFind<T>(this ICollection<T> collection, Func<T, bool> condition, out T foundItem)
    {
        foreach (var item in collection)
        {
            if (condition(item))
            {
                foundItem = item;

                return true;
            }
        }

        foundItem = default;

        return false;
    }
}