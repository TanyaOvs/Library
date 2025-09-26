using System;
using System.IO;

namespace Library
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание Book на основе ручного ввода
             Book firstBook = new Book(1, "979-5-12345-678-0", "Евгений Онегин", "Александр Сергеевич Пушкин", "Роман", 500, 800);
             firstBook.PrintFullInfo();
             firstBook.PrintShortInfo();

            // Создание BookPreview
            BookPreview bp = new BookPreview(1, "979-5-12345-678-0", "Евгений Онегин", "Александр Сергеевич Пушкин", "Роман", 500, 800);
            bp.PrintFullInfo();
            bp.PrintShortInfo();

            // Создание book на основе строки
            // Пример книги для ввода: 1;979-5-12345-678-0;1984;Джордж Оруэлл;Антиутопия;500;800
            Console.WriteLine("Введите данные в одну строку, разделяя данные знаком ';' в формате:\n'ID_Книги;ISBN;Название;Автор;Жанр;Залоговая_стоимость;Стоимость_проката'.");
            string input_data = Console.ReadLine();
            Book secondBook = new Book(input_data);
            Console.WriteLine("Экземпляр класса Book был успешно создан! :з");

            // Создание book на основе JSON файла
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "book.json");
                Book thirdbook = new Book(fullPath, true);
                Console.WriteLine("Книга успешно загружена из файла!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
