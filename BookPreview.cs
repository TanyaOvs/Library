using System;
using System.Text.RegularExpressions;

namespace Library
{
    class BookPreview : Book
    {
        private string titleAndAuthor;

        public string TitleAndAuthor
        {
            get { return titleAndAuthor; }
            set { ValidateTitleAndAuthor(value); titleAndAuthor = value; }
        }


        public BookPreview(int bookID, string isbn, string title, string author, string genre, double collateralValue, double rentalCost)
         : base(bookID, isbn, title, author, genre, collateralValue, rentalCost)
        {
            TitleAndAuthor = title + " - " + author;
        }

        private void ValidateTitleAndAuthor(string titleAndAuthor)
        {
            if (string.IsNullOrWhiteSpace(titleAndAuthor))
                throw new ArgumentException("Значение для поля 'Название книги и автор' не может быть пустым!");

            if (!Regex.IsMatch(titleAndAuthor, @"^[\d\w\s,:!?а-яА-ЯёЁ-]+[А-ЯЁ][а-яё]+(?:[\- ][А-ЯЁ]?[а-яё]+)*$"))
                throw new ArgumentException("Значение для поля 'Название книги и автор' содержит запрещенные символы!");
        }

        public override void PrintFullInfo()
        {
            Console.WriteLine($"ID: {BookID}\n" +
                   $"ISBN: {ISBN}\n" +
                   $"Залоговая стоимость: {CollateralValue}\n" +
                   $"Стоимость проката: {RentalCost}");
        }

        public override void PrintShortInfo()
        {
            Console.WriteLine($"{TitleAndAuthor}");
        }

    }
}
