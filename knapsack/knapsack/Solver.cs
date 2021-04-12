using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace knapsack
{
    public class Solver
    {
        private static RandomHelper randomHelper;
        private static int PopulationSize;
        private static int geneCount;
        private static double CrossOverRate;
        private static double MutationRate;
        public static List<Knapsack> currentGeneration = new List<Knapsack>();
        public static List<Knapsack> nextGeneration = new List<Knapsack>();
        public int generationCount = 0;

        public Solver(int geneCount, int populationSize, double crossOverRate, double mutationRate)
        {
            PopulationSize = populationSize;
            CrossOverRate = crossOverRate;
            MutationRate = mutationRate;
            randomHelper = RandomHelper.GetInstance();
        }
        public void Solve()
        {
            createInitialPopulation();
            Console.WriteLine("best of start:");
            var bestOfStart = getBestOfCurrentGeneration();
            bestOfStart.LogKnapsack();

            while (generationCount < 100) { //todo: track improvement instead
                createNextGeneration();
                generationCount++;
            }

            Console.WriteLine("solution:");
            var solution = getBestOfCurrentGeneration();
            solution.LogKnapsack();           
        }

        public Knapsack getBestOfCurrentGeneration()
        {
            var maxValue = currentGeneration.Max(k => k.Fitness);
            var solution = currentGeneration.FirstOrDefault(k => k.Fitness == maxValue);
            return solution;
        }

        public void createInitialPopulation()
        {
            Console.WriteLine("created the following candidates");
            for (int i = 0; i < PopulationSize; i++)
            {
                currentGeneration.Add(new Knapsack());
            }
        }

        public void createNextGeneration()
        {
            nextGeneration = new List<Knapsack>(100);
            do
            {
                var parentA = selectOne();
                var parentB = selectOne();

                var probability = randomHelper.GetProbability();
                var children = new Knapsack[] { parentA, parentB };
                if (probability < CrossOverRate)
                {
                    var crossOverPoint = randomHelper.GetRandomInt(0, geneCount - 1);
                    children = crossOver(parentA, parentB, crossOverPoint);
                }

                mutate(ref children);

                nextGeneration.AddRange(children);
            } while (nextGeneration.Count < PopulationSize);

            currentGeneration = nextGeneration;
            nextGeneration = null;
        }

        public Knapsack selectOne()
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

        public static Knapsack[] crossOver(Knapsack parentA, Knapsack parentB, int crossOverPoint)
        {
            //true true, false , false, true
            var piA = parentA.Genomes;
            var piB = parentB.Genomes;

            var childAItems = piA.Take(crossOverPoint)
                .Concat(piB.Skip(crossOverPoint).Take(geneCount - crossOverPoint))
                .ToArray();
            var childBItems = piB.Take(crossOverPoint)
                .Concat(piA.Skip(crossOverPoint).Take(geneCount - crossOverPoint))
                .ToArray();

            var childA = new Knapsack(childAItems);
            var childB = new Knapsack(childBItems);

            return new Knapsack[] { childA, childB };
        } 

        private void mutate(ref Knapsack[] children)
        {
            foreach(Knapsack child in children)
            {
                var probability = randomHelper.GetProbability();
                if (probability < MutationRate)
                {
                    var geneIndex = randomHelper.GetRandomInt(0, geneCount - 1);
                    child.Genomes[geneIndex] = child.Genomes[geneIndex] == "0" ? "1" : "0";
                }
            }
        }

    }
}
