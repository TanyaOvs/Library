using System;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Library
{
    public class Book_rep_yaml
    {
        public Book_rep_yaml() { }
        public List<Book> ReadYaml(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Файл не найден: {filePath}");

                var yaml = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

                var rawBooks = deserializer.Deserialize<List<Dictionary<string, object>>>(yaml);

                List<Book> bookList = new List<Book>();
                foreach (var item in rawBooks)
                {
                    string ISBN = item["isbn"].ToString();
                    string Title = item["title"].ToString();
                    string Author = item["author"].ToString();
                    string Genre = item["genre"].ToString();

                    if (item.ContainsKey("collateralValue") && item.ContainsKey("rentalCost"))
                    {
                        double CollateralValue = Convert.ToDouble(item["collateralValue"]);
                        double RentalCost = Convert.ToDouble(item["rentalCost"]);
                        BookPreview bookPreview = new BookPreview(ISBN, Title, Author, Genre, CollateralValue, RentalCost);
                        bookList.Add(bookPreview);
                        Console.WriteLine($"Книга '{bookPreview.Title}' успешно загружена из файла!");
                    }
                    else
                    {
                        Book book = new Book(ISBN, Title, Author, Genre);
                        bookList.Add(book);
                        Console.WriteLine($"Книга (превью)'{book.Title}' успешно загружена из файла!");
                    }
                }
                return bookList;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ошибка чтения файла: " + ex.Message);
            }
        }

        public void WriteYaml(List<Book> bookList)
        {
            if (bookList == null || bookList.Count == 0)
                throw new ArgumentException("Список книг пуст или не инициализирован, нельзя записать данные в файл!");
        
            string outputFile = Path.Combine(Directory.GetCurrentDirectory(), "written_books.yaml");
            try
            {
                // Создаем сериализатор с читаемым форматированием и CamelCase для YAML
                ISerializer serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                
                // Создаем вспомогательный список для сериализации
                List<object> yamlObjects = new List<object>();
                foreach (Book book in bookList)
                {
                    yamlObjects.Add(new
                    {
                        isbn = book.ISBN,
                        title = book.Title,
                        author = book.Author,
                        genre = book.Genre
                    });
                }
                string yamlString = serializer.Serialize(yamlObjects);
                File.WriteAllText(outputFile, yamlString);
                Console.WriteLine($"{bookList.Count} книг(-и) успешно записаны в файл: {outputFile}");
            }
            catch (Exception ex)
            {
                throw new IOException("Ошибка при записи YAML-файла: " + ex.Message);
            }
        }

        public Book GetBookByID(List<Book> bookList, int bookID)
        {
            if (bookList == null || bookList.Count == 0)
                throw new ArgumentException("Список книг пуст или не инициализирован.");
        
            // Ищем книгу с помощью метода LINQ, который возвращает первый элемент, удовлетворяющий условию с bookID
            Book foundBook = bookList.FirstOrDefault(book => book.BookID == bookID);
        
            if (foundBook == null)
                throw new ArgumentException($"Книга с ID={bookID} не найдена в списке.");
        
            return foundBook;
        }

        
        public void PrintBooksList<T>(List<T> bookList) where T : Book
        {
            if (bookList == null || bookList.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            string separator = "-------------------------";
            Console.WriteLine(separator);
            Console.WriteLine("Список книг:");

            foreach (var book in bookList)
            {
                Console.WriteLine(separator);
                book.PrintShortInfo();
            }

            Console.WriteLine(separator);
        }

        public List<BookPreview> ReadShortYaml(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Файл не найден: {filePath}");

                var yaml = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

                var rawBooks = deserializer.Deserialize<List<Dictionary<string, object>>>(yaml);

                List<BookPreview> bookList = new List<BookPreview>();
                foreach (var item in rawBooks)
                {
                    string ISBN = item["isbn"].ToString();
                    string Title = item["title"].ToString();
                    string Author = item["author"].ToString();
                    string Genre = item["genre"].ToString();
                    double CollateralValue = Convert.ToDouble(item["collateralValue"]);
                    double RentalCost = Convert.ToDouble(item["rentalCost"]);
                    BookPreview bookPreview = new BookPreview(ISBN, Title, Author, Genre, CollateralValue, RentalCost);
                    bookList.Add(bookPreview);
                    Console.WriteLine($"Книга (превью) '{bookPreview.Title}' успешно загружена из файла!");
                }
                return bookList;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ошибка чтения файла: " + ex.Message);
            }
        }

        public void Get_K_N_ShortList(List<BookPreview> k, int n)
        {
            const int pageSize = 2; // Количество объектов на одной "странице"

            if (k == null || k.Count == 0)
                throw new ArgumentException("Список книг пуст или не инициализирован!");

            if (n <= 0)
                throw new ArgumentException("Номер страницы должен быть положительным числом!");

            // Вычисляем, сколько элементов нужно пропустить, чтобы начать с нужной "страницы"
            int skipCount = (n - 1) * pageSize;

            if (skipCount >= k.Count)
                throw new ArgumentException("Номер страницы выходит за пределы списка книг!");

            List<BookPreview> result = k.Skip(skipCount).Take(pageSize).ToList();
            PrintBooksList(result);
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return !string.IsNullOrEmpty(str);
        }
        
        public void SortBooksByTitle(List<Book> bookList)
        {
            if (bookList == null || bookList.Count == 0)
            {
                Console.WriteLine("Список пуст или не существует.");
                return;
            }
        
            for (int i = 0; i < bookList.Count - 1; i++)
            {
                for (int j = i + 1; j < bookList.Count; j++)
                {
                    Book book1 = bookList[i];
                    Book book2 = bookList[j];
        
                    bool isDigits1 = IsDigitsOnly(book1.Title);
                    bool isDigits2 = IsDigitsOnly(book2.Title);
        
                    // Если обе книги содержат только цифры в названии, то сортируем как числа
                    if (isDigits1 && isDigits2)
                    {
                        long num1 = long.Parse(book1.Title);
                        long num2 = long.Parse(book2.Title);
        
                        if (num1 > num2)
                            (bookList[i], bookList[j]) = (bookList[j], bookList[i]);
                    }
        
                    // Если название первой книги содержит только цифры, то она идет раньше в списке
                    else if (isDigits1 && !isDigits2)
                        (bookList[i], bookList[j]) = (bookList[j], bookList[i]);
        
                    // Если название второй книги содержит только цифры, то она идёт позже в списке
                    else if (!isDigits1 && isDigits2)
                        continue;
        
                    // Если названия обеих книг строковые, то выполняем обычную лексикографическую сортировку
                    else if (string.Compare(book1.Title, book2.Title, StringComparison.OrdinalIgnoreCase) > 0)
                        (bookList[i], bookList[j]) = (bookList[j], bookList[i]);
                }
            }
        
            Console.WriteLine("Список книг успешно отсортирован по названию!");
            PrintBooksList(bookList);
        }

        public List<Book> AddBookInList(List<Book> bookList, Book book)
        {
            foreach (Book bookElem in bookList)
            {
                if (Book.CompareBooks(book, bookElem))
                {
                    throw new ArgumentException($"Нельзя добавить книгу {book.Title}: книга с таким же ISBN уже находится в списке!");
                }
            }
            bookList.Add(book);
            return bookList;
        }
        
    }
}
