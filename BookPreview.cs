namespace Library
{
    class BookPreview : Book
    {
        private double collateralValue;
        private double rentalCost;
        public double CollateralValue
        {
            get { return collateralValue; }
            set { ValidatePrice(value, "Залоговая стоимость"); collateralValue = value; }
        }

        public double RentalCost
        {
            get { return rentalCost; }
            set { ValidatePrice(value, "Стоимость проката"); rentalCost = value; }
        }

        public BookPreview(string isbn, string title, string author, string genre, double collateralValue, double rentalCost) : base(isbn, title, author, genre)
        {
            CollateralValue = collateralValue;
            RentalCost = rentalCost;
        }

        private static void ValidatePrice(double price, string fieldName)
        {
            const double MaxPriceValue = 10000;

            if (price < 0 || price == 0)
                throw new ArgumentException($"{fieldName} не может быть отрицательной или нулевой!");

            if (price > MaxPriceValue)
                throw new ArgumentException($"{fieldName} не может превышать {MaxPriceValue}!");
        }

        public override void PrintShortInfo()
        {
            Console.WriteLine($"Залоговая стоимость: {CollateralValue}\n" +
                              $"Стоимость проката: {RentalCost}\n");
        }

    }
}
