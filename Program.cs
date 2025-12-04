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
            // Работа с базой данных через декоратор
            try
            {
                Book_rep_DB brdb = new Book_rep_DB();
                IBookRepository repo = new Book_rep_adapter(brdb);

                // Пример использования фильтра
                repo = new FilterDecorator(repo, bp => bp.Genre == "Роман");

                // Пример использовния сортировки
                repo = new SortDecorator(repo, bp => bp.Author);

                // Отображение результатов
                List<BookPreview> page = repo.Get_K_N_ShortList(1);
                PrintBooksList(page);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Book_rep_db_connection.Instance.Close();
            }
        }
    }
}
