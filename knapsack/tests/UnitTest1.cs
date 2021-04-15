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
        private Solver solver;

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
            ItemList.Initialize(lowerLimit, upperLimit, nrOfItems);
            solver = new Solver(ItemList.Items, populationSize, crossOverRate, mutationRate);
        }

        [Test]
        public void crossOverFunction_shouldCrossGenesCorrectly()
        {
            //setup
            var parentA = new Knapsack("00000");
            var parentB = new Knapsack("11111");

            //act
            var children = solver.CrossOver(parentA, parentB, 2);

            //assert
            //expected = A { 11100 }; B { 00011 };
            Assert.AreEqual(children[0].GetGenomes(), "00111");
            Assert.AreEqual(children[1].GetGenomes(), "11000");
        }

        [Test]
        public void mutateFuntion_shouldSwitchBoolean()
        {
            var item = new Knapsack("00011");
            item.Mutate(1.0);

            Assert.AreNotEqual("00011", item.GetGenomes());
        }
    }
}