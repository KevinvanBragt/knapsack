using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    public interface ICandidate
    {
        abstract void Mutate(double mutationRate);

        /// <summary>
        /// returns the fitness value for the candidate
        /// </summary>
        abstract int GetFitness();

        abstract void EnsureCandidateIsValid();

        abstract string GetGenomes();
    }
}
