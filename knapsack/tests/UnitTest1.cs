using knapsack;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace tests
{
    public class Tests
    {
        private Item[] items;
        private List<Knapsack> generation;

        [SetUp]
        public void Setup()
        {
            //setup variables
            var nrOfItems = 5;
            var weightLimit = 1500;
            var lowerLimit = 0;
            var upperLimit = 1000;
            var mutationRate = 0.05;
            var populationSize = 100;
            var crossOverRate = 0.5;

            Knapsack.Initialize(weightLimit, nrOfItems);
            ItemList.Initialize(lowerLimit, upperLimit);
            items = ItemList.generateRandomItems(nrOfItems);
        }

        [Test]
        public void crossOverFunction_shouldCrossGenesCorrectly()
        {
            //setup
            var parentA = new Knapsack(items, true);
            var parentB = new Knapsack(items, true);

            foreach (Item item in parentA.Genomes)
            {
                item.included = true;
            }

            foreach (Item item in parentB.Genomes)
            {
                item.included = false;
            }

            //act
            //expected = A { 11100 }; B { 00011 };
            var children = Solver.crossOver(parentA, parentB, 2);
            var childContentA = children[0].Genomes;
            var childContentB = children[1].Genomes;
            var childAHasItemsIncluded = childContentA.Any(i => i.included);
            var childBHasItemsNotIncluded = childContentB.Any(i => !i.included);

            //assert
            Assert.IsTrue(childAHasItemsIncluded && childBHasItemsNotIncluded);
            Assert.IsTrue(childContentA[0].included && childContentA[1].included && childContentA[2].included && !childContentA[3].included && !childContentA[4].included);
        }
    }
}