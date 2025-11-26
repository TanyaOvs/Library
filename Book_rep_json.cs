using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;


namespace Library
{
    public class Book_rep_json : Book_rep
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
    }
}
