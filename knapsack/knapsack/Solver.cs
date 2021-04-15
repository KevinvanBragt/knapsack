using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticAlgorithm;

namespace knapsack
{
    public class Solver : IGeneticAlgorithmImpl
    {
        private RandomHelper randomHelper;
        public Item[] Items;
        public int generationCount = 0;

        private int PopulationSize;
        private int geneCount;
        private double CrossOverRate;
        private double MutationRate;

        private int solutionFitness = 0;
        private string solutionGenomes;

        public List<ICandidate> currentGeneration { get; private set; } = new List<ICandidate>();
        public List<ICandidate> nextGeneration { get; private set; } = new List<ICandidate>();

        public Solver(Item[] items, int populationSize, double crossOverRate, double mutationRate)
        {
            PopulationSize = populationSize;
            CrossOverRate = crossOverRate;
            MutationRate = mutationRate;
            randomHelper = RandomHelper.GetInstance();
            Items = items;
            geneCount = items?.Length ?? 0;
        }

        public ICandidate GetBestCandidateOfCurrentGeneration(bool isFinal = false)
        {
            var maxValue = currentGeneration.Max(k => k.GetFitness());

            if (isFinal && this.solutionFitness > maxValue)
            {
                return new Knapsack(this.solutionGenomes);
            } else
            {
                return currentGeneration.FirstOrDefault(k => k.GetFitness() == maxValue);
            }
        }

        public void CreateInitialPopulation()
        {
            Console.WriteLine("created the following candidates");
            for (int i = 0; i < PopulationSize; i++)
            {
                currentGeneration.Add(new Knapsack());
            }
        }

        public void CreateNextGeneration()
        {
            nextGeneration = new List<ICandidate>(100);
            do
            {
                var parentA = SelectOne();
                var parentB = SelectOne();

                var probability = randomHelper.GetProbability();
                var children = new ICandidate[] { parentA, parentB };
                if (probability < CrossOverRate)
                {
                    var crossOverPoint = randomHelper.GetRandomInt(1, geneCount - 1);
                    children = CrossOver(parentA, parentB, crossOverPoint);
                }

                children[0].Mutate(MutationRate);
                children[1].Mutate(MutationRate);

                nextGeneration.AddRange(children);
            } while (nextGeneration.Count < PopulationSize);

            currentGeneration = nextGeneration;
            nextGeneration = null;
            generationCount++;
        }

        public ICandidate SelectOne()
        {
            //roulette wheel selection
            var totalFitnessScore = currentGeneration.Sum(g => g.GetFitness());
            var randomValue = randomHelper.GetRandomInt(0, totalFitnessScore);
            
            for (int i=0; i< PopulationSize; i++)
            {
                randomValue -= currentGeneration[i].GetFitness();
                if (randomValue <= 0)
                {
                    return currentGeneration[i];
                }
            }

            return currentGeneration[PopulationSize-1];
        }

        public ICandidate[] CrossOver(ICandidate parentA, ICandidate parentB, int crossOverIndex)
        {
            //true true, false , false, true
            var piA = parentA.GetGenomes();
            var piB = parentB.GetGenomes();
                       
            var childAItems = piA.Substring(0, crossOverIndex) + piB.Substring(crossOverIndex, geneCount-crossOverIndex);
            var childBItems = piB.Substring(0, crossOverIndex) + piA.Substring(crossOverIndex, geneCount-crossOverIndex);

            var childA = new Knapsack(childAItems);
            var childB = new Knapsack(childBItems);

            return new Knapsack[] { childA, childB };
        }

        public bool IsTerminateCondition()
        {
            var bestOfGeneration = GetBestCandidateOfCurrentGeneration();

            if (this.solutionFitness < bestOfGeneration.GetFitness())
            {
                this.solutionFitness = bestOfGeneration.GetFitness();
                this.solutionGenomes = bestOfGeneration.GetGenomes();
                generationCount = 0;
            }

            return generationCount >= 25;
        }
    }
}
