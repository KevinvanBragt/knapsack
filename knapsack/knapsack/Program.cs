using System;
using System.Collections.Generic;

namespace knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup variables
            var nrOfItems = 20;
            var weightLimit = 1000;
            var lowerLimit = 0;
            var upperLimit = 300;
            var mutationRate = 0.25;
            var populationSize = 1000;
            var crossOverRate = 0.5;

            Knapsack.Initialize(weightLimit, nrOfItems);
            ItemList.Initialize(lowerLimit, upperLimit, nrOfItems);

            IGeneticAlgorithm solver = new Solver(ItemList.Items, populationSize, crossOverRate, mutationRate);
            solver.Run();
        }
    }
}
