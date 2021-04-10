using System;
using System.Collections.Generic;

namespace knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup variables
            var nrOfItems = 10;
            var weightLimit = 1500;
            var lowerLimit = 0;
            var upperLimit = 1000;
            var mutationRate = 0.05;
            var populationSize = 100;
            var crossOverRate = 0.5;
                       
            Knapsack.Initialize(weightLimit, nrOfItems);
            ItemList.Initialize(lowerLimit, upperLimit);
            
            //create some random items && show
            var items = ItemList.generateItems(nrOfItems);
            logConditions(items, weightLimit);

            //start process
            var solver = new Solver(items, populationSize, crossOverRate);
            solver.Solve();
        }

        private static void logConditions(Item[] items, int weightLimit)
        {
            Console.WriteLine("weightLimit: {0}", weightLimit );
            Console.WriteLine("you can choose between these items:");
            foreach (Item item in items) {
                Console.WriteLine("weight: {0}; value: {1}", item.weight, item.value);
            }
            Console.WriteLine();
        }
    }
}
