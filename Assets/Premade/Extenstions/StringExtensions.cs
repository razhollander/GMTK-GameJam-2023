public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return true;
        }

        foreach (var c in str)
        {
            if (!char.IsWhiteSpace(c))
            {
                return false;
            }
        }

        return true;
    }

    public static int CountSubstring(this string text, string value)
    {
        int count = 0, minIndex = text.IndexOf(value, 0);

        while (minIndex != -1)
        {
            minIndex = text.IndexOf(value, minIndex + value.Length);
            count++;
        }

        return count;
    }
}