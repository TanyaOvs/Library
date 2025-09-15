using System;
using System.Text.RegularExpressions;

namespace Library
{
    public class Book
    {
        private string title;
        private string author;
        private string genre;
        private double collateralValue;
        private double rentalCost;

        public string Title
        {
            get { return title; }
            set { ValidateTitle(value); title = value; }
        }

        public string Author
        {
            get { return author; }
            set { ValidateStringField(value, "ФИО автора"); author = value; }
        }

        public string Genre
        {
            get { return genre; }
            set { ValidateStringField(value, "Жанр"); genre = value; }
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

        public Book(string title, string author, string genre, double collateralValue, double rentalCost) 
        {
            Title = title;
            Author = author;
            Genre = genre;
            CollateralValue = collateralValue;
            RentalCost = rentalCost;
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым или состоять только из пробелов!");

            if (!Regex.IsMatch(title, @"^[\d\w\s,:!?а-яА-ЯёЁ-]+$"))
                throw new ArgumentException("Название книги содержит запрещенные символы!");
        }

        private static void ValidateStringField(string field, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentException($"{fieldName} не может быть пустым или состоять только из пробелов!");

            if (!Regex.IsMatch(field, @"^[А-ЯЁ][а-яё]+(?:[\- ][А-ЯЁ]?[а-яё]+)*$"))
                throw new ArgumentException($"{fieldName} может содержать только русские буквы, начинаться с заглавной буквы, и может содержать пробелы или тире!");
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
