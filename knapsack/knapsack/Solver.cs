using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace knapsack
{
    public class Solver
    {
        private static Item[] Items;
        private static RandomHelper randomHelper;
        private static int PopulationSize;
        private static int geneCount;
        private static double CrossOverRate;
        public static List<Knapsack> currentGeneration = new List<Knapsack>();
        public static List<Knapsack> nextGeneration = new List<Knapsack>();
        public int generationCount = 0;

        public Solver(Item[] items, int populationSize, double crossOverRate)
        {
            Items = items;
            geneCount = items.Length;
            PopulationSize = populationSize;
            CrossOverRate = crossOverRate;
            randomHelper = RandomHelper.GetInstance();
        }
        public void Solve()
        {
            createInitialPopulation();
            selectNextGeneration();
            
            while (generationCount < 100) { //todo: track improvement instead
                crossOver();
                mutate();
                currentGeneration = nextGeneration;
                selectNextGeneration();
                generationCount++;
            }

            Console.WriteLine("solution:");
            var solution = getBestOfCurrentGeneration();
            solution.LogKnapsack();           
        }

        private Knapsack getBestOfCurrentGeneration()
        {
            var maxValue = currentGeneration.Max(k => k.Fitness);
            var solution = currentGeneration.FirstOrDefault(k => k.Fitness == maxValue);
            return solution;
        }

        private void createInitialPopulation()
        {
            Console.WriteLine("created the following candidates");
            for (int i = 0; i < PopulationSize; i++)
            {
                var newCandidate = createCandidateSolution();
                currentGeneration.Add(newCandidate);
                newCandidate.LogKnapsack();
            }
        }

        private Knapsack createCandidateSolution()
        {
            return new Knapsack(Items, true);
        }

        private void selectNextGeneration()
        {
            nextGeneration = new List<Knapsack>(PopulationSize);
            for (int i=0; i < PopulationSize; i++)
            {
                nextGeneration.Add(selectOne());
            }
        }

        private Knapsack selectOne()
        {
            //roulette wheel selection
            //todo: hoe zit dit met dubbele values?
            var totalFitnessScore = currentGeneration.Sum(g => g.Fitness);
            var randomValue = randomHelper.GetRandomInt(0, totalFitnessScore);
            
            for (int i=0; i< PopulationSize; i++)
            {
                randomValue -= currentGeneration[i].Fitness;
                if (randomValue <= 0)
                {
                    return currentGeneration[i];
                }
            }

            return currentGeneration[PopulationSize-1];
        }

        public void crossOver()
        {
            var probability = randomHelper.GetProbability();

            Knapsack parentA;
            Knapsack parentB;
            for (int i=0; i<PopulationSize-2; i += 2)
            {
                parentA = nextGeneration[i];
                parentB = nextGeneration[i+1];

                if (probability < CrossOverRate)
                {
                    performCrossOver(parentA, parentB);
                }
            }
        } 

        private void performCrossOver(Knapsack parentA, Knapsack parentB)
        {
            var crossOverPoint = randomHelper.GetRandomInt(0, geneCount - 1);

        }

        private void mutate()
        {

        }

    }
}
