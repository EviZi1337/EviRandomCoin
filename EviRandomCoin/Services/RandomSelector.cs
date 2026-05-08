using System;
using System.Collections.Generic;

namespace EviRandomCoin.Services
{
    public static class RandomSelector
    {
        public static T Pick<T>(IEnumerable<T> source, Func<T, double> weightSelector, Random random)
        {
            if (source == null || weightSelector == null || random == null)
                return default(T);

            double totalWeight = 0d;
            List<T> items = new List<T>();
            List<double> weights = new List<double>();

            foreach (T item in source)
            {
                double weight = weightSelector(item);
                if (weight <= 0d)
                    continue;

                items.Add(item);
                weights.Add(weight);
                totalWeight += weight;
            }

            if (items.Count == 0 || totalWeight <= 0d)
                return default(T);

            double roll = random.NextDouble() * totalWeight;
            double cursor = 0d;

            for (int i = 0; i < items.Count; i++)
            {
                cursor += weights[i];
                if (roll <= cursor)
                    return items[i];
            }

            return items[items.Count - 1];
        }
    }
}
