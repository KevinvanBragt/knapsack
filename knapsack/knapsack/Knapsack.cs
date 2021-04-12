using System;
using System.Collections.Generic;
using System.Linq;

namespace knapsack
{

    public interface ICandidate
    {
        public int Fitness()
        {
            EnsureCandidateIsValid();
            return CalculateFitness();
        }
        abstract void EnsureCandidateIsValid();
        abstract int CalculateFitness();

        abstract void mutate();
    }

    /// <summary>
    /// indicates a possible solution (/genome)
    /// </summary>
    public class Knapsack : ICandidate
    {
        private static int WeightLimit;
        private static int GeneCount;
        private static RandomHelper RandomHelper;
        public string[] Genomes = null;
        public int Fitness;

        public static void Initialize(int weightLimit, int geneCount)
        {
            WeightLimit = weightLimit;
            GeneCount = geneCount;
            RandomHelper = RandomHelper.GetInstance();
        }

        public Knapsack(string[] genomes = null)
        {
            if (genomes != null)
            {
                Genomes = genomes;
            } 
            else
            {
                genomes = new string[GeneCount];
                for (int i=0; i< GeneCount; i++)
                {
                    genomes[i] = RandomHelper.GetRandomBool();
                }
            }

            this.Fitness = ((ICandidate)this).Fitness();
        }

        public int CalculateFitness()
        {
            var fitness = 0;
            for (int i=0; i< Genomes.Length; i++)
            {
                fitness += Genomes[i] == "1" ? ItemList.itemValue(i) : 0;
            }
            return fitness;
        }

        public void EnsureCandidateIsValid()
        {
            var excessWeight = this.weight() - WeightLimit;
            while (excessWeight > 0)
            {
                var randomNumber = RandomHelper.GetRandomInt(1, GeneCount);
                if (Genomes[randomNumber] == "1")
                {
                    excessWeight -= ItemList.itemWeight(randomNumber);
                    Genomes[randomNumber] = "0";
                }
            }
        }

        private int weight()
        {
            var weight = 0;
            for (int i = 0; i < Genomes.Length; i++)
            {
                weight += Genomes[i] == "1" ? ItemList.itemWeight(i) : 0;
            }

            return weight;
        }

        public void LogKnapsack()
        {
            foreach (string item in Genomes)
            {
                    Console.Write(item);
            }
            Console.WriteLine("fitness: {0}\n", Fitness);
        }
    }
}
