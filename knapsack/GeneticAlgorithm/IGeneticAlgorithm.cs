using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public interface IGeneticAlgorithmImpl
    {
        /// <summary>
        /// runs the loop of creating new selection, crossover, mutation until termination conditions have been met
        /// </summary>
        /// <returns></returns>
        void Run() {
            CreateInitialPopulation();

            Console.WriteLine("best of start:");
            var bestOfStart = GetBestCandidateOfCurrentGeneration();
            bestOfStart.LogCandidate();

            while (!IsTerminateCondition())
            { 
                CreateNextGeneration();
            }

            Console.WriteLine("solution:");
            var solution = GetBestCandidateOfCurrentGeneration(true);
            solution.LogCandidate();
        }

        /// <summary>
        /// crossover function switching genes
        /// </summary>
        /// <returns></returns>
        abstract ICandidate[] CrossOver(ICandidate parentA, ICandidate parentB, int crossOverPoint);

        /// <summary>
        /// creates starting population
        /// </summary>
        /// <returns></returns>
        abstract void CreateInitialPopulation();

        /// <summary>
        /// creates next generation
        /// </summary>
        /// <returns></returns>
        abstract void CreateNextGeneration();

        /// <summary>
        /// indicates whether the program should terminate
        /// </summary>
        /// <returns></returns>
        abstract bool IsTerminateCondition();

        /// <summary>
        /// returns the best <see cref="ICandidate"/> of the current generation
        /// </summary>
        /// <returns></returns>
        abstract ICandidate GetBestCandidateOfCurrentGeneration(bool isFinal = false);
    }
}