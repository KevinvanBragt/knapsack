using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public interface ICandidate
    {
        /// <summary>
        /// mutates the candidate
        /// </summary>
        abstract void Mutate(double mutationRate);

        /// <summary>
        /// returns the fitness value for the candidate
        /// </summary>
        abstract int GetFitness();

        /// <summary>
        /// ensures candidate does not exceed limitations
        /// </summary>
        abstract void EnsureCandidateIsValid();

        /// <summary>
        /// returns a string representation of genes
        /// </summary>
        abstract string GetGenomes();

        /// <summary>
        /// optional method to log the <see cref="ICandidate"/>
        /// </summary>
        void LogCandidate() { }
    }
}
