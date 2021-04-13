using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    public static class ItemList
    {
        public static Item[] Items { get; private set; }
        private static int LowerLimit = 0;
        private static int UpperLimit = 1000;

        public static int itemValue(int index) => Items[index].value;
        public static int itemWeight(int index) => Items[index].weight;

        public static void Initialize(int lowerLimit, int upperLimit, int geneCount)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            generateRandomItems(geneCount);
            logConditions();
        }

        private static void generateRandomItems(int geneCount)
        {
            var randomNumberGenerator = new Random();
            var items = new Item[geneCount];
            for (int i = 0; i < geneCount; i++)
            {
                items[i] = new Item
                {
                    value = randomNumberGenerator.Next(LowerLimit, UpperLimit) * 5,
                    weight = randomNumberGenerator.Next(LowerLimit, UpperLimit)
                };
            }

            Items = items;
        }

        private static void logConditions()
        {
            Console.WriteLine("you can choose between these items:");
            foreach (Item item in Items)
            {
                Console.WriteLine("weight: {0}; value: {1}", item.weight, item.value);
            }
            Console.WriteLine();
        }
    }

    public class Item
    {
        public int weight;
        public int value;
    }
}
