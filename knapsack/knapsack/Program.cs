using GeneticAlgorithm;

namespace knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup variables
            var nrOfItems = 100;
            var weightLimit = 1000;
            var lowerLimit = 0;
            var upperLimit = 300;
            var mutationRate = 0.5;
            var populationSize = 2500;
            var crossOverRate = 0.5;

            Knapsack.Initialize(weightLimit, nrOfItems);
            ItemList.Initialize(lowerLimit, upperLimit, nrOfItems);

            IGeneticAlgorithmImpl solver = new Solver(ItemList.Items, populationSize, crossOverRate, mutationRate);
            solver.Run();
        }
    }
}
