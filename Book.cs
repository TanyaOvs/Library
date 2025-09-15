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
            set { ValidateAuthor(value); author = value; }
        }

        public string Genre
        {
            get { return genre; }
            set { ValidateGenre(value); genre = value; }
        }

        public double CollateralValue
        {
            get { return collateralValue; }
            set { ValidateCollateralValue(value); collateralValue = value; }
        }

        public double RentalCost
        {
            get { return rentalCost; }
            set { ValidateRentalCost(value); rentalCost = value; }
        }

        public Book(string title, string author, string genre, double collateral_value, double rental_cost) 
        {
            Title = title;
            Author = author;
            Genre = genre;
            CollateralValue = collateral_value;
            RentalCost = rental_cost;
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым или состоять только из пробелов!");

            if (!Regex.IsMatch(title, @"^[\d\w\s,:!?а-яА-ЯёЁ-]+$"))
                throw new ArgumentException("Название книги содержит запрещенные символы!");
        }

        private static void ValidateAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("ФИО автора не может быть пустым или состоять только из пробелов!");

            if (!Regex.IsMatch(author, @"^[А-ЯЁ][а-яё]+(?:[\- ][А-ЯЁ]?[а-яё]+)*$"))
                throw new ArgumentException("ФИО автора должно содержать только русские буквы, начинаться с заглавной буквы, и может содержать пробелы или тире!");
        }

        private static void ValidateGenre(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Жанр не может быть пустым или состоять только из пробелов!");

            if (!Regex.IsMatch(genre, @"^[А-ЯЁ][а-яё]+(?:[\- ][А-ЯЁ]?[а-яё]+)*$"))
                throw new ArgumentException("Жанр должен содержать только русские буквы, начинаться с заглавной буквы, и может содержать пробелы или тире!");
        }

        private static void ValidateCollateralValue(double сollateralValue)
        {
            if (сollateralValue < 0)
                throw new ArgumentException("Залоговая стоимость не может быть отрицательной!");

            if (сollateralValue == 0)
                throw new ArgumentException("Залоговая стоимость не может быть нулевой!");

            if (сollateralValue > 10000)
                throw new ArgumentException("Залоговая стоимость слишком высока!");
        }

        private static void ValidateRentalCost(double rentallValue)
        {
            if (rentallValue < 0)
                throw new ArgumentException("Стоимость проката не может быть отрицательной!");

            if (rentallValue == 0)
                throw new ArgumentException("Стоимость проката не может быть нулевой!");

            if (rentallValue > 10000)
                throw new ArgumentException("Стоимость проката слишком высока!");
        }
    }
}
