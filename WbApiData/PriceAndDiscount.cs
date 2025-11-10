using Newtonsoft.Json;

namespace WbApiData
{
    public class PriceAndDiscount(int articule, string sellerArticule, int price, int discount, double discountedValue)
    {
        public int Articule { get; init; } = articule;
        public string SellerArticule { get; init; } = sellerArticule;
        public int Price { get; init; } = price;
        public int Discount { get; init; } = discount;
        public double DiscountedValue { get; init; } = discountedValue;
    }
}