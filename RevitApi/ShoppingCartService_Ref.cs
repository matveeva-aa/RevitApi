using System;
using System.Collections.Generic;

namespace Task_01_1
{
    public class ShoppingCartService_Ref
    {
        private const decimal RegularDiscountRate = 0.05m;

        public decimal CalculateTotalPrice(string customerType, List<decimal> itemPrices)
        {

            decimal baseTotal = CalculateBaseTotal(itemPrices);
            return CalculateFinalPriceForRegular(baseTotal);
        }

        public decimal CalculateTotalPriceWithQuantities(string customerType, Dictionary<decimal, int> itemsWithQuantities)
        {

            decimal baseTotal = CalculateBaseTotal(itemsWithQuantities);
            return CalculateFinalPriceForRegular(baseTotal);
        }

        private static decimal CalculateBaseTotal(List<decimal> itemPrices)
        {
            decimal baseTotal = 0;
            for (int i = 0; i < itemPrices.Count; i++)
            {
                baseTotal += itemPrices[i];
            }
            return baseTotal;
        }

        private static decimal CalculateBaseTotal(Dictionary<decimal, int> itemsWithQuantities)
        {
            decimal baseTotal = 0;
            foreach (var item in itemsWithQuantities)
            {
                baseTotal += item.Key * item.Value;
            }
            return baseTotal;
        }
        private decimal CalculateFinalPriceForRegular(decimal baseTotal)
        {
            decimal discount = baseTotal * RegularDiscountRate;
            decimal finalPrice = baseTotal - discount;

            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");

            return finalPrice;
        }
    }
}
    
