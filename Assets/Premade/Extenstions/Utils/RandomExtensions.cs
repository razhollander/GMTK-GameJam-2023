using System;

public static class RandomExtensions 
{
    //The following implementation uses the Fisher-Yates algorithm AKA the Knuth Shuffle.
    //It runs in O(n) time and shuffles in place, so is better performing than the 'sort by random' technique,
    //although it is more lines of code. See here for some comparative performance measurements.
    //I have used System.Random, which is fine for non-cryptographic purposes.*
    //https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
    public static void Shuffle<T>(this Random rng, T[] array)
    {
        var n = array.Length;

        while (n > 1)
        {
            var k = rng.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}