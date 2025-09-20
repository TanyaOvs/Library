using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace Library
{
    public class Book
    {
        private int bookID;
        private string isbn;
        private string title;
        private string author;
        private string genre;
        private double collateralValue;
        private double rentalCost;

        public int BookID
        {
            get { return bookID; }
            set { ValidateBookID(value);bookID = value; }
        }

        public string ISBN
        {
            get { return isbn; }
            set { ValidateISBN(value); isbn = value; }
        }

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

        public Book(int bookID, string isbn, string title, string author, string genre, double collateralValue, double rentalCost) 
        {
            BookID = bookID;
            ISBN = isbn;
            Title = title;
            Author = author;
            Genre = genre;
            CollateralValue = collateralValue;
            RentalCost = rentalCost;
        }

        public Book(string dataString)
        {
            var data_parts = dataString.Split(';');
            if (data_parts.Length != 7)
                throw new ArgumentException("Строка должна содержать 7 полей, разделенных ';'");

            BookID = int.Parse(data_parts[0]);
            ISBN = data_parts[1];
            Title = data_parts[2];
            Author = data_parts[3];
            Genre = data_parts[4];
            CollateralValue = double.Parse(data_parts[5]);
            RentalCost = double.Parse(data_parts[6]);
        }

        public Book(string filePath, bool fromFile)
        {
            //string absolutePath = Path.GetFullPath(filePath);
            ////Console.WriteLine($"Ищем файл по абсолютному пути: {absolutePath}");

            //if (!File.Exists(absolutePath))
            //    throw new FileNotFoundException($"Файл не найден: {absolutePath}");

            if (!fromFile)
                throw new ArgumentException("Для загрузки из JSON файла необходимо указать true в качестве второго параметра для конструктора!");

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден.", filePath);

            try
            {
                string jsonString = File.ReadAllText(filePath);
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                BookID = root.GetProperty("book_id").GetInt32();
                ISBN = root.GetProperty("isbn").GetString();
                Title = root.GetProperty("title").GetString();
                Author = root.GetProperty("author").GetString();
                Genre = root.GetProperty("genre").GetString();
                CollateralValue = root.GetProperty("collateralValue").GetDouble();
                RentalCost = root.GetProperty("rentalCost").GetDouble();
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Неверный JSON формат в файле: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ошибка чтения файла: " + ex.Message);
            }
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

        public void PrintFullBook()
        {
            Console.WriteLine($"ID: {BookID}\n" +
                   $"ISBN: {ISBN}\n" +
                   $"Название: {Title}\n" +
                   $"Автор: {Author}\n" +
                   $"Жанр: {Genre}\n" +
                   $"Залоговая стоимость: {CollateralValue}\n" +
                   $"Стоимость проката: {RentalCost}\n");
        }

        public void PrintShortBook()
        {
            Console.WriteLine($"'{Title}' - {Author}, {Genre}, ISBN: {ISBN}");
        }

        public static void CompareBooks(Book firstBook, Book secondBook) 
        {
            if (firstBook.ISBN == secondBook.ISBN)
                Console.WriteLine("Книги одинаковы.");
            else 
                Console.WriteLine("Книги разные.");
        }
    }
}
