using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    public class RandomHelper
    {
        private static RandomHelper instance;
        private Random random;
        private RandomHelper() { }
        public static RandomHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new RandomHelper();
                instance.random = new Random();
            }
            return instance;
        }

        public int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public double GetProbability()
        {
            return random.NextDouble();
        }
    }
}
