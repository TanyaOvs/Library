using System;
using System.Collections.Generic;

namespace Library
{
    class Program
    {
        private static void PrintBooksList<T>(List<T> bookList) where T : Book
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

        static void Main(string[] args)
        {
            // Работа с файлами через декоратор
            try
            {
                // Пример для JSON
                Book_rep_json brj = new Book_rep_json();
                brj.ReadShortFile("books.json");

                IBookRepository fileRepo = new Book_rep_file_adapter(brj);

                // Пример использования фильтра
                fileRepo = new FilterDecorator(fileRepo, b => b.Genre == "Детектив");

                // Пример использования сортировки
                fileRepo = new SortDecorator(fileRepo, b => b.Author);

                List<BookPreview> page = fileRepo.Get_K_N_ShortList(2);
                PrintBooksList(page);

                int count = fileRepo.Get_Count();
                Console.WriteLine($"Количество отфильтрованных книг: {count}");


                // Пример для YAML
                Book_rep_yaml bry = new Book_rep_yaml();
                bry.ReadShortFile("books.json");

                fileRepo = new Book_rep_file_adapter(bry);

                // Пример использования фильтра
                fileRepo = new FilterDecorator(fileRepo, b => b.Genre == "Роман");

                // Пример использования сортировки
                fileRepo = new SortDecorator(fileRepo, b => b.Author);

                List<BookPreview> pageTest = fileRepo.Get_K_N_ShortList(1);
                PrintBooksList(pageTest);

                int countTest = fileRepo.Get_Count();
                Console.WriteLine($"Количество отфильтрованных книг: {countTest}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
