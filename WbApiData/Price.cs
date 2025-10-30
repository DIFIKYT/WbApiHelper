using Newtonsoft.Json;

namespace WbApiData
{
    public class Price(int articule, string sellerArticule, int value, double discountedValue)
    {
        public int Articule { get; init; } = articule;
        public string SellerArticule { get; init; } = sellerArticule;
        public int Value { get; init; } = value;
        public double DiscountedValue { get; init; } = discountedValue;
    }
}