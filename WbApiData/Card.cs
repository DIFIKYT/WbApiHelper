namespace WbApiData
{
    public class Card(int subjectId, string subjectName, string brand,
        int articule, string sellerArticule,
        string photoUrl,
        int length, int width, int height, double weight,
        string techSize, string barcode)
    {
        public int SubjectId { get; init; } = subjectId;
        public string SubjectName { get; init; } = subjectName;
        public string Brand { get; init; } = brand;

        public int Articule { get; init; } = articule;
        public string SellerArticule { get; init; } = sellerArticule;

        public string PhotoUrl { get; init; } = photoUrl;

        public int Length { get; init; } = length;
        public int Width { get; init; } = width;
        public int Height { get; init; } = height;

        public double Weight { get; init; } = weight;

        public string TechSize { get; init; } = techSize;
        public string Barcode { get; init; } = barcode;
    }
}