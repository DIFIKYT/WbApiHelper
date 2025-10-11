namespace WbApiData
{
    public class CompanyStat(int advertId, int viewsCount, int clicksCount, double clickConversion, double sum)
    {
        public int AdvertId { get; init; } = advertId;
        public int ViewsCount { get; init; } = viewsCount;
        public int ClicksCount { get; init; } = clicksCount;
        public double ClickConversion { get; init; } = clickConversion;
        public double Expenses { get; init; } = sum;
    }
}