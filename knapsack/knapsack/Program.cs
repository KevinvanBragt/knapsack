using System;
using System.Collections.Generic;

namespace knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup variables
            var nrOfItems = 5;
            var weightLimit = 1500;
            var lowerLimit = 0;
            var upperLimit = 1000;
            var mutationRate = 0.05;
            var populationSize = 100;
            var crossOverRate = 0.5;
                       
            Knapsack.Initialize(weightLimit, nrOfItems);
            ItemList.Initialize(lowerLimit, upperLimit, nrOfItems);
            var solver = new Solver(nrOfItems, populationSize, crossOverRate, mutationRate);

            //start process
            solver.Solve();
        }
    }
}
