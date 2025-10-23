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
    }
}
