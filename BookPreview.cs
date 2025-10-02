using System;

namespace Library
{
    class BookPreview
    {
        private readonly Book book;
        public int BookID { get; }
        public string TitleAndAuthor { get; private set; }
        public string Genre { get; }
        public double RentalCost { get; }

        public BookPreview(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Объект Book не может быть путсым!");
            }
            this.book = book;
            BookID = book.BookID;
            TitleAndAuthor = $"{book.Title} - {book.Author}";
            Genre = book.Genre;
            RentalCost = book.RentalCost;
        }


        public void PrintFullInfo()
        {
            Console.WriteLine($"ID: {BookID}\n" +
                   $"Книга: {TitleAndAuthor}\n" +
                   $"Жанр: {Genre}\n" +
                   $"Стоимость проката: {RentalCost}\n");
        }

        public void PrintShortInfo()
        {
            Console.WriteLine($"{TitleAndAuthor}\n");
        }

    }
}
