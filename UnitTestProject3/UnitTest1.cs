using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//Item   Unit      Special
//         Price     Price
//  --------------------------
//    A     50       3 for 130
//    B     30       2 for 45
//    C     20
//    D     15

namespace UnitTestProject3
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IDictionary<string, int> _prices = new Dictionary<string, int>
            {
                {"A", 50},
                {"B", 30}
            };

        private readonly IList<Offer> _offers = new List<Offer>
            {
                new Offer("A", 3, 20)
            };

        [TestMethod]
        public void One_A_should_be_50()
        {
            var checkout = new Checkout(_prices, _offers);
            checkout.Scan("A");
            var actual = checkout.Total();
            Assert.AreEqual(50, actual);
        }

        [TestMethod]
        public void One_B_should_be_30()
        {
            var checkout = new Checkout(_prices, _offers);
            checkout.Scan("B");
            var actual = checkout.Total();
            Assert.AreEqual(30, actual);
        }

        [TestMethod]
        public void Two_A_should_be_100()
        {
            var checkout = new Checkout(_prices, _offers);
            checkout.Scan("A");
            checkout.Scan("A");
            var actual = checkout.Total();
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public void Three_A_should_be_130()
        {

            var checkout = new Checkout(_prices, _offers);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            var actual = checkout.Total();
            Assert.AreEqual(130, actual);
        }
    }

    public class Offer
    {
        private readonly string _item;
        private readonly int _offerAmount;
        private readonly int _discount;

        public Offer(string item, int offerAmount, int discount)
        {
            _item = item;
            _offerAmount = offerAmount;
            _discount = discount;
        }

        private bool OfferApplies(string item, int currentAmount)
        {
            return item == _item && currentAmount >= _offerAmount;
        }


        public int CalculateDiscount(string item, int currentAmount)
        {
            return OfferApplies(item, currentAmount) ? _discount : 0;
        }
    }

    public class Checkout
    {
        private readonly IDictionary<string, int> _prices;
        private readonly IList<Offer> _offers;
        private readonly IDictionary<string, int> _bag = new Dictionary<string, int>();

        public Checkout(IDictionary<string, int> prices, IList<Offer> offers)
        {
            _prices = prices;
            _offers = offers;
        }

        public int Total()
        {
            var total = 0;
            foreach (var item in _bag)
            {
                total += item.Value * _prices[item.Key];
                
                foreach (var offer in _offers)
                {
                    total -= offer.CalculateDiscount(item.Key, item.Value);
                }
            }


            return total;
        }

        public void Scan(string item)
        {
            if (_bag.ContainsKey(item))
            {
                _bag[item] += 1;
            }
            else
            {
                _bag.Add(item, 1);
            }
        }
    }
}
