using System;
using System.Linq;
using GeneticAlgorithm;

namespace knapsack
{
    /// <summary>
    /// indicates a possible solution (/genome)
    /// </summary>
    public class Knapsack : ICandidate
    {
        private int[] Genomes = null;
        protected static int WeightLimit;
        protected static int GeneCount;
        protected static RandomHelper RandomHelper;
        private int? Fitness = null;

        public static void Initialize(int weightLimit, int geneCount)
        {
            WeightLimit = weightLimit;
            GeneCount = geneCount;
            RandomHelper = RandomHelper.GetInstance();
        }

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
                    Genomes[i] = RandomHelper.GetRandomInt(0, 2);
                }
            }
        }
        
        public string GetGenomes()
        {
            var genome = "";
            foreach (int x in Genomes)
            {
                genome += x;
            }
            return genome;
        }
        
        public void LogCandidate()
        {
            Console.WriteLine("genomes: {0} & fitness: {1}\n", this.GetGenomes(), this.GetFitness());
        }
        
        public void Mutate(double mutationRate)
        {
            var probability = RandomHelper.GetProbability();
            if (probability < mutationRate)
            {
                var geneIndex = RandomHelper.GetRandomInt(0, Genomes.Length - 1);
                Genomes[geneIndex] = Genomes[geneIndex] == 0 ? 1 : 0;
            }
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
        
        public void EnsureCandidateIsValid()
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
        
        protected int CalculateFitness()
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