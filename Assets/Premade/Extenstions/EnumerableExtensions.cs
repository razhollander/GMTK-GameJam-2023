using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static T GetRandomItem<T>(this IEnumerable<T> itemsEnumerable, Func<T, int> weightKey)
        {
            var items = itemsEnumerable.ToList();

            var totalWeight = items.Sum(x => weightKey(x));
            var randomWeightedIndex = UnityEngine.Random.Range(0, totalWeight);
            var itemWeightedIndex = 0;
            
            foreach(var item in items)
            {
                itemWeightedIndex += weightKey(item);

                if (randomWeightedIndex < itemWeightedIndex)
                {
                    return item;
                }
            }
            
            throw new ArgumentException("Collection count and weights must be greater than 0");
        }
    }
}