using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;


namespace Library
{
    public class Book_rep_json
    {
        public Book_rep_json() { }

        public List<Book> ReadJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден.", filePath);

            try
            {
                List<Book> bookList = new List<Book>();

                string jsonString = File.ReadAllText(filePath);
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                foreach (JsonElement bookElement in root.EnumerateArray())
                {
                    string ISBN = bookElement.GetProperty("isbn").GetString();
                    string Title = bookElement.GetProperty("title").GetString();
                    string Author = bookElement.GetProperty("author").GetString();
                    string Genre = bookElement.GetProperty("genre").GetString();

                    Book book = new Book(ISBN, Title, Author, Genre);
                    bookList.Add(book);
                    Console.WriteLine($"Книга '{book.Title}' успешно загружена из файла!");
                }

                Console.WriteLine($"\nВсего книг загружено: {bookList.Count}\n");
                return bookList;
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

        public void WriteJson(List<Book> bookList)
        {
            if (bookList == null || bookList.Count == 0)
                throw new ArgumentException("Список книг пуст или не инициализирован, нельзя записать данные в файл!");

            string outputFile = Path.Combine(Directory.GetCurrentDirectory(), "written_books.json");

            try
            {
                // Настройки для форматированного вывода
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // отступы в файле
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // кодировка для читабельного русского языка
                };

                string jsonString = JsonSerializer.Serialize(bookList, options);
                File.WriteAllText(outputFile, jsonString);

                Console.WriteLine($"{bookList.Count} книг(-и) успешно записаны в файл: {outputFile}");
            }
            catch (Exception ex)
            {
                throw new IOException("Ошибка при записи JSON-файла: " + ex.Message);
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

        public List<BookPreview> ReadShortJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.");
        
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден.", filePath);
        
            try
            {
                List<BookPreview> bookList = new List<BookPreview>();
        
                string jsonString = File.ReadAllText(filePath);
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;
        
                foreach (JsonElement bookElement in root.EnumerateArray())
                {
                    string ISBN = bookElement.GetProperty("isbn").GetString();
                    string Title = bookElement.GetProperty("title").GetString();
                    string Author = bookElement.GetProperty("author").GetString();
                    string Genre = bookElement.GetProperty("genre").GetString();
                    double CollateralValue = bookElement.GetProperty("collateralValue").GetDouble();
                    double RentalCost = bookElement.GetProperty("rentalCost").GetDouble();
        
                    BookPreview bookPreview = new BookPreview(ISBN, Title, Author, Genre, CollateralValue, RentalCost);
                    bookList.Add(bookPreview);
                    Console.WriteLine($"Книга '{bookPreview.Title}' успешно загружена из файла!");
                }
        
                Console.WriteLine($"Всего книг загружено: {bookList.Count}");
                return bookList;
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

        private void PrintBooksList<T>(List<T> bookList) where T : Book
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
                book.PrintFullInfo();
            }
        
            Console.WriteLine(separator);
        }
        
        public List<BookPreview> Get_K_N_ShortList(List<BookPreview> k, int n)
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
        
            return result;
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
                Console.WriteLine("Список пуст или не существует!");
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
            bookList.Add(book);
            return bookList;
        }

        public List<Book> ChangeBookByID(List<Book> bookList, Book book, int bookID)
        {
            if (bookID < 0 || bookID >= bookList.Count)
                throw new ArgumentException("ID выходит за пределы списка bookList!");
        
            foreach (Book bookElem in bookList)
            { 
                if(bookElem.BookID == bookID)
                {
                    bookList[bookList.IndexOf(bookElem)] = book;
                    break;
                }
            }
            return bookList;
        }
        
        public List<Book> DeleteBookInList(List<Book> bookList, Book book)
        {
            bookList.Remove(book);
            return bookList;
        }
        
    }
}
