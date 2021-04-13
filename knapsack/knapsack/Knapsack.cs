using System;
using System.Collections.Generic;
using System.Linq;

namespace knapsack
{
    public abstract class Candidate : ICandidate
    {
        protected static int WeightLimit;
        protected static int GeneCount;
        protected static RandomHelper RandomHelper;
        private int? Fitness = null;

        /// <summary>
        /// initialize Candidate static fields
        /// </summary>
        /// <param name="weightLimit"></param>
        /// <param name="geneCount"></param>
        public static void Initialize(int weightLimit, int geneCount)
        {
            WeightLimit = weightLimit;
            GeneCount = geneCount;
            RandomHelper = RandomHelper.GetInstance();
        }

        public int GetFitness()
        {
            if (this.Fitness.HasValue)
            {
                return this.Fitness.Value;
            }
            else
            {
                this.EnsureCandidateIsValid();
                this.Fitness = this.CalculateFitness();
                return this.Fitness.Value;
            }
        }

        public abstract string GetGenomes();

        /// <summary>
        /// mutates the candidate given a probability
        /// </summary>
        public abstract void Mutate(double mutationRate);

        /// <summary>
        /// deals with invalid candidates
        /// </summary>
        public abstract void EnsureCandidateIsValid();

        /// <summary>
        /// calculates the fitness value
        /// </summary>
        /// <returns></returns>
        protected abstract int CalculateFitness();
    }

    /// <summary>
    /// indicates a possible solution (/genome)
    /// </summary>
    public class Knapsack : Candidate
    {
        private int[] Genomes = null;

        //takes a string so deep cloning is not necessary
        public Knapsack(string genomes = null)
        {
            if (!string.IsNullOrEmpty(genomes))
            {
                Genomes = new int[GeneCount];
                for (int x=0; x<genomes.Length; x++)
                {
                    Genomes[x] = int.Parse(genomes.Substring(x, 1));
                }
            } 
            else
            {
                Genomes = new int[GeneCount];
                for (int i=0; i<GeneCount; i++)
                {
                    Genomes[i] = RandomHelper.GetRandomBool();
                }
            }
        }
        public override string GetGenomes()
        {
            var genome = "";
            foreach (int x in Genomes)
            {
                genome += x;
            }
            return genome;
        }
        public void LogKnapsack()
        {
            Console.WriteLine("genomes: {0} & fitness: {1}\n", this.Genomes, this.GetFitness());
        }
        public override void Mutate(double mutationRate)
        {
            var probability = RandomHelper.GetProbability();
            if (probability < mutationRate)
            {
                var geneIndex = RandomHelper.GetRandomInt(0, Genomes.Length - 1);
                Genomes[geneIndex] = Genomes[geneIndex] == 0 ? 1 : 0;
            }
        }
        public override void EnsureCandidateIsValid()
        {
            var excessWeight = this.weight() - WeightLimit;
            while (excessWeight > 0)
            {
                var randomNumber = RandomHelper.GetRandomInt(1, GeneCount);
                if (Genomes.ElementAt(randomNumber).Equals("1"))
                {
                    excessWeight -= ItemList.itemWeight(randomNumber);
                    Genomes[randomNumber] = '0';
                }
            }
        }
        protected override int CalculateFitness()
        {
            var fitness = 0;
            for (int i = 0; i < Genomes.Length; i++)
            {
                fitness += Genomes[i] == 1 ? ItemList.itemValue(i) : 0;
            }
            return fitness;
        }
        private int weight()
        {
            var weight = 0;
            for (int i = 0; i < Genomes.Length; i++)
            {
                weight += Genomes[i] == '1' ? ItemList.itemWeight(i) : 0;
            }

            return weight;
        }
    }
}