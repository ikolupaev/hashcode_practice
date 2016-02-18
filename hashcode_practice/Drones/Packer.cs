using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hashcode_practice;

namespace Drones
{
    public class Packer
    {
        public static IEnumerable<Product> PackProducts(IEnumerable<Product> products, int capacity)
        {
            var denormalizedProducts = DenormalizeProducts(products).ToArray();
            var seq = new bool[denormalizedProducts.Count()];
            bool[] bestSeq = null;
            int bestWeight = 0;

            while ( GetNextSequence(seq) )
            {
                var weight = seq.Select((x, i) => x ? DronesData.ProductWeights[denormalizedProducts[i]] : 0).Sum();

                if( weight == capacity )
                {
                    return Normalize(denormalizedProducts, seq);
                }
                else if( weight < capacity && weight > bestWeight )
                {
                    bestSeq = (bool[]) seq.Clone();
                    bestWeight = weight;
                }
            }

            return Normalize(denormalizedProducts, bestSeq);
        }

        public static IEnumerable<Product> Normalize(int[] denormalizedProducts, bool[] seq)
        {
            var products = new List<int>();

            for(int i = 0; i< seq.Length;i++)
            {
                if (seq[i]) products.Add(denormalizedProducts[i]);
            }

            return from x in products
                   group x by x into g
                   select new Product(g.Key, g.Count());
                   
        }

        public static bool GetNextSequence(bool[] seq)
        {
            for( int i = 0; i< seq.Length; i++ )
            {
                if (seq[i])
                {
                    seq[i] = false;
                }
                else
                {
                    seq[i] = true;
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<int> DenormalizeProducts(IEnumerable<Product> products)
        {
            foreach (var p in products)
            {
                for (int i = 0; i < p.Quantity; i++)
                {
                    yield return p.ProductType;
                }
            }
        }
    }
}
