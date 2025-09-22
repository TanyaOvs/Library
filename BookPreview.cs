using System;
using System.Text.RegularExpressions;

namespace Library
{
    class BookPreview
    {
        private int bookID;
        private string isbn;
        private double collateralValue;
        private double rentalCost;

        public int BookID
        {
            get { return bookID; }
            set { ValidateBookID(value); bookID = value; }
        }

        public string ISBN
        {
            get { return isbn; }
            set { ValidateISBN(value); isbn = value; }
        }

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

        public BookPreview(int bookID, string isbn, double collateralValue, double rentalCost)
        {
            BookID = bookID;
            ISBN = isbn;
            CollateralValue = collateralValue;
            RentalCost = rentalCost;
        }
        
        private static void ValidateBookID(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID книги должен быть положительным числом!");

            if (id > 999999)
                throw new ArgumentException("ID книги не может превышать 6 цифр!");
        }

        private static void ValidateISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN не может быть пустым!");

            // Регулярное выражение для формата ISBN-13: (978|979)-5-12345-678-0
            if (!Regex.IsMatch(isbn, @"^(978|979)-\d-\d{5}-\d{3}-\d$"))
                throw new ArgumentException("ISBN должен быть в формате: '978-*-*****-***-*' или '979-*-*****-***-*'.");
        }
        
        private static void ValidatePrice(double price, string fieldName)
        {
            const double MaxPriceValue = 10000;

            if (price < 0 || price == 0)
                throw new ArgumentException($"{fieldName} не может быть отрицательной или нулевой!");

            if (price > MaxPriceValue)
                throw new ArgumentException($"{fieldName} не может превышать {MaxPriceValue}!");
        }

    }
}
