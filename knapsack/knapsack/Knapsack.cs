using System;
using System.Collections.Generic;
using System.Linq;

namespace knapsack
{
    /// <summary>
    /// indicates a possible solution (/genome)
    /// </summary>
    public class Knapsack
    {
        private static int WeightLimit;
        private static int GeneCount;
        private static RandomHelper RandomHelper;

        public Item[] content { get; private set; }
        private int weight => content.Sum(i => i.included ? i.weight : 0);
        public int Fitness { get; private set; }

        public static void Initialize(int weightLimit, int geneCount)
        {
            WeightLimit = weightLimit;
            GeneCount = geneCount;
            RandomHelper = RandomHelper.GetInstance();
        }

        public Knapsack(Item[] items, bool isGenerationZero)
        {
            content = items.Clone() as Item[];
            if (isGenerationZero)
            {
                foreach (Item item in content)
                {
                    item.included = RandomHelper.GetRandomBool();
                }
            }
            calculateFitness();
        }

        private void calculateFitness()
        {
            ensureSolutionIsValid();
            this.Fitness = content.Sum(i => i.included ? i.value : 0);
        }

        private void ensureSolutionIsValid()
        {
            var excessWeight = weight - WeightLimit;
            while (excessWeight > 0)
            {
                var randomNumber = RandomHelper.GetRandomInt(0, GeneCount - 1);
                var item = content[randomNumber];
                if (item.included)
                {
                    excessWeight -= item.weight;
                    item.included = false;
                }
            }
        }

        public void LogKnapsack()
        {
            //todo hier een logger voor maken
            foreach (Item item in content)
            {
                if (item.included)
                {
                    Console.Write("item value {0}, item weight {1}\n", item.value, item.weight);
                }
            }
            Console.WriteLine("fitness: {0}\n", Fitness);


        }
    }
}
