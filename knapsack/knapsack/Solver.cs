using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace knapsack
{
    public class Solver : IGeneticAlgorithm
    {
        private int PopulationSize;
        private int geneCount;
        private double CrossOverRate;
        private double MutationRate;

        private RandomHelper randomHelper;
        public List<ICandidate> currentGeneration = new List<ICandidate>();
        public List<ICandidate> nextGeneration = new List<ICandidate>();
        public int generationCount = 0;
        public Item[] Items;

        public Solver(Item[] items, int populationSize, double crossOverRate, double mutationRate)
        {
            PopulationSize = populationSize;
            CrossOverRate = crossOverRate;
            MutationRate = mutationRate;
            randomHelper = RandomHelper.GetInstance();
            Items = items;
            geneCount = items.Length;
        }
        public void Run()
        {
            CreateInitialPopulation();
            Console.WriteLine("best of start:");
            var bestOfStart = getBestOfCurrentGeneration();
            (bestOfStart as Knapsack).LogKnapsack();

            while (generationCount < 1000) { //todo: track improvement instead
                CreateNextGeneration();
                generationCount++;
            }

            Console.WriteLine("solution:");
            var solution = getBestOfCurrentGeneration();
            (solution as Knapsack).LogKnapsack();           
        }

        private ICandidate getBestOfCurrentGeneration()
        {
            var maxValue = currentGeneration.Max(k => k.GetFitness());
            var solution = currentGeneration.FirstOrDefault(k => k.GetFitness() == maxValue);
            return solution;
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
        }

        public ICandidate SelectOne()
        {
            //roulette wheel selection
            //todo: hoe zit dit met dubbele values?
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

        public ICandidate[] CrossOver(ICandidate parentA, ICandidate parentB, int crossOverPoint)
        {
            //true true, false , false, true
            var piA = parentA.GetGenomes();
            var piB = parentB.GetGenomes();
                       
            var childAItems = piA.Substring(0, crossOverPoint) + piB.Substring(crossOverPoint, geneCount-crossOverPoint);
            var childBItems = piB.Substring(0, crossOverPoint) + piA.Substring(crossOverPoint, geneCount-crossOverPoint);

            var childA = new Knapsack(childAItems);
            var childB = new Knapsack(childBItems);

            return new Knapsack[] { childA, childB };
        }

        public bool IsTerminateCondition()
        {
            return generationCount >= 100;
        }
    }
}
