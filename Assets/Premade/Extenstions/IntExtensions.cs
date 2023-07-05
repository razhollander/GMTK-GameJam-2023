using System.Collections.Generic;

namespace CoreDomain.Scripts.Extensions
{
    public static class IntExtensions
    {
        private const int Ten = 10;
        
        public static bool IsInRange(this int numberToCheck, int rangeStart, int rangeEnd)
        {
            return rangeStart <= numberToCheck && numberToCheck <= rangeEnd;
        }
        
        /// <summary>
        /// for example: get number = 123 return [1,2,3]
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int[] GetDigits(this int number)
        {
            var numbers = new Stack<int>();

            while(number > 0)
            {
                numbers.Push(number % Ten);
                number /= Ten;
            }

            return numbers.ToArray();
        }
    }
}