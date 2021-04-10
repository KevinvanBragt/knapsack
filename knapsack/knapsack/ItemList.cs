using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    public static class ItemList
    {
        private static int LowerLimit = 0;
        private static int UpperLimit = 1000;
        
        public static void Initialize(int lowerLimit, int upperLimit)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public static Item[] generateItems(int count)
        {
            var randomNumberGenerator = new Random();
            var items = new Item[count];
            for (int i = 0; i < count; i++)
            {
                items[i] = new Item
                {
                    value = randomNumberGenerator.Next(LowerLimit, UpperLimit) * 5,
                    weight = randomNumberGenerator.Next(LowerLimit, UpperLimit)
                };
            }

            return items;
        }
    }
}
