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
    }
}
