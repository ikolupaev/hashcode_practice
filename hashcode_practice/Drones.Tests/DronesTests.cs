using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using hashcode_practice;
using System.Linq;

namespace Drones.Tests
{
    [TestClass]
    public class DronesTests
    {
        [TestMethod]
        public void DenormalizeProducts_Should_Works_Fine()
        {
            var products = Packer.DenormalizeProducts(new[] { new Product(10, 1), new Product(11, 2) }).ToArray();

            Assert.IsTrue(products.Count() == 3);
            Assert.AreEqual(10,products[0]);
            Assert.AreEqual(11,products[2]);
        }

        [TestMethod]
        public void PackProducts_Should_Works_Fine_If_Exact()
        {
            DronesData.ProductWeights = new int[100];
            DronesData.ProductWeights[1] = 90;
            DronesData.ProductWeights[2] = 40;
            DronesData.ProductWeights[3] = 20;

            var products = Packer.PackProducts(new[] { new Product(1, 1), new Product(2, 2), new Product(3, 2) }, 100);

            Assert.IsTrue(products.Any(x => x.ProductType == 2 && x.Quantity == 2));
            Assert.IsTrue(products.Any(x => x.ProductType == 3 && x.Quantity == 1));
        }

        [TestMethod]
        public void PackProducts_Should_Works_Fine_If_No_Exact()
        {
            DronesData.ProductWeights = new int[100];
            DronesData.ProductWeights[1] = 90;
            DronesData.ProductWeights[2] = 40;
            DronesData.ProductWeights[3] = 6;

            var products = Packer.PackProducts(new[] { new Product(1, 1), new Product(2, 2), new Product(3, 2) }, 99);

            Assert.IsTrue(products.Any(x => x.ProductType == 1 && x.Quantity == 1));
            Assert.IsTrue(products.Any(x => x.ProductType == 3 && x.Quantity == 1));
        }

        [TestMethod]
        public void GetNextSequence_Should_Works_Fine()
        {
            var seq = new[] { false, false, false };

            Packer.GetNextSequence(seq);

            Assert.IsTrue(seq[0]);
            Assert.IsFalse(seq[1]);
            Assert.IsFalse(seq[2]);

            Packer.GetNextSequence(seq);

            Assert.IsFalse(seq[0]);
            Assert.IsTrue(seq[1]);
            Assert.IsFalse(seq[2]);
        }

        [TestMethod]
        public void GetNextSequence_Should_Returns_False_On_Last_Sequence()
        {
            var seq = new[] { true, true, true };

            var next = Packer.GetNextSequence(seq);

            Assert.IsFalse(next);
        }

        [TestMethod]
        public void GetNextSequence_Should_Returns_True_On_Middle_Sequence()
        {
            var seq = new[] { false, true, true };

            var next = Packer.GetNextSequence(seq);

            Assert.IsTrue(next);
        }
    }
}
