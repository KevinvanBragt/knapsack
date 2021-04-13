using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    interface IGeneticAlgorithm
    {
        abstract void Run();
        abstract ICandidate[] CrossOver(ICandidate parentA, ICandidate parentB, int crossOverPoint);
        abstract void CreateInitialPopulation();
        abstract void CreateNextGeneration();

        /// <summary>
        /// indicates whether the program should terminate
        /// </summary>
        /// <returns></returns>
        abstract bool IsTerminateCondition();
    }
}